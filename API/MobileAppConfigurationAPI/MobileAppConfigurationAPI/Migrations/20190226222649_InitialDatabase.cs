using Microsoft.EntityFrameworkCore.Migrations;

namespace MobileAppConfigurationAPI.Migrations
{
    public partial class InitialDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationConfiguration",
                columns: table => new
                {
                    IMEINumber = table.Column<string>(nullable: false),
                    SecurityServiceUri = table.Column<string>(nullable: true),
                    ManagementApiUri = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationConfiguration", x => x.IMEINumber);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationConfiguration");
        }
    }
}
