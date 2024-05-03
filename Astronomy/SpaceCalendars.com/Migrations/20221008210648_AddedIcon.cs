using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Galaxon.Calendars.SpaceCalendars.com.Migrations
{
    public partial class AddedIcon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Icon",
                table: "Documents");
        }
    }
}
