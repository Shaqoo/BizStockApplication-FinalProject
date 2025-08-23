using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class ThirdType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("7eae15b6-1826-4fd8-8cde-eac9a96cd6b1"));

            migrationBuilder.InsertData(
                table: "CustomerTypes",
                columns: new[] { "Id", "CreatedAt", "Description", "DiscountPercentage", "TypeName" },
                values: new object[,]
                {
                    { new Guid("1ce20b61-f3ab-48c5-a5cc-857244d350b0"), new DateTime(2025, 8, 13, 7, 54, 0, 966, DateTimeKind.Utc).AddTicks(7519), "Resellers who purchase for resale", 7.5m, "Reseller" },
                    { new Guid("2032b433-aeac-439b-86d0-382ce89c7ebb"), new DateTime(2025, 8, 13, 7, 54, 0, 966, DateTimeKind.Utc).AddTicks(7491), "Retail customers", 0m, "Retail" },
                    { new Guid("4e70f1a0-d39a-4220-b6f2-5354d52870d8"), new DateTime(2025, 8, 13, 7, 54, 0, 966, DateTimeKind.Utc).AddTicks(7521), "VIP customers with premium benefits", 15m, "VIP" },
                    { new Guid("51bf060f-9f64-4409-9189-2b37493992aa"), new DateTime(2025, 8, 13, 7, 54, 0, 966, DateTimeKind.Utc).AddTicks(7505), "Corporate clients with special contracts", 10m, "Corporate" },
                    { new Guid("e30d4a7b-95b3-4104-8ece-7ffcccb54b16"), new DateTime(2025, 8, 13, 7, 54, 0, 966, DateTimeKind.Utc).AddTicks(7504), "Wholesale buyers with bulk discounts", 5m, "Wholesale" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedBy", "DateCreated", "DateOfBirth", "Email", "FailedLoginAttempts", "FullName", "Gender", "HashSalt", "IsDeleted", "IsEmailVerified", "IsPhoneNumberVerified", "LastLoggedIn", "LastModified", "LockoutEnd", "Password", "PhoneNumber", "ProfilePictureUrl", "RefreshToken", "RefreshTokenExpiryTime", "TwoFactorSecret", "WalletId" },
                values: new object[] { new Guid("32ebf32e-18f6-45ed-9303-ac703beb4a45"), "", new DateTimeOffset(new DateTime(2025, 8, 13, 8, 54, 1, 3, DateTimeKind.Unspecified).AddTicks(2328), new TimeSpan(0, 1, 0, 0, 0)), "2000-04-22", "ShakirullahOhio@gmail.com", 0, "Shakirullah Ohio", "Male", "d08b4fb4-cdae-4841-89a7-a37d3fc19d51eb012524-180e-4127-9797-1bed34e94650", false, false, false, new DateTime(2025, 8, 13, 7, 54, 1, 3, DateTimeKind.Utc).AddTicks(2274), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "vd61YRFFSIHsbn15gK10i2oe7KTqb7rjYMWlxy6d0jFZF6vdjZ/4oMjHY/MQ+nPIT6U23fGaqeyXVa92W9QQupn5RSN2e6W8LTxzS1TNyeb7yfjrz0PXFOxnSs9NxV5c4Im/CFDi89WeGOOMxCxiKNdSKQoGDCVcIZacGhbqSYc=", "+2348109094694", null, "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: new Guid("1ce20b61-f3ab-48c5-a5cc-857244d350b0"));

            migrationBuilder.DeleteData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: new Guid("2032b433-aeac-439b-86d0-382ce89c7ebb"));

            migrationBuilder.DeleteData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: new Guid("4e70f1a0-d39a-4220-b6f2-5354d52870d8"));

            migrationBuilder.DeleteData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: new Guid("51bf060f-9f64-4409-9189-2b37493992aa"));

            migrationBuilder.DeleteData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: new Guid("e30d4a7b-95b3-4104-8ece-7ffcccb54b16"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("32ebf32e-18f6-45ed-9303-ac703beb4a45"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedBy", "DateCreated", "DateOfBirth", "Email", "FailedLoginAttempts", "FullName", "Gender", "HashSalt", "IsDeleted", "IsEmailVerified", "IsPhoneNumberVerified", "LastLoggedIn", "LastModified", "LockoutEnd", "Password", "PhoneNumber", "ProfilePictureUrl", "RefreshToken", "RefreshTokenExpiryTime", "TwoFactorSecret", "WalletId" },
                values: new object[] { new Guid("7eae15b6-1826-4fd8-8cde-eac9a96cd6b1"), "", new DateTimeOffset(new DateTime(2025, 8, 13, 8, 26, 22, 366, DateTimeKind.Unspecified).AddTicks(693), new TimeSpan(0, 1, 0, 0, 0)), "2000-04-22", "ShakirullahOhio@gmail.com", 0, "Shakirullah Ohio", "Male", "d08b4fb4-cdae-4841-89a7-a37d3fc19d51eb012524-180e-4127-9797-1bed34e94650", false, false, false, new DateTime(2025, 8, 13, 7, 26, 22, 366, DateTimeKind.Utc).AddTicks(659), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "vd61YRFFSIHsbn15gK10i2oe7KTqb7rjYMWlxy6d0jFZF6vdjZ/4oMjHY/MQ+nPIT6U23fGaqeyXVa92W9QQupn5RSN2e6W8LTxzS1TNyeb7yfjrz0PXFOxnSs9NxV5c4Im/CFDi89WeGOOMxCxiKNdSKQoGDCVcIZacGhbqSYc=", "+2348109094694", null, "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null });
        }
    }
}
