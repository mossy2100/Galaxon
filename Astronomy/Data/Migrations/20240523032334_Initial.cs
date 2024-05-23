using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Galaxon.Astronomy.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Apsides",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AstroObjectId = table.Column<int>(type: "int", nullable: false),
                    ApsideNumber = table.Column<double>(type: "double", nullable: false),
                    ApsideType = table.Column<string>(type: "varchar(9)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DateTimeUtcGalaxon = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    RadiusGalaxon_AU = table.Column<double>(type: "double", nullable: true),
                    DateTimeUtcUsno = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DateTimeUtcAstroPixels = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    RadiusAstroPixels_AU = table.Column<double>(type: "double", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apsides", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AstroObjectGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ParentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AstroObjectGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AstroObjectGroups_AstroObjectGroups_ParentId",
                        column: x => x.ParentId,
                        principalTable: "AstroObjectGroups",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AstroObjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Number = table.Column<uint>(type: "int unsigned", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AstroObjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AstroObjects_AstroObjects_ParentId",
                        column: x => x.ParentId,
                        principalTable: "AstroObjects",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DeltaTRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DecimalYear = table.Column<decimal>(type: "decimal(7,3)", nullable: false),
                    DeltaT = table.Column<decimal>(type: "decimal(9,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeltaTRecords", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "tinytext", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsFolder = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Published = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Content = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documents_Documents_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Documents",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EasterDates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EasterDates", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "IersBulletinCs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BulletinNumber = table.Column<int>(type: "int", nullable: false),
                    BulletinUrl = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DatePublished = table.Column<DateTime>(type: "date", nullable: false),
                    DateTimeParsed = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Value = table.Column<sbyte>(type: "tinyint", nullable: false),
                    LeapSecondDate = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IersBulletinCs", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LeapSeconds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Value = table.Column<sbyte>(type: "tinyint", nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeapSeconds", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LunarPhases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LunationNumber = table.Column<int>(type: "int", nullable: false),
                    PhaseType = table.Column<string>(type: "varchar(12)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DateTimeUtcGalaxon = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DateTimeUtcAstroPixels = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DateTimeUtcUsno = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LunarPhases", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Molecules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Symbol = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Molecules", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SeasonalMarkers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AstroObjectId = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    SeasonalMarkerType = table.Column<string>(type: "varchar(16)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DateTimeUtcGalaxon = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DateTimeUtcAstroPixels = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DateTimeUtcUsno = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeasonalMarkers", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "VSOP87D",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CodeOfBody = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    IndexOfCoordinate = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    Exponent = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    Rank = table.Column<ushort>(type: "smallint unsigned", nullable: false),
                    Amplitude = table.Column<decimal>(type: "decimal(18,11)", nullable: false),
                    Phase = table.Column<decimal>(type: "decimal(14,11)", nullable: false),
                    Frequency = table.Column<decimal>(type: "decimal(20,11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VSOP87D", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AstroObjectGroupRecordAstroObjectRecord",
                columns: table => new
                {
                    GroupsId = table.Column<int>(type: "int", nullable: false),
                    ObjectsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AstroObjectGroupRecordAstroObjectRecord", x => new { x.GroupsId, x.ObjectsId });
                    table.ForeignKey(
                        name: "FK_AstroObjectGroupRecordAstroObjectRecord_AstroObjectGroups_Gr~",
                        column: x => x.GroupsId,
                        principalTable: "AstroObjectGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AstroObjectGroupRecordAstroObjectRecord_AstroObjects_Objects~",
                        column: x => x.ObjectsId,
                        principalTable: "AstroObjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Atmospheres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AstroObjectId = table.Column<int>(type: "int", nullable: false),
                    SurfacePressure = table.Column<double>(type: "double", nullable: true),
                    ScaleHeight = table.Column<double>(type: "double", nullable: true),
                    IsSurfaceBoundedExosphere = table.Column<bool>(type: "tinyint(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Atmospheres", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Atmospheres_AstroObjects_AstroObjectId",
                        column: x => x.AstroObjectId,
                        principalTable: "AstroObjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Observationals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AstroObjectId = table.Column<int>(type: "int", nullable: false),
                    AbsMag = table.Column<double>(type: "double", nullable: true),
                    MinApparentMag = table.Column<double>(type: "double", nullable: true),
                    MaxApparentMag = table.Column<double>(type: "double", nullable: true),
                    MinAngularDiam = table.Column<double>(type: "double", nullable: true),
                    MaxAngularDiam = table.Column<double>(type: "double", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Observationals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Observationals_AstroObjects_AstroObjectId",
                        column: x => x.AstroObjectId,
                        principalTable: "AstroObjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Orbitals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AstroObjectId = table.Column<int>(type: "int", nullable: false),
                    Epoch = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Eccentricity = table.Column<double>(type: "double", nullable: true),
                    SemiMajorAxis = table.Column<double>(type: "double", nullable: true),
                    Inclination = table.Column<double>(type: "double", nullable: true),
                    LongAscNode = table.Column<double>(type: "double", nullable: true),
                    ArgPeriapsis = table.Column<double>(type: "double", nullable: true),
                    MeanAnomaly = table.Column<double>(type: "double", nullable: true),
                    Apoapsis = table.Column<double>(type: "double", nullable: true),
                    Periapsis = table.Column<double>(type: "double", nullable: true),
                    SiderealOrbitPeriod = table.Column<double>(type: "double", nullable: true),
                    SynodicOrbitPeriod = table.Column<double>(type: "double", nullable: true),
                    AvgOrbitSpeed = table.Column<double>(type: "double", nullable: true),
                    MeanMotion = table.Column<double>(type: "double", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orbitals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orbitals_AstroObjects_AstroObjectId",
                        column: x => x.AstroObjectId,
                        principalTable: "AstroObjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Physicals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AstroObjectId = table.Column<int>(type: "int", nullable: false),
                    RadiusA = table.Column<double>(type: "double", nullable: true),
                    RadiusB = table.Column<double>(type: "double", nullable: true),
                    RadiusC = table.Column<double>(type: "double", nullable: true),
                    IsRound = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    MeanRadius = table.Column<double>(type: "double", nullable: true),
                    Flattening = table.Column<double>(type: "double", nullable: true),
                    SurfaceArea = table.Column<double>(type: "double", nullable: true),
                    Volume = table.Column<double>(type: "double", nullable: true),
                    Mass = table.Column<double>(type: "double", nullable: true),
                    Density = table.Column<double>(type: "double", nullable: true),
                    SurfaceGrav = table.Column<double>(type: "double", nullable: true),
                    EscapeVelocity = table.Column<double>(type: "double", nullable: true),
                    StdGravParam = table.Column<double>(type: "double", nullable: true),
                    MomentOfInertiaFactor = table.Column<double>(type: "double", nullable: true),
                    HasGlobalMagField = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    HasRingSystem = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    SolarIrradiance = table.Column<double>(type: "double", nullable: true),
                    GeometricAlbedo = table.Column<double>(type: "double", nullable: true),
                    ColorBV = table.Column<double>(type: "double", nullable: true),
                    ColorUB = table.Column<double>(type: "double", nullable: true),
                    MinSurfaceTemp = table.Column<double>(type: "double", nullable: true),
                    MeanSurfaceTemp = table.Column<double>(type: "double", nullable: true),
                    MaxSurfaceTemp = table.Column<double>(type: "double", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Physicals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Physicals_AstroObjects_AstroObjectId",
                        column: x => x.AstroObjectId,
                        principalTable: "AstroObjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Rotationals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AstroObjectId = table.Column<int>(type: "int", nullable: false),
                    SiderealRotationPeriod = table.Column<double>(type: "double", nullable: true),
                    SynodicRotationPeriod = table.Column<double>(type: "double", nullable: true),
                    EquatRotationVelocity = table.Column<double>(type: "double", nullable: true),
                    Obliquity = table.Column<double>(type: "double", nullable: true),
                    NorthPoleRightAscension = table.Column<double>(type: "double", nullable: true),
                    NorthPoleDeclination = table.Column<double>(type: "double", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rotationals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rotationals_AstroObjects_AstroObjectId",
                        column: x => x.AstroObjectId,
                        principalTable: "AstroObjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Stellars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AstroObjectId = table.Column<int>(type: "int", nullable: false),
                    SpectralClass = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Metallicity = table.Column<double>(type: "double", nullable: true),
                    Luminosity = table.Column<double>(type: "double", nullable: true),
                    Radiance = table.Column<double>(type: "double", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stellars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stellars_AstroObjects_AstroObjectId",
                        column: x => x.AstroObjectId,
                        principalTable: "AstroObjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AtmosphereConstituents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AtmosphereId = table.Column<int>(type: "int", nullable: false),
                    MoleculeId = table.Column<int>(type: "int", nullable: false),
                    Percentage = table.Column<double>(type: "double", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AtmosphereConstituents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AtmosphereConstituents_Atmospheres_AtmosphereId",
                        column: x => x.AtmosphereId,
                        principalTable: "Atmospheres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AtmosphereConstituents_Molecules_MoleculeId",
                        column: x => x.MoleculeId,
                        principalTable: "Molecules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AstroObjectGroupRecordAstroObjectRecord_ObjectsId",
                table: "AstroObjectGroupRecordAstroObjectRecord",
                column: "ObjectsId");

            migrationBuilder.CreateIndex(
                name: "IX_AstroObjectGroups_ParentId",
                table: "AstroObjectGroups",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_AstroObjects_ParentId",
                table: "AstroObjects",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_AtmosphereConstituents_AtmosphereId",
                table: "AtmosphereConstituents",
                column: "AtmosphereId");

            migrationBuilder.CreateIndex(
                name: "IX_AtmosphereConstituents_MoleculeId",
                table: "AtmosphereConstituents",
                column: "MoleculeId");

            migrationBuilder.CreateIndex(
                name: "IX_Atmospheres_AstroObjectId",
                table: "Atmospheres",
                column: "AstroObjectId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Documents_ParentId",
                table: "Documents",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_IersBulletinCs_BulletinNumber",
                table: "IersBulletinCs",
                column: "BulletinNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Observationals_AstroObjectId",
                table: "Observationals",
                column: "AstroObjectId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orbitals_AstroObjectId",
                table: "Orbitals",
                column: "AstroObjectId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Physicals_AstroObjectId",
                table: "Physicals",
                column: "AstroObjectId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rotationals_AstroObjectId",
                table: "Rotationals",
                column: "AstroObjectId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stellars_AstroObjectId",
                table: "Stellars",
                column: "AstroObjectId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VSOP87D_CodeOfBody_IndexOfCoordinate_Exponent_Rank",
                table: "VSOP87D",
                columns: new[] { "CodeOfBody", "IndexOfCoordinate", "Exponent", "Rank" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Apsides");

            migrationBuilder.DropTable(
                name: "AstroObjectGroupRecordAstroObjectRecord");

            migrationBuilder.DropTable(
                name: "AtmosphereConstituents");

            migrationBuilder.DropTable(
                name: "DeltaTRecords");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "EasterDates");

            migrationBuilder.DropTable(
                name: "IersBulletinCs");

            migrationBuilder.DropTable(
                name: "LeapSeconds");

            migrationBuilder.DropTable(
                name: "LunarPhases");

            migrationBuilder.DropTable(
                name: "Observationals");

            migrationBuilder.DropTable(
                name: "Orbitals");

            migrationBuilder.DropTable(
                name: "Physicals");

            migrationBuilder.DropTable(
                name: "Rotationals");

            migrationBuilder.DropTable(
                name: "SeasonalMarkers");

            migrationBuilder.DropTable(
                name: "Stellars");

            migrationBuilder.DropTable(
                name: "VSOP87D");

            migrationBuilder.DropTable(
                name: "AstroObjectGroups");

            migrationBuilder.DropTable(
                name: "Atmospheres");

            migrationBuilder.DropTable(
                name: "Molecules");

            migrationBuilder.DropTable(
                name: "AstroObjects");
        }
    }
}
