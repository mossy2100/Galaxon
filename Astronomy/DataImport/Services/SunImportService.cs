using Galaxon.Astronomy.Data;
using Galaxon.Astronomy.Data.Models;
using Galaxon.Astronomy.Data.Repositories;
using Galaxon.Numerics.Geometry;
using Galaxon.Quantities;
using Galaxon.Time;

namespace Galaxon.Astronomy.DataImport.Services;

public class SunImportService(
    AstroDbContext astroDbContext,
    AstroObjectRepository astroObjectRepository,
    AstroObjectGroupRepository astroObjectGroupRepository)
{
    /// <summary>
    /// Initialize the Stars data, which for now just means adding the Sun to the database.
    /// </summary>
    public void Import()
    {
        AstroObject? sun = astroObjectRepository.Load("Sun", "Star");
        if (sun == null)
        {
            Console.WriteLine("Adding the Sun to the database.");
            // Create tne new object.
            sun = new AstroObject("Sun");
            astroDbContext.AstroObjects.Add(sun);
            astroDbContext.SaveChanges();
        }
        else
        {
            Console.WriteLine("Updating the Sun in the database.");
        }

        // Add the Sun to the right groups.
        astroObjectGroupRepository.AddToGroup(sun, "Star");
        astroObjectGroupRepository.AddToGroup(sun, "Main sequence");
        astroObjectGroupRepository.AddToGroup(sun, "Yellow dwarf");
        astroDbContext.SaveChanges();

        // Stellar parameters.
        sun.Stellar ??= new StellarRecord();
        // Spectral class.
        sun.Stellar.SpectralClass = "G2V";
        // Metallicity.
        sun.Stellar.Metallicity = 0.0122;
        // Luminosity in watts.
        sun.Stellar.Luminosity = 3.828e26;
        // Mean radiance in W/m2/sr.
        sun.Stellar.Radiance = 2.009e7;
        astroDbContext.SaveChanges();

        // Observational parameters.
        sun.Observation ??= new ObservationalRecord();
        // Apparent magnitude.
        sun.Observation.MinApparentMag = -26.74;
        sun.Observation.MaxApparentMag = -26.74;
        // Absolute magnitude.
        sun.Observation.AbsMag = 4.83;
        // Angular diameter.
        sun.Observation.MinAngularDiam = Angles.DegreesToRadians(0.527);
        sun.Observation.MaxAngularDiam = Angles.DegreesToRadians(0.545);
        astroDbContext.SaveChanges();

        // Physical parameters.
        sun.Physical ??= new PhysicalRecord();
        // Size and shape.
        sun.Physical.SetSphericalShape(695_700_000);
        // Flattening.
        sun.Physical.Flattening = 9e-6;
        // Surface area in m2.
        sun.Physical.SurfaceArea = 6.09e18;
        // Volume in m3.
        sun.Physical.Volume = 1.41e27;
        // Mass in kg.
        sun.Physical.Mass = 1.9885e30;
        // Density in kg/m3.
        sun.Physical.Density = Density.GramsPerCm3ToKgPerM3(1.408);
        // Gravity in m/s2.
        sun.Physical.SurfaceGrav = 274;
        // Moment of inertia factor.
        sun.Physical.MomentOfInertiaFactor = 0.070;
        // Escape velocity in m/s.
        sun.Physical.EscapeVelocity = 617_700;
        // Standard gravitational parameter in m3/s2.
        sun.Physical.StdGravParam = 1.327_124_400_18e20;
        // Color B-V
        sun.Physical.ColorBV = 0.63;
        // Magnetic field.
        sun.Physical.HasGlobalMagField = true;
        // Ring system.
        sun.Physical.HasRingSystem = false;
        // Mean surface temperature (photosphere).
        sun.Physical.MeanSurfaceTemp = 5772;
        astroDbContext.SaveChanges();

        // Orbital parameters.
        sun.Orbit ??= new OrbitalRecord();
        // 29,000 light years in metres.
        sun.Orbit.SemiMajorAxis = 29_000 * LengthConstants.METRES_PER_LIGHT_YEAR;
        // 230 million years in seconds.
        sun.Orbit.SiderealOrbitPeriod = 230_000_000 * TimeConstants.SECONDS_PER_YEAR;
        // Orbital speed in m/s.
        sun.Orbit.AvgOrbitSpeed = 251_000;
        astroDbContext.SaveChanges();

        // Rotational parameters.
        sun.Rotation ??= new RotationalRecord();
        // Obliquity in radians.
        sun.Rotation.Obliquity = Angles.DegreesToRadians(7.25);
        // North pole location in radians.
        sun.Rotation.NorthPoleRightAscension = Angles.DegreesToRadians(286.13);
        sun.Rotation.NorthPoleDeclination = Angles.DegreesToRadians(63.87);
        // Sidereal rotation period in seconds.
        sun.Rotation.SiderealRotationPeriod = 25.05 * TimeConstants.SECONDS_PER_DAY;
        // Equatorial rotation velocity in m/s.
        sun.Rotation.EquatRotationVelocity = 1997;
        astroDbContext.SaveChanges();

        // Atmosphere.
        sun.Atmosphere ??= new AtmosphereRecord();
        sun.Atmosphere.IsSurfaceBoundedExosphere = false;
        sun.Atmosphere.ScaleHeight = 140e3;

        // Make sure all the molecules are in the database.
        Molecule.CreateOrUpdate(astroDbContext, "Hydrogen", "H");
        Molecule.CreateOrUpdate(astroDbContext, "Helium", "He");
        Molecule.CreateOrUpdate(astroDbContext, "Oxygen", "O");
        Molecule.CreateOrUpdate(astroDbContext, "Carbon", "C");
        Molecule.CreateOrUpdate(astroDbContext, "Iron", "Fe");
        Molecule.CreateOrUpdate(astroDbContext, "Neon", "Ne");
        Molecule.CreateOrUpdate(astroDbContext, "Nitrogen", "N");
        Molecule.CreateOrUpdate(astroDbContext, "Silicon", "Si");
        Molecule.CreateOrUpdate(astroDbContext, "Magnesium", "Mg");
        Molecule.CreateOrUpdate(astroDbContext, "Sulphur", "S");

        // Add the constituents.
        sun.Atmosphere.AddConstituent(astroDbContext, "H", 73.46);
        sun.Atmosphere.AddConstituent(astroDbContext, "He", 24.85);
        sun.Atmosphere.AddConstituent(astroDbContext, "O", 0.77);
        sun.Atmosphere.AddConstituent(astroDbContext, "C", 0.29);
        sun.Atmosphere.AddConstituent(astroDbContext, "Fe", 0.16);
        sun.Atmosphere.AddConstituent(astroDbContext, "Ne", 0.12);
        sun.Atmosphere.AddConstituent(astroDbContext, "N", 0.09);
        sun.Atmosphere.AddConstituent(astroDbContext, "Si", 0.07);
        sun.Atmosphere.AddConstituent(astroDbContext, "Mg", 0.05);
        sun.Atmosphere.AddConstituent(astroDbContext, "S", 0.04);

        astroDbContext.SaveChanges();
    }
}
