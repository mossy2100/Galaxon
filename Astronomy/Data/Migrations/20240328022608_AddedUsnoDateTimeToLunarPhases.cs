using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Galaxon.Astronomy.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedUsnoDateTimeToLunarPhases : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateTimeUTC",
                table: "LunarPhases");

            migrationBuilder.RenameColumn(
                name: "DateTimeUTC",
                table: "SeasonalMarkers",
                newName: "DateTimeUtcUsno");

            migrationBuilder.RenameColumn(
                name: "DateTimeUTC",
                table: "Apsides",
                newName: "DateTimeUtc");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTimeUtcAstroPixels",
                table: "LunarPhases",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTimeUtcUsno",
                table: "LunarPhases",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateTimeUtcAstroPixels",
                table: "LunarPhases");

            migrationBuilder.DropColumn(
                name: "DateTimeUtcUsno",
                table: "LunarPhases");

            migrationBuilder.RenameColumn(
                name: "DateTimeUtcUsno",
                table: "SeasonalMarkers",
                newName: "DateTimeUTC");

            migrationBuilder.RenameColumn(
                name: "DateTimeUtc",
                table: "Apsides",
                newName: "DateTimeUTC");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTimeUTC",
                table: "LunarPhases",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
