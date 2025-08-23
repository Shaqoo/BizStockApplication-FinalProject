using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class SearchProduct2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseItems_Products_ProductId1",
                table: "WarehouseItems");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseItems_ProductId1",
                table: "WarehouseItems");

            migrationBuilder.DeleteData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: new Guid("3a2d513e-78bd-4387-a6fc-7f4f0df776e6"));

            migrationBuilder.DeleteData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: new Guid("7a70b7c2-715d-4a43-9e2d-90f7b3a98c71"));

            migrationBuilder.DeleteData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: new Guid("88a5c1b5-7d69-403d-b656-e38c9511d2dc"));

            migrationBuilder.DeleteData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: new Guid("a84de811-de55-4f2e-b755-b4e846b2331a"));

            migrationBuilder.DeleteData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: new Guid("c2e98782-d4af-4b6a-99ac-58efc9b3b805"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("207e1764-c510-4f0e-b5e6-dc9283d3911c"));

            migrationBuilder.DropColumn(
                name: "ProductId1",
                table: "WarehouseItems");

            migrationBuilder.InsertData(
                table: "CustomerTypes",
                columns: new[] { "Id", "CreatedAt", "Description", "DiscountPercentage", "TypeName" },
                values: new object[,]
                {
                    { new Guid("329bde4f-e33f-4959-8bba-a96ff2f348ac"), new DateTime(2025, 8, 19, 4, 19, 47, 6, DateTimeKind.Utc).AddTicks(9328), "Corporate clients with special contracts", 10m, "Corporate" },
                    { new Guid("374c3a59-9319-475a-8856-adcb1b6241f6"), new DateTime(2025, 8, 19, 4, 19, 47, 6, DateTimeKind.Utc).AddTicks(9326), "Wholesale buyers with bulk discounts", 5m, "Wholesale" },
                    { new Guid("70136f73-7d1a-4777-b7b2-7854ae1cb10a"), new DateTime(2025, 8, 19, 4, 19, 47, 6, DateTimeKind.Utc).AddTicks(9331), "Resellers who purchase for resale", 7.5m, "Reseller" },
                    { new Guid("e68d78bf-296e-409b-ab2c-ac92418763ec"), new DateTime(2025, 8, 19, 4, 19, 47, 6, DateTimeKind.Utc).AddTicks(9317), "Retail customers", 0m, "Retail" },
                    { new Guid("ec6a37ea-eb87-4cca-a248-839fba1d1a9b"), new DateTime(2025, 8, 19, 4, 19, 47, 6, DateTimeKind.Utc).AddTicks(9333), "VIP customers with premium benefits", 15m, "VIP" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedBy", "DateCreated", "DateOfBirth", "Email", "FailedLoginAttempts", "FullName", "Gender", "HashSalt", "IsDeleted", "IsEmailVerified", "IsPhoneNumberVerified", "IsTwoFactorEnabled", "LastLoggedIn", "LastModified", "LockoutEnd", "Password", "PhoneNumber", "ProfilePictureUrl", "RefreshToken", "RefreshTokenExpiryTime", "TwoFactorSecret", "WalletId" },
                values: new object[] { new Guid("d6340845-c968-4d65-97be-f5134cc50975"), "", new DateTimeOffset(new DateTime(2025, 8, 19, 4, 19, 47, 31, DateTimeKind.Unspecified).AddTicks(7276), new TimeSpan(0, 0, 0, 0, 0)), "2000-04-22", "ShakirullahOhio@gmail.com", 0, "Shakirullah Ohio", "Male", "d08b4fb4-cdae-4841-89a7-a37d3fc19d51eb012524-180e-4127-9797-1bed34e94650", false, false, false, true, new DateTime(2025, 8, 19, 4, 19, 47, 31, DateTimeKind.Utc).AddTicks(7243), new DateTimeOffset(new DateTime(2025, 8, 19, 4, 19, 47, 31, DateTimeKind.Unspecified).AddTicks(7276), new TimeSpan(0, 0, 0, 0, 0)), null, "vd61YRFFSIHsbn15gK10i2oe7KTqb7rjYMWlxy6d0jFZF6vdjZ/4oMjHY/MQ+nPIT6U23fGaqeyXVa92W9QQupn5RSN2e6W8LTxzS1TNyeb7yfjrz0PXFOxnSs9NxV5c4Im/CFDi89WeGOOMxCxiKNdSKQoGDCVcIZacGhbqSYc=", "+2348109094694", null, "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: new Guid("329bde4f-e33f-4959-8bba-a96ff2f348ac"));

            migrationBuilder.DeleteData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: new Guid("374c3a59-9319-475a-8856-adcb1b6241f6"));

            migrationBuilder.DeleteData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: new Guid("70136f73-7d1a-4777-b7b2-7854ae1cb10a"));

            migrationBuilder.DeleteData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: new Guid("e68d78bf-296e-409b-ab2c-ac92418763ec"));

            migrationBuilder.DeleteData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: new Guid("ec6a37ea-eb87-4cca-a248-839fba1d1a9b"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d6340845-c968-4d65-97be-f5134cc50975"));

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId1",
                table: "WarehouseItems",
                type: "uuid",
                nullable: true);

            migrationBuilder.InsertData(
                table: "CustomerTypes",
                columns: new[] { "Id", "CreatedAt", "Description", "DiscountPercentage", "TypeName" },
                values: new object[,]
                {
                    { new Guid("3a2d513e-78bd-4387-a6fc-7f4f0df776e6"), new DateTime(2025, 8, 19, 4, 18, 23, 627, DateTimeKind.Utc).AddTicks(9519), "VIP customers with premium benefits", 15m, "VIP" },
                    { new Guid("7a70b7c2-715d-4a43-9e2d-90f7b3a98c71"), new DateTime(2025, 8, 19, 4, 18, 23, 627, DateTimeKind.Utc).AddTicks(9514), "Corporate clients with special contracts", 10m, "Corporate" },
                    { new Guid("88a5c1b5-7d69-403d-b656-e38c9511d2dc"), new DateTime(2025, 8, 19, 4, 18, 23, 627, DateTimeKind.Utc).AddTicks(9518), "Resellers who purchase for resale", 7.5m, "Reseller" },
                    { new Guid("a84de811-de55-4f2e-b755-b4e846b2331a"), new DateTime(2025, 8, 19, 4, 18, 23, 627, DateTimeKind.Utc).AddTicks(9512), "Wholesale buyers with bulk discounts", 5m, "Wholesale" },
                    { new Guid("c2e98782-d4af-4b6a-99ac-58efc9b3b805"), new DateTime(2025, 8, 19, 4, 18, 23, 627, DateTimeKind.Utc).AddTicks(9505), "Retail customers", 0m, "Retail" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedBy", "DateCreated", "DateOfBirth", "Email", "FailedLoginAttempts", "FullName", "Gender", "HashSalt", "IsDeleted", "IsEmailVerified", "IsPhoneNumberVerified", "IsTwoFactorEnabled", "LastLoggedIn", "LastModified", "LockoutEnd", "Password", "PhoneNumber", "ProfilePictureUrl", "RefreshToken", "RefreshTokenExpiryTime", "TwoFactorSecret", "WalletId" },
                values: new object[] { new Guid("207e1764-c510-4f0e-b5e6-dc9283d3911c"), "", new DateTimeOffset(new DateTime(2025, 8, 19, 4, 18, 23, 649, DateTimeKind.Unspecified).AddTicks(9006), new TimeSpan(0, 0, 0, 0, 0)), "2000-04-22", "ShakirullahOhio@gmail.com", 0, "Shakirullah Ohio", "Male", "d08b4fb4-cdae-4841-89a7-a37d3fc19d51eb012524-180e-4127-9797-1bed34e94650", false, false, false, true, new DateTime(2025, 8, 19, 4, 18, 23, 649, DateTimeKind.Utc).AddTicks(8965), new DateTimeOffset(new DateTime(2025, 8, 19, 4, 18, 23, 649, DateTimeKind.Unspecified).AddTicks(9006), new TimeSpan(0, 0, 0, 0, 0)), null, "vd61YRFFSIHsbn15gK10i2oe7KTqb7rjYMWlxy6d0jFZF6vdjZ/4oMjHY/MQ+nPIT6U23fGaqeyXVa92W9QQupn5RSN2e6W8LTxzS1TNyeb7yfjrz0PXFOxnSs9NxV5c4Im/CFDi89WeGOOMxCxiKNdSKQoGDCVcIZacGhbqSYc=", "+2348109094694", null, "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null });

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseItems_ProductId1",
                table: "WarehouseItems",
                column: "ProductId1");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseItems_Products_ProductId1",
                table: "WarehouseItems",
                column: "ProductId1",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
