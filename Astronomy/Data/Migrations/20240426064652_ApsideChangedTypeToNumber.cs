using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Galaxon.Astronomy.Data.Migrations
{
    /// <inheritdoc />
    public partial class ApsideChangedTypeToNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Apsides",
                newName: "ApsideNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ApsideNumber",
                table: "Apsides",
                newName: "Type");
        }
    }
}
