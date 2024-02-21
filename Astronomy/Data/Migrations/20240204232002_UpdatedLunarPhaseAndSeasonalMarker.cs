using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Galaxon.Astronomy.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedLunarPhaseAndSeasonalMarker : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UtcDateTime",
                table: "SeasonalMarkers",
                newName: "DateTimeUTC");

            migrationBuilder.RenameColumn(
                name: "UtcDateTime",
                table: "LunarPhases",
                newName: "DateTimeUTC");

            migrationBuilder.RenameColumn(
                name: "PhaseNumber",
                table: "LunarPhases",
                newName: "PhaseType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateTimeUTC",
                table: "SeasonalMarkers",
                newName: "UtcDateTime");

            migrationBuilder.RenameColumn(
                name: "PhaseType",
                table: "LunarPhases",
                newName: "PhaseNumber");

            migrationBuilder.RenameColumn(
                name: "DateTimeUTC",
                table: "LunarPhases",
                newName: "UtcDateTime");
        }
    }
}
