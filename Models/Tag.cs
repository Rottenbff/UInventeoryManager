using System;
using System.ComponentModel.DataAnnotations;

namespace InventoryManager.Models
{
    public class Tag
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public required string Name { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}