using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class RecoveryUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DeleteData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: new Guid("1dc9b21e-83ef-4db9-84b1-8789a3f1e940"));

            migrationBuilder.DeleteData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: new Guid("7d71c4fd-0d82-4383-a0cc-61fd34bdb997"));

            migrationBuilder.DeleteData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: new Guid("969d937e-b092-4f8c-8fa7-b3648ad4f41b"));

            migrationBuilder.DeleteData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: new Guid("ada75eca-8f30-499b-aa46-6925970126b7"));

            migrationBuilder.DeleteData(
                table: "CustomerTypes",
                keyColumn: "Id",
                keyValue: new Guid("c5938526-eba1-49f0-9967-d81a1c4dbe5f"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("94f439c8-4702-4127-8998-c47771567f8a"));

            migrationBuilder.AddColumn<bool>(
                name: "IsTwoFactorEnabled",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "LostAccessRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserIdentifier = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    AlternateEmail = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    AlternatePhone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    ProblemDescription = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    AdminNotes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    SubmittedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LostAccessRequests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRecoveryCodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false),
                    IsUsed = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UsedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRecoveryCodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRecoveryCodes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "IX_UserRecoveryCodes_UserId",
                table: "UserRecoveryCodes",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LostAccessRequests");

            migrationBuilder.DropTable(
                name: "UserRecoveryCodes");

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
                name: "IsTwoFactorEnabled",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Action = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    EntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    EntityName = table.Column<string>(type: "text", nullable: true),
                    IpAddress = table.Column<string>(type: "text", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserAgent = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "CustomerTypes",
                columns: new[] { "Id", "CreatedAt", "Description", "DiscountPercentage", "TypeName" },
                values: new object[,]
                {
                    { new Guid("1dc9b21e-83ef-4db9-84b1-8789a3f1e940"), new DateTime(2025, 8, 15, 9, 55, 36, 193, DateTimeKind.Utc).AddTicks(5138), "Resellers who purchase for resale", 7.5m, "Reseller" },
                    { new Guid("7d71c4fd-0d82-4383-a0cc-61fd34bdb997"), new DateTime(2025, 8, 15, 9, 55, 36, 193, DateTimeKind.Utc).AddTicks(5123), "Corporate clients with special contracts", 10m, "Corporate" },
                    { new Guid("969d937e-b092-4f8c-8fa7-b3648ad4f41b"), new DateTime(2025, 8, 15, 9, 55, 36, 193, DateTimeKind.Utc).AddTicks(5141), "VIP customers with premium benefits", 15m, "VIP" },
                    { new Guid("ada75eca-8f30-499b-aa46-6925970126b7"), new DateTime(2025, 8, 15, 9, 55, 36, 193, DateTimeKind.Utc).AddTicks(5111), "Retail customers", 0m, "Retail" },
                    { new Guid("c5938526-eba1-49f0-9967-d81a1c4dbe5f"), new DateTime(2025, 8, 15, 9, 55, 36, 193, DateTimeKind.Utc).AddTicks(5121), "Wholesale buyers with bulk discounts", 5m, "Wholesale" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedBy", "DateCreated", "DateOfBirth", "Email", "FailedLoginAttempts", "FullName", "Gender", "HashSalt", "IsDeleted", "IsEmailVerified", "IsPhoneNumberVerified", "LastLoggedIn", "LastModified", "LockoutEnd", "Password", "PhoneNumber", "ProfilePictureUrl", "RefreshToken", "RefreshTokenExpiryTime", "TwoFactorSecret", "WalletId" },
                values: new object[] { new Guid("94f439c8-4702-4127-8998-c47771567f8a"), "", new DateTimeOffset(new DateTime(2025, 8, 15, 9, 55, 36, 229, DateTimeKind.Unspecified).AddTicks(3912), new TimeSpan(0, 0, 0, 0, 0)), "2000-04-22", "ShakirullahOhio@gmail.com", 0, "Shakirullah Ohio", "Male", "d08b4fb4-cdae-4841-89a7-a37d3fc19d51eb012524-180e-4127-9797-1bed34e94650", false, false, false, new DateTime(2025, 8, 15, 9, 55, 36, 229, DateTimeKind.Utc).AddTicks(3874), new DateTimeOffset(new DateTime(2025, 8, 15, 9, 55, 36, 229, DateTimeKind.Unspecified).AddTicks(3912), new TimeSpan(0, 0, 0, 0, 0)), null, "vd61YRFFSIHsbn15gK10i2oe7KTqb7rjYMWlxy6d0jFZF6vdjZ/4oMjHY/MQ+nPIT6U23fGaqeyXVa92W9QQupn5RSN2e6W8LTxzS1TNyeb7yfjrz0PXFOxnSs9NxV5c4Im/CFDi89WeGOOMxCxiKNdSKQoGDCVcIZacGhbqSYc=", "+2348109094694", null, "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null });
        }
    }
}
