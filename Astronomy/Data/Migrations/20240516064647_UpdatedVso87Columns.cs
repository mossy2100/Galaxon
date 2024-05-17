using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Galaxon.Astronomy.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedVso87Columns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VSOP87D_AstroObjects_AstroObjectId",
                table: "VSOP87D");

            migrationBuilder.DropIndex(
                name: "IX_VSOP87D_AstroObjectId",
                table: "VSOP87D");

            migrationBuilder.DropColumn(
                name: "AstroObjectId",
                table: "VSOP87D");

            migrationBuilder.DropColumn(
                name: "Variable",
                table: "VSOP87D");

            migrationBuilder.RenameColumn(
                name: "Index",
                table: "VSOP87D",
                newName: "Rank");

            migrationBuilder.AddColumn<int>(
                name: "AstroObjectRecordId",
                table: "VSOP87D",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "CodeOfBody",
                table: "VSOP87D",
                type: "tinyint unsigned",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "IndexOfCoordinate",
                table: "VSOP87D",
                type: "tinyint unsigned",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.CreateIndex(
                name: "IX_VSOP87D_AstroObjectRecordId",
                table: "VSOP87D",
                column: "AstroObjectRecordId");

            migrationBuilder.AddForeignKey(
                name: "FK_VSOP87D_AstroObjects_AstroObjectRecordId",
                table: "VSOP87D",
                column: "AstroObjectRecordId",
                principalTable: "AstroObjects",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VSOP87D_AstroObjects_AstroObjectRecordId",
                table: "VSOP87D");

            migrationBuilder.DropIndex(
                name: "IX_VSOP87D_AstroObjectRecordId",
                table: "VSOP87D");

            migrationBuilder.DropColumn(
                name: "AstroObjectRecordId",
                table: "VSOP87D");

            migrationBuilder.DropColumn(
                name: "CodeOfBody",
                table: "VSOP87D");

            migrationBuilder.DropColumn(
                name: "IndexOfCoordinate",
                table: "VSOP87D");

            migrationBuilder.RenameColumn(
                name: "Rank",
                table: "VSOP87D",
                newName: "Index");

            migrationBuilder.AddColumn<int>(
                name: "AstroObjectId",
                table: "VSOP87D",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Variable",
                table: "VSOP87D",
                type: "varchar(1)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_VSOP87D_AstroObjectId",
                table: "VSOP87D",
                column: "AstroObjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_VSOP87D_AstroObjects_AstroObjectId",
                table: "VSOP87D",
                column: "AstroObjectId",
                principalTable: "AstroObjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
