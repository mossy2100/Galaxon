using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Galaxon.Astronomy.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedColumnNamesAndTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AtmosphereConstituents_AtmosphereRecords_AtmosphereId",
                table: "AtmosphereConstituents");

            migrationBuilder.DropForeignKey(
                name: "FK_AtmosphereConstituents_Molecules__MoleculeRecordId",
                table: "AtmosphereConstituents");

            migrationBuilder.DropForeignKey(
                name: "FK_AtmosphereRecords_AstroObjects_AstroObjectId",
                table: "AtmosphereRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Documents_FolderId",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_ObservationalRecords_AstroObjects_AstroObjectId",
                table: "ObservationalRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_OrbitalRecords_AstroObjects_AstroObjectId",
                table: "OrbitalRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_PhysicalRecords_AstroObjects_AstroObjectId",
                table: "PhysicalRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_RotationalRecords_AstroObjects_AstroObjectId",
                table: "RotationalRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_StellarRecords_AstroObjects_AstroObjectId",
                table: "StellarRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_VSOP87DRecords_AstroObjects_AstroObjectId",
                table: "VSOP87DRecords");

            migrationBuilder.DropIndex(
                name: "IX_AtmosphereConstituents__MoleculeRecordId",
                table: "AtmosphereConstituents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VSOP87DRecords",
                table: "VSOP87DRecords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StellarRecords",
                table: "StellarRecords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RotationalRecords",
                table: "RotationalRecords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PhysicalRecords",
                table: "PhysicalRecords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrbitalRecords",
                table: "OrbitalRecords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ObservationalRecords",
                table: "ObservationalRecords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AtmosphereRecords",
                table: "AtmosphereRecords");

            migrationBuilder.DropColumn(
                name: "_MoleculeRecordId",
                table: "AtmosphereConstituents");

            migrationBuilder.RenameTable(
                name: "VSOP87DRecords",
                newName: "VSOP87D");

            migrationBuilder.RenameTable(
                name: "StellarRecords",
                newName: "Stellars");

            migrationBuilder.RenameTable(
                name: "RotationalRecords",
                newName: "Rotationals");

            migrationBuilder.RenameTable(
                name: "PhysicalRecords",
                newName: "Physicals");

            migrationBuilder.RenameTable(
                name: "OrbitalRecords",
                newName: "Orbitals");

            migrationBuilder.RenameTable(
                name: "ObservationalRecords",
                newName: "Observationals");

            migrationBuilder.RenameTable(
                name: "AtmosphereRecords",
                newName: "Atmospheres");

            migrationBuilder.RenameColumn(
                name: "FolderId",
                table: "Documents",
                newName: "ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_Documents_FolderId",
                table: "Documents",
                newName: "IX_Documents_ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_VSOP87DRecords_AstroObjectId",
                table: "VSOP87D",
                newName: "IX_VSOP87D_AstroObjectId");

            migrationBuilder.RenameIndex(
                name: "IX_StellarRecords_AstroObjectId",
                table: "Stellars",
                newName: "IX_Stellars_AstroObjectId");

            migrationBuilder.RenameIndex(
                name: "IX_RotationalRecords_AstroObjectId",
                table: "Rotationals",
                newName: "IX_Rotationals_AstroObjectId");

            migrationBuilder.RenameIndex(
                name: "IX_PhysicalRecords_AstroObjectId",
                table: "Physicals",
                newName: "IX_Physicals_AstroObjectId");

            migrationBuilder.RenameIndex(
                name: "IX_OrbitalRecords_AstroObjectId",
                table: "Orbitals",
                newName: "IX_Orbitals_AstroObjectId");

            migrationBuilder.RenameIndex(
                name: "IX_ObservationalRecords_AstroObjectId",
                table: "Observationals",
                newName: "IX_Observationals_AstroObjectId");

            migrationBuilder.RenameIndex(
                name: "IX_AtmosphereRecords_AstroObjectId",
                table: "Atmospheres",
                newName: "IX_Atmospheres_AstroObjectId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "LeapSeconds",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "DATE");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LeapSecondDate",
                table: "IersBulletinCs",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "DATE",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "EasterDates",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "DATE");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VSOP87D",
                table: "VSOP87D",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Stellars",
                table: "Stellars",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rotationals",
                table: "Rotationals",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Physicals",
                table: "Physicals",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orbitals",
                table: "Orbitals",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Observationals",
                table: "Observationals",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Atmospheres",
                table: "Atmospheres",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AtmosphereConstituents_MoleculeId",
                table: "AtmosphereConstituents",
                column: "MoleculeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AtmosphereConstituents_Atmospheres_AtmosphereId",
                table: "AtmosphereConstituents",
                column: "AtmosphereId",
                principalTable: "Atmospheres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AtmosphereConstituents_Molecules_MoleculeId",
                table: "AtmosphereConstituents",
                column: "MoleculeId",
                principalTable: "Molecules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Atmospheres_AstroObjects_AstroObjectId",
                table: "Atmospheres",
                column: "AstroObjectId",
                principalTable: "AstroObjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Documents_ParentId",
                table: "Documents",
                column: "ParentId",
                principalTable: "Documents",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Observationals_AstroObjects_AstroObjectId",
                table: "Observationals",
                column: "AstroObjectId",
                principalTable: "AstroObjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orbitals_AstroObjects_AstroObjectId",
                table: "Orbitals",
                column: "AstroObjectId",
                principalTable: "AstroObjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Physicals_AstroObjects_AstroObjectId",
                table: "Physicals",
                column: "AstroObjectId",
                principalTable: "AstroObjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rotationals_AstroObjects_AstroObjectId",
                table: "Rotationals",
                column: "AstroObjectId",
                principalTable: "AstroObjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stellars_AstroObjects_AstroObjectId",
                table: "Stellars",
                column: "AstroObjectId",
                principalTable: "AstroObjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VSOP87D_AstroObjects_AstroObjectId",
                table: "VSOP87D",
                column: "AstroObjectId",
                principalTable: "AstroObjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AtmosphereConstituents_Atmospheres_AtmosphereId",
                table: "AtmosphereConstituents");

            migrationBuilder.DropForeignKey(
                name: "FK_AtmosphereConstituents_Molecules_MoleculeId",
                table: "AtmosphereConstituents");

            migrationBuilder.DropForeignKey(
                name: "FK_Atmospheres_AstroObjects_AstroObjectId",
                table: "Atmospheres");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Documents_ParentId",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_Observationals_AstroObjects_AstroObjectId",
                table: "Observationals");

            migrationBuilder.DropForeignKey(
                name: "FK_Orbitals_AstroObjects_AstroObjectId",
                table: "Orbitals");

            migrationBuilder.DropForeignKey(
                name: "FK_Physicals_AstroObjects_AstroObjectId",
                table: "Physicals");

            migrationBuilder.DropForeignKey(
                name: "FK_Rotationals_AstroObjects_AstroObjectId",
                table: "Rotationals");

            migrationBuilder.DropForeignKey(
                name: "FK_Stellars_AstroObjects_AstroObjectId",
                table: "Stellars");

            migrationBuilder.DropForeignKey(
                name: "FK_VSOP87D_AstroObjects_AstroObjectId",
                table: "VSOP87D");

            migrationBuilder.DropIndex(
                name: "IX_AtmosphereConstituents_MoleculeId",
                table: "AtmosphereConstituents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VSOP87D",
                table: "VSOP87D");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Stellars",
                table: "Stellars");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rotationals",
                table: "Rotationals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Physicals",
                table: "Physicals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orbitals",
                table: "Orbitals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Observationals",
                table: "Observationals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Atmospheres",
                table: "Atmospheres");

            migrationBuilder.RenameTable(
                name: "VSOP87D",
                newName: "VSOP87DRecords");

            migrationBuilder.RenameTable(
                name: "Stellars",
                newName: "StellarRecords");

            migrationBuilder.RenameTable(
                name: "Rotationals",
                newName: "RotationalRecords");

            migrationBuilder.RenameTable(
                name: "Physicals",
                newName: "PhysicalRecords");

            migrationBuilder.RenameTable(
                name: "Orbitals",
                newName: "OrbitalRecords");

            migrationBuilder.RenameTable(
                name: "Observationals",
                newName: "ObservationalRecords");

            migrationBuilder.RenameTable(
                name: "Atmospheres",
                newName: "AtmosphereRecords");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "Documents",
                newName: "FolderId");

            migrationBuilder.RenameIndex(
                name: "IX_Documents_ParentId",
                table: "Documents",
                newName: "IX_Documents_FolderId");

            migrationBuilder.RenameIndex(
                name: "IX_VSOP87D_AstroObjectId",
                table: "VSOP87DRecords",
                newName: "IX_VSOP87DRecords_AstroObjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Stellars_AstroObjectId",
                table: "StellarRecords",
                newName: "IX_StellarRecords_AstroObjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Rotationals_AstroObjectId",
                table: "RotationalRecords",
                newName: "IX_RotationalRecords_AstroObjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Physicals_AstroObjectId",
                table: "PhysicalRecords",
                newName: "IX_PhysicalRecords_AstroObjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Orbitals_AstroObjectId",
                table: "OrbitalRecords",
                newName: "IX_OrbitalRecords_AstroObjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Observationals_AstroObjectId",
                table: "ObservationalRecords",
                newName: "IX_ObservationalRecords_AstroObjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Atmospheres_AstroObjectId",
                table: "AtmosphereRecords",
                newName: "IX_AtmosphereRecords_AstroObjectId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "LeapSeconds",
                type: "DATE",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LeapSecondDate",
                table: "IersBulletinCs",
                type: "DATE",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "EasterDates",
                type: "DATE",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AddColumn<int>(
                name: "_MoleculeRecordId",
                table: "AtmosphereConstituents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_VSOP87DRecords",
                table: "VSOP87DRecords",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StellarRecords",
                table: "StellarRecords",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RotationalRecords",
                table: "RotationalRecords",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PhysicalRecords",
                table: "PhysicalRecords",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrbitalRecords",
                table: "OrbitalRecords",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ObservationalRecords",
                table: "ObservationalRecords",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AtmosphereRecords",
                table: "AtmosphereRecords",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AtmosphereConstituents__MoleculeRecordId",
                table: "AtmosphereConstituents",
                column: "_MoleculeRecordId");

            migrationBuilder.AddForeignKey(
                name: "FK_AtmosphereConstituents_AtmosphereRecords_AtmosphereId",
                table: "AtmosphereConstituents",
                column: "AtmosphereId",
                principalTable: "AtmosphereRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AtmosphereConstituents_Molecules__MoleculeRecordId",
                table: "AtmosphereConstituents",
                column: "_MoleculeRecordId",
                principalTable: "Molecules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AtmosphereRecords_AstroObjects_AstroObjectId",
                table: "AtmosphereRecords",
                column: "AstroObjectId",
                principalTable: "AstroObjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Documents_FolderId",
                table: "Documents",
                column: "FolderId",
                principalTable: "Documents",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ObservationalRecords_AstroObjects_AstroObjectId",
                table: "ObservationalRecords",
                column: "AstroObjectId",
                principalTable: "AstroObjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrbitalRecords_AstroObjects_AstroObjectId",
                table: "OrbitalRecords",
                column: "AstroObjectId",
                principalTable: "AstroObjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PhysicalRecords_AstroObjects_AstroObjectId",
                table: "PhysicalRecords",
                column: "AstroObjectId",
                principalTable: "AstroObjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RotationalRecords_AstroObjects_AstroObjectId",
                table: "RotationalRecords",
                column: "AstroObjectId",
                principalTable: "AstroObjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StellarRecords_AstroObjects_AstroObjectId",
                table: "StellarRecords",
                column: "AstroObjectId",
                principalTable: "AstroObjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VSOP87DRecords_AstroObjects_AstroObjectId",
                table: "VSOP87DRecords",
                column: "AstroObjectId",
                principalTable: "AstroObjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
