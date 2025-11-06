using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class SimplifiedCustomFieldsAndIds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CustomBool1Value",
                table: "Items",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "CustomBool2Value",
                table: "Items",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "CustomBool3Value",
                table: "Items",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CustomInt1Value",
                table: "Items",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CustomInt2Value",
                table: "Items",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CustomInt3Value",
                table: "Items",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomString1Value",
                table: "Items",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomString2Value",
                table: "Items",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomString3Value",
                table: "Items",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "CustomBool1Enabled",
                table: "Inventories",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CustomBool1Name",
                table: "Inventories",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CustomBool1Order",
                table: "Inventories",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "CustomBool2Enabled",
                table: "Inventories",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CustomBool2Name",
                table: "Inventories",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CustomBool2Order",
                table: "Inventories",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "CustomBool3Enabled",
                table: "Inventories",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CustomBool3Name",
                table: "Inventories",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CustomBool3Order",
                table: "Inventories",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "CustomInt1Enabled",
                table: "Inventories",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CustomInt1Name",
                table: "Inventories",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CustomInt1Order",
                table: "Inventories",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "CustomInt2Enabled",
                table: "Inventories",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CustomInt2Name",
                table: "Inventories",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CustomInt2Order",
                table: "Inventories",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "CustomInt3Enabled",
                table: "Inventories",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CustomInt3Name",
                table: "Inventories",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CustomInt3Order",
                table: "Inventories",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "CustomString1Enabled",
                table: "Inventories",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CustomString1Name",
                table: "Inventories",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CustomString1Order",
                table: "Inventories",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "CustomString2Enabled",
                table: "Inventories",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CustomString2Name",
                table: "Inventories",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CustomString2Order",
                table: "Inventories",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "CustomString3Enabled",
                table: "Inventories",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CustomString3Name",
                table: "Inventories",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CustomString3Order",
                table: "Inventories",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomBool1Value",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CustomBool2Value",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CustomBool3Value",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CustomInt1Value",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CustomInt2Value",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CustomInt3Value",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CustomString1Value",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CustomString2Value",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CustomString3Value",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CustomBool1Enabled",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "CustomBool1Name",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "CustomBool1Order",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "CustomBool2Enabled",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "CustomBool2Name",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "CustomBool2Order",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "CustomBool3Enabled",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "CustomBool3Name",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "CustomBool3Order",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "CustomInt1Enabled",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "CustomInt1Name",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "CustomInt1Order",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "CustomInt2Enabled",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "CustomInt2Name",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "CustomInt2Order",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "CustomInt3Enabled",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "CustomInt3Name",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "CustomInt3Order",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "CustomString1Enabled",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "CustomString1Name",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "CustomString1Order",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "CustomString2Enabled",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "CustomString2Name",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "CustomString2Order",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "CustomString3Enabled",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "CustomString3Name",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "CustomString3Order",
                table: "Inventories");
        }
    }
}
