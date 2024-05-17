using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Galaxon.Astronomy.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedLunarPhaseColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "LunarPhases",
                newName: "PhaseType");

            migrationBuilder.RenameColumn(
                name: "Lunation",
                table: "LunarPhases",
                newName: "LunationNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhaseType",
                table: "LunarPhases",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "LunationNumber",
                table: "LunarPhases",
                newName: "Lunation");
        }
    }
}
