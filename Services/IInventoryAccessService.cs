using InventoryManager.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryManager.Services
{
    public interface IInventoryAccessService
    {
        Task<bool> HasWriteAccessAsync(Guid inventoryId, Guid userId);
        Task<bool> GrantWriteAccessAsync(Guid inventoryId, Guid userId);
        Task<bool> RevokeWriteAccessAsync(Guid inventoryId, Guid userId);
        Task<List<User>> GetUsersWithWriteAccessAsync(Guid inventoryId);
        Task<List<Inventory>> GetInventoriesWithWriteAccessAsync(Guid userId);
    }
}