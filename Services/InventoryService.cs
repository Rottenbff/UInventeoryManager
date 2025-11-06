using InventoryManager.Data;
using InventoryManager.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManager.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICustomIdGeneratorService _customIdGenerator;

        public InventoryService(ApplicationDbContext context, ICustomIdGeneratorService customIdGenerator)
        {
            _context = context;
            _customIdGenerator = customIdGenerator;
        }

        public async Task<List<Inventory>> GetInventoriesForUserAsync(Guid userId)
        {
            return await _context.Inventories
                .Where(i => i.CreatorId == userId)
                .Include(i => i.Creator)
                .ToListAsync();
        }

        public async Task<Inventory?> GetInventoryByIdAsync(Guid id)
        {
            return await _context.Inventories
                .Include(i => i.Creator)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Inventory> CreateInventoryAsync(Inventory newInventory)
        {
            _context.Inventories.Add(newInventory);
            await _context.SaveChangesAsync();
            return newInventory;
        }

        public async Task<Inventory> UpdateInventoryAsync(Inventory inventory)
        {
            // Handle optimistic concurrency
            try
            {
                _context.Inventories.Update(inventory);
                await _context.SaveChangesAsync();
                return inventory;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Handle concurrency conflict
                throw new InvalidOperationException("The inventory has been modified by another user. Please refresh the page and try again.", ex);
            }
        }

        public async Task<bool> DeleteInventoryAsync(Guid id, Guid userId)
        {
            var inventory = await _context.Inventories
                .FirstOrDefaultAsync(i => i.Id == id && i.CreatorId == userId);

            if (inventory == null)
                return false;

            try
            {
                _context.Inventories.Remove(inventory);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Handle concurrency conflict
                throw new InvalidOperationException("The inventory has been modified by another user. Please refresh the page and try again.", ex);
            }
        }

        public async Task<List<Inventory>> GetRecentInventoriesAsync(int count)
        {
            // Note: For a more robust solution, you should add a CreatedAt field to sort by
            return await _context.Inventories
                .Include(i => i.Creator)
                .OrderByDescending(i => i.Id) // Assuming Id is sequential
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<Inventory>> GetTopInventoriesAsync(int count)
        {
            return await _context.Inventories
                .Include(i => i.Creator)
                .Include(i => i.Items)
                .OrderByDescending(i => i.Items.Count)
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<Inventory>> SearchInventoriesAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return new List<Inventory>();
            }

            // Use simple LIKE-based search for better compatibility
            // This searches in Title, Description, Category, and Tag names
            var lowerSearchTerm = searchTerm.ToLower();

            return await _context.Inventories
                .Include(i => i.Creator)
                .Include(i => i.Items)
                .Include(i => i.InventoryTags)
                    .ThenInclude(it => it.Tag)
                .Where(i =>
                    i.Title.ToLower().Contains(lowerSearchTerm) ||
                    (i.Description != null && i.Description.ToLower().Contains(lowerSearchTerm)) ||
                    i.Category.ToLower().Contains(lowerSearchTerm) ||
                    i.InventoryTags.Any(it => it.Tag.Name.ToLower().Contains(lowerSearchTerm)))
                .Distinct()
                .ToListAsync();
        }

        public async Task<Item> CreateItemAsync(Item item)
        {
            // Generate custom ID if not provided
            if (string.IsNullOrEmpty(item.CustomItemId))
            {
                item.CustomItemId = await _customIdGenerator.GenerateCustomIdAsync(item.InventoryId);
            }

            bool saved = false;
            int attempts = 0;
            const int maxAttempts = 3;

            while (!saved && attempts < maxAttempts)
            {
                try
                {
                    _context.Items.Add(item);
                    await _context.SaveChangesAsync();
                    saved = true;
                }
                catch (DbUpdateException ex) when (IsUniqueConstraintViolation(ex))
                {
                    attempts++;
                    if (attempts >= maxAttempts)
                    {
                        throw new InvalidOperationException("Unable to generate a unique custom ID after multiple attempts. Please try again or enter a custom ID manually.", ex);
                    }
                    
                    // Generate a new ID and try again
                    item.CustomItemId = await _customIdGenerator.GenerateCustomIdAsync(item.InventoryId);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    // Handle concurrency conflict
                    throw new InvalidOperationException("The item has been modified by another user. Please refresh the page and try again.", ex);
                }
            }

            return item;
        }

        public async Task<Item> UpdateItemAsync(Item item)
        {
            try
            {
                _context.Items.Update(item);
                await _context.SaveChangesAsync();
                return item;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Handle concurrency conflict
                throw new InvalidOperationException("The item has been modified by another user. Please refresh the page and try again.", ex);
            }
        }

        public async Task<bool> DeleteItemAsync(Guid itemId)
        {
            var item = await _context.Items.FindAsync(itemId);
            if (item == null)
                return false;

            try
            {
                _context.Items.Remove(item);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Handle concurrency conflict
                throw new InvalidOperationException("The item has been modified by another user. Please refresh the page and try again.", ex);
            }
        }

        public async Task<List<Item>> GetItemsForInventoryAsync(Guid inventoryId)
        {
            return await _context.Items
                .Where(i => i.InventoryId == inventoryId)
                .ToListAsync();
        }

        private bool IsUniqueConstraintViolation(DbUpdateException ex)
        {
            // Check if the exception is due to a unique constraint violation
            // This is a simplified check - in a real application, you'd want to be more specific
            return ex.InnerException?.Message?.Contains("duplicate key") == true ||
                   ex.Message?.Contains("duplicate key") == true;
        }
    }
}