using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SD.Data.Migrations
{
    public partial class updatingusersensorstable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sensors_AspNetUsers_ApplicationUserId",
                table: "Sensors");

            migrationBuilder.DropForeignKey(
                name: "FK_Sensors_UserSensors_UserSensorId",
                table: "Sensors");

            migrationBuilder.DropTable(
                name: "Sensor Data");

            migrationBuilder.DropIndex(
                name: "IX_Sensors_ApplicationUserId",
                table: "Sensors");

            migrationBuilder.DropIndex(
                name: "IX_Sensors_UserSensorId",
                table: "Sensors");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Sensors");

            migrationBuilder.DropColumn(
                name: "UserSensorId",
                table: "Sensors");

            migrationBuilder.RenameColumn(
                name: "type",
                table: "UserSensors",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "ApiDataSourceId",
                table: "UserSensors",
                newName: "SensorId");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "UserSensors",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateTable(
                name: "SensorsData",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    TimeStamp = table.Column<DateTime>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    ValueType = table.Column<string>(nullable: true),
                    SensorId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensorsData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SensorsData_Sensors_SensorId",
                        column: x => x.SensorId,
                        principalTable: "Sensors",
                        principalColumn: "SensorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserSensors_SensorId",
                table: "UserSensors",
                column: "SensorId");

            migrationBuilder.CreateIndex(
                name: "IX_SensorsData_SensorId",
                table: "SensorsData",
                column: "SensorId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSensors_Sensors_SensorId",
                table: "UserSensors",
                column: "SensorId",
                principalTable: "Sensors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSensors_Sensors_SensorId",
                table: "UserSensors");

            migrationBuilder.DropTable(
                name: "SensorsData");

            migrationBuilder.DropIndex(
                name: "IX_UserSensors_SensorId",
                table: "UserSensors");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "UserSensors",
                newName: "type");

            migrationBuilder.RenameColumn(
                name: "SensorId",
                table: "UserSensors",
                newName: "ApiDataSourceId");

            migrationBuilder.AlterColumn<int>(
                name: "type",
                table: "UserSensors",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Sensors",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserSensorId",
                table: "Sensors",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Sensor Data",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    SensorId = table.Column<Guid>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    ValueType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sensor Data", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sensor Data_Sensors_SensorId",
                        column: x => x.SensorId,
                        principalTable: "Sensors",
                        principalColumn: "SensorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_ApplicationUserId",
                table: "Sensors",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_UserSensorId",
                table: "Sensors",
                column: "UserSensorId");

            migrationBuilder.CreateIndex(
                name: "IX_Sensor Data_SensorId",
                table: "Sensor Data",
                column: "SensorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sensors_AspNetUsers_ApplicationUserId",
                table: "Sensors",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Sensors_UserSensors_UserSensorId",
                table: "Sensors",
                column: "UserSensorId",
                principalTable: "UserSensors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
