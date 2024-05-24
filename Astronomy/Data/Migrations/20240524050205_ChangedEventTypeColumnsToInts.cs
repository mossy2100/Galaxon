using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Galaxon.Astronomy.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangedEventTypeColumnsToInts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "SeasonalMarkerType",
                table: "SeasonalMarkers",
                type: "tinyint unsigned",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(16)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<byte>(
                name: "PhaseType",
                table: "LunarPhases",
                type: "tinyint unsigned",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(12)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<byte>(
                name: "ApsideType",
                table: "Apsides",
                type: "tinyint unsigned",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(9)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SeasonalMarkerType",
                table: "SeasonalMarkers",
                type: "varchar(16)",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint unsigned")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "PhaseType",
                table: "LunarPhases",
                type: "varchar(12)",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint unsigned")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "ApsideType",
                table: "Apsides",
                type: "varchar(9)",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint unsigned")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
