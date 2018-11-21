using Microsoft.EntityFrameworkCore.Migrations;

namespace SD.Data.Migrations
{
    public partial class update2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Sensors_SensorId",
                table: "Sensors",
                column: "SensorId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Sensors_SensorId",
                table: "Sensors");
        }
    }
}
