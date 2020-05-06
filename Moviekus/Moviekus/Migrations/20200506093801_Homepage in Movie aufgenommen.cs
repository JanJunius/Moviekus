using Microsoft.EntityFrameworkCore.Migrations;

namespace Moviekus.Migrations
{
    public partial class HomepageinMovieaufgenommen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Homepage",
                table: "Movies",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Homepage",
                table: "Movies");
        }
    }
}
