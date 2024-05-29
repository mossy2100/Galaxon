using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Galaxon.Astronomy.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangedUnits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EquatorialRotationalVelocity_km_s",
                table: "Rotationals",
                newName: "EquatorialRotationalVelocity_m_s");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EquatorialRotationalVelocity_m_s",
                table: "Rotationals",
                newName: "EquatorialRotationalVelocity_km_s");
        }
    }
}
