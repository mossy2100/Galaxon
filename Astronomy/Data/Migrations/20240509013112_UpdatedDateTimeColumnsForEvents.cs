using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Galaxon.Astronomy.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedDateTimeColumnsForEvents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateTimeUtc",
                table: "SeasonalMarkers",
                newName: "DateTimeUtcGalaxon");

            migrationBuilder.RenameColumn(
                name: "DateTimeUtc",
                table: "LunarPhases",
                newName: "DateTimeUtcGalaxon");

            migrationBuilder.RenameColumn(
                name: "DateTimeUtc",
                table: "Apsides",
                newName: "DateTimeUtcGalaxon");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTimeUtcAstroPixels",
                table: "SeasonalMarkers",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTimeUtcAstroPixels",
                table: "Apsides",
                type: "datetime(6)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateTimeUtcAstroPixels",
                table: "SeasonalMarkers");

            migrationBuilder.DropColumn(
                name: "DateTimeUtcAstroPixels",
                table: "Apsides");

            migrationBuilder.RenameColumn(
                name: "DateTimeUtcGalaxon",
                table: "SeasonalMarkers",
                newName: "DateTimeUtc");

            migrationBuilder.RenameColumn(
                name: "DateTimeUtcGalaxon",
                table: "LunarPhases",
                newName: "DateTimeUtc");

            migrationBuilder.RenameColumn(
                name: "DateTimeUtcGalaxon",
                table: "Apsides",
                newName: "DateTimeUtc");
        }
    }
}
