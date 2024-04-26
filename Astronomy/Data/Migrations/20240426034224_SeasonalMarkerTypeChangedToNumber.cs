using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Galaxon.Astronomy.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeasonalMarkerTypeChangedToNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "SeasonalMarkers",
                newName: "MarkerNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MarkerNumber",
                table: "SeasonalMarkers",
                newName: "Type");
        }
    }
}
