using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NpgsqlTypes;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class SearchProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<NpgsqlTsVector>(
                name: "SearchVector",
                table: "Products",
                type: "tsvector",
                nullable: false,
                computedColumnSql: "to_tsvector('english', coalesce(\"Name\", '') || ' ' || coalesce(\"Description\", ''))",
                stored: true);

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
                name: "IX_Products_SearchVector",
                table: "Products",
                column: "SearchVector")
                .Annotation("Npgsql:IndexMethod", "GIN");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_SearchVector",
                table: "Products");

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
                name: "SearchVector",
                table: "Products");

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
        }
    }
}
