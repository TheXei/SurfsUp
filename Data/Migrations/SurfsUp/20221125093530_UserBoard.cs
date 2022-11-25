using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurfsUp.Migrations.SurfsUp
{
    public partial class UserBoard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Board",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Board",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Board_OwnerId",
                table: "Board",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Board_ApplicationUser_OwnerId",
                table: "Board",
                column: "OwnerId",
                principalTable: "ApplicationUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Board_ApplicationUser_OwnerId",
                table: "Board");

            migrationBuilder.DropIndex(
                name: "IX_Board_OwnerId",
                table: "Board");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Board");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Board");
        }
    }
}
