using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KadoshRepository.Migrations
{
    public partial class AddedBoundedCustomerRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BoundedToCustomerId",
                table: "Customers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_BoundedToCustomerId",
                table: "Customers",
                column: "BoundedToCustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Customers_BoundedToCustomerId",
                table: "Customers",
                column: "BoundedToCustomerId",
                principalTable: "Customers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Customers_BoundedToCustomerId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_BoundedToCustomerId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "BoundedToCustomerId",
                table: "Customers");
        }
    }
}
