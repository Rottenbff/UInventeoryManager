using InventoryManager.Data;
using InventoryManager.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManager.Services
{
    public class InventoryAccessService : IInventoryAccessService
    {
        private readonly ApplicationDbContext _context;

        public InventoryAccessService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> HasWriteAccessAsync(Guid inventoryId, Guid userId)
        {
            // Check if user is the creator/owner
            var inventory = await _context.Inventories
                .FirstOrDefaultAsync(i => i.Id == inventoryId);

            if (inventory != null && inventory.CreatorId == userId)
                return true;

            // Check if user has been granted write access
            return await _context.InventoryAccesses
                .AnyAsync(ia => ia.InventoryId == inventoryId && ia.UserId == userId);
        }

        public async Task<bool> GrantWriteAccessAsync(Guid inventoryId, Guid userId)
        {
            // Check if access already exists
            var existingAccess = await _context.InventoryAccesses
                .FirstOrDefaultAsync(ia => ia.InventoryId == inventoryId && ia.UserId == userId);

            if (existingAccess != null)
                return false; // Access already granted

            var access = new InventoryAccess
            {
                Id = Guid.NewGuid(),
                InventoryId = inventoryId,
                UserId = userId
            };

            _context.InventoryAccesses.Add(access);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RevokeWriteAccessAsync(Guid inventoryId, Guid userId)
        {
            var access = await _context.InventoryAccesses
                .FirstOrDefaultAsync(ia => ia.InventoryId == inventoryId && ia.UserId == userId);

            if (access == null)
                return false; // Access not found

            _context.InventoryAccesses.Remove(access);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<User>> GetUsersWithWriteAccessAsync(Guid inventoryId)
        {
            return await _context.InventoryAccesses
                .Where(ia => ia.InventoryId == inventoryId)
                .Select(ia => ia.User!)
                .Where(u => u != null)
                .ToListAsync();
        }

        public async Task<List<Inventory>> GetInventoriesWithWriteAccessAsync(Guid userId)
        {
            // Fixed the query by applying Include before Select
            var inventoryIds = await _context.InventoryAccesses
                .Where(ia => ia.UserId == userId)
                .Select(ia => ia.InventoryId)
                .ToListAsync();

            return await _context.Inventories
                .Where(i => inventoryIds.Contains(i.Id))
                .Include(i => i.Creator)
                .ToListAsync();
        }
    }
}