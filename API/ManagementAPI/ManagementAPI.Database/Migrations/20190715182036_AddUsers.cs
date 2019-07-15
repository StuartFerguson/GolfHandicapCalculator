using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ManagementAPI.Database.Migrations
{
    public partial class AddUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    GolfClubId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    FamilyName = table.Column<string>(nullable: true),
                    GivenName = table.Column<string>(nullable: true),
                    MiddleName = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    UserType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => new { x.GolfClubId, x.UserId });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
