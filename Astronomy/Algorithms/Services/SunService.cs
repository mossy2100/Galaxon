using Galaxon.Astronomy.Algorithms.Utilities;
using Galaxon.Astronomy.Data;
using Galaxon.Astronomy.Data.Models;
using Galaxon.Astronomy.Data.Repositories;
using Galaxon.Numerics.Extensions;
using Galaxon.Numerics.Geometry;
using Galaxon.Quantities;
using Galaxon.Time;

namespace Galaxon.Astronomy.Algorithms.Services;

public class SunService(
    AstroDbContext astroDbContext,
    AstroObjectGroupRepository astroObjectGroupRepository,
    AstroObjectRepository astroObjectRepository,
    PlanetService planetService,
    EarthService earthService)
{
    /// <summary>
    /// Initialize the Stars data, which for now just means adding the Sun to the database.
    /// </summary>
    public void InitializeData()
    {
        AstroObject? sun = astroObjectRepository.Load("Sun", "star");
        if (sun == null)
        {
            Console.WriteLine("Adding the Sun to the database.");
            // Create tne new object.
            sun = new AstroObject("Sun");
        }
        else
        {
            Console.WriteLine("Updating the Sun in the database.");
        }

        // Set the Sun's basic details.
        astroObjectGroupRepository.AddToGroup(sun, "star");

        // Save the Sun object so it has an Id.
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
        // 29,000 light years in metres (rounded to 2 significant figures).
        sun.Orbit.SemiMajorAxis =
            DoubleExtensions.RoundSigFigs(29e3 * LengthConstants.METRES_PER_LIGHT_YEAR, 2);
        // 230 million years in seconds (rounded to 2 significant figures).
        sun.Orbit.SiderealOrbitPeriod =
            DoubleExtensions.RoundSigFigs(230e6 * TimeConstants.SECONDS_PER_YEAR, 2);
        // Orbital speed in m/s.
        sun.Orbit.AvgOrbitSpeed = 251e3;
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
        // Make sure all the molecules are in the database first.
        Molecule.CreateOrUpdate(astroDbContext, "hydrogen", "H");
        Molecule.CreateOrUpdate(astroDbContext, "helium", "He");
        Molecule.CreateOrUpdate(astroDbContext, "oxygen", "O");
        Molecule.CreateOrUpdate(astroDbContext, "carbon", "C");
        Molecule.CreateOrUpdate(astroDbContext, "iron", "Fe");
        Molecule.CreateOrUpdate(astroDbContext, "neon", "Ne");
        Molecule.CreateOrUpdate(astroDbContext, "nitrogen", "N");
        Molecule.CreateOrUpdate(astroDbContext, "silicon", "Si");
        Molecule.CreateOrUpdate(astroDbContext, "magnesium", "Mg");
        Molecule.CreateOrUpdate(astroDbContext, "sulphur", "S");
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
        sun.Atmosphere.IsSurfaceBoundedExosphere = false;
        sun.Atmosphere.ScaleHeight = 140e3;
        astroDbContext.SaveChanges();
    }

    /// <summary>
    /// Calculate apparent solar latitude and longitude for a given instant specified as a Julian
    /// Date in Terrestrial Time (this is also known as a Julian Ephemeris Day or JED (or JDE), and
    /// differs from the Julian Date by ∆T).
    /// This method uses the higher accuracy algorithm from AA2 Ch25 p166 (p174 in PDF)
    /// </summary>
    /// <param name="JD_TT">The Julian Ephemeris Day.</param>
    /// <returns>The longitude of the Sun (Ls) in radians at the given
    /// instant.</returns>
    public (double Longitude, double Latitude) CalcPosition(double JD_TT)
    {
        // Get the Earth's heliocentric position.
        var earth = earthService.GetPlanet();
        (double Le, double Be, double Re) = planetService.CalcPlanetPosition(earth, JD_TT);

        // Reverse to get the mean dynamical ecliptic and equinox of the date.
        double Ls = Angles.WrapRadians(Le + PI);
        double Bs = -Be;

        // Convert to FK5.
        // This gives the true ("geometric") longitude of the Sun referred to the mean equinox of
        // the date.
        double julCen = JulianDateUtility.JulianCenturiesSinceJ2000(JD_TT);
        double lambdaPrime = Ls
            - Angles.DegreesToRadians(1.397) * julCen
            - Angles.DegreesToRadians(0.000_31) * julCen * julCen;
        Ls -= Angles.DMSToRadians(0, 0, 0.090_33);
        Bs += Angles.DMSToRadians(0, 0, 0.039_16)
            * (Cos(lambdaPrime) - Sin(lambdaPrime));

        // TODO
        // To obtain the apparent longitude, nutation and aberration have to be taken into account.
        // SofaLibrary.iauNut06a(JD_TT, 0, out double dpsi, out double deps);
        // lngSun += dpsi;

        // Calculate and add aberration.
        double julMil = julCen / 10;
        double julMil2 = julMil * julMil;
        double dLambda_as = 3548.193
            + 118.568 * Angles.SinDegrees(87.5287 + 359_993.7286 * julMil)
            + 2.476 * Angles.SinDegrees(85.0561 + 719_987.4571 * julMil)
            + 1.376 * Angles.SinDegrees(27.8502 + 4452_671.1152 * julMil)
            + 0.119 * Angles.SinDegrees(73.1375 + 450_368.8564 * julMil)
            + 0.114 * Angles.SinDegrees(337.2264 + 329_644.6718 * julMil)
            + 0.086 * Angles.SinDegrees(222.5400 + 659_289.3436 * julMil)
            + 0.078 * Angles.SinDegrees(162.8136 + 9224_659.7915 * julMil)
            + 0.054 * Angles.SinDegrees(82.5823 + 1079_981.1857 * julMil)
            + 0.052 * Angles.SinDegrees(171.5189 + 225_184.4282 * julMil)
            + 0.034 * Angles.SinDegrees(30.3214 + 4092_677.3866 * julMil)
            + 0.033 * Angles.SinDegrees(119.8105 + 337_181.4711 * julMil)
            + 0.023 * Angles.SinDegrees(247.5418 + 299_295.6151 * julMil)
            + 0.023 * Angles.SinDegrees(325.1526 + 315_559.5560 * julMil)
            + 0.021 * Angles.SinDegrees(155.1241 + 675_553.2846 * julMil)
            + 7.311 * julMil * Angles.SinDegrees(333.4515 + 359_993.7286 * julMil)
            + 0.305 * julMil * Angles.SinDegrees(330.9814 + 719_987.4571 * julMil)
            + 0.010 * julMil * Angles.SinDegrees(328.5170 + 1079_981.1857 * julMil)
            + 0.309 * julMil2 * Angles.SinDegrees(241.4518 + 359_993.7286 * julMil)
            + 0.021 * julMil2 * Angles.SinDegrees(205.0482 + 719_987.4571 * julMil)
            + 0.004 * julMil2 * Angles.SinDegrees(297.8610 + 4452_671.1152 * julMil)
            + 0.010 * julMil2 * Angles.SinDegrees(154.7066 + 359_993.7286 * julMil);
        double dLambda_rad = dLambda_as / Angles.ARCSECONDS_PER_RADIAN;
        double R_AU = Re / LengthConstants.METRES_PER_ASTRONOMICAL_UNIT;
        double aberration = -0.005_775_518 * R_AU * dLambda_rad;
        Ls += aberration;

        return (Ls, Bs);
    }

    /// <summary>
    /// Calculate apparent solar latitude and longitude for a given instant specified as a DateTime
    /// (UT).
    /// </summary>
    /// <param name="dt">The instant specified as a DateTime (UT).</param>
    /// <returns>The latitude and longitude of the Sun, in radians, at the given instant.</returns>
    public (double Longitude, double Latitude) CalcPosition(DateTime dt)
    {
        double JD = JulianDateUtility.DateTime_to_JulianDate(dt);
        double JD_TT = JulianDateUtility.JulianDate_UT_to_TT(JD);
        return CalcPosition(JD_TT);
    }
}
