﻿// <auto-generated />
using System;
using Galaxon.Astronomy.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Galaxon.Astronomy.Data.Migrations
{
    [DbContext(typeof(AstroDbContext))]
    [Migration("20240529085439_UpdatedUnitsAndAddedProperties")]
    partial class UpdatedUnitsAndAddedProperties
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("AstroObjectGroupRecordAstroObjectRecord", b =>
                {
                    b.Property<int>("GroupsId")
                        .HasColumnType("int");

                    b.Property<int>("ObjectsId")
                        .HasColumnType("int");

                    b.HasKey("GroupsId", "ObjectsId");

                    b.HasIndex("ObjectsId");

                    b.ToTable("AstroObjectGroupRecordAstroObjectRecord");
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.ApsideRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<byte>("ApsideType")
                        .HasColumnType("tinyint unsigned");

                    b.Property<int>("AstroObjectId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DateTimeUtcAstroPixels")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DateTimeUtcGalaxon")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DateTimeUtcUsno")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("OrbitNumber")
                        .HasColumnType("int");

                    b.Property<decimal?>("RadiusAstroPixels_AU")
                        .HasColumnType("decimal(8,7)");

                    b.Property<double?>("RadiusGalaxon_AU")
                        .HasColumnType("double");

                    b.HasKey("Id");

                    b.ToTable("Apsides");
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.AstroObjectGroupRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("varchar(30)");

                    b.Property<int?>("ParentId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("AstroObjectGroups");
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.AstroObjectRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.Property<uint?>("Number")
                        .HasColumnType("int unsigned");

                    b.Property<int?>("ParentId")
                        .HasColumnType("int");

                    b.Property<string>("WikipediaUrl")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("AstroObjects");
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.AtmosphereConstituentRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AtmosphereId")
                        .HasColumnType("int");

                    b.Property<int>("MoleculeId")
                        .HasColumnType("int");

                    b.Property<double?>("Percentage")
                        .HasColumnType("double");

                    b.HasKey("Id");

                    b.HasIndex("AtmosphereId");

                    b.HasIndex("MoleculeId");

                    b.ToTable("AtmosphereConstituents");
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.AtmosphereRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AstroObjectId")
                        .HasColumnType("int");

                    b.Property<bool?>("IsSurfaceBoundedExosphere")
                        .HasColumnType("tinyint(1)");

                    b.Property<double?>("ScaleHeight_km")
                        .HasColumnType("double");

                    b.Property<double?>("SurfacePressure_Pa")
                        .HasColumnType("double");

                    b.HasKey("Id");

                    b.HasIndex("AstroObjectId")
                        .IsUnique();

                    b.ToTable("Atmospheres");
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.DeltaTRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("DecimalYear")
                        .HasColumnType("decimal(6, 2)");

                    b.Property<decimal>("DeltaT")
                        .HasColumnType("decimal(9, 4)");

                    b.HasKey("Id");

                    b.ToTable("DeltaTRecords");
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.DocumentRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .HasColumnType("longtext");

                    b.Property<bool>("IsFolder")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.Property<int?>("ParentId")
                        .HasColumnType("int");

                    b.Property<bool>("Published")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("tinytext");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("Documents");
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.EasterDateRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("date");

                    b.HasKey("Id");

                    b.ToTable("EasterDates");
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.IersBulletinCRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BulletinNumber")
                        .HasColumnType("int");

                    b.Property<string>("BulletinUrl")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<DateTime>("DatePublished")
                        .HasColumnType("date");

                    b.Property<DateTime>("DateTimeParsed")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("LeapSecondDate")
                        .HasColumnType("date");

                    b.Property<sbyte>("Value")
                        .HasColumnType("tinyint");

                    b.HasKey("Id");

                    b.HasIndex("BulletinNumber")
                        .IsUnique();

                    b.ToTable("IersBulletinCs");
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.LeapSecondRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("date");

                    b.Property<sbyte>("Value")
                        .HasColumnType("tinyint");

                    b.HasKey("Id");

                    b.ToTable("LeapSeconds");
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.LunarPhaseRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("DateTimeUtcAstroPixels")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DateTimeUtcGalaxon")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DateTimeUtcUsno")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("LunationNumber")
                        .HasColumnType("int");

                    b.Property<byte>("PhaseType")
                        .HasColumnType("tinyint unsigned");

                    b.HasKey("Id");

                    b.ToTable("LunarPhases");
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.MoleculeRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.HasKey("Id");

                    b.ToTable("Molecules");
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.ObservationRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<double?>("AbsoluteMagnitude")
                        .HasColumnType("double");

                    b.Property<int>("AstroObjectId")
                        .HasColumnType("int");

                    b.Property<double?>("MaxAngularDiameter_deg")
                        .HasColumnType("double");

                    b.Property<double?>("MaxApparentMagnitude")
                        .HasColumnType("double");

                    b.Property<double?>("MinAngularDiameter_deg")
                        .HasColumnType("double");

                    b.Property<double?>("MinApparentMagnitude")
                        .HasColumnType("double");

                    b.HasKey("Id");

                    b.HasIndex("AstroObjectId")
                        .IsUnique();

                    b.ToTable("Observationals");
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.OrbitRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<double?>("Apoapsis_km")
                        .HasColumnType("double");

                    b.Property<double?>("ArgumentPeriapsis_deg")
                        .HasColumnType("double");

                    b.Property<int>("AstroObjectId")
                        .HasColumnType("int");

                    b.Property<double?>("AverageOrbitalSpeed_km_s")
                        .HasColumnType("double");

                    b.Property<double?>("Eccentricity")
                        .HasColumnType("double");

                    b.Property<DateTime?>("Epoch")
                        .HasColumnType("datetime(6)");

                    b.Property<double?>("Inclination_deg")
                        .HasColumnType("double");

                    b.Property<double?>("LongitudeAscendingNode_deg")
                        .HasColumnType("double");

                    b.Property<double?>("MeanAnomaly_deg")
                        .HasColumnType("double");

                    b.Property<double?>("MeanMotion_deg_d")
                        .HasColumnType("double");

                    b.Property<double?>("Periapsis_km")
                        .HasColumnType("double");

                    b.Property<double?>("SemiMajorAxis_km")
                        .HasColumnType("double");

                    b.Property<double?>("SiderealOrbitPeriod_d")
                        .HasColumnType("double");

                    b.Property<double?>("SynodicOrbitPeriod_d")
                        .HasColumnType("double");

                    b.HasKey("Id");

                    b.HasIndex("AstroObjectId")
                        .IsUnique();

                    b.ToTable("Orbitals");
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.PhysicalRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AstroObjectId")
                        .HasColumnType("int");

                    b.Property<double?>("Density_kg_m3")
                        .HasColumnType("double");

                    b.Property<double?>("EquatorialRadius2_km")
                        .HasColumnType("double");

                    b.Property<double?>("EquatorialRadius_km")
                        .HasColumnType("double");

                    b.Property<double?>("EscapeVelocity_km_s")
                        .HasColumnType("double");

                    b.Property<double?>("Flattening")
                        .HasColumnType("double");

                    b.Property<double?>("GeometricAlbedo")
                        .HasColumnType("double");

                    b.Property<bool?>("HasGlobalMagneticField")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool?>("HasRings")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool?>("IsRound")
                        .HasColumnType("tinyint(1)");

                    b.Property<double?>("Mass_kg")
                        .HasColumnType("double");

                    b.Property<double?>("MaxSurfaceTemperature_K")
                        .HasColumnType("double");

                    b.Property<double?>("MeanRadius_km")
                        .HasColumnType("double");

                    b.Property<double?>("MeanSurfaceTemperature_K")
                        .HasColumnType("double");

                    b.Property<double?>("MinSurfaceTemperature_K")
                        .HasColumnType("double");

                    b.Property<double?>("PolarRadius_km")
                        .HasColumnType("double");

                    b.Property<double?>("StandardGravitationalParameter_m3_s2")
                        .HasColumnType("double");

                    b.Property<double?>("SurfaceArea_km2")
                        .HasColumnType("double");

                    b.Property<double?>("SurfaceEquivalentDoseRate_microSv_h")
                        .HasColumnType("double");

                    b.Property<double?>("SurfaceGravity_m_s2")
                        .HasColumnType("double");

                    b.Property<double?>("Volume_km3")
                        .HasColumnType("double");

                    b.HasKey("Id");

                    b.HasIndex("AstroObjectId")
                        .IsUnique();

                    b.ToTable("Physicals");
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.RotationRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AstroObjectId")
                        .HasColumnType("int");

                    b.Property<double?>("EquatorialRotationalVelocity_km_s")
                        .HasColumnType("double");

                    b.Property<double?>("NorthPoleDeclination_deg")
                        .HasColumnType("double");

                    b.Property<double?>("NorthPoleRightAscension_deg")
                        .HasColumnType("double");

                    b.Property<double?>("Obliquity_deg")
                        .HasColumnType("double");

                    b.Property<double?>("SiderealRotationPeriod_d")
                        .HasColumnType("double");

                    b.Property<double?>("SynodicRotationPeriod_d")
                        .HasColumnType("double");

                    b.HasKey("Id");

                    b.HasIndex("AstroObjectId")
                        .IsUnique();

                    b.ToTable("Rotationals");
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.SeasonalMarkerRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AstroObjectId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DateTimeUtcAstroPixels")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DateTimeUtcGalaxon")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DateTimeUtcUsno")
                        .HasColumnType("datetime(6)");

                    b.Property<byte>("MarkerType")
                        .HasColumnType("tinyint unsigned");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("SeasonalMarkers");
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.StellarRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AstroObjectId")
                        .HasColumnType("int");

                    b.Property<double?>("Luminosity_W")
                        .HasColumnType("double");

                    b.Property<double?>("Metallicity")
                        .HasColumnType("double");

                    b.Property<double?>("Radiance_W_sr_m2")
                        .HasColumnType("double");

                    b.Property<string>("SpectralClass")
                        .HasMaxLength(5)
                        .HasColumnType("varchar(5)");

                    b.HasKey("Id");

                    b.HasIndex("AstroObjectId")
                        .IsUnique();

                    b.ToTable("Stellars");
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.VSOP87DRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Amplitude")
                        .HasColumnType("decimal(18, 11)");

                    b.Property<byte>("CodeOfBody")
                        .HasColumnType("tinyint unsigned");

                    b.Property<byte>("Exponent")
                        .HasColumnType("tinyint unsigned");

                    b.Property<decimal>("Frequency")
                        .HasColumnType("decimal(20, 11)");

                    b.Property<byte>("IndexOfCoordinate")
                        .HasColumnType("tinyint unsigned");

                    b.Property<decimal>("Phase")
                        .HasColumnType("decimal(14, 11)");

                    b.Property<ushort>("Rank")
                        .HasColumnType("smallint unsigned");

                    b.HasKey("Id");

                    b.HasIndex("CodeOfBody", "IndexOfCoordinate", "Exponent", "Rank")
                        .IsUnique();

                    b.ToTable("VSOP87D");
                });

            modelBuilder.Entity("AstroObjectGroupRecordAstroObjectRecord", b =>
                {
                    b.HasOne("Galaxon.Astronomy.Data.Models.AstroObjectGroupRecord", null)
                        .WithMany()
                        .HasForeignKey("GroupsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Galaxon.Astronomy.Data.Models.AstroObjectRecord", null)
                        .WithMany()
                        .HasForeignKey("ObjectsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.AstroObjectGroupRecord", b =>
                {
                    b.HasOne("Galaxon.Astronomy.Data.Models.AstroObjectGroupRecord", "Parent")
                        .WithMany()
                        .HasForeignKey("ParentId");

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.AstroObjectRecord", b =>
                {
                    b.HasOne("Galaxon.Astronomy.Data.Models.AstroObjectRecord", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId");

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.AtmosphereConstituentRecord", b =>
                {
                    b.HasOne("Galaxon.Astronomy.Data.Models.AtmosphereRecord", "Atmosphere")
                        .WithMany("Constituents")
                        .HasForeignKey("AtmosphereId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Galaxon.Astronomy.Data.Models.MoleculeRecord", "Molecule")
                        .WithMany()
                        .HasForeignKey("MoleculeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Atmosphere");

                    b.Navigation("Molecule");
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.AtmosphereRecord", b =>
                {
                    b.HasOne("Galaxon.Astronomy.Data.Models.AstroObjectRecord", "AstroObject")
                        .WithOne("Atmosphere")
                        .HasForeignKey("Galaxon.Astronomy.Data.Models.AtmosphereRecord", "AstroObjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AstroObject");
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.DocumentRecord", b =>
                {
                    b.HasOne("Galaxon.Astronomy.Data.Models.DocumentRecord", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId");

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.ObservationRecord", b =>
                {
                    b.HasOne("Galaxon.Astronomy.Data.Models.AstroObjectRecord", "AstroObject")
                        .WithOne("Observation")
                        .HasForeignKey("Galaxon.Astronomy.Data.Models.ObservationRecord", "AstroObjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AstroObject");
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.OrbitRecord", b =>
                {
                    b.HasOne("Galaxon.Astronomy.Data.Models.AstroObjectRecord", "AstroObject")
                        .WithOne("Orbit")
                        .HasForeignKey("Galaxon.Astronomy.Data.Models.OrbitRecord", "AstroObjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AstroObject");
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.PhysicalRecord", b =>
                {
                    b.HasOne("Galaxon.Astronomy.Data.Models.AstroObjectRecord", "AstroObject")
                        .WithOne("Physical")
                        .HasForeignKey("Galaxon.Astronomy.Data.Models.PhysicalRecord", "AstroObjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AstroObject");
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.RotationRecord", b =>
                {
                    b.HasOne("Galaxon.Astronomy.Data.Models.AstroObjectRecord", "AstroObject")
                        .WithOne("Rotation")
                        .HasForeignKey("Galaxon.Astronomy.Data.Models.RotationRecord", "AstroObjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AstroObject");
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.StellarRecord", b =>
                {
                    b.HasOne("Galaxon.Astronomy.Data.Models.AstroObjectRecord", "AstroObject")
                        .WithOne("Stellar")
                        .HasForeignKey("Galaxon.Astronomy.Data.Models.StellarRecord", "AstroObjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AstroObject");
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.AstroObjectRecord", b =>
                {
                    b.Navigation("Atmosphere");

                    b.Navigation("Children");

                    b.Navigation("Observation");

                    b.Navigation("Orbit");

                    b.Navigation("Physical");

                    b.Navigation("Rotation");

                    b.Navigation("Stellar");
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.AtmosphereRecord", b =>
                {
                    b.Navigation("Constituents");
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.DocumentRecord", b =>
                {
                    b.Navigation("Children");
                });
#pragma warning restore 612, 618
        }
    }
}