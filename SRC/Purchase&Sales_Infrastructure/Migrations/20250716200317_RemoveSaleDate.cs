using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Purchase_Sales_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSaleDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "saleDate",
                table: "sales");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "saleDate",
                table: "sales",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
