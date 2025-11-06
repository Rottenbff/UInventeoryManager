using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManager.Models
{
    public class InventoryTag
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid InventoryId { get; set; }

        [ForeignKey("InventoryId")]
        public virtual Inventory? Inventory { get; set; }

        [Required]
        public Guid TagId { get; set; }

        [ForeignKey("TagId")]
        public virtual Tag? Tag { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}