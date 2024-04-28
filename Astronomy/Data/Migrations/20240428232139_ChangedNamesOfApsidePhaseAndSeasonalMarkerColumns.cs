using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Galaxon.Astronomy.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangedNamesOfApsidePhaseAndSeasonalMarkerColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MarkerNumber",
                table: "SeasonalMarkers",
                newName: "Marker");

            migrationBuilder.RenameColumn(
                name: "PhaseNumber",
                table: "LunarPhases",
                newName: "Phase");

            migrationBuilder.RenameColumn(
                name: "ApsideNumber",
                table: "Apsides",
                newName: "Apside");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Marker",
                table: "SeasonalMarkers",
                newName: "MarkerNumber");

            migrationBuilder.RenameColumn(
                name: "Phase",
                table: "LunarPhases",
                newName: "PhaseNumber");

            migrationBuilder.RenameColumn(
                name: "Apside",
                table: "Apsides",
                newName: "ApsideNumber");
        }
    }
}
