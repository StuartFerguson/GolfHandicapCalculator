using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ManagementAPI.Database.Migrations
{
    public partial class RenameClubConfigurationAggregate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClubInformation");

            migrationBuilder.CreateTable(
                name: "GolfClub",
                columns: table => new
                {
                    GolfClubId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    AddressLine1 = table.Column<string>(nullable: true),
                    AddressLine2 = table.Column<string>(nullable: true),
                    Town = table.Column<string>(nullable: true),
                    Region = table.Column<string>(nullable: true),
                    PostalCode = table.Column<string>(nullable: true),
                    TelephoneNumber = table.Column<string>(nullable: true),
                    WebSite = table.Column<string>(nullable: true),
                    EmailAddress = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GolfClub", x => x.GolfClubId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GolfClub");

            migrationBuilder.CreateTable(
                name: "ClubInformation",
                columns: table => new
                {
                    ClubConfigurationId = table.Column<Guid>(nullable: false),
                    AddressLine1 = table.Column<string>(nullable: true),
                    AddressLine2 = table.Column<string>(nullable: true),
                    EmailAddress = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    PostalCode = table.Column<string>(nullable: true),
                    Region = table.Column<string>(nullable: true),
                    TelephoneNumber = table.Column<string>(nullable: true),
                    Town = table.Column<string>(nullable: true),
                    WebSite = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubInformation", x => x.ClubConfigurationId);
                });
        }
    }
}
