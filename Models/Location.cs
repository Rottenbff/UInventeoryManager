using System.ComponentModel.DataAnnotations;

namespace InventoryManager.Models
{
    public class Location
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [StringLength(200)]
        public string? Address { get; set; }
    }
}