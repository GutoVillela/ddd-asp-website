using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KadoshRepository.Migrations
{
    public partial class AddedCustomerUserData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Customers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordSalt",
                table: "Customers",
                type: "longblob",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PasswordSaltIterations",
                table: "Customers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Customers",
                type: "varchar(20)",
                maxLength: 20,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Username",
                table: "Customers",
                column: "Username",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Customers_Username",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "PasswordSalt",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "PasswordSaltIterations",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Customers");
        }
    }
}
