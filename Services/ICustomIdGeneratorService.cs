using InventoryManager.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryManager.Services
{
    public interface ICustomIdGeneratorService
    {
        Task<string> GenerateCustomIdAsync(Guid inventoryId);
    }
}