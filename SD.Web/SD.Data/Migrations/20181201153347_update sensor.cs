using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SD.Data.Migrations
{
    public partial class updatesensor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastTimeStamp",
                table: "Sensors",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastValue",
                table: "Sensors",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastTimeStamp",
                table: "Sensors");

            migrationBuilder.DropColumn(
                name: "LastValue",
                table: "Sensors");
        }
    }
}
