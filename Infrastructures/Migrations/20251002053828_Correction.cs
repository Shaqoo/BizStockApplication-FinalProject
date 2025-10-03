using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class Correction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Weight",
                table: "Products",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "RecipientEmail",
                table: "DeliveryAssignments",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecipientName",
                table: "DeliveryAssignments",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RecipientPhone",
                table: "DeliveryAssignments",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "RecipientEmail",
                table: "DeliveryAssignments");

            migrationBuilder.DropColumn(
                name: "RecipientName",
                table: "DeliveryAssignments");

            migrationBuilder.DropColumn(
                name: "RecipientPhone",
                table: "DeliveryAssignments");
        }
    }
}
