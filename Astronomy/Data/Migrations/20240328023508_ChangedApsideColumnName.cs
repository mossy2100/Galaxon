using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Galaxon.Astronomy.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangedApsideColumnName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateTimeUtc",
                table: "Apsides",
                newName: "DateTimeUtcUsno");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateTimeUtcUsno",
                table: "Apsides",
                newName: "DateTimeUtc");
        }
    }
}
