using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Galaxon.Astronomy.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedUnitsAndAddedProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Density_g_cm3",
                table: "Physicals",
                newName: "SurfaceEquivalentDoseRate_microSv_h");

            migrationBuilder.AddColumn<double>(
                name: "Density_kg_m3",
                table: "Physicals",
                type: "double",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WikipediaUrl",
                table: "AstroObjects",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Density_kg_m3",
                table: "Physicals");

            migrationBuilder.DropColumn(
                name: "WikipediaUrl",
                table: "AstroObjects");

            migrationBuilder.RenameColumn(
                name: "SurfaceEquivalentDoseRate_microSv_h",
                table: "Physicals",
                newName: "Density_g_cm3");
        }
    }
}
