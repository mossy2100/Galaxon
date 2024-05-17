using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Galaxon.Astronomy.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovedForeignKeyFromAstroObjectToVsop87Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AstroObjectRecordId",
                table: "VSOP87D",
                type: "int",
                nullable: true);

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
    }
}
