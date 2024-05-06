using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace Galaxon.Astronomy.Data;

public partial class GalaxonDbContext2 : DbContext
{
    public GalaxonDbContext2()
    {
    }

    public GalaxonDbContext2(DbContextOptions<GalaxonDbContext2> options)
        : base(options)
    {
    }

    public virtual DbSet<Apside> Apsides { get; set; }

    public virtual DbSet<AstroObject> AstroObjects { get; set; }

    public virtual DbSet<AstroObjectAstroObjectGroup> AstroObjectAstroObjectGroups { get; set; }

    public virtual DbSet<AstroObjectGroup> AstroObjectGroups { get; set; }

    public virtual DbSet<AtmosphereConstituent> AtmosphereConstituents { get; set; }

    public virtual DbSet<AtmosphereRecord> AtmosphereRecords { get; set; }

    public virtual DbSet<EasterDate> EasterDates { get; set; }

    public virtual DbSet<EfmigrationsHistory> EfmigrationsHistories { get; set; }

    public virtual DbSet<IersBulletinC> IersBulletinCs { get; set; }

    public virtual DbSet<LeapSecond> LeapSeconds { get; set; }

    public virtual DbSet<LunarPhase> LunarPhases { get; set; }

    public virtual DbSet<Molecule> Molecules { get; set; }

    public virtual DbSet<ObservationalRecord> ObservationalRecords { get; set; }

    public virtual DbSet<OrbitalRecord> OrbitalRecords { get; set; }

    public virtual DbSet<PhysicalRecord> PhysicalRecords { get; set; }

    public virtual DbSet<RotationalRecord> RotationalRecords { get; set; }

    public virtual DbSet<SeasonalMarker> SeasonalMarkers { get; set; }

    public virtual DbSet<StellarRecord> StellarRecords { get; set; }

    public virtual DbSet<Vsop87drecord> Vsop87drecords { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;port=3306;database=galaxon;user=shaun;password=freedom", Microsoft.EntityFrameworkCore.ServerVersion.Parse("11.2.2-mariadb"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Apside>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.ApsideNumber).HasColumnType("tinyint(4)");
            entity.Property(e => e.DateTimeUtcUsno).HasColumnType("datetime");
            entity.Property(e => e.Id).HasColumnType("int(11)");
        });

        modelBuilder.Entity<AstroObject>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Name).HasMaxLength(20);
            entity.Property(e => e.Number).HasColumnType("int(11)");
            entity.Property(e => e.ParentId).HasColumnType("int(11)");
        });

        modelBuilder.Entity<AstroObjectAstroObjectGroup>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("AstroObjectAstroObjectGroup");

            entity.Property(e => e.GroupsId).HasColumnType("int(11)");
            entity.Property(e => e.ObjectsId).HasColumnType("int(11)");
        });

        modelBuilder.Entity<AstroObjectGroup>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Name).HasMaxLength(30);
            entity.Property(e => e.ParentId).HasColumnType("int(11)");
        });

        modelBuilder.Entity<AtmosphereConstituent>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.AtmosphereId).HasColumnType("int(11)");
            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.MoleculeId).HasColumnType("int(11)");
        });

        modelBuilder.Entity<AtmosphereRecord>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.AstroObjectId).HasColumnType("int(11)");
            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.IsSurfaceBoundedExosphere).HasColumnType("bit(1)");
        });

        modelBuilder.Entity<EasterDate>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Id).HasColumnType("int(11)");
        });

        modelBuilder.Entity<EfmigrationsHistory>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("__EFMigrationsHistory");

            entity.Property(e => e.MigrationId).HasMaxLength(150);
            entity.Property(e => e.ProductVersion).HasMaxLength(32);
        });

        modelBuilder.Entity<IersBulletinC>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.BulletinNumber).HasColumnType("int(11)");
            entity.Property(e => e.BulletinUrl).HasMaxLength(100);
            entity.Property(e => e.DateTimeParsed).HasColumnType("datetime");
            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Value).HasColumnType("smallint(6)");
        });

        modelBuilder.Entity<LeapSecond>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Value).HasColumnType("smallint(6)");
        });

        modelBuilder.Entity<LunarPhase>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.DateTimeUtcAstroPixels).HasColumnType("datetime");
            entity.Property(e => e.DateTimeUtcUsno).HasColumnType("datetime");
            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.LunationNumber).HasColumnType("int(11)");
            entity.Property(e => e.PhaseNumber).HasColumnType("tinyint(4)");
        });

        modelBuilder.Entity<Molecule>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Symbol).HasMaxLength(20);
        });

        modelBuilder.Entity<ObservationalRecord>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.AstroObjectId).HasColumnType("int(11)");
            entity.Property(e => e.Id).HasColumnType("int(11)");
        });

        modelBuilder.Entity<OrbitalRecord>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.AstroObjectId).HasColumnType("int(11)");
            entity.Property(e => e.Epoch).HasColumnType("timestamp");
            entity.Property(e => e.Id).HasColumnType("int(11)");
        });

        modelBuilder.Entity<PhysicalRecord>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.AstroObjectId).HasColumnType("int(11)");
            entity.Property(e => e.ColorBv).HasColumnName("ColorBV");
            entity.Property(e => e.ColorUb).HasColumnName("ColorUB");
            entity.Property(e => e.HasGlobalMagField).HasColumnType("bit(1)");
            entity.Property(e => e.HasRingSystem).HasColumnType("bit(1)");
            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.IsRound).HasColumnType("bit(1)");
        });

        modelBuilder.Entity<RotationalRecord>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.AstroObjectId).HasColumnType("int(11)");
            entity.Property(e => e.Id).HasColumnType("int(11)");
        });

        modelBuilder.Entity<SeasonalMarker>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.DateTimeUtcUsno).HasColumnType("datetime");
            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.MarkerNumber).HasColumnType("tinyint(4)");
        });

        modelBuilder.Entity<StellarRecord>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.AstroObjectId).HasColumnType("int(11)");
            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.SpectralClass).HasMaxLength(5);
        });

        modelBuilder.Entity<Vsop87drecord>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("VSOP87DRecords");

            entity.Property(e => e.AstroObjectId).HasColumnType("int(11)");
            entity.Property(e => e.Exponent).HasColumnType("tinyint(4)");
            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Index).HasColumnType("smallint(6)");
            entity.Property(e => e.Variable)
                .HasMaxLength(1)
                .IsFixedLength();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
