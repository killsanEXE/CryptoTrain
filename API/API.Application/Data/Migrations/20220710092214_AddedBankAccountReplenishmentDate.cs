using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Application.Data.Migrations
{
    public partial class AddedBankAccountReplenishmentDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastReplenishmentDate",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastReplenishmentDate",
                table: "AspNetUsers");
        }
    }
}
