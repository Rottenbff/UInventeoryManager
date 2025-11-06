using InventoryManager.Data;
using InventoryManager.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManager.Services
{
    public class CommentService : ICommentService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<InventoryManager.Hubs.CommentHub> _hubContext;

        public CommentService(ApplicationDbContext context, IHubContext<InventoryManager.Hubs.CommentHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public async Task<List<Comment>> GetCommentsForInventoryAsync(Guid inventoryId)
        {
            return await _context.Comments
                .Include(c => c.User)
                .Where(c => c.InventoryId == inventoryId)
                .OrderBy(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<Comment> CreateCommentAsync(Comment comment)
        {
            // Add the comment to the database
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            // Load the comment with user information
            var commentWithUser = await _context.Comments
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == comment.Id);

            // Broadcast the new comment to all clients in the inventory group
            if (commentWithUser != null)
            {
                await _hubContext.Clients.Group($"Inventory_{comment.InventoryId}")
                    .SendAsync("ReceiveNewComment", new
                    {
                        Id = commentWithUser.Id,
                        InventoryId = commentWithUser.InventoryId,
                        UserId = commentWithUser.UserId,
                        Content = commentWithUser.Content,
                        CreatedAt = commentWithUser.CreatedAt,
                        UserName = commentWithUser.User?.Name
                    });
            }

            return comment;
        }

        public async Task<bool> DeleteCommentAsync(Guid commentId, Guid userId)
        {
            var comment = await _context.Comments
                .FirstOrDefaultAsync(c => c.Id == commentId && c.UserId == userId);

            if (comment == null)
                return false;

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}