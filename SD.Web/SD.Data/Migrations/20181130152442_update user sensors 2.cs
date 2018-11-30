using Microsoft.EntityFrameworkCore.Migrations;

namespace SD.Data.Migrations
{
    public partial class updateusersensors2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Coordinates",
                table: "UserSensors");

            migrationBuilder.RenameColumn(
                name: "isPublic",
                table: "UserSensors",
                newName: "IsPublic");

            migrationBuilder.AddColumn<int>(
                name: "Latitude",
                table: "UserSensors",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Longitude",
                table: "UserSensors",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PollingInterval",
                table: "UserSensors",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "UserSensors");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "UserSensors");

            migrationBuilder.DropColumn(
                name: "PollingInterval",
                table: "UserSensors");

            migrationBuilder.RenameColumn(
                name: "IsPublic",
                table: "UserSensors",
                newName: "isPublic");

            migrationBuilder.AddColumn<string>(
                name: "Coordinates",
                table: "UserSensors",
                maxLength: 18,
                nullable: true);
        }
    }
}
