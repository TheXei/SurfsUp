using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurfsUp.Migrations.SurfsUp
{
    public partial class BoardPremium : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Premium",
                table: "Board",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Premium",
                table: "Board");
        }
    }
}
