using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KadoshRepository.Migrations
{
    public partial class AddedSaltParametersToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordSalt",
                table: "Users",
                type: "longblob",
                nullable: false);

            migrationBuilder.AddColumn<int>(
                name: "PasswordSaltIterations",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordSalt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PasswordSaltIterations",
                table: "Users");
        }
    }
}
