using InventoryManager.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryManager.Services
{
    public interface ICommentService
    {
        Task<List<Comment>> GetCommentsForInventoryAsync(Guid inventoryId);
        Task<Comment> CreateCommentAsync(Comment comment);
        Task<bool> DeleteCommentAsync(Guid commentId, Guid userId);
    }
}