namespace InventoryManager.Models
{
    public class TagWithCount
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public int Count { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
