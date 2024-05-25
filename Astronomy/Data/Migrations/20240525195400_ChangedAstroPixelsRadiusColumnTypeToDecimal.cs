using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Galaxon.Astronomy.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangedAstroPixelsRadiusColumnTypeToDecimal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SeasonalMarkerType",
                table: "SeasonalMarkers",
                newName: "MarkerType");

            migrationBuilder.AlterColumn<decimal>(
                name: "RadiusAstroPixels_AU",
                table: "Apsides",
                type: "decimal(8,7)",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MarkerType",
                table: "SeasonalMarkers",
                newName: "SeasonalMarkerType");

            migrationBuilder.AlterColumn<double>(
                name: "RadiusAstroPixels_AU",
                table: "Apsides",
                type: "double",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,7)",
                oldNullable: true);
        }
    }
}
