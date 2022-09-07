using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurfsUp.Migrations
{
    public partial class RentId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Rent",
                table: "Rent");

            migrationBuilder.DropIndex(
                name: "IX_Rent_BoardId",
                table: "Rent");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Rent");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rent",
                table: "Rent",
                column: "BoardId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Rent",
                table: "Rent");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Rent",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rent",
                table: "Rent",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Rent_BoardId",
                table: "Rent",
                column: "BoardId",
                unique: true);
        }
    }
}
