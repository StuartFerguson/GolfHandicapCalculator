using Microsoft.EntityFrameworkCore.Migrations;

namespace ManagementAPI.Database.Migrations
{
    public partial class PlayersSignUpAndScoresCountInTournament : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PlayersScoresRecordedCount",
                table: "Tournament",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PlayersSignedUpCount",
                table: "Tournament",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlayersScoresRecordedCount",
                table: "Tournament");

            migrationBuilder.DropColumn(
                name: "PlayersSignedUpCount",
                table: "Tournament");
        }
    }
}
