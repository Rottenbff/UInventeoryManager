using System;
using System.ComponentModel.DataAnnotations;

namespace InventoryManager.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        public string? Name { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsBlocked { get; set; }

        public string? Provider { get; set; }

        public string? ProviderId { get; set; }
    }
}