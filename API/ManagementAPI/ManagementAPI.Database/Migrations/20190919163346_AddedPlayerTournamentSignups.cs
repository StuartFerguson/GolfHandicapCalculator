using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ManagementAPI.Database.Migrations
{
    public partial class AddedPlayerTournamentSignups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlayerTournamentSignUps",
                columns: table => new
                {
                    PlayerId = table.Column<Guid>(nullable: false),
                    TournamentId = table.Column<Guid>(nullable: false),
                    GolfClubId = table.Column<Guid>(nullable: false),
                    GolfClubName = table.Column<string>(nullable: true),
                    MeasuredCourseId = table.Column<Guid>(nullable: false),
                    MeasuredCourseName = table.Column<string>(nullable: true),
                    MeasuredCourseTeeColour = table.Column<string>(nullable: true),
                    ScoreEntered = table.Column<bool>(nullable: false),
                    TournamentDate = table.Column<DateTime>(nullable: false),
                    TournamentFormat = table.Column<int>(nullable: false),
                    TournamentName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerTournamentSignUps", x => new { x.PlayerId, x.TournamentId });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerTournamentSignUps");
        }
    }
}
