using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Galaxon.Astronomy.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovedDeltaTRecordsAddedApsides : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeltaTRecords");

            migrationBuilder.RenameColumn(
                name: "MarkerNumber",
                table: "SeasonalMarkers",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "PhaseType",
                table: "LunarPhases",
                newName: "Type");

            migrationBuilder.CreateTable(
                name: "Apsides",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<byte>(type: "tinyint", nullable: false),
                    DateTimeUTC = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apsides", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Apsides");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "SeasonalMarkers",
                newName: "MarkerNumber");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "LunarPhases",
                newName: "PhaseType");

            migrationBuilder.CreateTable(
                name: "DeltaTRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeltaT = table.Column<double>(type: "float", nullable: false),
                    Year = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeltaTRecords", x => x.Id);
                });
        }
    }
}
