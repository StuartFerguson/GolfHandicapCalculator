using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ManagementAPI.Database.Migrations
{
    public partial class AddMeasuredCourse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MeasuredCourses",
                columns: table => new
                {
                    GolfClubId = table.Column<Guid>(nullable: false),
                    MeasuredCourseId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    SSS = table.Column<int>(nullable: false),
                    TeeColour = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeasuredCourses", x => new { x.GolfClubId, x.MeasuredCourseId });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MeasuredCourses");
        }
    }
}
