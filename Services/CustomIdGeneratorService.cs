using InventoryManager.Data;
using InventoryManager.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManager.Services
{
    public class CustomIdGeneratorService : ICustomIdGeneratorService
    {
        private readonly ApplicationDbContext _context;

        public CustomIdGeneratorService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> GenerateCustomIdAsync(Guid inventoryId)
        {
            // For the simplified approach, we'll use a basic format
            // In a real implementation, you might want to store format preferences in the Inventory entity
            var inventory = await _context.Inventories.FirstOrDefaultAsync(i => i.Id == inventoryId);
            
            if (inventory == null)
            {
                // Default format if inventory not found
                return $"ITEM-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..4].ToUpper()}";
            }

            // Simple format: ITEM-[DATE]-[RANDOM]
            var datePart = DateTime.Now.ToString("yyyyMMdd");
            var randomPart = new Random().Next(1000, 9999);
            
            return $"ITEM-{datePart}-{randomPart}";
        }
    }
}