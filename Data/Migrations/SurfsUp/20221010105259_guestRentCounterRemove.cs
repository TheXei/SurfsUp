using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurfsUp.Migrations.SurfsUp
{
    public partial class guestRentCounterRemove : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RentCount",
                table: "Guest");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RentCount",
                table: "Guest",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
