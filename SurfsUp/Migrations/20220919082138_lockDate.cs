using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurfsUp.Migrations
{
    public partial class lockDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LockDate",
                table: "Board",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LockDate",
                table: "Board");
        }
    }
}
