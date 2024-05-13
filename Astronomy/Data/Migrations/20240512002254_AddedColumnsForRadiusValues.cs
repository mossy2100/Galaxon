using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Galaxon.Astronomy.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedColumnsForRadiusValues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "RadiusAuAstroPixels",
                table: "Apsides",
                type: "double",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "RadiusAuGalaxon",
                table: "Apsides",
                type: "double",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RadiusAuAstroPixels",
                table: "Apsides");

            migrationBuilder.DropColumn(
                name: "RadiusAuGalaxon",
                table: "Apsides");
        }
    }
}
