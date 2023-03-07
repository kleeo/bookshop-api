using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookshop.Migrations
{
    /// <inheritdoc />
    public partial class ChangeStoreModelProductId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StoreItems_Products_ProductId",
                table: "StoreItems");

            migrationBuilder.DropIndex(
                name: "IX_StoreItems_ProductId",
                table: "StoreItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_StoreItems_ProductId",
                table: "StoreItems",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_StoreItems_Products_ProductId",
                table: "StoreItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
