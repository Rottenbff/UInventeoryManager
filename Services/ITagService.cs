using InventoryManager.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryManager.Services
{
    public interface ITagService
    {
        Task<List<Tag>> GetAllTagsAsync();
        Task<List<Tag>> GetTagsForInventoryAsync(Guid inventoryId);
        Task<bool> AddTagToInventoryAsync(Guid inventoryId, string tagName);
        Task<bool> RemoveTagFromInventoryAsync(Guid inventoryId, Guid tagId);
        Task<List<TagWithCount>> GetPopularTagsAsync(int count);
    }
}