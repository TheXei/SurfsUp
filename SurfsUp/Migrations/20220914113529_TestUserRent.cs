using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurfsUp.Migrations
{
    public partial class TestUserRent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rent_ApplicationUser_ApplicationUserId",
                table: "Rent");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationUser",
                table: "ApplicationUser");

            migrationBuilder.RenameTable(
                name: "ApplicationUser",
                newName: "ApplcationUser");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplcationUser",
                table: "ApplcationUser",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rent_ApplcationUser_ApplicationUserId",
                table: "Rent",
                column: "ApplicationUserId",
                principalTable: "ApplcationUser",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rent_ApplcationUser_ApplicationUserId",
                table: "Rent");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplcationUser",
                table: "ApplcationUser");

            migrationBuilder.RenameTable(
                name: "ApplcationUser",
                newName: "ApplicationUser");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationUser",
                table: "ApplicationUser",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rent_ApplicationUser_ApplicationUserId",
                table: "Rent",
                column: "ApplicationUserId",
                principalTable: "ApplicationUser",
                principalColumn: "Id");
        }
    }
}
