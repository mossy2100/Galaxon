using System.Data;
using Galaxon.Astronomy.Algorithms.Records;
using Galaxon.Astronomy.Data.Enums;
using Galaxon.Astronomy.Data.Models;
using Galaxon.Astronomy.Data.Repositories;
using Galaxon.Core.Exceptions;
using Galaxon.Numerics.Algebra;
using Galaxon.Numerics.Geometry;
using Galaxon.Quantities.Kinds;
using Galaxon.Time;

namespace Galaxon.Astronomy.Algorithms.Services;

public class ApsideService(
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
        { 8, [2_468_895.1, 60_190.33, 0.034_29] }
    };

    /// <summary>
    /// Values for correcting apside result for Earth, taken from AA2 p273.
    /// </summary>
    private static readonly double[][] _EarthCorrectionValues =
    [
        [328.41, 132.788_585, 1.278, -1.352],
        [316.13, 584.903_153, -0.055, 0.061],
        [346.2, 450.380_738, -0.091, 0.062],
        [136.95, 659.306_737, -0.056, 0.029],
        [249.52, 329.653_368, -0.045, 0.031]
    ];

    /// <summary>
    /// Get the perihelion or aphelion closest to a given point in time, specified by a Julian Date
    /// (TT).
    /// Approximate version (faster).
    /// Algorithm is from Chapter 38 in Astronomical Algorithms (2nd ed.) by Jeen Meeus (p. 269).
    /// </summary>
    /// <param name="planet">The planet.</param>
    /// <param name="jdtt">The approximate moment of the apside.</param>
    /// <returns>The apside event information, excluding the radius (distance to the Sun).</returns>
    public ApsideEvent GetClosestApsideApprox(AstroObjectRecord planet, double jdtt)
    {
        // Check the object is a major planet.
        if (!astroObjectGroupRepository.IsInGroup(planet, "Planet"))
        {
            throw new ArgumentOutOfRangeException(nameof(planet),
                "Only major planets are supported.");
        }

        // Check the planet number. If there's an exception here, the issue is with the data.
        if (planet.Number is null or < 1 or > 8)
        {
            throw new DataException("Planet number must be in the range 1-8.");
        }

        // Convert the DateTime to a decimal year without doing any delta-T calculation, as the
        // input JD(TT) is only approximate.
        double year = TimeScales.JulianDateToDecimalYear(jdtt);

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

        // Round off k to nearest 0.5.
        k = Round(k * 2) / 2;
        EApside apside = double.IsInteger(k) ? EApside.Periapsis : EApside.Apoapsis;

        // Get the coefficients for the given planet.
        double[] coeffs = _Coefficients[planet.Number.Value];

        // Plug into formula to get the JD(TT) of the perihelion.
        double jdtt2 = Polynomials.EvaluatePolynomial(coeffs, k);

        // Special handling for Earth.
        if (planet.Name == "Earth")
        {
            double correction = 0;
            foreach (double[] values in _EarthCorrectionValues)
            {
                if (apside == EApside.Periapsis)
                {
                    correction += values[2] * Angles.SinDegrees(values[0] + values[1] * k);
                }
                else
                {
                    correction += values[3] * Angles.SinDegrees(values[0] + values[1] * k);
                }
            }
            jdtt2 += correction;
        }

        // Get the orbit number.
        int orbitNumber = (int)Floor(k);

        // Get the datetime rounded off to the nearest minute.
        DateTime dt2 = TimeScales.JulianDateTerrestrialToDateTimeUniversal(jdtt2)
            .RoundToNearestMinute();

        return new ApsideEvent(planet, orbitNumber, apside, jdtt2, dt2);
    }

    /// <summary>
    /// Get the perihelion or aphelion closest to a given point in time, specified by a Julian Date
    /// (TT).
    /// Accurate version (slower).
    /// Algorithm is from Chapter 38 in Astronomical Algorithms (2nd ed.) by Jeen Meeus (p. 269).
    /// </summary>
    /// <param name="planet">The planet.</param>
    /// <param name="jdtt">The approximate moment of the apside as a Julian Date (TT).</param>
    /// <returns>The apside event information.</returns>
    public ApsideEvent GetClosestApside(AstroObjectRecord planet, double jdtt)
    {
        // Get the orbital period in days.
        double? orbitalPeriodInSeconds = planet.Orbit?.SiderealOrbitPeriod;
        if (orbitalPeriodInSeconds == null)
        {
            throw new DataNotFoundException("Sidereal orbit period not found in the database.");
        }
        double orbitalPeriodInDays = orbitalPeriodInSeconds.Value / TimeConstants.SECONDS_PER_DAY;

        // Get the approximate moment of the apside.
        ApsideEvent approxApsideEvent = GetClosestApsideApprox(planet, jdtt);
        double jdtt1 = approxApsideEvent.JulianDateTerrestrial;

        // Set the tolerance for the search. Ideally we want a result within one second of the
        // actual apside event.
        double tolerance = 1.0 / TimeConstants.SECONDS_PER_DAY;

        // The function to calculate distance to Sun (radius) from JD(TT).
        Func<double, double> func = jdtt => planetService.CalcPlanetPosition(planet, jdtt).Radius;

        // If we are looking for a minimum or a maximum radius.
        bool findMax = approxApsideEvent.Type == EApside.Apoapsis;

        // Result variables.
        double jdttResult;
        double radiusInMetresResult;

        // Use a golden-section search to find a more accurate result.
        // Handle Neptune differently because of the potential for a double minimum or maximum.
        if (planet.Name == "Neptune")
        {
            // Set the initial boundaries to ±5% of the approximate value for Neptune.
            double fracOfOrbit = orbitalPeriodInDays / 20;
            double a = jdtt1 - fracOfOrbit;
            double b = jdtt1 + fracOfOrbit;

            // Run 2 searches, which may find extremum on either side of the approximate result.
            (double jdtt3, double radiusInMetres3) =
                GoldenSectionSearch.FindExtremum(func, a, jdtt1, findMax, tolerance);
            (double jdtt4, double radiusInMetres4) =
                GoldenSectionSearch.FindExtremum(func, jdtt1, b, findMax, tolerance);

            // Compare to see which result is best.
            if ((!findMax && radiusInMetres3 < radiusInMetres4)
                || (findMax && radiusInMetres3 > radiusInMetres4))
            {
                jdttResult = jdtt3;
                radiusInMetresResult = radiusInMetres3;
            }
            else
            {
                jdttResult = jdtt4;
                radiusInMetresResult = radiusInMetres4;
            }
        }
        else
        {
            // For other planets set the initial boundaries to ±1% of the approximate value.
            double fracOfOrbit = orbitalPeriodInDays / 100;
            double a = jdtt1 - fracOfOrbit;
            double b = jdtt1 + fracOfOrbit;
            (jdttResult, radiusInMetresResult) =
                GoldenSectionSearch.FindExtremum(func, a, b, findMax, tolerance);
        }

        // Get the datetime of the event rounded off to the nearest minute.
        DateTime dtResult = TimeScales.JulianDateTerrestrialToDateTimeUniversal(jdttResult)
            .RoundToNearestMinute();

        // Compute the distance in AU.
        double radiusInAu = radiusInMetresResult / Length.METRES_PER_ASTRONOMICAL_UNIT;

        return new ApsideEvent(planet, approxApsideEvent.Orbit, approxApsideEvent.Type,
            jdttResult, dtResult, radiusInMetresResult, radiusInAu);
    }
}
