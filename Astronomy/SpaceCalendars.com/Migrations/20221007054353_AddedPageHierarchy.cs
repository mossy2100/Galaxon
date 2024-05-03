using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Galaxon.Calendars.SpaceCalendars.com.Migrations
{
    public partial class AddedPageHierarchy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PathAlias",
                table: "Documents");

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "Documents",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Documents_ParentId",
                table: "Documents",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Documents_ParentId",
                table: "Documents",
                column: "ParentId",
                principalTable: "Documents",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Documents_ParentId",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Documents_ParentId",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Documents");

            migrationBuilder.AddColumn<string>(
                name: "PathAlias",
                table: "Documents",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
