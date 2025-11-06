using InventoryManager.Models;
using System;
using System.Threading.Tasks;

namespace InventoryManager.Services
{
    public interface ILikeService
    {
        Task<bool> LikeItemAsync(Guid itemId, Guid userId);
        Task<bool> UnlikeItemAsync(Guid itemId, Guid userId);
        Task<int> GetLikeCountAsync(Guid itemId);
        Task<bool> IsItemLikedByUserAsync(Guid itemId, Guid userId);
    }
}