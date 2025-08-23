using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class productdescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: new Guid("0fe4d251-5d43-4c58-80d9-809cfe52a866"));

            migrationBuilder.DeleteData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: new Guid("11fa4de5-d632-4a09-b3db-aa6f268a6408"));

            migrationBuilder.DeleteData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: new Guid("27a610ad-5e20-401f-babf-728fcfe5f84a"));

            migrationBuilder.DeleteData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: new Guid("b1b18b15-294e-4755-8613-3db97edb929b"));

            migrationBuilder.DeleteData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: new Guid("be582801-87d0-4c43-aa58-64f6ac606a86"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("1f08f86b-66b9-4f16-bdc9-762a177ceb5e"));

            migrationBuilder.InsertData(
                table: "CustomerTypes",
                columns: new[] { "Id", "CreatedAt", "Description", "DiscountPercentage", "TypeName" },
                values: new object[,]
                {
                    { new Guid("1bf0fe3a-016d-4234-b041-9f6134dacfae"), new DateTime(2025, 8, 23, 7, 37, 25, 876, DateTimeKind.Utc).AddTicks(7349), "Resellers who purchase for resale", 7.5m, "Reseller" },
                    { new Guid("5144ae08-9e2a-4344-8aca-9a58616e5881"), new DateTime(2025, 8, 23, 7, 37, 25, 876, DateTimeKind.Utc).AddTicks(7352), "VIP customers with premium benefits", 15m, "VIP" },
                    { new Guid("5c2ba6fe-0a50-4def-af54-7060cb624e15"), new DateTime(2025, 8, 23, 7, 37, 25, 876, DateTimeKind.Utc).AddTicks(7344), "Corporate clients with special contracts", 10m, "Corporate" },
                    { new Guid("c8983d6f-4010-4fa0-9bb5-04dae4cc4bab"), new DateTime(2025, 8, 23, 7, 37, 25, 876, DateTimeKind.Utc).AddTicks(7341), "Wholesale buyers with bulk discounts", 5m, "Wholesale" },
                    { new Guid("d44f360b-d8e9-492f-8840-10b67da479e0"), new DateTime(2025, 8, 23, 7, 37, 25, 876, DateTimeKind.Utc).AddTicks(7327), "Retail customers", 0m, "Retail" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedBy", "DateCreated", "DateOfBirth", "Email", "FailedLoginAttempts", "FullName", "Gender", "HashSalt", "IsDeleted", "IsEmailVerified", "IsPhoneNumberVerified", "IsTwoFactorEnabled", "LastLoggedIn", "LastModified", "LockoutEnd", "Password", "PhoneNumber", "ProfilePictureUrl", "RefreshToken", "RefreshTokenExpiryTime", "TwoFactorSecret", "WalletId" },
                values: new object[] { new Guid("c46db72b-88be-407f-90ed-7ec45f3a911a"), "", new DateTimeOffset(new DateTime(2025, 8, 23, 7, 37, 25, 908, DateTimeKind.Unspecified).AddTicks(314), new TimeSpan(0, 0, 0, 0, 0)), "2000-04-22", "ShakirullahOhio@gmail.com", 0, "Shakirullah Ohio", "Male", "d08b4fb4-cdae-4841-89a7-a37d3fc19d51eb012524-180e-4127-9797-1bed34e94650", false, false, false, true, new DateTime(2025, 8, 23, 7, 37, 25, 908, DateTimeKind.Utc).AddTicks(270), new DateTimeOffset(new DateTime(2025, 8, 23, 7, 37, 25, 908, DateTimeKind.Unspecified).AddTicks(315), new TimeSpan(0, 0, 0, 0, 0)), null, "vd61YRFFSIHsbn15gK10i2oe7KTqb7rjYMWlxy6d0jFZF6vdjZ/4oMjHY/MQ+nPIT6U23fGaqeyXVa92W9QQupn5RSN2e6W8LTxzS1TNyeb7yfjrz0PXFOxnSs9NxV5c4Im/CFDi89WeGOOMxCxiKNdSKQoGDCVcIZacGhbqSYc=", "+2348109094694", null, "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: new Guid("1bf0fe3a-016d-4234-b041-9f6134dacfae"));

            migrationBuilder.DeleteData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: new Guid("5144ae08-9e2a-4344-8aca-9a58616e5881"));

            migrationBuilder.DeleteData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: new Guid("5c2ba6fe-0a50-4def-af54-7060cb624e15"));

            migrationBuilder.DeleteData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: new Guid("c8983d6f-4010-4fa0-9bb5-04dae4cc4bab"));

            migrationBuilder.DeleteData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: new Guid("d44f360b-d8e9-492f-8840-10b67da479e0"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("c46db72b-88be-407f-90ed-7ec45f3a911a"));

            migrationBuilder.InsertData(
                table: "CustomerTypes",
                columns: new[] { "Id", "CreatedAt", "Description", "DiscountPercentage", "TypeName" },
                values: new object[,]
                {
                    { new Guid("0fe4d251-5d43-4c58-80d9-809cfe52a866"), new DateTime(2025, 8, 23, 7, 30, 10, 865, DateTimeKind.Utc).AddTicks(8711), "Resellers who purchase for resale", 7.5m, "Reseller" },
                    { new Guid("11fa4de5-d632-4a09-b3db-aa6f268a6408"), new DateTime(2025, 8, 23, 7, 30, 10, 865, DateTimeKind.Utc).AddTicks(8698), "Corporate clients with special contracts", 10m, "Corporate" },
                    { new Guid("27a610ad-5e20-401f-babf-728fcfe5f84a"), new DateTime(2025, 8, 23, 7, 30, 10, 865, DateTimeKind.Utc).AddTicks(8687), "Retail customers", 0m, "Retail" },
                    { new Guid("b1b18b15-294e-4755-8613-3db97edb929b"), new DateTime(2025, 8, 23, 7, 30, 10, 865, DateTimeKind.Utc).AddTicks(8697), "Wholesale buyers with bulk discounts", 5m, "Wholesale" },
                    { new Guid("be582801-87d0-4c43-aa58-64f6ac606a86"), new DateTime(2025, 8, 23, 7, 30, 10, 865, DateTimeKind.Utc).AddTicks(8713), "VIP customers with premium benefits", 15m, "VIP" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedBy", "DateCreated", "DateOfBirth", "Email", "FailedLoginAttempts", "FullName", "Gender", "HashSalt", "IsDeleted", "IsEmailVerified", "IsPhoneNumberVerified", "IsTwoFactorEnabled", "LastLoggedIn", "LastModified", "LockoutEnd", "Password", "PhoneNumber", "ProfilePictureUrl", "RefreshToken", "RefreshTokenExpiryTime", "TwoFactorSecret", "WalletId" },
                values: new object[] { new Guid("1f08f86b-66b9-4f16-bdc9-762a177ceb5e"), "", new DateTimeOffset(new DateTime(2025, 8, 23, 7, 30, 10, 907, DateTimeKind.Unspecified).AddTicks(2469), new TimeSpan(0, 0, 0, 0, 0)), "2000-04-22", "ShakirullahOhio@gmail.com", 0, "Shakirullah Ohio", "Male", "d08b4fb4-cdae-4841-89a7-a37d3fc19d51eb012524-180e-4127-9797-1bed34e94650", false, false, false, true, new DateTime(2025, 8, 23, 7, 30, 10, 907, DateTimeKind.Utc).AddTicks(2428), new DateTimeOffset(new DateTime(2025, 8, 23, 7, 30, 10, 907, DateTimeKind.Unspecified).AddTicks(2469), new TimeSpan(0, 0, 0, 0, 0)), null, "vd61YRFFSIHsbn15gK10i2oe7KTqb7rjYMWlxy6d0jFZF6vdjZ/4oMjHY/MQ+nPIT6U23fGaqeyXVa92W9QQupn5RSN2e6W8LTxzS1TNyeb7yfjrz0PXFOxnSs9NxV5c4Im/CFDi89WeGOOMxCxiKNdSKQoGDCVcIZacGhbqSYc=", "+2348109094694", null, "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null });
        }
    }
}
