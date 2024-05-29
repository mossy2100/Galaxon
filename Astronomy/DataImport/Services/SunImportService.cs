using Galaxon.Astronomy.Data;
using Galaxon.Astronomy.Data.Models;
using Galaxon.Astronomy.Data.Repositories;
using Galaxon.Core.Exceptions;
using Galaxon.Quantities.Kinds;
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
    public async Task Import()
    {
        AstroObjectRecord sun;
        try
        {
            sun = astroObjectRepository.LoadByName("Sun", "Star");
            Console.WriteLine("Updating the Sun in the database.");
        }
        catch (DataNotFoundException)
        {
            Console.WriteLine("Adding the Sun to the database.");
            // Create new object.
            sun = new AstroObjectRecord("Sun");
            astroDbContext.AstroObjects.Add(sun);
            await astroDbContext.SaveChangesAsync();
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
        sun.Stellar.Luminosity_W = 3.828e26;
        // Mean radiance in W/m2/sr.
        sun.Stellar.Radiance_W_sr_m2 = 2.009e7;
        await astroDbContext.SaveChangesAsync();

        // Observational parameters.
        sun.Observation ??= new ObservationRecord();
        // Apparent magnitude.
        sun.Observation.MinApparentMagnitude = -26.74;
        sun.Observation.MaxApparentMagnitude = -26.74;
        // Absolute magnitude.
        sun.Observation.AbsoluteMagnitude = 4.83;
        // Angular diameter.
        sun.Observation.MinAngularDiameter_deg = DegreesToRadians(0.527);
        sun.Observation.MaxAngularDiameter_deg = DegreesToRadians(0.545);
        await astroDbContext.SaveChangesAsync();

        // Physical parameters.
        sun.Physical ??= new PhysicalRecord();
        // Size and shape.
        sun.Physical.SetSphericalShape(695_700_000);
        // Flattening.
        sun.Physical.Flattening = 9e-6;
        // Surface area in m2.
        sun.Physical.SurfaceArea_km2 = 6.09e18;
        // Volume in m3.
        sun.Physical.Volume_km3 = 1.41e27;
        // Mass in kg.
        sun.Physical.Mass_kg = 1.9885e30;
        // Density in kg/m3.
        sun.Physical.Density_kg_m3 = Density.GramsPerCm3ToKgPerM3(1.408);
        // Gravity in m/s2.
        sun.Physical.SurfaceGravity_m_s2 = 274;
        // Moment of inertia factor.
        // sun.Physical.MomentOfInertiaFactor = 0.070;
        // Escape velocity in m/s.
        sun.Physical.EscapeVelocity_km_s = 617_700;
        // Standard gravitational parameter in m3/s2.
        sun.Physical.StandardGravitationalParameter_m3_s2 = 1.327_124_400_18e20;
        // Color B-V
        // sun.Physical.ColorBV = 0.63;
        // Magnetic field.
        sun.Physical.HasGlobalMagneticField = true;
        // Ring system.
        sun.Physical.HasRings = false;
        // Mean surface temperature (photosphere).
        sun.Physical.MeanSurfaceTemperature_K = 5772;
        await astroDbContext.SaveChangesAsync();

        // Orbital parameters.
        sun.Orbit ??= new OrbitRecord();
        // 29,000 light years in metres.
        sun.Orbit.SemiMajorAxis_km = (double)29_000 * Length.METRES_PER_LIGHT_YEAR;
        // 230 million years in seconds.
        sun.Orbit.SiderealOrbitPeriod_d = 230_000_000.0 * TimeConstants.SECONDS_PER_YEAR;
        // Orbital speed in m/s.
        sun.Orbit.AverageOrbitalSpeed_km_s = 251_000;
        await astroDbContext.SaveChangesAsync();

        // Rotational parameters.
        sun.Rotation ??= new RotationRecord();
        // Obliquity in radians.
        sun.Rotation.Obliquity_deg = DegreesToRadians(7.25);
        // North Pole location in radians.
        sun.Rotation.NorthPoleRightAscension_deg = DegreesToRadians(286.13);
        sun.Rotation.NorthPoleDeclination_deg = DegreesToRadians(63.87);
        // Sidereal rotation period in seconds.
        sun.Rotation.SiderealRotationPeriod_d = 25.05 * TimeConstants.SECONDS_PER_DAY;
        // Equatorial rotation velocity in m/s.
        sun.Rotation.EquatorialRotationalVelocity_m_s = 1997;
        await astroDbContext.SaveChangesAsync();

        // Atmosphere.
        sun.Atmosphere ??= new AtmosphereRecord();
        sun.Atmosphere.IsSurfaceBoundedExosphere = false;
        sun.Atmosphere.ScaleHeight_km = 140e3;

        // Make sure all the molecules are in the database.
        MoleculeRecord.CreateOrUpdate(astroDbContext, "Hydrogen", "H");
        MoleculeRecord.CreateOrUpdate(astroDbContext, "Helium", "He");
        MoleculeRecord.CreateOrUpdate(astroDbContext, "Oxygen", "O");
        MoleculeRecord.CreateOrUpdate(astroDbContext, "Carbon", "C");
        MoleculeRecord.CreateOrUpdate(astroDbContext, "Iron", "Fe");
        MoleculeRecord.CreateOrUpdate(astroDbContext, "Neon", "Ne");
        MoleculeRecord.CreateOrUpdate(astroDbContext, "Nitrogen", "N");
        MoleculeRecord.CreateOrUpdate(astroDbContext, "Silicon", "Si");
        MoleculeRecord.CreateOrUpdate(astroDbContext, "Magnesium", "Mg");
        MoleculeRecord.CreateOrUpdate(astroDbContext, "Sulphur", "S");

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

        await astroDbContext.SaveChangesAsync();
    }
}
