using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class seededState : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DeliveryAgents_Email",
                table: "DeliveryAgents");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "DeliveryAgents",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150);

            migrationBuilder.CreateTable(
                name: "States",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_States", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lgas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    StateId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lgas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lgas_States_StateId",
                        column: x => x.StateId,
                        principalTable: "States",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "States",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Abia" },
                    { 2, "Adamawa" },
                    { 3, "Akwa Ibom" },
                    { 4, "Anambra" },
                    { 5, "Bauchi" },
                    { 6, "Bayelsa" },
                    { 7, "Benue" },
                    { 8, "Borno" },
                    { 9, "Cross River" },
                    { 10, "Delta" },
                    { 11, "Ebonyi" },
                    { 12, "Edo" },
                    { 13, "Ekiti" },
                    { 14, "Enugu" },
                    { 15, "Gombe" },
                    { 16, "Imo" },
                    { 17, "Jigawa" },
                    { 18, "Kaduna" },
                    { 19, "Kano" },
                    { 20, "Katsina" },
                    { 21, "Kebbi" },
                    { 22, "Kogi" },
                    { 23, "Kwara" },
                    { 24, "Lagos" },
                    { 25, "Nasarawa" },
                    { 26, "Niger" },
                    { 27, "Ogun" },
                    { 28, "Ondo" },
                    { 29, "Osun" },
                    { 30, "Oyo" },
                    { 31, "Plateau" },
                    { 32, "Rivers" },
                    { 33, "Sokoto" },
                    { 34, "Taraba" },
                    { 35, "Yobe" },
                    { 36, "Zamfara" },
                    { 37, "Federal Capital Territory" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lgas_StateId",
                table: "Lgas",
                column: "StateId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Lgas");

            migrationBuilder.DropTable(
                name: "States");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "DeliveryAgents",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryAgents_Email",
                table: "DeliveryAgents",
                column: "Email",
                unique: true);
        }
    }
}
