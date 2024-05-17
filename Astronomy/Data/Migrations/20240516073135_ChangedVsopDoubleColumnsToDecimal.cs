using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Galaxon.Astronomy.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangedVsopDoubleColumnsToDecimal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Phase",
                table: "VSOP87D",
                type: "decimal(14,11)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double");

            migrationBuilder.AlterColumn<decimal>(
                name: "Frequency",
                table: "VSOP87D",
                type: "decimal(20,11)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amplitude",
                table: "VSOP87D",
                type: "decimal(18,11)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Phase",
                table: "VSOP87D",
                type: "double",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(14,11)");

            migrationBuilder.AlterColumn<double>(
                name: "Frequency",
                table: "VSOP87D",
                type: "double",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,11)");

            migrationBuilder.AlterColumn<double>(
                name: "Amplitude",
                table: "VSOP87D",
                type: "double",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,11)");
        }
    }
}
