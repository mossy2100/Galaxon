using System.Globalization;
using CsvHelper;
using Galaxon.Astronomy.Data;
using Galaxon.Astronomy.Data.Models;
using Galaxon.Astronomy.Data.Repositories;
using Galaxon.Core.Exceptions;
using Galaxon.Numerics.Geometry;
using Galaxon.Quantities.Kinds;
using Galaxon.Time;

namespace DataImport.Services;

public class PlanetImportService(
    AstroDbContext astroDbContext,
    AstroObjectRepository astroObjectRepository,
    AstroObjectGroupRepository astroObjectGroupRepository)
{
    /// <summary>
    /// Attempt to get a double value from the CSV file.
    /// If successful, multiply the value by the multiplier before returning.
    /// </summary>
    private static double? GetDoubleValue(CsvReader csv, int csvFieldNumber, double multiplier = 1)
    {
        return double.TryParse(csv.GetField(csvFieldNumber), out double value)
            ? value * multiplier
            : null;
    }

    /// <summary>
    /// Initialize the planet data from the CSV file.
    /// </summary>
    public void Import()
    {
        // Get the Sun.
        AstroObject sun = astroObjectRepository.LoadByName("Sun", "Star");

        // Open the CSV file for parsing.
        string csvPath = $"{AstroDbContext.DataDirectory()}/Planets/Planets.csv";
        using StreamReader stream = new (csvPath);
        using CsvReader csv = new (stream, CultureInfo.InvariantCulture);

        // Skip the header row.
        csv.Read();
        csv.ReadHeader();

        // Create or update the planet records.
        while (csv.Read())
        {
            string? name = csv.GetField(0);
            string? type = csv.GetField(2);

            if (name == null)
            {
                continue;
            }

            AstroObject planet;
            try
            {
                planet = astroObjectRepository.LoadByName(name, "Planet");
                // Update planet in the database.
                Console.WriteLine($"Updating planet {name}.");
                astroDbContext.AstroObjects.Attach(planet);
            }
            catch (DataNotFoundException)
            {
                // Create a new planet in the database.
                Console.WriteLine($"Adding new planet {name}.");
                planet = new AstroObject(name);
                astroDbContext.AstroObjects.Add(planet);
            }

            // Set the planet's basic parameters.

            // Set its number.
            string? num = csv.GetField(1);
            planet.Number = num == null ? null : uint.Parse(num);

            // Set its groups.
            astroObjectGroupRepository.AddToGroup(planet, "Planet");
            astroObjectGroupRepository.AddToGroup(planet, $"{type} planet");

            // Set its parent.
            planet.Parent = sun;

            // Save the planet object now to ensure it has an id before attaching composition
            // objects to it.
            astroDbContext.SaveChanges();

            // Orbital parameters.
            planet.Orbit ??= new OrbitalRecord();

            // Apoapsis is provided in km, convert to m.
            planet.Orbit.Apoapsis = GetDoubleValue(csv, 3, 1000);

            // Periapsis is provided in km, convert to m.
            planet.Orbit.Periapsis = GetDoubleValue(csv, 4, 1000);

            // Semi-major axis is provided in km, convert to m.
            planet.Orbit.SemiMajorAxis = GetDoubleValue(csv, 5, 1000);

            // Eccentricity.
            planet.Orbit.Eccentricity = GetDoubleValue(csv, 6);

            // Sidereal orbit period is provided in days, convert to seconds.
            planet.Orbit.SiderealOrbitPeriod =
                GetDoubleValue(csv, 7, TimeConstants.SECONDS_PER_DAY);

            // Synodic orbit period is provided in days, convert to seconds.
            planet.Orbit.SynodicOrbitPeriod = GetDoubleValue(csv, 8, TimeConstants.SECONDS_PER_DAY);

            // Avg orbital speed is provided in km/s, convert to m/s.
            planet.Orbit.AvgOrbitSpeed = GetDoubleValue(csv, 9, 1000);

            // Mean anomaly is provided in degrees, convert to radians.
            planet.Orbit.MeanAnomaly = GetDoubleValue(csv, 10, Angles.RADIANS_PER_DEGREE);

            // Inclination is provided in degrees, convert to radians.
            planet.Orbit.Inclination = GetDoubleValue(csv, 11, Angles.RADIANS_PER_DEGREE);

            // Long. of asc. node is provided in degrees, convert to radians.
            planet.Orbit.LongAscNode = GetDoubleValue(csv, 12, Angles.RADIANS_PER_DEGREE);

            // Arg. of perihelion is provided in degrees, convert to radians.
            planet.Orbit.ArgPeriapsis = GetDoubleValue(csv, 13, Angles.RADIANS_PER_DEGREE);

            // Calculate the mean motion in rad/s.
            planet.Orbit.MeanMotion = Math.Tau / planet.Orbit.SiderealOrbitPeriod;

            // Save the orbital parameters.
            astroDbContext.SaveChanges();

            // Physical parameters.
            planet.Physical ??= new PhysicalRecord();
            double? equatRadius = GetDoubleValue(csv, 15, 1000);
            double? polarRadius = GetDoubleValue(csv, 16, 1000);
            planet.Physical.SetSpheroidalShape(equatRadius ?? 0, polarRadius ?? 0);
            planet.Physical.MeanRadius = GetDoubleValue(csv, 14, 1000);
            planet.Physical.Flattening = GetDoubleValue(csv, 17);
            planet.Physical.SurfaceArea = GetDoubleValue(csv, 18, 1e6);
            planet.Physical.Volume = GetDoubleValue(csv, 19, 1e9);
            planet.Physical.Mass = GetDoubleValue(csv, 20);

            double? density = GetDoubleValue(csv, 21);
            planet.Physical.Density =
                density == null ? null : Density.GramsPerCm3ToKgPerM3(density.Value);

            planet.Physical.SurfaceGrav = GetDoubleValue(csv, 22);
            planet.Physical.MomentOfInertiaFactor = GetDoubleValue(csv, 23);
            planet.Physical.EscapeVelocity = GetDoubleValue(csv, 24, 1000);
            planet.Physical.StdGravParam = GetDoubleValue(csv, 25);
            planet.Physical.GeometricAlbedo = GetDoubleValue(csv, 26);
            planet.Physical.SolarIrradiance = GetDoubleValue(csv, 27);
            planet.Physical.HasGlobalMagField = csv.GetField(28) == "Y";
            planet.Physical.HasRingSystem = csv.GetField(29) == "Y";
            planet.Physical.MinSurfaceTemp = GetDoubleValue(csv, 36);
            planet.Physical.MeanSurfaceTemp = GetDoubleValue(csv, 37);
            planet.Physical.MaxSurfaceTemp = GetDoubleValue(csv, 38);
            astroDbContext.SaveChanges();

            // Rotational parameters.
            planet.Rotation ??= new RotationalRecord();
            planet.Rotation.SynodicRotationPeriod =
                GetDoubleValue(csv, 30, TimeConstants.SECONDS_PER_DAY);
            planet.Rotation.SiderealRotationPeriod =
                GetDoubleValue(csv, 31, TimeConstants.SECONDS_PER_DAY);
            planet.Rotation.EquatRotationVelocity = GetDoubleValue(csv, 32);
            planet.Rotation.Obliquity = GetDoubleValue(csv, 33, Angles.RADIANS_PER_DEGREE);
            planet.Rotation.NorthPoleRightAscension =
                GetDoubleValue(csv, 34, Angles.RADIANS_PER_DEGREE);
            planet.Rotation.NorthPoleDeclination =
                GetDoubleValue(csv, 35, Angles.RADIANS_PER_DEGREE);
            astroDbContext.SaveChanges();

            // Atmosphere.
            planet.Atmosphere ??= new AtmosphereRecord();
            planet.Atmosphere.SurfacePressure = GetDoubleValue(csv, 40);
            planet.Atmosphere.ScaleHeight = GetDoubleValue(csv, 41, 1000);
            planet.Atmosphere.IsSurfaceBoundedExosphere = name == "Mercury";
            astroDbContext.SaveChanges();
        }
    }
}
