using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ManagementAPI.Database.Migrations
{
    public partial class AddTournamentResults : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tournament",
                columns: table => new
                {
                    TournamentId = table.Column<Guid>(nullable: false),
                    Format = table.Column<int>(nullable: false),
                    GolfClubId = table.Column<Guid>(nullable: false),
                    GolfClubName = table.Column<string>(nullable: true),
                    HasResultBeenProduced = table.Column<bool>(nullable: false),
                    MeasuredCourseId = table.Column<Guid>(nullable: false),
                    MeasuredCourseName = table.Column<string>(nullable: true),
                    MeasuredCourseSSS = table.Column<int>(nullable: false),
                    MeasuredCourseTeeColour = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    PlayerCategory = table.Column<int>(nullable: false),
                    TournamentDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tournament", x => x.TournamentId);
                });

            migrationBuilder.CreateTable(
                name: "TournamentResultForPlayerScore",
                columns: table => new
                {
                    TournamentResultForPlayerId = table.Column<Guid>(nullable: false),
                    Division = table.Column<int>(nullable: false),
                    DivisionPosition = table.Column<int>(nullable: false),
                    GrossScore = table.Column<int>(nullable: false),
                    Last3Holes = table.Column<decimal>(nullable: false),
                    Last6Holes = table.Column<decimal>(nullable: false),
                    Last9Holes = table.Column<decimal>(nullable: false),
                    NetScore = table.Column<int>(nullable: false),
                    PlayerId = table.Column<Guid>(nullable: false),
                    PlayingHandicap = table.Column<int>(nullable: false),
                    TournamentId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentResultForPlayerScore", x => x.TournamentResultForPlayerId);
                    table.ForeignKey(
                        name: "FK_TournamentResultForPlayerScore_Tournament_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournament",
                        principalColumn: "TournamentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TournamentResultForPlayerScore_TournamentId",
                table: "TournamentResultForPlayerScore",
                column: "TournamentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TournamentResultForPlayerScore");

            migrationBuilder.DropTable(
                name: "Tournament");
        }
    }
}
