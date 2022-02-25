using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KadoshRepository.Migrations
{
    public partial class RemovingCustomerFromCustomerPostings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomersPostings_Customers_CustomerId",
                table: "CustomersPostings");

            migrationBuilder.DropIndex(
                name: "IX_CustomersPostings_CustomerId",
                table: "CustomersPostings");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "CustomersPostings");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "CustomersPostings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomersPostings_CustomerId",
                table: "CustomersPostings",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomersPostings_Customers_CustomerId",
                table: "CustomersPostings",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id");
        }
    }
}
