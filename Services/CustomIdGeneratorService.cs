using InventoryManager.Data;
using InventoryManager.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

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
            var inventory = await _context.Inventories.FirstOrDefaultAsync(i => i.Id == inventoryId);

            if (inventory == null || !inventory.IdFormatEnabled)
            {
                return ""; // Return empty if not enabled or not found
            }

            List<IdFormatComponent> components;
            if (!string.IsNullOrEmpty(inventory.IdFormatSettings))
            {
                try
                {
                    components = JsonSerializer.Deserialize<List<IdFormatComponent>>(inventory.IdFormatSettings);
                }
                catch (JsonException)
                {
                    return $"Error: Invalid format settings";
                }
            }
            else
            {
                // Fallback to legacy IdFormatComponents if new one is empty
                if (string.IsNullOrEmpty(inventory.IdFormatComponents))
                    return "";

                components = new List<IdFormatComponent>();
                var componentTypes = inventory.IdFormatComponents.Split(',', StringSplitOptions.RemoveEmptyEntries);
                foreach (var type in componentTypes)
                {
                    components.Add(new IdFormatComponent { Type = type });
                }
            }

            if (components == null || components.Count == 0)
            {
                return "";
            }

            var idBuilder = new StringBuilder();
            bool sequenceUpdated = false;

            for (int j = 0; j < components.Count; j++)
            {
                var component = components[j];
                switch (component.Type)
                {
                    case "FixedText":
                        idBuilder.Append(component.TextValue);
                        break;

                    case "Date":
                        idBuilder.Append(DateTime.Now.ToString(component.DateFormat));
                        break;

                    case "RandomNumber":
                        if (int.TryParse(component.MinValue, out int min) && int.TryParse(component.MaxValue, out int max))
                        {
                            idBuilder.Append(new Random().Next(min, max + 1));
                        }
                        break;

                    case "Guid":
                        idBuilder.Append(Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper());
                        break;

                    case "SequenceNumber":
                        if (!sequenceUpdated)
                        {
                            long startValue = long.TryParse(component.StartValue, out var s) ? s : 1;
                            int increment = int.TryParse(component.IncrementValue, out var i) ? i : 1;

                            // Reset sequence if the reset period has passed (e.g., daily, weekly)
                            // For simplicity, this example resets daily.
                            if (inventory.SequenceLastUpdated.Date < DateTime.UtcNow.Date)
                            {
                                inventory.SequenceLastValue = startValue;
                            }
                            else
                            {
                                inventory.SequenceLastValue += increment;
                            }

                            inventory.SequenceLastUpdated = DateTime.UtcNow;
                            idBuilder.Append(inventory.SequenceLastValue);
                            sequenceUpdated = true; // Ensure we only increment sequence once per ID
                        }
                        break;
                }

                if (j < components.Count - 1)
                {
                    idBuilder.Append("-");
                }
            }

            // Save changes to the sequence number in the database
            if (sequenceUpdated)
            {
                await _context.SaveChangesAsync();
            }

            return idBuilder.ToString();
        }

        // This class must be defined here to be used by the JsonSerializer
        private class IdFormatComponent
        {
            public string Type { get; set; } = "";
            public string TextValue { get; set; } = "";
            public string MinValue { get; set; } = "0";
            public string MaxValue { get; set; } = "9999";
            public string StartValue { get; set; } = "1";
            public string IncrementValue { get; set; } = "1";
            public string DateFormat { get; set; } = "yyyyMMdd";
        }
    }
}