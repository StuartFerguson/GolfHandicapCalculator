using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ManagementAPI.Database.Migrations
{
    public partial class PlayerMebershipList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClubMembershipRequest");

            migrationBuilder.CreateTable(
                name: "PlayerClubMembership",
                columns: table => new
                {
                    GolfClubId = table.Column<Guid>(nullable: false),
                    PlayerId = table.Column<Guid>(nullable: false),
                    AcceptedDateTime = table.Column<DateTime>(nullable: true),
                    GolfClubName = table.Column<string>(nullable: true),
                    MembershipId = table.Column<Guid>(nullable: false),
                    MembershipNumber = table.Column<string>(nullable: true),
                    RejectedDateTime = table.Column<DateTime>(nullable: true),
                    RejectionReason = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerClubMembership", x => new { x.PlayerId, x.GolfClubId });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerClubMembership");

            migrationBuilder.CreateTable(
                name: "ClubMembershipRequest",
                columns: table => new
                {
                    MembershipRequestId = table.Column<Guid>(nullable: false),
                    Age = table.Column<int>(nullable: false),
                    ClubId = table.Column<Guid>(nullable: false),
                    ExactHandicap = table.Column<decimal>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    Gender = table.Column<string>(nullable: true),
                    HandicapCategory = table.Column<int>(nullable: false),
                    LastName = table.Column<string>(nullable: true),
                    MembershipRequestedDateAndTime = table.Column<DateTime>(nullable: false),
                    MiddleName = table.Column<string>(nullable: true),
                    PlayerId = table.Column<Guid>(nullable: false),
                    PlayingHandicap = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubMembershipRequest", x => x.MembershipRequestId);
                });
        }
    }
}
