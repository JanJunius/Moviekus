using Microsoft.EntityFrameworkCore.Migrations;

namespace Moviekus.Migrations
{
    public partial class DefaultFilter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"PRAGMA foreign_keys = 0;
                CREATE TABLE temp_table AS SELECT * FROM Settings;
                DROP TABLE Settings;
                CREATE TABLE Settings(Id TEXT NOT NULL CONSTRAINT PK_Settings PRIMARY KEY, MovieDb_ApiKey TEXT, MovieDb_Language TEXT);
                INSERT INTO Settings(Id, MovieDb_ApiKey, MovieDb_Language) SELECT Id, MovieDb_ApiKey, MovieDb_Language FROM temp_table;
                DROP TABLE temp_table;
                PRAGMA foreign_keys = 1; ");    

            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "Filter",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "Filter");

            migrationBuilder.AddColumn<string>(
                name: "ImDb_ApiKey",
                table: "Settings",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImDb_Language",
                table: "Settings",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OneDrive_ApplicationId",
                table: "Settings",
                type: "TEXT",
                nullable: true);
        }
    }
}
