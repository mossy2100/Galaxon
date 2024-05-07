using Galaxon.Astronomy.Data.Converters;
using Galaxon.Astronomy.Data.Models;
using Galaxon.Core.Files;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Galaxon.Astronomy.Data;

/// <summary>
/// Handles the database connection and operations.
/// </summary>
public class AstroDbContext : DbContext
{
    /// <summary>
    /// Default constructor.
    /// </summary>
    public AstroDbContext() { }

    #region Database tables

    // ---------------------------------------------------------------------------------------------
    // Astronomical objects and groups.

    public DbSet<AstroObjectRecord> AstroObjects => Set<AstroObjectRecord>();

    public DbSet<AstroObjectGroupRecord> AstroObjectGroups => Set<AstroObjectGroupRecord>();

    // ---------------------------------------------------------------------------------------------
    // AstroObject components.

    public DbSet<PhysicalRecord> Physicals => Set<PhysicalRecord>();

    public DbSet<RotationalRecord> Rotationals => Set<RotationalRecord>();

    public DbSet<OrbitalRecord> Orbitals => Set<OrbitalRecord>();

    public DbSet<ObservationalRecord> Observationals => Set<ObservationalRecord>();

    public DbSet<StellarRecord> Stellars => Set<StellarRecord>();

    // ---------------------------------------------------------------------------------------------
    // Atmospheres.

    public DbSet<AtmosphereRecord> Atmospheres => Set<AtmosphereRecord>();

    public DbSet<AtmosphereConstituentRecord> AtmosphereConstituents => Set<AtmosphereConstituentRecord>();

    public DbSet<MoleculeRecord> Molecules => Set<MoleculeRecord>();

    // ---------------------------------------------------------------------------------------------
    // Leap seconds.

    public DbSet<LeapSecondRecord> LeapSeconds => Set<LeapSecondRecord>();

    public DbSet<IersBulletinCRecord> IersBulletinCs => Set<IersBulletinCRecord>();

    // ---------------------------------------------------------------------------------------------
    // Other stuff.

    public DbSet<SeasonalMarkerRecord> SeasonalMarkers => Set<SeasonalMarkerRecord>();

    public DbSet<LunarPhaseRecord> LunarPhases => Set<LunarPhaseRecord>();

    public DbSet<ApsideRecord> Apsides => Set<ApsideRecord>();

    public DbSet<EasterDateRecord> EasterDates => Set<EasterDateRecord>();

    // public DbSet<DeltaTRecord> DeltaTRecords => Set<DeltaTRecord>();

    public DbSet<VSOP87DRecord> VSOP87D => Set<VSOP87DRecord>();

    // ---------------------------------------------------------------------------------------------
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
        // Load the configuration.
        string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
            ?? Environment.GetEnvironmentVariable("CONSOLEAPP_ENVIRONMENT")
            ?? "Production";
        IConfigurationRoot configuration = new ConfigurationBuilder()
            // Ensure this path is correct, particularly when running from a different context like
            // a class library.
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, true)
            .AddJsonFile($"appsettings.{environment}.json", true, true)
            .Build();

        // Get the database connection string.
        string? connectionString = configuration.GetConnectionString("Astro");
        if (connectionString == null)
        {
            throw new InvalidOperationException(
                "Connection string for Astro database not found.");
        }

        // Old connection strings:
        // MS SQL:
        //     "AstroAPI": "Server=localhost,1433; Database=Astro; User Id=SA; Password=HappyHealthyRichFree!; Encrypt=False";
        //     "SpaceCalendars": "Server=localhost,1433; Database=SpaceCalendars; User Id=SA; Password=HappyDays2023"
        // MySQL:
        //     "Galaxon": "Server=localhost;port=3306;database=galaxon;user=shaun;password=freedom"

        // Configure the DbContext.
        optionsBuilder
            .UseLazyLoadingProxies()
            .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
            .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name },
                LogLevel.Warning)
            .EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<AstroObjectRecord>()
            .HasMany(ao => ao.Groups)
            .WithMany(g => g.Objects);
        builder.Entity<AstroObjectRecord>()
            .HasOne(ao => ao.Parent)
            .WithMany(ao => ao.Children);
        builder.Entity<AstroObjectRecord>()
            .HasOne(ao => ao.Physical)
            .WithOne(phys => phys.AstroObject)
            .HasForeignKey<PhysicalRecord>(phys => phys.AstroObjectId);
        builder.Entity<AstroObjectRecord>()
            .HasOne(ao => ao.Rotation)
            .WithOne(rot => rot.AstroObject)
            .HasForeignKey<RotationalRecord>(rot => rot.AstroObjectId);
        builder.Entity<AstroObjectRecord>()
            .HasOne(ao => ao.Orbit)
            .WithOne(orb => orb.AstroObject)
            .HasForeignKey<OrbitalRecord>(orb => orb.AstroObjectId);
        builder.Entity<AstroObjectRecord>()
            .HasOne(ao => ao.Observation)
            .WithOne(obs => obs.AstroObject)
            .HasForeignKey<ObservationalRecord>(obs => obs.AstroObjectId);
        builder.Entity<AstroObjectRecord>()
            .HasOne(ao => ao.Atmosphere)
            .WithOne(atmo => atmo.AstroObject)
            .HasForeignKey<AtmosphereRecord>(atmo => atmo.AstroObjectId);
        builder.Entity<AstroObjectRecord>()
            .HasOne(ao => ao.Stellar)
            .WithOne(ss => ss.AstroObject)
            .HasForeignKey<StellarRecord>(ss => ss.AstroObjectId);
        // builder.Entity<AstroObject>()
        //     .HasOne(ao => ao.MinorPlanet)
        //     .WithOne(mpr => mpr.AstroObject)
        //     .HasForeignKey<MinorPlanetRecord>(mpr => mpr.AstroObjectId);
        builder.Entity<AstroObjectRecord>()
            .HasMany(ao => ao.VSOP87DRecords)
            .WithOne(vr => vr.AstroObject)
            .HasForeignKey(vr => vr.AstroObjectId);

        // Converters.
        builder
            .Entity<ApsideRecord>()
            .Property(e => e.Type)
            .HasConversion(new ApsideConverter());
        builder
            .Entity<LunarPhaseRecord>()
            .Property(e => e.Type)
            .HasConversion(new LunarPhaseConverter());
        builder
            .Entity<SeasonalMarkerRecord>()
            .Property(e => e.Type)
            .HasConversion(new SeasonalMarkerConverter());
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<DateOnly>()
            .HaveConversion<DateOnlyConverter>();
        configurationBuilder
            .Properties<DateTime>()
            .HaveConversion<DateTimeConverter>();
        configurationBuilder
            .Properties<DateOnly?>()
            .HaveConversion<NullableDateOnlyConverter>();
        configurationBuilder
            .Properties<DateTime>()
            .HaveConversion<NullableDateTimeConverter>();
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
