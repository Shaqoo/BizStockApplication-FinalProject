using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class RefundAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeliveryStatus",
                table: "SalesOrderItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "FezOrderNo",
                table: "SalesOrderItems",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UniqueId",
                table: "SalesOrderItems",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeliveryAddressId",
                table: "DeliveryAssignments",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Refunds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SalesOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    PaymentMethod = table.Column<int>(type: "integer", nullable: false),
                    TransactionReference = table.Column<string>(type: "text", nullable: false),
                    RefundReference = table.Column<string>(type: "text", nullable: false),
                    RequestedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: false),
                    DateCreated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Refunds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Refunds_SalesOrders_SalesOrderId",
                        column: x => x.SalesOrderId,
                        principalTable: "SalesOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryAssignments_DeliveryAddressId",
                table: "DeliveryAssignments",
                column: "DeliveryAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Refunds_SalesOrderId",
                table: "Refunds",
                column: "SalesOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryAssignments_DeliveryAddresses_DeliveryAddressId",
                table: "DeliveryAssignments",
                column: "DeliveryAddressId",
                principalTable: "DeliveryAddresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryAssignments_DeliveryAddresses_DeliveryAddressId",
                table: "DeliveryAssignments");

            migrationBuilder.DropTable(
                name: "Refunds");

            migrationBuilder.DropIndex(
                name: "IX_DeliveryAssignments_DeliveryAddressId",
                table: "DeliveryAssignments");

            migrationBuilder.DropColumn(
                name: "DeliveryStatus",
                table: "SalesOrderItems");

            migrationBuilder.DropColumn(
                name: "FezOrderNo",
                table: "SalesOrderItems");

            migrationBuilder.DropColumn(
                name: "UniqueId",
                table: "SalesOrderItems");

            migrationBuilder.DropColumn(
                name: "DeliveryAddressId",
                table: "DeliveryAssignments");
        }
    }
}
