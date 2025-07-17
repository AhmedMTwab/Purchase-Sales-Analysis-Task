using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Purchase_Sales_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ProductNamePK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_sales_products_productId",
                table: "sales");

            migrationBuilder.DropPrimaryKey(
                name: "PK_products",
                table: "products");

            migrationBuilder.DropColumn(
                name: "productId",
                table: "products");

            migrationBuilder.RenameColumn(
                name: "productId",
                table: "sales",
                newName: "productName");

            migrationBuilder.RenameIndex(
                name: "IX_sales_productId",
                table: "sales",
                newName: "IX_sales_productName");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "products",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_products",
                table: "products",
                column: "name");

            migrationBuilder.AddForeignKey(
                name: "FK_sales_products_productName",
                table: "sales",
                column: "productName",
                principalTable: "products",
                principalColumn: "name",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_sales_products_productName",
                table: "sales");

            migrationBuilder.DropPrimaryKey(
                name: "PK_products",
                table: "products");

            migrationBuilder.RenameColumn(
                name: "productName",
                table: "sales",
                newName: "productId");

            migrationBuilder.RenameIndex(
                name: "IX_sales_productName",
                table: "sales",
                newName: "IX_sales_productId");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "products",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "productId",
                table: "products",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_products",
                table: "products",
                column: "productId");

            migrationBuilder.AddForeignKey(
                name: "FK_sales_products_productId",
                table: "sales",
                column: "productId",
                principalTable: "products",
                principalColumn: "productId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
