using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIdFormatFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdFormatComponents",
                table: "Inventories",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IdFormatEnabled",
                table: "Inventories",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "IdFormatSettings",
                table: "Inventories",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdFormatComponents",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "IdFormatEnabled",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "IdFormatSettings",
                table: "Inventories");
        }
    }
}
