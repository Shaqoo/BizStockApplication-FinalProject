using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class deliveryAdressChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdditionalPhoneNumber",
                table: "DeliveryAddresses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "DeliveryAddresses",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "DeliveryAddresses",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "DeliveryAddresses",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalPhoneNumber",
                table: "DeliveryAddresses");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "DeliveryAddresses");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "DeliveryAddresses");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "DeliveryAddresses");
        }
    }
}
