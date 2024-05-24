using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Galaxon.Astronomy.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangedApsideColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApsideNumber",
                table: "Apsides");

            migrationBuilder.AddColumn<int>(
                name: "OrbitNumber",
                table: "Apsides",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrbitNumber",
                table: "Apsides");

            migrationBuilder.AddColumn<double>(
                name: "ApsideNumber",
                table: "Apsides",
                type: "double",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
