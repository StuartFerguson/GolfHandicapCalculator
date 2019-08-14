using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ManagementAPI.Database.Migrations
{
    public partial class AddGolfClubMembershipReporting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GolfClubMembershipReporting",
                columns: table => new
                {
                    GolfClubId = table.Column<Guid>(nullable: false),
                    PlayerId = table.Column<Guid>(nullable: false),
                    GolfClubName = table.Column<string>(nullable: true),
                    PlayerName = table.Column<string>(nullable: true),
                    PlayerGender = table.Column<string>(nullable: true),
                    HandicapCategory = table.Column<int>(nullable: false),
                    DateOfBirth = table.Column<DateTime>(nullable: false),
                    DateJoined = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GolfClubMembershipReporting", x => new { x.GolfClubId, x.PlayerId });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GolfClubMembershipReporting");
        }
    }
}
