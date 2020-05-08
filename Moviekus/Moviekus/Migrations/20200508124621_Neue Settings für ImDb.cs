using Microsoft.EntityFrameworkCore.Migrations;

namespace Moviekus.Migrations
{
    public partial class NeueSettingsfürImDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImDb_ApiKey",
                table: "Settings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImDb_Language",
                table: "Settings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OneDrive_ApplicationId",
                table: "Settings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImDb_ApiKey",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "ImDb_Language",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "OneDrive_ApplicationId",
                table: "Settings");
        }
    }
}
