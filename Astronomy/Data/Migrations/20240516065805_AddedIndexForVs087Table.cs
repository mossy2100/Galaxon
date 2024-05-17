using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Galaxon.Astronomy.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedIndexForVs087Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_VSOP87D_CodeOfBody_IndexOfCoordinate_Exponent_Rank",
                table: "VSOP87D",
                columns: new[] { "CodeOfBody", "IndexOfCoordinate", "Exponent", "Rank" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VSOP87D_CodeOfBody_IndexOfCoordinate_Exponent_Rank",
                table: "VSOP87D");
        }
    }
}
