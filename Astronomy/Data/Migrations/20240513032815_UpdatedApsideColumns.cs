using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Galaxon.Astronomy.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedApsideColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Orbit",
                table: "Apsides");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Apsides");

            migrationBuilder.RenameColumn(
                name: "RadiusAuGalaxon",
                table: "Apsides",
                newName: "RadiusGalaxon_AU");

            migrationBuilder.RenameColumn(
                name: "RadiusAuAstroPixels",
                table: "Apsides",
                newName: "RadiusAstroPixels_AU");

            migrationBuilder.AddColumn<double>(
                name: "ApsideNumber",
                table: "Apsides",
                type: "double",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApsideNumber",
                table: "Apsides");

            migrationBuilder.RenameColumn(
                name: "RadiusGalaxon_AU",
                table: "Apsides",
                newName: "RadiusAuGalaxon");

            migrationBuilder.RenameColumn(
                name: "RadiusAstroPixels_AU",
                table: "Apsides",
                newName: "RadiusAuAstroPixels");

            migrationBuilder.AddColumn<int>(
                name: "Orbit",
                table: "Apsides",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Apsides",
                type: "varchar(20)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
