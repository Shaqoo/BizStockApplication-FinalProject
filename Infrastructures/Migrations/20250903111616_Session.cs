using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class Session : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "SessionId",
                table: "Carts",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.InsertData(
                table: "CustomerTypes",
                columns: new[] { "Id", "CreatedAt", "Description", "DiscountPercentage", "TypeName" },
                values: new object[,]
                {
                    { new Guid("05bdba65-0fc4-4b6b-8c16-b9c979489c32"), new DateTime(2025, 9, 3, 11, 16, 15, 528, DateTimeKind.Utc).AddTicks(1912), "Wholesale buyers with bulk discounts", 5m, "Wholesale" },
                    { new Guid("990e48e1-e52b-429c-b5c1-5ddbaa5ce05f"), new DateTime(2025, 9, 3, 11, 16, 15, 528, DateTimeKind.Utc).AddTicks(1914), "Corporate clients with special contracts", 10m, "Corporate" },
                    { new Guid("9c38afbe-dd2e-475b-8a9c-642dea4cdb2d"), new DateTime(2025, 9, 3, 11, 16, 15, 528, DateTimeKind.Utc).AddTicks(1918), "Resellers who purchase for resale", 7.5m, "Reseller" },
                    { new Guid("cee1aa27-83ea-4fe7-853f-814491bdb4ce"), new DateTime(2025, 9, 3, 11, 16, 15, 528, DateTimeKind.Utc).AddTicks(1890), "Retail customers", 0m, "Retail" },
                    { new Guid("d23ffe32-2dd4-4a29-86d2-7553c6aff499"), new DateTime(2025, 9, 3, 11, 16, 15, 528, DateTimeKind.Utc).AddTicks(1920), "VIP customers with premium benefits", 15m, "VIP" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedBy", "DateCreated", "DateOfBirth", "Email", "FailedLoginAttempts", "FullName", "Gender", "HashSalt", "IsDeleted", "IsEmailVerified", "IsPhoneNumberVerified", "IsTwoFactorEnabled", "LastLoggedIn", "LastModified", "LockoutEnd", "Password", "PhoneNumber", "ProfilePictureUrl", "RefreshToken", "RefreshTokenExpiryTime", "TwoFactorSecret", "WalletId" },
                values: new object[] { new Guid("dc3427f3-01b7-480d-bd79-9e0c4034763e"), "", new DateTimeOffset(new DateTime(2025, 9, 3, 11, 16, 15, 574, DateTimeKind.Unspecified).AddTicks(2280), new TimeSpan(0, 0, 0, 0, 0)), "2000-04-22", "ShakirullahOhio@gmail.com", 0, "Shakirullah Ohio", "Male", "d08b4fb4-cdae-4841-89a7-a37d3fc19d51eb012524-180e-4127-9797-1bed34e94650", false, false, false, true, new DateTime(2025, 9, 3, 11, 16, 15, 574, DateTimeKind.Utc).AddTicks(2246), new DateTimeOffset(new DateTime(2025, 9, 3, 11, 16, 15, 574, DateTimeKind.Unspecified).AddTicks(2280), new TimeSpan(0, 0, 0, 0, 0)), null, "vd61YRFFSIHsbn15gK10i2oe7KTqb7rjYMWlxy6d0jFZF6vdjZ/4oMjHY/MQ+nPIT6U23fGaqeyXVa92W9QQupn5RSN2e6W8LTxzS1TNyeb7yfjrz0PXFOxnSs9NxV5c4Im/CFDi89WeGOOMxCxiKNdSKQoGDCVcIZacGhbqSYc=", "+2348109094694", null, "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: new Guid("05bdba65-0fc4-4b6b-8c16-b9c979489c32"));

            migrationBuilder.DeleteData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: new Guid("990e48e1-e52b-429c-b5c1-5ddbaa5ce05f"));

            migrationBuilder.DeleteData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: new Guid("9c38afbe-dd2e-475b-8a9c-642dea4cdb2d"));

            migrationBuilder.DeleteData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: new Guid("cee1aa27-83ea-4fe7-853f-814491bdb4ce"));

            migrationBuilder.DeleteData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: new Guid("d23ffe32-2dd4-4a29-86d2-7553c6aff499"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("dc3427f3-01b7-480d-bd79-9e0c4034763e"));

            migrationBuilder.AlterColumn<string>(
                name: "SessionId",
                table: "Carts",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

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
    }
}
