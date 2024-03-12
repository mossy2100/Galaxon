using System.Globalization;
using CsvHelper;
using Galaxon.Astronomy.Data;
using Galaxon.Astronomy.Data.Models;
using Galaxon.Astronomy.Data.Repositories;
using Galaxon.Numerics.Geometry;
using Galaxon.Quantities;
using Galaxon.Time;

namespace Galaxon.Astronomy.DataImport.Services;

public class PlanetImportService(
    AstroDbContext astroDbContext,
    AstroObjectRepository astroObjectRepository,
    AstroObjectGroupRepository astroObjectGroupRepository)
{
    /// <summary>
    /// Initialize the planet data from the CSV file.
    /// </summary>
    public void ImportPlanets()
    {
        // Get the Sun.
        AstroObject? sun = astroObjectRepository.Load("Sun", "star");

        // Open the CSV file for parsing.
        var csvPath = $"{AstroDbContext.DataDirectory()}/Planets/Planets.csv";
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

            AstroObject? planet = astroObjectRepository.Load(name, "planet");

            if (planet == null)
            {
                // Create a new planet in the database.
                Console.WriteLine($"Adding new planet {name}.");
                planet = new AstroObject(name);
                astroDbContext.AstroObjects.Add(planet);
            }
            else
            {
                // Update planet in the database.
                Console.WriteLine($"Updating planet {name}.");
                astroDbContext.AstroObjects.Attach(planet);
            }

            // Set the planet's basic parameters.

            // Set its number.
            string? num = csv.GetField(1);
            planet.Number = num == null ? null : uint.Parse(num);

            // Set its groups.
            astroObjectGroupRepository.AddToGroup(planet, "planet");
            astroObjectGroupRepository.AddToGroup(planet, $"{type} planet");

            // Set its parent.
            planet.Parent = sun;

            // Save the planet object now to ensure it has an Id before attaching composition
            // objects to it.
            astroDbContext.SaveChanges();

            // Orbital parameters.
            // TODO: Fix this. The Orbit and other objects aren't loaded by default now when the
            // TODO: planet is loaded, so we have to look in the database for the Orbit object
            // TODO: first, before creating a new one.
            planet.Orbit ??= new OrbitalRecord();

            // Apoapsis is provided in km, convert to m.
            const double kilo = 1000;
            double apoapsis = double.Parse(csv.GetField(3));
            planet.Orbit.Apoapsis = apoapsis * kilo;

            // Periapsis is provided in km, convert to m.
            double periapsis = double.Parse(csv.GetField(4));
            planet.Orbit.Periapsis = periapsis * kilo;

            // Semi-major axis is provided in km, convert to m.
            double semiMajorAxis = double.Parse(csv.GetField(5));
            planet.Orbit.SemiMajorAxis = semiMajorAxis * kilo;

            // Eccentricity.
            planet.Orbit.Eccentricity = double.Parse(csv.GetField(6));

            // Sidereal orbit period is provided in days, convert to seconds.
            planet.Orbit.SiderealOrbitPeriod =
                double.Parse(csv.GetField(7)) * TimeConstants.SECONDS_PER_DAY;

            // Synodic orbit period is provided in days, convert to seconds.
            var synodicPeriod = double.Parse(csv.GetField(8));
            planet.Orbit.SynodicOrbitPeriod = synodicPeriod * TimeConstants.SECONDS_PER_DAY;

            // Avg orbital speed is provided in km/s, convert to m/s.
            planet.Orbit.AvgOrbitSpeed = double.Parse(csv.GetField(9)) * kilo;

            // Mean anomaly is provided in degrees, convert to radians.
            planet.Orbit.MeanAnomaly = double.Parse(csv.GetField(10)) * Angles.RADIANS_PER_DEGREE;

            // Inclination is provided in degrees, convert to radians.
            planet.Orbit.Inclination = double.Parse(csv.GetField(11)) * Angles.RADIANS_PER_DEGREE;

            // Long. of asc. node is provided in degrees, convert to radians.
            planet.Orbit.LongAscNode = double.Parse(csv.GetField(12)) * Angles.RADIANS_PER_DEGREE;

            // Arg. of perihelion is provided in degrees, convert to radians.
            planet.Orbit.ArgPeriapsis = double.Parse(csv.GetField(13)) * Angles.RADIANS_PER_DEGREE;

            // Calculate the mean motion in rad/s.
            planet.Orbit.MeanMotion = Math.Tau / planet.Orbit.SiderealOrbitPeriod;

            // Save the orbital parameters.
            astroDbContext.SaveChanges();

            // Physical parameters.
            planet.Physical ??= new PhysicalRecord();
            double? equatRadius = double.Parse(csv.GetField(15)) * kilo;
            double? polarRadius = double.Parse(csv.GetField(16)) * kilo;
            planet.Physical.SetSpheroidalShape(equatRadius ?? 0, polarRadius ?? 0);
            planet.Physical.MeanRadius = double.Parse(csv.GetField(14)) * kilo;
            planet.Physical.Flattening = double.Parse(csv.GetField(17));
            planet.Physical.SurfaceArea = double.Parse(csv.GetField(18)) * kilo * kilo;
            planet.Physical.Volume = double.Parse(csv.GetField(19)) * Math.Pow(kilo, 3);
            planet.Physical.Mass = double.Parse(csv.GetField(20));
            planet.Physical.Density = Density.GramsPerCm3ToKgPerM3(double.Parse(csv.GetField(21)));
            planet.Physical.SurfaceGrav = double.Parse(csv.GetField(22));
            planet.Physical.MomentOfInertiaFactor = double.Parse(csv.GetField(23));
            planet.Physical.EscapeVelocity = double.Parse(csv.GetField(24)) * kilo;
            planet.Physical.StdGravParam = double.Parse(csv.GetField(25));
            planet.Physical.GeometricAlbedo = double.Parse(csv.GetField(26));
            planet.Physical.SolarIrradiance = double.Parse(csv.GetField(27));
            planet.Physical.HasGlobalMagField = csv.GetField(28) == "Y";
            planet.Physical.HasRingSystem = csv.GetField(29) == "Y";
            planet.Physical.MinSurfaceTemp = double.Parse(csv.GetField(36));
            planet.Physical.MeanSurfaceTemp = double.Parse(csv.GetField(37));
            planet.Physical.MaxSurfaceTemp = double.Parse(csv.GetField(38));
            astroDbContext.SaveChanges();

            // Rotational parameters.
            planet.Rotation ??= new RotationalRecord();
            planet.Rotation.SynodicRotationPeriod =
                double.Parse(csv.GetField(30)) * TimeConstants.SECONDS_PER_DAY;
            planet.Rotation.SiderealRotationPeriod =
                double.Parse(csv.GetField(31)) * TimeConstants.SECONDS_PER_DAY;
            planet.Rotation.EquatRotationVelocity = double.Parse(csv.GetField(32));
            planet.Rotation.Obliquity = double.Parse(csv.GetField(33)) * Angles.RADIANS_PER_DEGREE;
            planet.Rotation.NorthPoleRightAscension =
                double.Parse(csv.GetField(34)) * Angles.RADIANS_PER_DEGREE;
            planet.Rotation.NorthPoleDeclination =
                double.Parse(csv.GetField(35)) * Angles.RADIANS_PER_DEGREE;
            astroDbContext.SaveChanges();

            // Atmosphere.
            planet.Atmosphere ??= new AtmosphereRecord();
            planet.Atmosphere.SurfacePressure = double.Parse(csv.GetField(40));
            planet.Atmosphere.ScaleHeight = double.Parse(csv.GetField(41)) * kilo;
            planet.Atmosphere.IsSurfaceBoundedExosphere = name == "Mercury";
            astroDbContext.SaveChanges();
        }
    }
}
