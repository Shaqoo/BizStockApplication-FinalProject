using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class AgainCartAndItemsUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.InsertData(
                table: "CustomerTypes",
                columns: new[] { "Id", "CreatedAt", "Description", "DiscountPercentage", "TypeName" },
                values: new object[,]
                {
                    { new Guid("638e1a75-e650-4726-83fb-96f110670304"), new DateTime(2025, 8, 31, 2, 56, 58, 863, DateTimeKind.Utc).AddTicks(7486), "Resellers who purchase for resale", 7.5m, "Reseller" },
                    { new Guid("a123dad2-14ef-48f5-8ec1-26cb32f7ce47"), new DateTime(2025, 8, 31, 2, 56, 58, 863, DateTimeKind.Utc).AddTicks(7478), "Corporate clients with special contracts", 10m, "Corporate" },
                    { new Guid("d83e4de8-bc16-4956-97f2-16215835e6da"), new DateTime(2025, 8, 31, 2, 56, 58, 863, DateTimeKind.Utc).AddTicks(7458), "Retail customers", 0m, "Retail" },
                    { new Guid("fa325971-52c1-4d10-9dd0-dca0a7a1c910"), new DateTime(2025, 8, 31, 2, 56, 58, 863, DateTimeKind.Utc).AddTicks(7471), "Wholesale buyers with bulk discounts", 5m, "Wholesale" },
                    { new Guid("ff6ae7e6-1f04-46fc-86ea-9485fad482f7"), new DateTime(2025, 8, 31, 2, 56, 58, 863, DateTimeKind.Utc).AddTicks(7493), "VIP customers with premium benefits", 15m, "VIP" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedBy", "DateCreated", "DateOfBirth", "Email", "FailedLoginAttempts", "FullName", "Gender", "HashSalt", "IsDeleted", "IsEmailVerified", "IsPhoneNumberVerified", "IsTwoFactorEnabled", "LastLoggedIn", "LastModified", "LockoutEnd", "Password", "PhoneNumber", "ProfilePictureUrl", "RefreshToken", "RefreshTokenExpiryTime", "TwoFactorSecret", "WalletId" },
                values: new object[] { new Guid("e40ed7a0-0371-4534-948d-a00bcf13cc43"), "", new DateTimeOffset(new DateTime(2025, 8, 31, 2, 56, 58, 918, DateTimeKind.Unspecified).AddTicks(256), new TimeSpan(0, 0, 0, 0, 0)), "2000-04-22", "ShakirullahOhio@gmail.com", 0, "Shakirullah Ohio", "Male", "d08b4fb4-cdae-4841-89a7-a37d3fc19d51eb012524-180e-4127-9797-1bed34e94650", false, false, false, true, new DateTime(2025, 8, 31, 2, 56, 58, 918, DateTimeKind.Utc).AddTicks(220), new DateTimeOffset(new DateTime(2025, 8, 31, 2, 56, 58, 918, DateTimeKind.Unspecified).AddTicks(256), new TimeSpan(0, 0, 0, 0, 0)), null, "vd61YRFFSIHsbn15gK10i2oe7KTqb7rjYMWlxy6d0jFZF6vdjZ/4oMjHY/MQ+nPIT6U23fGaqeyXVa92W9QQupn5RSN2e6W8LTxzS1TNyeb7yfjrz0PXFOxnSs9NxV5c4Im/CFDi89WeGOOMxCxiKNdSKQoGDCVcIZacGhbqSYc=", "+2348109094694", null, "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: new Guid("638e1a75-e650-4726-83fb-96f110670304"));

            migrationBuilder.DeleteData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: new Guid("a123dad2-14ef-48f5-8ec1-26cb32f7ce47"));

            migrationBuilder.DeleteData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: new Guid("d83e4de8-bc16-4956-97f2-16215835e6da"));

            migrationBuilder.DeleteData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: new Guid("fa325971-52c1-4d10-9dd0-dca0a7a1c910"));

            migrationBuilder.DeleteData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: new Guid("ff6ae7e6-1f04-46fc-86ea-9485fad482f7"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("e40ed7a0-0371-4534-948d-a00bcf13cc43"));

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
        }
    }
}
