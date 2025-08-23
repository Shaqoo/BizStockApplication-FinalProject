using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class productdescription1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop the generated column (adjust the name if different)
            migrationBuilder.DropColumn(
                name: "SearchVector",
                table: "Products");

            // Alter the Description column length
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Products",
                type: "character varying(3000)",
                maxLength: 3000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            // Recreate the generated column with the exact same definition
            migrationBuilder.AddColumn<string>(
                name: "SearchVector",
                table: "Products",
                type: "tsvector",
                nullable: false,
                computedColumnSql: "to_tsvector('english', coalesce(\"Name\", '') || ' ' || coalesce(\"Description\", ''))",
                stored: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop the generated column again before reverting
            migrationBuilder.DropColumn(
                name: "SearchVector",
                table: "Products");

            // Revert Description column length
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Products",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(3000)",
                oldMaxLength: 3000,
                oldNullable: true);

            // Recreate the generated column again with original definition
            migrationBuilder.AddColumn<string>(
                name: "SearchVector",
                table: "Products",
                type: "tsvector",
                nullable: false,
                computedColumnSql: "to_tsvector('english', coalesce(\"Name\", '') || ' ' || coalesce(\"Description\", ''))",
                stored: true);
        }


    }
}
