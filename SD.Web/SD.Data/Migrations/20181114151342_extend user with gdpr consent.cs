using Microsoft.EntityFrameworkCore.Migrations;

namespace SD.Data.Migrations
{
    public partial class extenduserwithgdprconsent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "GDPR",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GDPR",
                table: "AspNetUsers");
        }
    }
}
