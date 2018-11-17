using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SD.Data.Migrations
{
    public partial class ModelsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sensors",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 35, nullable: true),
                    Description = table.Column<string>(maxLength: 300, nullable: true),
                    type = table.Column<int>(nullable: false),
                    UserInterval = table.Column<int>(nullable: false),
                    LastValueUser = table.Column<int>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    Coordinates = table.Column<string>(maxLength: 18, nullable: true),
                    isPublic = table.Column<bool>(nullable: false),
                    AlarmTriggered = table.Column<bool>(nullable: false),
                    AlarmMin = table.Column<int>(nullable: false),
                    AlarmMax = table.Column<int>(nullable: false),
                    ApiDataSourceId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    UserId1 = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sensors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sensors_AspNetUsers_UserId1",
                        column: x => x.UserId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApiDataSourses",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Tag = table.Column<string>(maxLength: 50, nullable: true),
                    Description = table.Column<string>(maxLength: 300, nullable: true),
                    MeasureType = table.Column<int>(nullable: false),
                    ApiInterval = table.Column<int>(nullable: false),
                    LastValueApi = table.Column<int>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    GotOutOfRange = table.Column<bool>(maxLength: 18, nullable: false),
                    RangeMin = table.Column<int>(nullable: false),
                    RangeMax = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    SensorId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiDataSourses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApiDataSourses_Sensors_SensorId",
                        column: x => x.SensorId,
                        principalTable: "Sensors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApiDataSourses_SensorId",
                table: "ApiDataSourses",
                column: "SensorId");

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_UserId1",
                table: "Sensors",
                column: "UserId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApiDataSourses");

            migrationBuilder.DropTable(
                name: "Sensors");
        }
    }
}
