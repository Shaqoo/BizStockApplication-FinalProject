using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class customerchange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Customers_Email",
                table: "Customers");

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
                table: "UserRoles",
                keyColumns: new[] { "Role", "UserId" },
                keyValues: new object[] { "Admin", new Guid("88e8d13a-4a6e-4a18-9cc3-87f62f564ada") });

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("dc3427f3-01b7-480d-bd79-9e0c4034763e"));

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Customers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Customers",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

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
                table: "UserRoles",
                columns: new[] { "Role", "UserId" },
                values: new object[] { "Admin", new Guid("88e8d13a-4a6e-4a18-9cc3-87f62f564ada") });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedBy", "DateCreated", "DateOfBirth", "Email", "FailedLoginAttempts", "FullName", "Gender", "HashSalt", "IsDeleted", "IsEmailVerified", "IsPhoneNumberVerified", "IsTwoFactorEnabled", "LastLoggedIn", "LastModified", "LockoutEnd", "Password", "PhoneNumber", "ProfilePictureUrl", "RefreshToken", "RefreshTokenExpiryTime", "TwoFactorSecret", "WalletId" },
                values: new object[] { new Guid("dc3427f3-01b7-480d-bd79-9e0c4034763e"), "", new DateTimeOffset(new DateTime(2025, 9, 3, 11, 16, 15, 574, DateTimeKind.Unspecified).AddTicks(2280), new TimeSpan(0, 0, 0, 0, 0)), "2000-04-22", "ShakirullahOhio@gmail.com", 0, "Shakirullah Ohio", "Male", "d08b4fb4-cdae-4841-89a7-a37d3fc19d51eb012524-180e-4127-9797-1bed34e94650", false, false, false, true, new DateTime(2025, 9, 3, 11, 16, 15, 574, DateTimeKind.Utc).AddTicks(2246), new DateTimeOffset(new DateTime(2025, 9, 3, 11, 16, 15, 574, DateTimeKind.Unspecified).AddTicks(2280), new TimeSpan(0, 0, 0, 0, 0)), null, "vd61YRFFSIHsbn15gK10i2oe7KTqb7rjYMWlxy6d0jFZF6vdjZ/4oMjHY/MQ+nPIT6U23fGaqeyXVa92W9QQupn5RSN2e6W8LTxzS1TNyeb7yfjrz0PXFOxnSs9NxV5c4Im/CFDi89WeGOOMxCxiKNdSKQoGDCVcIZacGhbqSYc=", "+2348109094694", null, "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Email",
                table: "Customers",
                column: "Email",
                unique: true);
        }
    }
}
