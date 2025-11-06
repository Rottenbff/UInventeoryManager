using InventoryManager.Data;
using InventoryManager.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManager.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task BlockUsersAsync(List<Guid> userIds)
        {
            if (userIds == null || !userIds.Any())
                return;

            var users = await _context.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();
            foreach (var user in users)
            {
                user.IsBlocked = true;
            }
            await _context.SaveChangesAsync();
        }

        public async Task UnblockUsersAsync(List<Guid> userIds)
        {
            if (userIds == null || !userIds.Any())
                return;

            var users = await _context.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();
            foreach (var user in users)
            {
                user.IsBlocked = false;
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUsersAsync(List<Guid> userIds)
        {
            if (userIds == null || !userIds.Any())
                return;

            var users = await _context.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();
            _context.Users.RemoveRange(users);
            await _context.SaveChangesAsync();
        }

        public async Task AddToAdminAsync(List<Guid> userIds)
        {
            if (userIds == null || !userIds.Any())
                return;

            var users = await _context.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();
            foreach (var user in users)
            {
                user.IsAdmin = true;
            }
            await _context.SaveChangesAsync();
        }

        public async Task RemoveFromAdminAsync(List<Guid> userIds)
        {
            if (userIds == null || !userIds.Any())
                return;

            var users = await _context.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();
            foreach (var user in users)
            {
                user.IsAdmin = false;
            }
            await _context.SaveChangesAsync();
        }
    }
}