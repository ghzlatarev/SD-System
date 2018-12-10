using Microsoft.EntityFrameworkCore.Migrations;

namespace SD.Data.Migrations
{
    public partial class addingisStatebool : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsState",
                table: "Sensors",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsState",
                table: "Sensors");
        }
    }
}
