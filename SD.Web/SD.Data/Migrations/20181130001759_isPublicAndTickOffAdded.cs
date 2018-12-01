using Microsoft.EntityFrameworkCore.Migrations;

namespace SD.Data.Migrations
{
    public partial class isPublicAndTickOffAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isPublic",
                table: "UserSensors",
                newName: "IsPublic");

            migrationBuilder.AddColumn<bool>(
                name: "TickOff",
                table: "UserSensors",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TickOff",
                table: "UserSensors");

            migrationBuilder.RenameColumn(
                name: "IsPublic",
                table: "UserSensors",
                newName: "isPublic");
        }
    }
}
