using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Galaxon.Calendars.SpaceCalendars.com.Migrations
{
    public partial class AddedPublishedFlag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Icon",
                table: "Documents");

            migrationBuilder.AddColumn<bool>(
                name: "Published",
                table: "Documents",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Published",
                table: "Documents");

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
