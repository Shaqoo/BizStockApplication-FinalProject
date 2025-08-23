using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class Gtyaya : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_DeliveryAgents_DeliveryAgentId1",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Products_ProductId1",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Users_ReviewerId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_DeliveryAgentId1",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_ProductId1",
                table: "Reviews");

            migrationBuilder.DeleteData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: new Guid("0bc49116-6c25-4400-a7b9-8110cc8f86ab"));

            migrationBuilder.DeleteData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: new Guid("3ebadece-a3b1-46c8-ad02-1b84e81ce013"));

            migrationBuilder.DeleteData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: new Guid("baaec651-28fc-4e8f-86f5-28dd9a3e89cf"));

            migrationBuilder.DeleteData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: new Guid("c33df559-61d9-49d7-a4ed-91dd70b22631"));

            migrationBuilder.DeleteData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: new Guid("c493beaa-89a3-4273-bd72-f23a9c9f0f9b"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("3b840cc0-f946-476a-8c60-a689a0b0b920"));

            migrationBuilder.DropColumn(
                name: "DeliveryAgentId1",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "ProductId1",
                table: "Reviews");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "UserRecoveryCodes",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(400)",
                oldMaxLength: 400);

            migrationBuilder.InsertData(
                table: "CustomerTypes",
                columns: new[] { "Id", "CreatedAt", "Description", "DiscountPercentage", "TypeName" },
                values: new object[,]
                {
                    { new Guid("0fef1580-043d-46a9-8993-4b1232a3d453"), new DateTime(2025, 8, 18, 17, 14, 43, 260, DateTimeKind.Utc).AddTicks(1697), "Corporate clients with special contracts", 10m, "Corporate" },
                    { new Guid("1e6a06bd-0c4a-4d82-ae58-4f623f75b880"), new DateTime(2025, 8, 18, 17, 14, 43, 260, DateTimeKind.Utc).AddTicks(1708), "Resellers who purchase for resale", 7.5m, "Reseller" },
                    { new Guid("bc53d3fe-4cc5-443e-afd9-a822b0807499"), new DateTime(2025, 8, 18, 17, 14, 43, 260, DateTimeKind.Utc).AddTicks(1710), "VIP customers with premium benefits", 15m, "VIP" },
                    { new Guid("d042d8e9-335c-4fde-bd4c-5db57dd221f9"), new DateTime(2025, 8, 18, 17, 14, 43, 260, DateTimeKind.Utc).AddTicks(1687), "Retail customers", 0m, "Retail" },
                    { new Guid("d153fa56-7445-4ab1-8687-6d188d655a8b"), new DateTime(2025, 8, 18, 17, 14, 43, 260, DateTimeKind.Utc).AddTicks(1695), "Wholesale buyers with bulk discounts", 5m, "Wholesale" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedBy", "DateCreated", "DateOfBirth", "Email", "FailedLoginAttempts", "FullName", "Gender", "HashSalt", "IsDeleted", "IsEmailVerified", "IsPhoneNumberVerified", "IsTwoFactorEnabled", "LastLoggedIn", "LastModified", "LockoutEnd", "Password", "PhoneNumber", "ProfilePictureUrl", "RefreshToken", "RefreshTokenExpiryTime", "TwoFactorSecret", "WalletId" },
                values: new object[] { new Guid("3ea24b5e-ef42-4cde-bc7f-8778b7fa902e"), "", new DateTimeOffset(new DateTime(2025, 8, 18, 17, 14, 43, 291, DateTimeKind.Unspecified).AddTicks(4235), new TimeSpan(0, 0, 0, 0, 0)), "2000-04-22", "ShakirullahOhio@gmail.com", 0, "Shakirullah Ohio", "Male", "d08b4fb4-cdae-4841-89a7-a37d3fc19d51eb012524-180e-4127-9797-1bed34e94650", false, false, false, true, new DateTime(2025, 8, 18, 17, 14, 43, 291, DateTimeKind.Utc).AddTicks(4204), new DateTimeOffset(new DateTime(2025, 8, 18, 17, 14, 43, 291, DateTimeKind.Unspecified).AddTicks(4235), new TimeSpan(0, 0, 0, 0, 0)), null, "vd61YRFFSIHsbn15gK10i2oe7KTqb7rjYMWlxy6d0jFZF6vdjZ/4oMjHY/MQ+nPIT6U23fGaqeyXVa92W9QQupn5RSN2e6W8LTxzS1TNyeb7yfjrz0PXFOxnSs9NxV5c4Im/CFDi89WeGOOMxCxiKNdSKQoGDCVcIZacGhbqSYc=", "+2348109094694", null, "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null });

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Users_ReviewerId",
                table: "Reviews",
                column: "ReviewerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Users_ReviewerId",
                table: "Reviews");

            migrationBuilder.DeleteData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: new Guid("0fef1580-043d-46a9-8993-4b1232a3d453"));

            migrationBuilder.DeleteData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: new Guid("1e6a06bd-0c4a-4d82-ae58-4f623f75b880"));

            migrationBuilder.DeleteData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: new Guid("bc53d3fe-4cc5-443e-afd9-a822b0807499"));

            migrationBuilder.DeleteData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: new Guid("d042d8e9-335c-4fde-bd4c-5db57dd221f9"));

            migrationBuilder.DeleteData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: new Guid("d153fa56-7445-4ab1-8687-6d188d655a8b"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("3ea24b5e-ef42-4cde-bc7f-8778b7fa902e"));

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "UserRecoveryCodes",
                type: "character varying(400)",
                maxLength: 400,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<Guid>(
                name: "DeliveryAgentId1",
                table: "Reviews",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId1",
                table: "Reviews",
                type: "uuid",
                nullable: true);

            migrationBuilder.InsertData(
                table: "CustomerTypes",
                columns: new[] { "Id", "CreatedAt", "Description", "DiscountPercentage", "TypeName" },
                values: new object[,]
                {
                    { new Guid("0bc49116-6c25-4400-a7b9-8110cc8f86ab"), new DateTime(2025, 8, 18, 14, 13, 51, 288, DateTimeKind.Utc).AddTicks(9023), "Retail customers", 0m, "Retail" },
                    { new Guid("3ebadece-a3b1-46c8-ad02-1b84e81ce013"), new DateTime(2025, 8, 18, 14, 13, 51, 288, DateTimeKind.Utc).AddTicks(9042), "VIP customers with premium benefits", 15m, "VIP" },
                    { new Guid("baaec651-28fc-4e8f-86f5-28dd9a3e89cf"), new DateTime(2025, 8, 18, 14, 13, 51, 288, DateTimeKind.Utc).AddTicks(9036), "Corporate clients with special contracts", 10m, "Corporate" },
                    { new Guid("c33df559-61d9-49d7-a4ed-91dd70b22631"), new DateTime(2025, 8, 18, 14, 13, 51, 288, DateTimeKind.Utc).AddTicks(9040), "Resellers who purchase for resale", 7.5m, "Reseller" },
                    { new Guid("c493beaa-89a3-4273-bd72-f23a9c9f0f9b"), new DateTime(2025, 8, 18, 14, 13, 51, 288, DateTimeKind.Utc).AddTicks(9034), "Wholesale buyers with bulk discounts", 5m, "Wholesale" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedBy", "DateCreated", "DateOfBirth", "Email", "FailedLoginAttempts", "FullName", "Gender", "HashSalt", "IsDeleted", "IsEmailVerified", "IsPhoneNumberVerified", "IsTwoFactorEnabled", "LastLoggedIn", "LastModified", "LockoutEnd", "Password", "PhoneNumber", "ProfilePictureUrl", "RefreshToken", "RefreshTokenExpiryTime", "TwoFactorSecret", "WalletId" },
                values: new object[] { new Guid("3b840cc0-f946-476a-8c60-a689a0b0b920"), "", new DateTimeOffset(new DateTime(2025, 8, 18, 14, 13, 51, 324, DateTimeKind.Unspecified).AddTicks(2684), new TimeSpan(0, 0, 0, 0, 0)), "2000-04-22", "ShakirullahOhio@gmail.com", 0, "Shakirullah Ohio", "Male", "d08b4fb4-cdae-4841-89a7-a37d3fc19d51eb012524-180e-4127-9797-1bed34e94650", false, false, false, true, new DateTime(2025, 8, 18, 14, 13, 51, 324, DateTimeKind.Utc).AddTicks(2644), new DateTimeOffset(new DateTime(2025, 8, 18, 14, 13, 51, 324, DateTimeKind.Unspecified).AddTicks(2684), new TimeSpan(0, 0, 0, 0, 0)), null, "vd61YRFFSIHsbn15gK10i2oe7KTqb7rjYMWlxy6d0jFZF6vdjZ/4oMjHY/MQ+nPIT6U23fGaqeyXVa92W9QQupn5RSN2e6W8LTxzS1TNyeb7yfjrz0PXFOxnSs9NxV5c4Im/CFDi89WeGOOMxCxiKNdSKQoGDCVcIZacGhbqSYc=", "+2348109094694", null, "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null });

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_DeliveryAgentId1",
                table: "Reviews",
                column: "DeliveryAgentId1");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ProductId1",
                table: "Reviews",
                column: "ProductId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_DeliveryAgents_DeliveryAgentId1",
                table: "Reviews",
                column: "DeliveryAgentId1",
                principalTable: "DeliveryAgents",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Products_ProductId1",
                table: "Reviews",
                column: "ProductId1",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Users_ReviewerId",
                table: "Reviews",
                column: "ReviewerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
