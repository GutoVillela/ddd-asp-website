using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KadoshRepository.Migrations
{
    public partial class SaleItemsFKCorrection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SalesItems",
                table: "SalesItems");

            migrationBuilder.DropIndex(
                name: "IX_SalesItems_SaleId",
                table: "SalesItems");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SalesItems",
                table: "SalesItems",
                columns: new[] { "SaleId", "ProductId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SalesItems",
                table: "SalesItems");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SalesItems",
                table: "SalesItems",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_SalesItems_SaleId",
                table: "SalesItems",
                column: "SaleId");
        }
    }
}
