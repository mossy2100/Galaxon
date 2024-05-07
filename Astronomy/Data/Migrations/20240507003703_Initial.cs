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
                    Orbit = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DateTimeUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DateTimeUtcUsno = table.Column<DateTime>(type: "datetime(6)", nullable: true)
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
                    FolderId = table.Column<int>(type: "int", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documents_Documents_FolderId",
                        column: x => x.FolderId,
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
                    Date = table.Column<DateTime>(type: "DATE", nullable: false)
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
                    DateTimeParsed = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Value = table.Column<sbyte>(type: "tinyint", nullable: false),
                    LeapSecondDate = table.Column<DateTime>(type: "DATE", nullable: true)
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
                    LeapSecondDate = table.Column<DateTime>(type: "DATE", nullable: false)
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
                    Lunation = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DateTimeUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
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
                    Type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DateTimeUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DateTimeUtcUsno = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeasonalMarkers", x => x.Id);
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
                name: "AtmosphereRecords",
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
                    table.PrimaryKey("PK_AtmosphereRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AtmosphereRecords_AstroObjects_AstroObjectId",
                        column: x => x.AstroObjectId,
                        principalTable: "AstroObjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ObservationalRecords",
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
                    table.PrimaryKey("PK_ObservationalRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ObservationalRecords_AstroObjects_AstroObjectId",
                        column: x => x.AstroObjectId,
                        principalTable: "AstroObjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OrbitalRecords",
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
                    table.PrimaryKey("PK_OrbitalRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrbitalRecords_AstroObjects_AstroObjectId",
                        column: x => x.AstroObjectId,
                        principalTable: "AstroObjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PhysicalRecords",
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
                    table.PrimaryKey("PK_PhysicalRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhysicalRecords_AstroObjects_AstroObjectId",
                        column: x => x.AstroObjectId,
                        principalTable: "AstroObjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RotationalRecords",
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
                    table.PrimaryKey("PK_RotationalRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RotationalRecords_AstroObjects_AstroObjectId",
                        column: x => x.AstroObjectId,
                        principalTable: "AstroObjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "StellarRecords",
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
                    table.PrimaryKey("PK_StellarRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StellarRecords_AstroObjects_AstroObjectId",
                        column: x => x.AstroObjectId,
                        principalTable: "AstroObjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "VSOP87DRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AstroObjectId = table.Column<int>(type: "int", nullable: false),
                    Variable = table.Column<string>(type: "varchar(1)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Exponent = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    Index = table.Column<ushort>(type: "smallint unsigned", nullable: false),
                    Amplitude = table.Column<double>(type: "double", nullable: false),
                    Phase = table.Column<double>(type: "double", nullable: false),
                    Frequency = table.Column<double>(type: "double", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VSOP87DRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VSOP87DRecords_AstroObjects_AstroObjectId",
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
                    _MoleculeRecordId = table.Column<int>(type: "int", nullable: false),
                    Percentage = table.Column<double>(type: "double", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AtmosphereConstituents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AtmosphereConstituents_AtmosphereRecords_AtmosphereId",
                        column: x => x.AtmosphereId,
                        principalTable: "AtmosphereRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AtmosphereConstituents_Molecules__MoleculeRecordId",
                        column: x => x._MoleculeRecordId,
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
                name: "IX_AtmosphereConstituents__MoleculeRecordId",
                table: "AtmosphereConstituents",
                column: "_MoleculeRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_AtmosphereConstituents_AtmosphereId",
                table: "AtmosphereConstituents",
                column: "AtmosphereId");

            migrationBuilder.CreateIndex(
                name: "IX_AtmosphereRecords_AstroObjectId",
                table: "AtmosphereRecords",
                column: "AstroObjectId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Documents_FolderId",
                table: "Documents",
                column: "FolderId");

            migrationBuilder.CreateIndex(
                name: "IX_IersBulletinCs_BulletinNumber",
                table: "IersBulletinCs",
                column: "BulletinNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ObservationalRecords_AstroObjectId",
                table: "ObservationalRecords",
                column: "AstroObjectId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrbitalRecords_AstroObjectId",
                table: "OrbitalRecords",
                column: "AstroObjectId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalRecords_AstroObjectId",
                table: "PhysicalRecords",
                column: "AstroObjectId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RotationalRecords_AstroObjectId",
                table: "RotationalRecords",
                column: "AstroObjectId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StellarRecords_AstroObjectId",
                table: "StellarRecords",
                column: "AstroObjectId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VSOP87DRecords_AstroObjectId",
                table: "VSOP87DRecords",
                column: "AstroObjectId");
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
                name: "ObservationalRecords");

            migrationBuilder.DropTable(
                name: "OrbitalRecords");

            migrationBuilder.DropTable(
                name: "PhysicalRecords");

            migrationBuilder.DropTable(
                name: "RotationalRecords");

            migrationBuilder.DropTable(
                name: "SeasonalMarkers");

            migrationBuilder.DropTable(
                name: "StellarRecords");

            migrationBuilder.DropTable(
                name: "VSOP87DRecords");

            migrationBuilder.DropTable(
                name: "AstroObjectGroups");

            migrationBuilder.DropTable(
                name: "AtmosphereRecords");

            migrationBuilder.DropTable(
                name: "Molecules");

            migrationBuilder.DropTable(
                name: "AstroObjects");
        }
    }
}
