using System.Data;
using Galaxon.Astronomy.Data.Enums;
using Galaxon.Astronomy.Data.Models;
using Galaxon.Astronomy.Data.Repositories;
using Galaxon.Core.Exceptions;
using Galaxon.Numerics.Algebra;
using Galaxon.Time;

namespace Galaxon.Astronomy.Algorithms.Services;

public class ApsideService(
    AstroObjectRepository astroObjectRepository,
    AstroObjectGroupRepository astroObjectGroupRepository,
    PlanetService planetService)
{
    /// <summary>
    /// Coefficients copied from start of Chapter 38 in Astronomical Algorithms (2nd ed.) by Jeen
    /// Meeus (p. 269).
    /// </summary>
    private static readonly Dictionary<uint, double[]> _Coefficients = new ()
    {
        { 1, [2_451_590.257, 87.969_349_63, 0] },
        { 2, [2_451_738.233, 224.700_818_8, -0.000_000_032_7] },
        { 3, [2_451_547.507, 365.259_635_8, 0.000_000_015_6] },
        { 4, [2_452_195.026, 686.995_785_7, -0.000_000_118_7] },
        { 5, [2_455_636.936, 4_332.897_065, 0.000_136_7] },
        { 6, [2_452_830.12, 10_764.216_76, 0.000_827] },
        { 7, [2_470_213.5, 30_694.876_7, -0.005_41] },
        { 8, [2_468_895.1, 60_190.33, 0.034_29] },
    };

    /// <summary>
    /// Get the perihelion or aphelion closest to a given point in time, specified by a Julian Date
    /// (TT).
    /// Algorithm is from Chapter 38 in Astronomical Algorithms (2nd ed.) by Jeen Meeus (p. 269).
    /// </summary>
    /// <param name="planet">The planet.</param>
    /// <param name="apside">Perihelion or aphelion.</param>
    /// <param name="jdtt0">The approximate moment of the apside.</param>
    /// <returns>The moment of the apside as a Julian Date (TT).</returns>
    public DateTime GetClosestApsideApprox(AstroObject planet, EApside apside, DateTime dt)
    {
        // Check the object is a major planet.
        if (!astroObjectGroupRepository.IsInGroup(planet, "Planet"))
        {
            throw new ArgumentOutOfRangeException(nameof(planet),
                "Only major planets are supported.");
        }

        // Check the planet number. If there's an exception here, the issue is with the data.
        if (planet.Number == null || planet.Number < 1 || planet.Number > 8)
        {
            throw new DataException("Planet number must be in the range 1-8.");
        }

        // Convert the DateTime to a decimal year.
        double year = TimeScales.DateTimeToDecimalYear(dt);

        // Get approximate value for k.
        double k = planet.Number switch
        {
            1 => 4.15201 * (year - 2000.12),
            2 => 1.62549 * (year - 2000.53),
            3 => 0.99997 * (year - 2000.01),
            4 => 0.53166 * (year - 2001.78),
            5 => 0.08430 * (year - 2011.20),
            6 => 0.03393 * (year - 2003.52),
            7 => 0.01190 * (year - 2051.1),
            8 => 0.00607 * (year - 2047.5),
            _ => 0
        };

        // Round off k to nearest integer value for perihelion, or integer + 0.5 for aphelion.
        if (apside == EApside.Periapsis)
        {
            k = double.Round(k);
        }
        else
        {
            k = double.Round(k - 0.5) + 0.5;
        }

        // Get the coefficients for the given planet.
        double[] coeffs = _Coefficients[planet.Number.Value];

        // Plug into formula to get the JD(TT) of the perihelion.
        double jdtt = Polynomials.EvaluatePolynomial(coeffs, k);

        // Get the event datetime.
        return TimeScales.JulianDateTerrestrialToDateTimeUniversal(jdtt);
    }

    public (DateTime, double) GetClosestApside(AstroObject planet, EApside apside, DateTime dt)
    {
        // Get the orbital period.
        double? orbitalPeriodInSeconds = planet.Orbit?.SiderealOrbitPeriod;
        if (orbitalPeriodInSeconds == null)
        {
            throw new DataNotFoundException("Sidereal orbit period not found in the database.");
        }

        // Get the approximate value.
        DateTime dt1 = GetClosestApsideApprox(planet, apside, dt);

        // Convert the event datetime to a Julian Date (TT).
        double jdtt1 = TimeScales.DateTimeUniversalToJulianDateTerrestrial(dt1);

        // Use a golden-section search to find a more accurate result.
        // Set the initial boundaries to ±1% of the approximate value.
        double fracOfOrbit = orbitalPeriodInSeconds.Value / TimeConstants.SECONDS_PER_DAY / 100;
        double a = jdtt1 - fracOfOrbit;
        double b = jdtt1 + fracOfOrbit;

        // We ideally want a result within one second of the actual apside event.
        double tolerance = 1.0 / TimeConstants.SECONDS_PER_DAY;

        // The function to calculate distance to Sun (radius) from JD(TT).
        Func<double, double> func = jdtt => planetService.CalcPlanetPosition(planet, jdtt).Radius;

        // If we are looking for a minumum or a maximum radius.
        Boolean findMax = apside == EApside.Apoapsis;

        // Run the search.
        (double jdtt2, double radius) =
            GoldenSectionSearch.FindExtremum(func, a, b, findMax, tolerance);

        // Special handling for Earth.
        if (planet.Name == "Earth") { }

        // Get the event datetime.
        DateTime dt2 = TimeScales.JulianDateTerrestrialToDateTimeUniversal(jdtt2);
        return (dt2, radius);
    }
}
