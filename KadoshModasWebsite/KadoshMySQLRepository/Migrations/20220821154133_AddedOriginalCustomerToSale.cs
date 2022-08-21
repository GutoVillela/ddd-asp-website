using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KadoshRepository.Migrations
{
    public partial class AddedOriginalCustomerToSale : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OriginalCustomerId",
                table: "Sales",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sales_OriginalCustomerId",
                table: "Sales",
                column: "OriginalCustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Customers_OriginalCustomerId",
                table: "Sales",
                column: "OriginalCustomerId",
                principalTable: "Customers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Customers_OriginalCustomerId",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_Sales_OriginalCustomerId",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "OriginalCustomerId",
                table: "Sales");
        }
    }
}
