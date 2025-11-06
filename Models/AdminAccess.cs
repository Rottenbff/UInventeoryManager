using System.ComponentModel.DataAnnotations;

namespace InventoryManager.Models
{
    public class AdminAccess
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string GeneratedLink { get; set; } = string.Empty;

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public string UserEmail { get; set; } = string.Empty;

        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UsedAt { get; set; }

        public bool IsUsed { get; set; } = false;

        public string? UsedByUserId { get; set; }

        public string? UsedByEmail { get; set; }
    }
}
