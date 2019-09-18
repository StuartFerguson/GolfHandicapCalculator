using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ManagementAPI.Database.Migrations
{
    public partial class addPublishedPlayerScore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PublishedPlayerScores",
                columns: table => new
                {
                    PlayerId = table.Column<Guid>(nullable: false),
                    TournamentId = table.Column<Guid>(nullable: false),
                    GolfClubId = table.Column<Guid>(nullable: false),
                    MeasuredCourseId = table.Column<Guid>(nullable: false),
                    TournamentDate = table.Column<DateTime>(nullable: false),
                    GrossScore = table.Column<int>(nullable: false),
                    NetScore = table.Column<int>(nullable: false),
                    TournamentFormat = table.Column<int>(nullable: false),
                    CSS = table.Column<int>(nullable: false),
                    TournamentName = table.Column<string>(nullable:false),
                    GolfClubName = table.Column<string>(nullable: false),
                    MeasuredCourseName = table.Column<string>(nullable: false),
                    MeasuredCourseTeeColour = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublishedPlayerScores", x => new { x.PlayerId, x.GolfClubId, x.TournamentId, x.MeasuredCourseId });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PublishedPlayerScores");
        }
    }
}
