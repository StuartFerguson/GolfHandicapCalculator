using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ManagementAPI.Database.Migrations
{
    public partial class PlayerHandicapListAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlayerHandicapListReporting",
                columns: table => new
                {
                    GolfClubId = table.Column<Guid>(nullable: false),
                    PlayerId = table.Column<Guid>(nullable: false),
                    ExactHandicap = table.Column<decimal>(nullable: false),
                    HandicapCategory = table.Column<int>(nullable: false),
                    PlayerName = table.Column<string>(nullable: true),
                    PlayingHandicap = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerHandicapListReporting", x => new { x.PlayerId, x.GolfClubId });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerHandicapListReporting");
        }
    }
}
