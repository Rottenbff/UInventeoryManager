using InventoryManager.Data;
using InventoryManager.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManager.Services
{
    public class LikeService : ILikeService
    {
        private readonly ApplicationDbContext _context;

        public LikeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> LikeItemAsync(Guid itemId, Guid userId)
        {
            // Check if the like already exists
            var existingLike = await _context.Likes
                .FirstOrDefaultAsync(l => l.ItemId == itemId && l.UserId == userId);

            if (existingLike != null)
                return false; // Already liked

            var like = new Like
            {
                Id = Guid.NewGuid(),
                ItemId = itemId,
                UserId = userId
            };

            _context.Likes.Add(like);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UnlikeItemAsync(Guid itemId, Guid userId)
        {
            var like = await _context.Likes
                .FirstOrDefaultAsync(l => l.ItemId == itemId && l.UserId == userId);

            if (like == null)
                return false; // Not liked

            _context.Likes.Remove(like);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetLikeCountAsync(Guid itemId)
        {
            return await _context.Likes
                .CountAsync(l => l.ItemId == itemId);
        }

        public async Task<bool> IsItemLikedByUserAsync(Guid itemId, Guid userId)
        {
            return await _context.Likes
                .AnyAsync(l => l.ItemId == itemId && l.UserId == userId);
        }
    }
}