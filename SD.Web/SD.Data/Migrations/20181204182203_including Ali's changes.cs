using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SD.Data.Migrations
{
    public partial class includingAlischanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Notifications",
                nullable: false,
                oldClrType: typeof(Guid));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Notifications",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
