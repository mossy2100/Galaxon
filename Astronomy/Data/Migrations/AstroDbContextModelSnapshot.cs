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
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AstroObjectAstroObjectGroup", b =>
                {
                    b.Property<int>("GroupsId")
                        .HasColumnType("int");

                    b.Property<int>("ObjectsId")
                        .HasColumnType("int");

                    b.HasKey("GroupsId", "ObjectsId");

                    b.HasIndex("ObjectsId");

                    b.ToTable("AstroObjectAstroObjectGroup");
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.Apside", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateTimeUtcUsno")
                        .HasColumnType("datetime2");

                    b.Property<byte>("Type")
                        .HasColumnType("tinyint");

                    b.HasKey("Id");

                    b.ToTable("Apsides");
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.AstroObject", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int?>("Number")
                        .HasColumnType("int");

                    b.Property<int?>("ParentId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("AstroObjects");
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.AstroObjectGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<int?>("ParentId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("AstroObjectGroups");
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.AtmosphereConstituent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AtmosphereId")
                        .HasColumnType("int");

                    b.Property<int>("MoleculeId")
                        .HasColumnType("int");

                    b.Property<double?>("Percentage")
                        .HasColumnType("float");

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

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AstroObjectId")
                        .HasColumnType("int");

                    b.Property<bool?>("IsSurfaceBoundedExosphere")
                        .HasColumnType("bit");

                    b.Property<double?>("ScaleHeight")
                        .HasColumnType("float");

                    b.Property<double?>("SurfacePressure")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("AstroObjectId")
                        .IsUnique();

                    b.ToTable("AtmosphereRecords");
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.EasterDate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("date");

                    b.HasKey("Id");

                    b.ToTable("EasterDates");
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.IersBulletinC", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BulletinNumber")
                        .HasColumnType("int");

                    b.Property<string>("BulletinUrl")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("DateTimeParsed")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LeapSecondDate")
                        .HasColumnType("date");

                    b.Property<short>("Value")
                        .HasColumnType("smallint");

                    b.HasKey("Id");

                    b.HasIndex("BulletinNumber")
                        .IsUnique();

                    b.ToTable("IersBulletinCs");
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.LeapSecond", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("LeapSecondDate")
                        .HasColumnType("date");

                    b.Property<short>("Value")
                        .HasColumnType("smallint");

                    b.HasKey("Id");

                    b.ToTable("LeapSeconds");
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.LunarPhaseRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("DateTimeUtcAstroPixels")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateTimeUtcUsno")
                        .HasColumnType("datetime2");

                    b.Property<int>("LunationNumber")
                        .HasColumnType("int");

                    b.Property<byte>("PhaseNumber")
                        .HasColumnType("tinyint");

                    b.HasKey("Id");

                    b.ToTable("LunarPhases");
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.Molecule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.ToTable("Molecules");
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.ObservationalRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double?>("AbsMag")
                        .HasColumnType("float");

                    b.Property<int>("AstroObjectId")
                        .HasColumnType("int");

                    b.Property<double?>("MaxAngularDiam")
                        .HasColumnType("float");

                    b.Property<double?>("MaxApparentMag")
                        .HasColumnType("float");

                    b.Property<double?>("MinAngularDiam")
                        .HasColumnType("float");

                    b.Property<double?>("MinApparentMag")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("AstroObjectId")
                        .IsUnique();

                    b.ToTable("ObservationalRecords");
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.OrbitalRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double?>("Apoapsis")
                        .HasColumnType("float");

                    b.Property<double?>("ArgPeriapsis")
                        .HasColumnType("float");

                    b.Property<int>("AstroObjectId")
                        .HasColumnType("int");

                    b.Property<double?>("AvgOrbitSpeed")
                        .HasColumnType("float");

                    b.Property<double?>("Eccentricity")
                        .HasColumnType("float");

                    b.Property<DateTime?>("Epoch")
                        .HasColumnType("datetime2");

                    b.Property<double?>("Inclination")
                        .HasColumnType("float");

                    b.Property<double?>("LongAscNode")
                        .HasColumnType("float");

                    b.Property<double?>("MeanAnomaly")
                        .HasColumnType("float");

                    b.Property<double?>("MeanMotion")
                        .HasColumnType("float");

                    b.Property<double?>("Periapsis")
                        .HasColumnType("float");

                    b.Property<double?>("SemiMajorAxis")
                        .HasColumnType("float");

                    b.Property<double?>("SiderealOrbitPeriod")
                        .HasColumnType("float");

                    b.Property<double?>("SynodicOrbitPeriod")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("AstroObjectId")
                        .IsUnique();

                    b.ToTable("OrbitalRecords");
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.PhysicalRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AstroObjectId")
                        .HasColumnType("int");

                    b.Property<double?>("ColorBV")
                        .HasColumnType("float");

                    b.Property<double?>("ColorUB")
                        .HasColumnType("float");

                    b.Property<double?>("Density")
                        .HasColumnType("float");

                    b.Property<double?>("EscapeVelocity")
                        .HasColumnType("float");

                    b.Property<double?>("Flattening")
                        .HasColumnType("float");

                    b.Property<double?>("GeometricAlbedo")
                        .HasColumnType("float");

                    b.Property<bool?>("HasGlobalMagField")
                        .HasColumnType("bit");

                    b.Property<bool?>("HasRingSystem")
                        .HasColumnType("bit");

                    b.Property<bool?>("IsRound")
                        .HasColumnType("bit");

                    b.Property<double?>("Mass")
                        .HasColumnType("float");

                    b.Property<double?>("MaxSurfaceTemp")
                        .HasColumnType("float");

                    b.Property<double?>("MeanRadius")
                        .HasColumnType("float");

                    b.Property<double?>("MeanSurfaceTemp")
                        .HasColumnType("float");

                    b.Property<double?>("MinSurfaceTemp")
                        .HasColumnType("float");

                    b.Property<double?>("MomentOfInertiaFactor")
                        .HasColumnType("float");

                    b.Property<double?>("RadiusA")
                        .HasColumnType("float");

                    b.Property<double?>("RadiusB")
                        .HasColumnType("float");

                    b.Property<double?>("RadiusC")
                        .HasColumnType("float");

                    b.Property<double?>("SolarIrradiance")
                        .HasColumnType("float");

                    b.Property<double?>("StdGravParam")
                        .HasColumnType("float");

                    b.Property<double?>("SurfaceArea")
                        .HasColumnType("float");

                    b.Property<double?>("SurfaceGrav")
                        .HasColumnType("float");

                    b.Property<double?>("Volume")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("AstroObjectId")
                        .IsUnique();

                    b.ToTable("PhysicalRecords");
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.RotationalRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AstroObjectId")
                        .HasColumnType("int");

                    b.Property<double?>("EquatRotationVelocity")
                        .HasColumnType("float");

                    b.Property<double?>("NorthPoleDeclination")
                        .HasColumnType("float");

                    b.Property<double?>("NorthPoleRightAscension")
                        .HasColumnType("float");

                    b.Property<double?>("Obliquity")
                        .HasColumnType("float");

                    b.Property<double?>("SiderealRotationPeriod")
                        .HasColumnType("float");

                    b.Property<double?>("SynodicRotationPeriod")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("AstroObjectId")
                        .IsUnique();

                    b.ToTable("RotationalRecords");
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.SeasonalMarkerRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateTimeUtcUsno")
                        .HasColumnType("datetime2");

                    b.Property<byte>("Type")
                        .HasColumnType("tinyint");

                    b.HasKey("Id");

                    b.ToTable("SeasonalMarkers");
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.StellarRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AstroObjectId")
                        .HasColumnType("int");

                    b.Property<double?>("Luminosity")
                        .HasColumnType("float");

                    b.Property<double?>("Metallicity")
                        .HasColumnType("float");

                    b.Property<double?>("Radiance")
                        .HasColumnType("float");

                    b.Property<string>("SpectralClass")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.HasKey("Id");

                    b.HasIndex("AstroObjectId")
                        .IsUnique();

                    b.ToTable("StellarRecords");
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.VSOP87DRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("Amplitude")
                        .HasColumnType("float");

                    b.Property<int>("AstroObjectId")
                        .HasColumnType("int");

                    b.Property<byte>("Exponent")
                        .HasColumnType("tinyint");

                    b.Property<double>("Frequency")
                        .HasColumnType("float");

                    b.Property<short>("Index")
                        .HasColumnType("smallint");

                    b.Property<double>("Phase")
                        .HasColumnType("float");

                    b.Property<string>("Variable")
                        .IsRequired()
                        .HasColumnType("char(1)");

                    b.HasKey("Id");

                    b.HasIndex("AstroObjectId");

                    b.ToTable("VSOP87DRecords");
                });

            modelBuilder.Entity("AstroObjectAstroObjectGroup", b =>
                {
                    b.HasOne("Galaxon.Astronomy.Data.Models.AstroObjectGroup", null)
                        .WithMany()
                        .HasForeignKey("GroupsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Galaxon.Astronomy.Data.Models.AstroObject", null)
                        .WithMany()
                        .HasForeignKey("ObjectsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.AstroObject", b =>
                {
                    b.HasOne("Galaxon.Astronomy.Data.Models.AstroObject", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId");

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.AstroObjectGroup", b =>
                {
                    b.HasOne("Galaxon.Astronomy.Data.Models.AstroObjectGroup", "Parent")
                        .WithMany()
                        .HasForeignKey("ParentId");

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.AtmosphereConstituent", b =>
                {
                    b.HasOne("Galaxon.Astronomy.Data.Models.AtmosphereRecord", "Atmosphere")
                        .WithMany("Constituents")
                        .HasForeignKey("AtmosphereId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Galaxon.Astronomy.Data.Models.Molecule", "Molecule")
                        .WithMany()
                        .HasForeignKey("MoleculeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Atmosphere");

                    b.Navigation("Molecule");
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.AtmosphereRecord", b =>
                {
                    b.HasOne("Galaxon.Astronomy.Data.Models.AstroObject", "AstroObject")
                        .WithOne("Atmosphere")
                        .HasForeignKey("Galaxon.Astronomy.Data.Models.AtmosphereRecord", "AstroObjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AstroObject");
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.ObservationalRecord", b =>
                {
                    b.HasOne("Galaxon.Astronomy.Data.Models.AstroObject", "AstroObject")
                        .WithOne("Observation")
                        .HasForeignKey("Galaxon.Astronomy.Data.Models.ObservationalRecord", "AstroObjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AstroObject");
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.OrbitalRecord", b =>
                {
                    b.HasOne("Galaxon.Astronomy.Data.Models.AstroObject", "AstroObject")
                        .WithOne("Orbit")
                        .HasForeignKey("Galaxon.Astronomy.Data.Models.OrbitalRecord", "AstroObjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AstroObject");
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.PhysicalRecord", b =>
                {
                    b.HasOne("Galaxon.Astronomy.Data.Models.AstroObject", "AstroObject")
                        .WithOne("Physical")
                        .HasForeignKey("Galaxon.Astronomy.Data.Models.PhysicalRecord", "AstroObjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AstroObject");
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.RotationalRecord", b =>
                {
                    b.HasOne("Galaxon.Astronomy.Data.Models.AstroObject", "AstroObject")
                        .WithOne("Rotation")
                        .HasForeignKey("Galaxon.Astronomy.Data.Models.RotationalRecord", "AstroObjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AstroObject");
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.StellarRecord", b =>
                {
                    b.HasOne("Galaxon.Astronomy.Data.Models.AstroObject", "AstroObject")
                        .WithOne("Stellar")
                        .HasForeignKey("Galaxon.Astronomy.Data.Models.StellarRecord", "AstroObjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AstroObject");
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.VSOP87DRecord", b =>
                {
                    b.HasOne("Galaxon.Astronomy.Data.Models.AstroObject", "AstroObject")
                        .WithMany("VSOP87DRecords")
                        .HasForeignKey("AstroObjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AstroObject");
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.AstroObject", b =>
                {
                    b.Navigation("Atmosphere");

                    b.Navigation("Children");

                    b.Navigation("Observation");

                    b.Navigation("Orbit");

                    b.Navigation("Physical");

                    b.Navigation("Rotation");

                    b.Navigation("Stellar");

                    b.Navigation("VSOP87DRecords");
                });

            modelBuilder.Entity("Galaxon.Astronomy.Data.Models.AtmosphereRecord", b =>
                {
                    b.Navigation("Constituents");
                });
#pragma warning restore 612, 618
        }
    }
}
