using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManager.Models
{
    public class Item
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid InventoryId { get; set; }

        [ForeignKey("InventoryId")]
        public virtual Inventory? Inventory { get; set; }

        [StringLength(100)]
        public string? CustomItemId { get; set; }

        // Simplified Custom Field Values - String Types
        [StringLength(500)]
        public string? CustomString1Value { get; set; }

        [StringLength(500)]
        public string? CustomString2Value { get; set; }

        [StringLength(500)]
        public string? CustomString3Value { get; set; }

        // Simplified Custom Field Values - Integer Types
        public int? CustomInt1Value { get; set; }

        public int? CustomInt2Value { get; set; }

        public int? CustomInt3Value { get; set; }

        // Simplified Custom Field Values - Boolean Types
        public bool? CustomBool1Value { get; set; }

        public bool? CustomBool2Value { get; set; }

        public bool? CustomBool3Value { get; set; }

        [StringLength(500)]
        public string? ImageUrl { get; set; }

        public virtual ICollection<Like> Likes { get; set; } = new List<Like>();
    }
}