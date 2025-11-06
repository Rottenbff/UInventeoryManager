using InventoryManager.Data;
using InventoryManager.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManager.Services
{
    public class TagService : ITagService
    {
        private readonly ApplicationDbContext _context;

        public TagService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Tag>> GetAllTagsAsync()
        {
            return await _context.Tags.ToListAsync();
        }

        public async Task<List<Tag>> GetTagsForInventoryAsync(Guid inventoryId)
        {
            return await _context.InventoryTags
                .Where(it => it.InventoryId == inventoryId)
                .Select(it => it.Tag!)
                .Where(t => t != null)
                .ToListAsync();
        }

        public async Task<bool> AddTagToInventoryAsync(Guid inventoryId, string tagName)
        {
            // Check if tag exists, create if not
            var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Name.ToLower() == tagName.ToLower());
            if (tag == null)
            {
                tag = new Tag
                {
                    Id = Guid.NewGuid(),
                    Name = tagName
                };
                _context.Tags.Add(tag);
            }

            // Check if inventory-tag relationship already exists
            var existingRelationship = await _context.InventoryTags
                .FirstOrDefaultAsync(it => it.InventoryId == inventoryId && it.TagId == tag.Id);

            if (existingRelationship != null)
                return false; // Tag already assigned to inventory

            // Create inventory-tag relationship
            var inventoryTag = new InventoryTag
            {
                Id = Guid.NewGuid(),
                InventoryId = inventoryId,
                TagId = tag.Id
            };

            _context.InventoryTags.Add(inventoryTag);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveTagFromInventoryAsync(Guid inventoryId, Guid tagId)
        {
            var inventoryTag = await _context.InventoryTags
                .FirstOrDefaultAsync(it => it.InventoryId == inventoryId && it.TagId == tagId);

            if (inventoryTag == null)
                return false; // Tag not assigned to inventory

            _context.InventoryTags.Remove(inventoryTag);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<TagWithCount>> GetPopularTagsAsync(int count)
        {
            var results = await _context.InventoryTags
                .GroupBy(it => it.TagId)
                .Select(g => new { TagId = g.Key, TagName = g.FirstOrDefault()!.Tag.Name, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .Take(count)
                .ToListAsync();

            return results.Select(r => new TagWithCount
            {
                Id = r.TagId,
                Name = r.TagName,
                Count = r.Count
            }).ToList();
        }
    }
}