using Galaxon.Astronomy.Data.Converters;
using Galaxon.Astronomy.Data.Enums;
using Galaxon.Astronomy.Data.Models;
using Galaxon.Core.Files;
using Galaxon.Development.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Galaxon.Astronomy.Data;

/// <summary>
/// Handles the database connection and operations.
/// </summary>
public class AstroDbContext : DbContext
{
    #region Database tables

    ////////////////////////////////////////////////////////////////////////////////////////////////
    // Astronomical objects and groups.

    public DbSet<AstroObjectRecord> AstroObjects => Set<AstroObjectRecord>();

    public DbSet<AstroObjectGroupRecord> AstroObjectGroups => Set<AstroObjectGroupRecord>();

    ////////////////////////////////////////////////////////////////////////////////////////////////
    // AstroObject components.

    public DbSet<PhysicalRecord> Physicals => Set<PhysicalRecord>();

    public DbSet<RotationalRecord> Rotationals => Set<RotationalRecord>();

    public DbSet<OrbitalRecord> Orbitals => Set<OrbitalRecord>();

    public DbSet<ObservationalRecord> Observationals => Set<ObservationalRecord>();

    public DbSet<StellarRecord> Stellars => Set<StellarRecord>();

    ////////////////////////////////////////////////////////////////////////////////////////////////
    // Atmospheres.

    public DbSet<AtmosphereRecord> Atmospheres => Set<AtmosphereRecord>();

    public DbSet<AtmosphereConstituentRecord> AtmosphereConstituents =>
        Set<AtmosphereConstituentRecord>();

    public DbSet<MoleculeRecord> Molecules => Set<MoleculeRecord>();

    ////////////////////////////////////////////////////////////////////////////////////////////////
    // Leap seconds.

    public DbSet<LeapSecondRecord> LeapSeconds => Set<LeapSecondRecord>();

    public DbSet<IersBulletinCRecord> IersBulletinCs => Set<IersBulletinCRecord>();

    ////////////////////////////////////////////////////////////////////////////////////////////////
    // Events.

    public DbSet<SeasonalMarkerRecord> SeasonalMarkers => Set<SeasonalMarkerRecord>();

    public DbSet<LunarPhaseRecord> LunarPhases => Set<LunarPhaseRecord>();

    public DbSet<ApsideRecord> Apsides => Set<ApsideRecord>();

    public DbSet<EasterDateRecord> EasterDates => Set<EasterDateRecord>();

    ////////////////////////////////////////////////////////////////////////////////////////////////
    // Data for computing planetary positions.

    public DbSet<VSOP87DRecord> VSOP87D => Set<VSOP87DRecord>();

    ////////////////////////////////////////////////////////////////////////////////////////////////
    // Website documents.

    public DbSet<DocumentRecord> Documents => Set<DocumentRecord>();

    #endregion Database tables

    /// <summary>
    /// Set the DbContext options.
    /// </summary>
    /// <param name="optionsBuilder"></param>
    /// <exception cref="InvalidOperationException"></exception>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Get the database connection string.
        IConfiguration configuration = SetupTools.LoadConfiguration();
        string? connectionString = configuration.GetConnectionString("Astro");
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("Connection string for Astro database not found.");
        }

        // Configure the connection.
        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
            .UseLazyLoadingProxies()
            .EnableSensitiveDataLogging();

        // Old connection strings:
        // MS SQL:
        //     "AstroAPI": "Server=localhost,1433; Database=Astro; User Id=SA; Password=HappyHealthyRichFree!; Encrypt=False";
        //     "SpaceCalendars": "Server=localhost,1433; Database=SpaceCalendars; User Id=SA; Password=HappyDays2023"
        // MySQL:
        //     "Galaxon": "Server=localhost;port=3306;database=galaxon;user=shaun;password=freedom"
    }

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<AstroObjectRecord>(entity =>
        {
            entity.HasMany(ao => ao.Groups)
                .WithMany(g => g.Objects);

            entity.HasOne(ao => ao.Parent)
                .WithMany(ao => ao.Children);

            entity.HasOne(ao => ao.Physical)
                .WithOne(phys => phys.AstroObject)
                .HasForeignKey<PhysicalRecord>(phys => phys.AstroObjectId);

            entity.HasOne(ao => ao.Rotation)
                .WithOne(rot => rot.AstroObject)
                .HasForeignKey<RotationalRecord>(rot => rot.AstroObjectId);

            entity.HasOne(ao => ao.Orbit)
                .WithOne(orb => orb.AstroObject)
                .HasForeignKey<OrbitalRecord>(orb => orb.AstroObjectId);

            entity.HasOne(ao => ao.Observation)
                .WithOne(obs => obs.AstroObject)
                .HasForeignKey<ObservationalRecord>(obs => obs.AstroObjectId);

            entity.HasOne(ao => ao.Atmosphere)
                .WithOne(atmo => atmo.AstroObject)
                .HasForeignKey<AtmosphereRecord>(atmo => atmo.AstroObjectId);

            entity.HasOne(ao => ao.Stellar)
                .WithOne(ss => ss.AstroObject)
                .HasForeignKey<StellarRecord>(ss => ss.AstroObjectId);
        });

        builder.Entity<VSOP87DRecord>()
            .HasIndex(p => new { p.CodeOfBody, p.IndexOfCoordinate, p.Exponent, p.Rank })
            .IsUnique();
    }

    /// <inheritdoc/>
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<DateOnly>()
            .HaveConversion<DateOnlyConverter>();

        configurationBuilder.Properties<DateTime>()
            .HaveConversion<DateTimeConverter>();

        configurationBuilder.Properties<DateOnly?>()
            .HaveConversion<NullableDateOnlyConverter>();

        configurationBuilder.Properties<DateTime?>()
            .HaveConversion<NullableDateTimeConverter>();

        configurationBuilder.Properties<EApsideType>()
            .HaveConversion<ApsideConverter>();

        configurationBuilder.Properties<ELunarPhaseType>()
            .HaveConversion<LunarPhaseConverter>();

        configurationBuilder.Properties<ESeasonalMarkerType>()
            .HaveConversion<SeasonalMarkerConverter>();
    }

    public static string DataDirectory()
    {
        string? solnDir = DirectoryUtility.GetSolutionDirectory();
        if (solnDir == null)
        {
            throw new InvalidOperationException("Solution directory not found.");
        }

        return Path.Combine(solnDir, "Astronomy/DataImport/data");
    }
}
