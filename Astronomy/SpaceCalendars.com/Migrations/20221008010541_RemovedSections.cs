using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Galaxon.Calendars.SpaceCalendars.com.Migrations
{
    public partial class RemovedSections : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Documents_ParentId",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Sections_SectionId",
                table: "Documents");

            migrationBuilder.DropTable(
                name: "Sections");

            migrationBuilder.DropIndex(
                name: "IX_Documents_ParentId",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Documents");

            migrationBuilder.RenameColumn(
                name: "SectionId",
                table: "Documents",
                newName: "FolderId");

            migrationBuilder.RenameIndex(
                name: "IX_Documents_SectionId",
                table: "Documents",
                newName: "IX_Documents_FolderId");

            migrationBuilder.AddColumn<bool>(
                name: "IsFolder",
                table: "Documents",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Documents_FolderId",
                table: "Documents",
                column: "FolderId",
                principalTable: "Documents",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Documents_FolderId",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "IsFolder",
                table: "Documents");

            migrationBuilder.RenameColumn(
                name: "FolderId",
                table: "Documents",
                newName: "SectionId");

            migrationBuilder.RenameIndex(
                name: "IX_Documents_FolderId",
                table: "Documents",
                newName: "IX_Documents_SectionId");

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "Documents",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Sections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sections", x => x.Id);
                });

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

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Sections_SectionId",
                table: "Documents",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id");
        }
    }
}
