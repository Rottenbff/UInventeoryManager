using Microsoft.EntityFrameworkCore;
using InventoryManager.Models;

namespace InventoryManager.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<InventoryTransaction> InventoryTransactions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<InventoryAccess> InventoryAccesses { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<InventoryTag> InventoryTags { get; set; }
        public DbSet<AdminAccess> AdminAccesses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure unique constraint for User Email
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Configure Inventory entity for optimistic concurrency
            modelBuilder.Entity<Inventory>()
                .Property(i => i.Version)
                .IsRowVersion();

            // Configure composite unique index for Item CustomItemId
            modelBuilder.Entity<Item>()
                .HasIndex(i => new { i.InventoryId, i.CustomItemId })
                .IsUnique();

            // Configure cascade delete for Inventory -> Items
            modelBuilder.Entity<Inventory>()
                .HasMany(i => i.Items)
                .WithOne(it => it.Inventory)
                .HasForeignKey(it => it.InventoryId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure cascade delete for Inventory -> Comments
            modelBuilder.Entity<Inventory>()
                .HasMany(i => i.Comments)
                .WithOne(c => c.Inventory)
                .HasForeignKey(c => c.InventoryId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure unique constraint for Like (one like per user per item)
            modelBuilder.Entity<Like>()
                .HasIndex(l => new { l.ItemId, l.UserId })
                .IsUnique();

            // Configure unique constraint for InventoryAccess (one access per user per inventory)
            modelBuilder.Entity<InventoryAccess>()
                .HasIndex(ia => new { ia.InventoryId, ia.UserId })
                .IsUnique();

            // Configure unique constraint for Tag Name
            modelBuilder.Entity<Tag>()
                .HasIndex(t => t.Name)
                .IsUnique();

            // Configure composite unique index for InventoryTag
            modelBuilder.Entity<InventoryTag>()
                .HasIndex(it => new { it.InventoryId, it.TagId })
                .IsUnique();
        }
    }
}