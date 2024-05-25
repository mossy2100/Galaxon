﻿// <auto-generated />
using System;
using Galaxon.Astronomy.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Galaxon.Astronomy.Data.Migrations
{
    [DbContext(typeof(AstroDbContext))]
    partial class AstroDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

                    b.Property<double?>("ScaleHeight")
                        .HasColumnType("double");

                    b.Property<double?>("SurfacePressure")
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
                        .HasColumnType("decimal(7, 3)");

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

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.ObservationalRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<double?>("AbsMag")
                        .HasColumnType("double");

                    b.Property<int>("AstroObjectId")
                        .HasColumnType("int");

                    b.Property<double?>("MaxAngularDiam")
                        .HasColumnType("double");

                    b.Property<double?>("MaxApparentMag")
                        .HasColumnType("double");

                    b.Property<double?>("MinAngularDiam")
                        .HasColumnType("double");

                    b.Property<double?>("MinApparentMag")
                        .HasColumnType("double");

                    b.HasKey("Id");

                    b.HasIndex("AstroObjectId")
                        .IsUnique();

                    b.ToTable("Observationals");
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.OrbitalRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<double?>("Apoapsis")
                        .HasColumnType("double");

                    b.Property<double?>("ArgPeriapsis")
                        .HasColumnType("double");

                    b.Property<int>("AstroObjectId")
                        .HasColumnType("int");

                    b.Property<double?>("AvgOrbitSpeed")
                        .HasColumnType("double");

                    b.Property<double?>("Eccentricity")
                        .HasColumnType("double");

                    b.Property<DateTime?>("Epoch")
                        .HasColumnType("datetime(6)");

                    b.Property<double?>("Inclination")
                        .HasColumnType("double");

                    b.Property<double?>("LongAscNode")
                        .HasColumnType("double");

                    b.Property<double?>("MeanAnomaly")
                        .HasColumnType("double");

                    b.Property<double?>("MeanMotion")
                        .HasColumnType("double");

                    b.Property<double?>("Periapsis")
                        .HasColumnType("double");

                    b.Property<double?>("SemiMajorAxis")
                        .HasColumnType("double");

                    b.Property<double?>("SiderealOrbitPeriod")
                        .HasColumnType("double");

                    b.Property<double?>("SynodicOrbitPeriod")
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

                    b.Property<double?>("ColorBV")
                        .HasColumnType("double");

                    b.Property<double?>("ColorUB")
                        .HasColumnType("double");

                    b.Property<double?>("Density")
                        .HasColumnType("double");

                    b.Property<double?>("EscapeVelocity")
                        .HasColumnType("double");

                    b.Property<double?>("Flattening")
                        .HasColumnType("double");

                    b.Property<double?>("GeometricAlbedo")
                        .HasColumnType("double");

                    b.Property<bool?>("HasGlobalMagField")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool?>("HasRingSystem")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool?>("IsRound")
                        .HasColumnType("tinyint(1)");

                    b.Property<double?>("Mass")
                        .HasColumnType("double");

                    b.Property<double?>("MaxSurfaceTemp")
                        .HasColumnType("double");

                    b.Property<double?>("MeanRadius")
                        .HasColumnType("double");

                    b.Property<double?>("MeanSurfaceTemp")
                        .HasColumnType("double");

                    b.Property<double?>("MinSurfaceTemp")
                        .HasColumnType("double");

                    b.Property<double?>("MomentOfInertiaFactor")
                        .HasColumnType("double");

                    b.Property<double?>("RadiusA")
                        .HasColumnType("double");

                    b.Property<double?>("RadiusB")
                        .HasColumnType("double");

                    b.Property<double?>("RadiusC")
                        .HasColumnType("double");

                    b.Property<double?>("SolarIrradiance")
                        .HasColumnType("double");

                    b.Property<double?>("StdGravParam")
                        .HasColumnType("double");

                    b.Property<double?>("SurfaceArea")
                        .HasColumnType("double");

                    b.Property<double?>("SurfaceGrav")
                        .HasColumnType("double");

                    b.Property<double?>("Volume")
                        .HasColumnType("double");

                    b.HasKey("Id");

                    b.HasIndex("AstroObjectId")
                        .IsUnique();

                    b.ToTable("Physicals");
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.RotationalRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AstroObjectId")
                        .HasColumnType("int");

                    b.Property<double?>("EquatRotationVelocity")
                        .HasColumnType("double");

                    b.Property<double?>("NorthPoleDeclination")
                        .HasColumnType("double");

                    b.Property<double?>("NorthPoleRightAscension")
                        .HasColumnType("double");

                    b.Property<double?>("Obliquity")
                        .HasColumnType("double");

                    b.Property<double?>("SiderealRotationPeriod")
                        .HasColumnType("double");

                    b.Property<double?>("SynodicRotationPeriod")
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

                    b.Property<double?>("Luminosity")
                        .HasColumnType("double");

                    b.Property<double?>("Metallicity")
                        .HasColumnType("double");

                    b.Property<double?>("Radiance")
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

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.ObservationalRecord", b =>
                {
                    b.HasOne("Galaxon.Astronomy.Data.Models.AstroObjectRecord", "AstroObject")
                        .WithOne("Observation")
                        .HasForeignKey("Galaxon.Astronomy.Data.Models.ObservationalRecord", "AstroObjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AstroObject");
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.OrbitalRecord", b =>
                {
                    b.HasOne("Galaxon.Astronomy.Data.Models.AstroObjectRecord", "AstroObject")
                        .WithOne("Orbit")
                        .HasForeignKey("Galaxon.Astronomy.Data.Models.OrbitalRecord", "AstroObjectId")
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

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.RotationalRecord", b =>
                {
                    b.HasOne("Galaxon.Astronomy.Data.Models.AstroObjectRecord", "AstroObject")
                        .WithOne("Rotation")
                        .HasForeignKey("Galaxon.Astronomy.Data.Models.RotationalRecord", "AstroObjectId")
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
