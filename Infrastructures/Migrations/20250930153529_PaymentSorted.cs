using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class PaymentSorted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Users_PayerId",
                table: "Payments");

            migrationBuilder.AddColumn<Guid>(
                name: "PaymentId",
                table: "WalletTransactions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "InvoiceId",
                table: "Payments",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<int>(
                name: "Purpose",
                table: "Payments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Payments",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExternalDeliveryService",
                table: "DeliveryAssignments",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExternalJobId",
                table: "DeliveryAssignments",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsExternal",
                table: "DeliveryAssignments",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "DeliveryStationId",
                table: "DeliveryAddresses",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DeliveryStation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    StateId = table.Column<int>(type: "integer", nullable: false),
                    LgaId = table.Column<int>(type: "integer", nullable: false),
                    BaseFee = table.Column<decimal>(type: "numeric", nullable: false),
                    FeePerKm = table.Column<decimal>(type: "numeric", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    DateCreated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryStation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeliveryStation_Lgas_LgaId",
                        column: x => x.LgaId,
                        principalTable: "Lgas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeliveryStation_States_StateId",
                        column: x => x.StateId,
                        principalTable: "States",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WalletTransactions_PaymentId",
                table: "WalletTransactions",
                column: "PaymentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_UserId",
                table: "Payments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryAddresses_DeliveryStationId",
                table: "DeliveryAddresses",
                column: "DeliveryStationId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryStation_LgaId",
                table: "DeliveryStation",
                column: "LgaId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryStation_StateId",
                table: "DeliveryStation",
                column: "StateId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryAddresses_DeliveryStation_DeliveryStationId",
                table: "DeliveryAddresses",
                column: "DeliveryStationId",
                principalTable: "DeliveryStation",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Customers_PayerId",
                table: "Payments",
                column: "PayerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Users_UserId",
                table: "Payments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WalletTransactions_Payments_PaymentId",
                table: "WalletTransactions",
                column: "PaymentId",
                principalTable: "Payments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryAddresses_DeliveryStation_DeliveryStationId",
                table: "DeliveryAddresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Customers_PayerId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Users_UserId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_WalletTransactions_Payments_PaymentId",
                table: "WalletTransactions");

            migrationBuilder.DropTable(
                name: "DeliveryStation");

            migrationBuilder.DropIndex(
                name: "IX_WalletTransactions_PaymentId",
                table: "WalletTransactions");

            migrationBuilder.DropIndex(
                name: "IX_Payments_UserId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_DeliveryAddresses_DeliveryStationId",
                table: "DeliveryAddresses");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "WalletTransactions");

            migrationBuilder.DropColumn(
                name: "Purpose",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ExternalDeliveryService",
                table: "DeliveryAssignments");

            migrationBuilder.DropColumn(
                name: "ExternalJobId",
                table: "DeliveryAssignments");

            migrationBuilder.DropColumn(
                name: "IsExternal",
                table: "DeliveryAssignments");

            migrationBuilder.DropColumn(
                name: "DeliveryStationId",
                table: "DeliveryAddresses");

            migrationBuilder.AlterColumn<Guid>(
                name: "InvoiceId",
                table: "Payments",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Users_PayerId",
                table: "Payments",
                column: "PayerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
