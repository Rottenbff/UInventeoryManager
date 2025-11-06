using InventoryManager.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryManager.Services
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsersAsync();
        Task BlockUsersAsync(List<Guid> userIds);
        Task UnblockUsersAsync(List<Guid> userIds);
        Task DeleteUsersAsync(List<Guid> userIds);
        Task AddToAdminAsync(List<Guid> userIds);
        Task RemoveFromAdminAsync(List<Guid> userIds);
    }
}