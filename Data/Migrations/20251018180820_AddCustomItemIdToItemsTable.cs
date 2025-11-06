using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomItemIdToItemsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomItemId",
                table: "Items",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_InventoryId_CustomItemId",
                table: "Items",
                columns: new[] { "InventoryId", "CustomItemId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Items_InventoryId_CustomItemId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CustomItemId",
                table: "Items");
        }
    }
}