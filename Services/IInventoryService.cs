using InventoryManager.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryManager.Services
{
    public interface IInventoryService
    {
        Task<List<Inventory>> GetInventoriesForUserAsync(Guid userId);
        Task<Inventory?> GetInventoryByIdAsync(Guid id);
        Task<Inventory> CreateInventoryAsync(Inventory newInventory);
        Task<Inventory> UpdateInventoryAsync(Inventory inventory);
        Task<bool> DeleteInventoryAsync(Guid id, Guid userId);
        Task<List<Inventory>> GetRecentInventoriesAsync(int count);
        Task<List<Inventory>> GetTopInventoriesAsync(int count);
        Task<List<Inventory>> SearchInventoriesAsync(string searchTerm);
        Task<Item> CreateItemAsync(Item item);
        Task<Item> UpdateItemAsync(Item item);
        Task<bool> DeleteItemAsync(Guid itemId);
        Task<List<Item>> GetItemsForInventoryAsync(Guid inventoryId);
    }
}