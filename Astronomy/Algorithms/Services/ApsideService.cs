using System.Data;
using Galaxon.Astronomy.Algorithms.Records;
using Galaxon.Astronomy.Algorithms.Utilities;
using Galaxon.Astronomy.Data.Enums;
using Galaxon.Astronomy.Data.Models;
using Galaxon.Astronomy.Data.Repositories;
using Galaxon.Core.Exceptions;
using Galaxon.Numerics.Algebra;
using Galaxon.Time;
using Galaxon.Time.Extensions;

namespace Galaxon.Astronomy.Algorithms.Services;

public class ApsideService(
    AstroObjectRepository astroObjectRepository,
    AstroObjectGroupRepository astroObjectGroupRepository,
    PlanetService planetService)
{
    /// <summary>
    /// Values used in the formula to get the approximate value for k.
    /// Copied from page 269 of (start of Chapter 38) in Astronomical Algorithms (2nd ed.) by Jeen
    /// Meeus.
    /// </summary>
    private static readonly double[]?[] _ApproximateKFormulaInputs =
    [
        null,
        [4.15201, 2000.12],
        [1.62549, 2000.53],
        [0.99997, 2000.01],
        [0.53166, 2001.78],
        [0.08430, 2011.20],
        [0.03393, 2003.52],
        [0.01190, 2051.1],
        [0.00607, 2047.5],
    ];

    /// <summary>
    /// Coefficients used in the formula to compute the moment of apside.
    /// Copied from page 269 of (start of Chapter 38) in Astronomical Algorithms (2nd ed.) by Jeen
    /// Meeus.
    /// </summary>
    private static readonly double[]?[] _ApsideFormulaCoefficients =
    [
        null,
        [2_451_590.257, 87.969_349_63, 0],
        [2_451_738.233, 224.700_818_8, -0.000_000_032_7],
        [2_451_547.507, 365.259_635_8, 0.000_000_015_6],
        [2_452_195.026, 686.995_785_7, -0.000_000_118_7],
        [2_455_636.936, 4_332.897_065, 0.000_136_7],
        [2_452_830.12, 10_764.216_76, 0.000_827],
        [2_470_213.5, 30_694.876_7, -0.005_41],
        [2_468_895.1, 60_190.33, 0.034_29]
    ];

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
    /// <param name="apsideType">
    /// The apside type to look for. Optional. If null, the algorithm will find the closest apside of
    /// either type.
    /// </param>
    /// <returns>The apside event information, excluding the radius (distance to the Sun).</returns>
    public ApsideEvent GetClosestApsideApprox(AstroObjectRecord planet, double jdtt,
        EApsideType? apsideType = null)
    {
        // Check the object is a major planet.
        if (!astroObjectGroupRepository.IsInGroup(planet, "Planet"))
        {
            throw new ArgumentOutOfRangeException(nameof(planet),
                "Only major planets are supported.");
        }

        // Check for a valid planet number.
        if (planet.Number is null
            || planet.Number < 1
            || planet.Number >= AstroObjectRepository.PLANET_NAMES.Length
            || AstroObjectRepository.PLANET_NAMES[planet.Number.Value] is null)
        {
            throw new DataException("Invalid planet number.");
        }

        // Convert the DateTime to a decimal year without making any adjustment for delta-T, as the
        // input JD(TT) is only approximate.
        double year = JulianDateUtility.JulianDateToDecimalYear(jdtt);

        // Get approximate value for k.
        double[] formulaInputs = _ApproximateKFormulaInputs[planet.Number.Value]!;
        double k = formulaInputs[0] * (year - formulaInputs[1]);

        // Calculate k depending on the type of apside we're looking for.
        switch (apsideType)
        {
            case EApsideType.Periapsis:
                // Round off k to nearest integer.
                k = Round(k);
                break;

            case EApsideType.Apoapsis:
                // Round off k to nearest "integer + 0.5" value.
                k = Round(k - 0.5) + 0.5;
                break;

            default:
                // Round off to nearest 0.5
                k = Round(k * 2) / 2;
                // Determine the apside type.
                apsideType = double.IsInteger(k) ? EApsideType.Periapsis : EApsideType.Apoapsis;
                break;
        }

        // Get the coefficients for the given planet.
        double[] coeffs = _ApsideFormulaCoefficients[planet.Number.Value]!;

        // Plug into formula to get the JD(TT) of the perihelion.
        double jdtt2 = Polynomials.EvaluatePolynomial(coeffs, k);

        // Special handling for Earth.
        if (planet.Name == "Earth")
        {
            jdtt2 += _EarthCorrectionValues.Sum(values =>
                (apsideType == EApsideType.Periapsis ? values[2] : values[3])
                * SinDegrees(values[0] + values[1] * k));
        }

        // Get the datetime rounded off to the nearest minute.
        DateTime dt2 = JulianDateUtility.JulianDateTerrestrialToDateTimeUniversal(jdtt2)
            .RoundToNearestMinute();

        // Get the orbit number.
        int orbitNumber = (int)Floor(k);

        return new ApsideEvent(planet, orbitNumber, apsideType.Value, jdtt2, dt2);
    }

    /// <summary>
    /// Get the perihelion or aphelion closest to a given point in time, specified by a Julian Date
    /// (TT).
    /// Accurate version (slower).
    /// Algorithm is from Chapter 38 in Astronomical Algorithms (2nd ed.) by Jeen Meeus (p. 269).
    /// </summary>
    /// <param name="planet">The planet.</param>
    /// <param name="jdtt">The approximate moment of the apside as a Julian Date (TT).</param>
    /// <param name="apsideType">
    /// The apside type to look for. Optional. If null, the algorithm will find the closest apside of
    /// either type.
    /// </param>
    /// <returns>The apside event information.</returns>
    public ApsideEvent GetClosestApside(AstroObjectRecord planet, double jdtt,
        EApsideType? apsideType = null)
    {
        // Get the orbital period in days.
        double? orbitalPeriod_s = planet.Orbit?.SiderealOrbitPeriod_d;
        if (orbitalPeriod_s == null)
        {
            throw new DataNotFoundException("Sidereal orbit period not found in the database.");
        }
        double orbitalPeriod_d = orbitalPeriod_s.Value / TimeConstants.SECONDS_PER_DAY;

        // Get the approximate moment of the apside.
        ApsideEvent approxApsideEvent = GetClosestApsideApprox(planet, jdtt, apsideType);
        double jdtt1 = approxApsideEvent.JulianDateTerrestrial;

        // Set the tolerance for the search. We want a result within 30 seconds of the
        // actual apside event, so we get the right datetime when rounding off the nearest minute.
        double tolerance = 30.0 / TimeConstants.SECONDS_PER_DAY;

        // The function to calculate distance to Sun (radius) from JD(TT).
        Func<double, double> func = jdtt =>
            planetService.CalcPlanetPosition(planet, jdtt).Radius_AU;

        // If we are looking for a minimum or a maximum radius.
        bool findMax = approxApsideEvent.ApsideType == EApsideType.Apoapsis;

        // Result variables.
        double jdttResult;
        double radius_AU;

        // Use a golden-section search to find a more accurate result.
        // Handle Neptune differently because of the potential for a double minimum or maximum.
        if (planet.Name == "Neptune")
        {
            // Set the initial boundaries to ±5% of the approximate value for Neptune.
            double fracOfOrbit = orbitalPeriod_d * 0.05;
            double a = jdtt1 - fracOfOrbit;
            double b = jdtt1 + fracOfOrbit;

            // Run 2 searches, which may find extremum on either side of the approximate result.
            (double jdttA, double radiusA_AU) =
                GoldenSectionSearch.FindExtremum(func, a, jdtt1, findMax, tolerance);
            (double jdttB, double radiusB_AU) =
                GoldenSectionSearch.FindExtremum(func, jdtt1, b, findMax, tolerance);

            // Compare to see which result is best.
            if ((!findMax && radiusA_AU < radiusB_AU) || (findMax && radiusA_AU > radiusB_AU))
            {
                jdttResult = jdttA;
                radius_AU = radiusA_AU;
            }
            else
            {
                jdttResult = jdttB;
                radius_AU = radiusB_AU;
            }

            if (jdttResult == a || jdttResult == b)
            {
                throw new Exception(
                    "The result JD(TT) matches a boundary value, so it's probably not the true extremum.");
            }
        }
        else
        {
            // For other planets set the initial boundaries to ±2% of the approximate value.
            double fracOfOrbit = orbitalPeriod_d * 0.01;
            double a = jdtt1 - fracOfOrbit;
            double b = jdtt1 + fracOfOrbit;
            (jdttResult, radius_AU) =
                GoldenSectionSearch.FindExtremum(func, a, b, findMax, tolerance);

            if (jdttResult == a || jdttResult == b)
            {
                throw new Exception(
                    "The result JD(TT) matches a boundary value, so it's probably not the true extremum.");
            }
        }

        // Get the datetime of the event rounded off to the nearest minute.
        DateTime dtResult = JulianDateUtility.JulianDateTerrestrialToDateTimeUniversal(jdttResult)
            .RoundToNearestMinute();

        // Return the updated event.
        return approxApsideEvent with
        {
            JulianDateTerrestrial = jdttResult,
            DateTimeUtc = dtResult,
            Radius_AU = radius_AU
        };
    }

    /// <summary>
    /// Variation of the above method that accepts the planet name as a string.
    /// </summary>
    /// <param name="planetName"></param>
    /// <param name="jdtt"></param>
    /// <param name="apsideType"></param>
    /// <returns></returns>
    public ApsideEvent GetClosestApside(string planetName, double jdtt,
        EApsideType? apsideType = null)
    {
        AstroObjectRecord planet = astroObjectRepository.LoadByName(planetName, "Planet");
        return GetClosestApside(planet, jdtt, apsideType);
    }
}
