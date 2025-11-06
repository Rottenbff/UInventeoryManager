using System.ComponentModel.DataAnnotations;

namespace InventoryManager.Models
{
    public enum TransactionType
    {
        Purchase,
        Sale,
        Adjustment
    }

    public class InventoryTransaction
    {
        public int Id { get; set; }

        [Required]
        public Guid ItemId { get; set; }
        public Item? Item { get; set; }

        [Required]
        [Range(1, 10000)]
        public int Quantity { get; set; }

        [Required]
        public TransactionType Type { get; set; }
        
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
        
        [StringLength(500)]
        public string? Notes { get; set; }
    }
}