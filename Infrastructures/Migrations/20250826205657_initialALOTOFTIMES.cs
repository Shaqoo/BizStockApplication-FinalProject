using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class initialALOTOFTIMES : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: new Guid("105a19f5-5805-4c1e-8feb-3058e245ba03"));

            migrationBuilder.DeleteData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: new Guid("2055356a-2fe5-41ef-b8c1-84331312768a"));

            migrationBuilder.DeleteData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: new Guid("37323767-feb4-40de-8115-ad444a9b8d7b"));

            migrationBuilder.DeleteData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: new Guid("ae0fbdbb-dcc8-46a7-84b2-8474c004061b"));

            migrationBuilder.DeleteData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: new Guid("b2132bbd-155a-4245-98e8-2dd6c9a31188"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("7f2d1792-929b-42c5-9495-db04646bd139"));

            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                table: "Reviews",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Specifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(3000)", maxLength: 3000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductSpecifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    SpecificationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSpecifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductSpecifications_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductSpecifications_Specifications_SpecificationId",
                        column: x => x.SpecificationId,
                        principalTable: "Specifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CustomerTypes",
                columns: new[] { "Id", "CreatedAt", "Description", "DiscountPercentage", "TypeName" },
                values: new object[,]
                {
                    { new Guid("06a917b4-332a-4949-a9da-b1ea1f9cf6c8"), new DateTime(2025, 8, 26, 20, 56, 56, 102, DateTimeKind.Utc).AddTicks(7007), "Wholesale buyers with bulk discounts", 5m, "Wholesale" },
                    { new Guid("25b410ab-d140-45ee-86a4-d8829cdd1862"), new DateTime(2025, 8, 26, 20, 56, 56, 102, DateTimeKind.Utc).AddTicks(7014), "Resellers who purchase for resale", 7.5m, "Reseller" },
                    { new Guid("8831d812-d998-4315-a829-1eb72e6330a9"), new DateTime(2025, 8, 26, 20, 56, 56, 102, DateTimeKind.Utc).AddTicks(6994), "Retail customers", 0m, "Retail" },
                    { new Guid("b914774a-e41f-467b-ab0e-684ed42b3f6e"), new DateTime(2025, 8, 26, 20, 56, 56, 102, DateTimeKind.Utc).AddTicks(7010), "Corporate clients with special contracts", 10m, "Corporate" },
                    { new Guid("ce07a33a-fa3b-47f3-b1b1-690927f53543"), new DateTime(2025, 8, 26, 20, 56, 56, 102, DateTimeKind.Utc).AddTicks(7027), "VIP customers with premium benefits", 15m, "VIP" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedBy", "DateCreated", "DateOfBirth", "Email", "FailedLoginAttempts", "FullName", "Gender", "HashSalt", "IsDeleted", "IsEmailVerified", "IsPhoneNumberVerified", "IsTwoFactorEnabled", "LastLoggedIn", "LastModified", "LockoutEnd", "Password", "PhoneNumber", "ProfilePictureUrl", "RefreshToken", "RefreshTokenExpiryTime", "TwoFactorSecret", "WalletId" },
                values: new object[] { new Guid("686fa6eb-3569-4c85-afa1-bc573a9743c3"), "", new DateTimeOffset(new DateTime(2025, 8, 26, 20, 56, 56, 139, DateTimeKind.Unspecified).AddTicks(6646), new TimeSpan(0, 0, 0, 0, 0)), "2000-04-22", "ShakirullahOhio@gmail.com", 0, "Shakirullah Ohio", "Male", "d08b4fb4-cdae-4841-89a7-a37d3fc19d51eb012524-180e-4127-9797-1bed34e94650", false, false, false, true, new DateTime(2025, 8, 26, 20, 56, 56, 139, DateTimeKind.Utc).AddTicks(6608), new DateTimeOffset(new DateTime(2025, 8, 26, 20, 56, 56, 139, DateTimeKind.Unspecified).AddTicks(6646), new TimeSpan(0, 0, 0, 0, 0)), null, "vd61YRFFSIHsbn15gK10i2oe7KTqb7rjYMWlxy6d0jFZF6vdjZ/4oMjHY/MQ+nPIT6U23fGaqeyXVa92W9QQupn5RSN2e6W8LTxzS1TNyeb7yfjrz0PXFOxnSs9NxV5c4Im/CFDi89WeGOOMxCxiKNdSKQoGDCVcIZacGhbqSYc=", "+2348109094694", null, "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null });

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_OrderId",
                table: "Reviews",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSpecifications_ProductId",
                table: "ProductSpecifications",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSpecifications_SpecificationId",
                table: "ProductSpecifications",
                column: "SpecificationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_SalesOrders_OrderId",
                table: "Reviews",
                column: "OrderId",
                principalTable: "SalesOrders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_SalesOrders_OrderId",
                table: "Reviews");

            migrationBuilder.DropTable(
                name: "ProductSpecifications");

            migrationBuilder.DropTable(
                name: "Specifications");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_OrderId",
                table: "Reviews");

            migrationBuilder.DeleteData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: new Guid("06a917b4-332a-4949-a9da-b1ea1f9cf6c8"));

            migrationBuilder.DeleteData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: new Guid("25b410ab-d140-45ee-86a4-d8829cdd1862"));

            migrationBuilder.DeleteData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: new Guid("8831d812-d998-4315-a829-1eb72e6330a9"));

            migrationBuilder.DeleteData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: new Guid("b914774a-e41f-467b-ab0e-684ed42b3f6e"));

            migrationBuilder.DeleteData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: new Guid("ce07a33a-fa3b-47f3-b1b1-690927f53543"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("686fa6eb-3569-4c85-afa1-bc573a9743c3"));

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Reviews");

            migrationBuilder.InsertData(
                table: "CustomerTypes",
                columns: new[] { "Id", "CreatedAt", "Description", "DiscountPercentage", "TypeName" },
                values: new object[,]
                {
                    { new Guid("105a19f5-5805-4c1e-8feb-3058e245ba03"), new DateTime(2025, 8, 23, 7, 38, 23, 978, DateTimeKind.Utc).AddTicks(5081), "Resellers who purchase for resale", 7.5m, "Reseller" },
                    { new Guid("2055356a-2fe5-41ef-b8c1-84331312768a"), new DateTime(2025, 8, 23, 7, 38, 23, 978, DateTimeKind.Utc).AddTicks(5060), "Retail customers", 0m, "Retail" },
                    { new Guid("37323767-feb4-40de-8115-ad444a9b8d7b"), new DateTime(2025, 8, 23, 7, 38, 23, 978, DateTimeKind.Utc).AddTicks(5074), "Corporate clients with special contracts", 10m, "Corporate" },
                    { new Guid("ae0fbdbb-dcc8-46a7-84b2-8474c004061b"), new DateTime(2025, 8, 23, 7, 38, 23, 978, DateTimeKind.Utc).AddTicks(5082), "VIP customers with premium benefits", 15m, "VIP" },
                    { new Guid("b2132bbd-155a-4245-98e8-2dd6c9a31188"), new DateTime(2025, 8, 23, 7, 38, 23, 978, DateTimeKind.Utc).AddTicks(5072), "Wholesale buyers with bulk discounts", 5m, "Wholesale" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedBy", "DateCreated", "DateOfBirth", "Email", "FailedLoginAttempts", "FullName", "Gender", "HashSalt", "IsDeleted", "IsEmailVerified", "IsPhoneNumberVerified", "IsTwoFactorEnabled", "LastLoggedIn", "LastModified", "LockoutEnd", "Password", "PhoneNumber", "ProfilePictureUrl", "RefreshToken", "RefreshTokenExpiryTime", "TwoFactorSecret", "WalletId" },
                values: new object[] { new Guid("7f2d1792-929b-42c5-9495-db04646bd139"), "", new DateTimeOffset(new DateTime(2025, 8, 23, 7, 38, 24, 7, DateTimeKind.Unspecified).AddTicks(2823), new TimeSpan(0, 0, 0, 0, 0)), "2000-04-22", "ShakirullahOhio@gmail.com", 0, "Shakirullah Ohio", "Male", "d08b4fb4-cdae-4841-89a7-a37d3fc19d51eb012524-180e-4127-9797-1bed34e94650", false, false, false, true, new DateTime(2025, 8, 23, 7, 38, 24, 7, DateTimeKind.Utc).AddTicks(2762), new DateTimeOffset(new DateTime(2025, 8, 23, 7, 38, 24, 7, DateTimeKind.Unspecified).AddTicks(2823), new TimeSpan(0, 0, 0, 0, 0)), null, "vd61YRFFSIHsbn15gK10i2oe7KTqb7rjYMWlxy6d0jFZF6vdjZ/4oMjHY/MQ+nPIT6U23fGaqeyXVa92W9QQupn5RSN2e6W8LTxzS1TNyeb7yfjrz0PXFOxnSs9NxV5c4Im/CFDi89WeGOOMxCxiKNdSKQoGDCVcIZacGhbqSYc=", "+2348109094694", null, "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null });
        }
    }
}
