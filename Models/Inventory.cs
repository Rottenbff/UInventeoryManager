using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManager.Models
{
    public class Inventory
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string Title { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        [StringLength(50)]
        public string? Category { get; set; }

        [Required]
        public Guid CreatorId { get; set; }

        [ForeignKey("CreatorId")]
        public virtual User? Creator { get; set; }

        [Timestamp]
        public byte[]? Version { get; set; }

        [StringLength(500)]
        public string? ImageUrl { get; set; }

        // Simplified Custom Fields - String Types
        public bool CustomString1Enabled { get; set; }
        [StringLength(50)]
        public string? CustomString1Name { get; set; }
        public int CustomString1Order { get; set; }

        public bool CustomString2Enabled { get; set; }
        [StringLength(50)]
        public string? CustomString2Name { get; set; }
        public int CustomString2Order { get; set; }

        public bool CustomString3Enabled { get; set; }
        [StringLength(50)]
        public string? CustomString3Name { get; set; }
        public int CustomString3Order { get; set; }

        // Simplified Custom Fields - Integer Types
        public bool CustomInt1Enabled { get; set; }
        [StringLength(50)]
        public string? CustomInt1Name { get; set; }
        public int CustomInt1Order { get; set; }

        public bool CustomInt2Enabled { get; set; }
        [StringLength(50)]
        public string? CustomInt2Name { get; set; }
        public int CustomInt2Order { get; set; }

        public bool CustomInt3Enabled { get; set; }
        [StringLength(50)]
        public string? CustomInt3Name { get; set; }
        public int CustomInt3Order { get; set; }

        // Simplified Custom Fields - Boolean Types
        public bool CustomBool1Enabled { get; set; }
        [StringLength(50)]
        public string? CustomBool1Name { get; set; }
        public int CustomBool1Order { get; set; }

        public bool CustomBool2Enabled { get; set; }
        [StringLength(50)]
        public string? CustomBool2Name { get; set; }
        public int CustomBool2Order { get; set; }

        public bool CustomBool3Enabled { get; set; }
        [StringLength(50)]
        public string? CustomBool3Name { get; set; }
        public int CustomBool3Order { get; set; }

        // ID Format Configuration
        public bool IdFormatEnabled { get; set; }
        [StringLength(500)]
        public string? IdFormatComponents { get; set; }
        [StringLength(1000)]
        public string? IdFormatSettings { get; set; }

        // Sequence Number State
        public long SequenceLastValue { get; set; }
        public DateTime SequenceLastUpdated { get; set; }

        public virtual ICollection<Item> Items { get; set; } = new List<Item>();
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public virtual ICollection<InventoryTag> InventoryTags { get; set; } = new List<InventoryTag>();
    }
}