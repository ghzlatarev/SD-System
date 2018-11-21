using Microsoft.EntityFrameworkCore.Migrations;

namespace SD.Data.Migrations
{
    public partial class update3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Sensor Data_SensorId",
                table: "Sensor Data");

            migrationBuilder.CreateIndex(
                name: "IX_Sensor Data_SensorId",
                table: "Sensor Data",
                column: "SensorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Sensor Data_SensorId",
                table: "Sensor Data");

            migrationBuilder.CreateIndex(
                name: "IX_Sensor Data_SensorId",
                table: "Sensor Data",
                column: "SensorId",
                unique: true);
        }
    }
}
