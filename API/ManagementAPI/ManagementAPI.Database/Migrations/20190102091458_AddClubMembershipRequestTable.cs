using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ManagementAPI.Database.Migrations
{
    public partial class AddClubMembershipRequestTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClubMembershipRequest",
                columns: table => new
                {
                    MembershipRequestId = table.Column<Guid>(nullable: false),
                    ClubId = table.Column<Guid>(nullable: false),
                    PlayerId = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    MiddleName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Gender = table.Column<string>(nullable: true),
                    Age = table.Column<int>(nullable: false),
                    ExactHandicap = table.Column<decimal>(nullable: false),
                    PlayingHandicap = table.Column<int>(nullable: false),
                    HandicapCategory = table.Column<int>(nullable: false),
                    MembershipRequestedDateAndTime = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubMembershipRequest", x => x.MembershipRequestId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClubMembershipRequest");
        }
    }
}
