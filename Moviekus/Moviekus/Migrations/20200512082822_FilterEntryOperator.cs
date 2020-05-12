using Microsoft.EntityFrameworkCore.Migrations;

namespace Moviekus.Migrations
{
    public partial class FilterEntryOperator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Operator",
                table: "FilterEntries",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Operator",
                table: "FilterEntries");
        }
    }
}
