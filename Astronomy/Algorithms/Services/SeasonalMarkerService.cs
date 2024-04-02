using Galaxon.Astronomy.Data.Enums;
using Galaxon.Numerics.Algebra;
using Galaxon.Numerics.Geometry;
using Galaxon.Time;

namespace Galaxon.Astronomy.Algorithms.Services;

public class SeasonalMarkerService(SunService sunService)
{
    #region Static fields and properties

    /// <summary>
    /// How far the mean Sun moves through its orbit in half a second.
    /// </summary>
    public const double SUN_MOVEMENT_RADIANS_PER_HALF_SECOND =
        Angles.RADIANS_PER_CIRCLE / TimeConstants.SECONDS_PER_TROPICAL_YEAR / 2;

    /// <summary>
    /// Values from Table 27.C in Astronomical Algorithms 2nd ed.
    /// NB: B and C are in degrees.
    /// </summary>
    /// <returns></returns>
    public static List<(double A, double B, double C)> PeriodicTerms { get; } =
    [
        (485, 324.96, 1934.136),
        (203, 337.23, 32964.467),
        (199, 342.08, 20.186),
        (182, 27.85, 445267.112),
        (156, 73.14, 45036.886),
        (136, 171.52, 22518.443),
        (77, 222.54, 65928.934),
        (74, 296.72, 3034.906),
        (70, 243.58, 9037.513),
        (58, 119.81, 33718.147),
        (52, 297.17, 150.678),
        (50, 21.02, 2281.226),
        (45, 247.54, 29929.562),
        (44, 325.15, 31555.956),
        (29, 60.93, 4443.417),
        (18, 155.12, 67555.328),
        (17, 288.79, 4562.452),
        (16, 198.04, 62894.029),
        (14, 199.76, 31436.921),
        (12, 95.39, 14577.848),
        (12, 287.11, 31931.756),
        (12, 320.81, 34777.259),
        (9, 227.73, 1222.114),
        (8, 15.45, 16859.074)
    ];

    #endregion Static fields and properties

    #region Instance methods

    /// <summary>
    /// High-accuracy method for calculating seasonal marker.
    /// The algorithm is from "Astronomical Algorithms, 2nd Ed." by Jean Meeus, Chapter 27
    /// "Equinoxes and Solstices" (p180).
    /// </summary>
    /// <param name="year">The year (-1000..3000)</param>
    /// <param name="markerType">The marker number (use enum)</param>
    /// <returns>The result as a Julian Date (TT).</returns>
    public double GetSeasonalMarkerAsJulianDateTerrestrialTime(int year, ESeasonalMarkerType markerType)
    {
        // Get the mean value as a Julian Date (TT).
        double jdtt = GetSeasonalMarkerMean(year, markerType);

        // Find the target Ls in radians (0, π/2, -π, or -π/2).
        double targetLs = Angles.WrapRadians((int)markerType * Angles.RADIANS_PER_QUADRANT);

        // Calculate max difference to get within 0.5 second.
        const double sunMovementRadiansPerHalfSecond =
            Angles.RADIANS_PER_CIRCLE / TimeConstants.SECONDS_PER_TROPICAL_YEAR / 2;

        // Loop until sufficient accuracy is achieved.
        do
        {
            // Get the longitude of the Sun in radians.
            (double Ls, double _, double _) = sunService.CalcPosition(jdtt);

            // Calculate the difference between the computed longitude of the Sun at this time, and
            // the target value.
            double diffLs = Angles.WrapRadians(targetLs - Ls);

            // Check if we're done.
            if (Abs(diffLs) < sunMovementRadiansPerHalfSecond)
            {
                break;
            }

            // Make a correction.
            jdtt += 58 * Sin(diffLs);
        }
        while (true);

        return jdtt;
    }

    /// <summary>
    /// High-accuracy method for calculating seasonal marker.
    /// </summary>
    /// <param name="year">The year (-1000..3000)</param>
    /// <param name="markerType">The marker number (use enum)</param>
    /// <returns>The result as a DateTime (UT).</returns>
    public DateTime GetSeasonalMarkerAsDateTime(int year, ESeasonalMarkerType markerType)
    {
        double jdtt = GetSeasonalMarkerAsJulianDateTerrestrialTime(year, markerType);

        // Convert to DateTime.
        DateTime dt = JulianDateService.JulianDateTerrestrialTimeToDateTime(jdtt);

        // Round it off to the nearest minute.
        return DateTimeExtensions.RoundToNearestMinute(dt);
    }

    /// <summary>
    /// Calculate the moment of the Besselian New Year (Ls=280°), for a given year.
    /// Note, the result could be early in the following year.
    /// </summary>
    /// <param name="year">The year (-1000..3000)</param>
    /// <returns>The result as a Julian Date (TT).</returns>
    public double GetBesselianNewYearAsJulianDateTerrestrialTime(int year)
    {
        // Get the approximate moment of the northern winter (southern summer) solstice as a Julian
        // Date (TT).
        double jdtt = GetSeasonalMarkerMean(year, ESeasonalMarkerType.SouthernSolstice);

        // Add 10°.
        jdtt += 10.0 / 360 * TimeConstants.DAYS_PER_TROPICAL_YEAR;

        // Find the target Ls in radians.
        double targetLs = Angles.WrapRadians(Angles.DegreesToRadians(280));

        // Loop until sufficient accuracy is achieved.
        do
        {
            // Get the longitude of the Sun in radians.
            (double Ls, double _, double _) = sunService.CalcPosition(jdtt);

            // Calculate the difference between the computed longitude of the Sun at this time, and
            // the target value.
            double diffLs = Angles.WrapRadians(targetLs - Ls);

            // Check if we're done.
            if (Abs(diffLs) < SUN_MOVEMENT_RADIANS_PER_HALF_SECOND)
            {
                break;
            }

            // Make a correction.
            jdtt += 58 * Sin(diffLs);
        }
        while (true);

        return jdtt;
    }

    /// <summary>
    /// High-accuracy method for calculating Besselian New Year.
    /// Note, the result could be early in the following year.
    /// </summary>
    /// <param name="year">The year (-1000..3000)</param>
    /// <returns>The result as a DateTime (UT).</returns>
    public DateTime GetBesselianNewYearAsDateTime(int year)
    {
        double jdtt = GetBesselianNewYearAsJulianDateTerrestrialTime(year);

        // Convert to DateTime.
        DateTime dt = JulianDateService.JulianDateTerrestrialTimeToDateTime(jdtt);

        // Round it off to the nearest minute.
        return DateTimeExtensions.RoundToNearestMinute(dt);
    }

    #endregion Instance methods

    #region Static methods

    /// <summary>
    /// Calculate mean value for a seasonal marker as as a Julian Date in Terrestrial Time.
    /// Algorithm from AA2 p178.
    /// </summary>
    /// <param name="year">The year (Gregorian) in the range -1000..3000.</param>
    /// <param name="markerTypeNumber">The marker number (use the enum).</param>
    /// <returns></returns>
    public double GetSeasonalMarkerMean(int year, ESeasonalMarkerType markerTypeNumber)
    {
        // Check year is in valid range.
        if (year is < -1000 or > 3000)
        {
            throw new ArgumentOutOfRangeException(nameof(year),
                "Must be in the range -1000..3000.");
        }

        // Check seasonal marker is in valid range.
        if (markerTypeNumber is < ESeasonalMarkerType.NorthwardEquinox
            or > ESeasonalMarkerType.SouthernSolstice)
        {
            throw new ArgumentOutOfRangeException(nameof(markerTypeNumber), "Invalid value.");
        }

        // Find instant of mean seasonal marker.
        double Y;
        if (year <= 1000)
        {
            Y = year / 1000.0;
            return markerTypeNumber switch
            {
                ESeasonalMarkerType.NorthwardEquinox => Polynomials.EvaluatePolynomial([
                    1721139.29189, 365242.13740, 0.06134, 0.00111, -0.00071
                ], Y),
                ESeasonalMarkerType.NorthernSolstice => Polynomials.EvaluatePolynomial([
                    1721233.25401, 365241.72562, -0.05323, 0.00907, 0.00025
                ], Y),
                ESeasonalMarkerType.SouthwardEquinox => Polynomials.EvaluatePolynomial([
                    1721325.70455, 365242.49558, -0.11677, -0.00297, 0.00074
                ], Y),
                ESeasonalMarkerType.SouthernSolstice => Polynomials.EvaluatePolynomial([
                    1721414.39987, 365242.88257, -0.00769, -0.00933, -0.00006
                ], Y),
                _ => throw new ArgumentOutOfRangeException(nameof(markerTypeNumber), "Invalid value.")
            };
        }
        else
        {
            Y = (year - 2000) / 1000.0;
            return markerTypeNumber switch
            {
                ESeasonalMarkerType.NorthwardEquinox => Polynomials.EvaluatePolynomial([
                    2451623.80984, 365242.37404, 0.05169, -0.00411, -0.00057
                ], Y),
                ESeasonalMarkerType.NorthernSolstice => Polynomials.EvaluatePolynomial([
                    2451716.56767, 365241.62603, 0.00325, 0.00888, -0.0003
                ], Y),
                ESeasonalMarkerType.SouthwardEquinox => Polynomials.EvaluatePolynomial([
                    2451810.21715, 365242.01767, -0.11575, 0.00337, 0.00078
                ], Y),
                ESeasonalMarkerType.SouthernSolstice => Polynomials.EvaluatePolynomial([
                    2451900.05952, 365242.74049, -0.06223, -0.00823, 0.00032
                ], Y),
                _ => throw new ArgumentOutOfRangeException(nameof(markerTypeNumber), "Invalid value.")
            };
        }
    }

    /// <summary>
    /// Calculate approximate datetime of a seasonal marker.
    /// The algorithm is from "Astronomical Algorithms, 2nd Ed." by Jean Meeus, Chapter 27
    /// "Equinoxes and Solstices" (pp177-180).
    /// This method is accurate to within 51 seconds for years 1951-2050 (see the book).
    /// Therefore, results are given rounded off to nearest minute, as anything more precise would
    /// be false precision.
    /// For improved accuracy <see cref="GetSeasonalMarkerAsJulianDateTerrestrialTime"/>
    /// </summary>
    /// <param name="year">The year (-1000..3000)</param>
    /// <param name="markerTypeNumber">The marker number (as enum)</param>
    /// <returns>The result in universal time.</returns>
    public DateTime GetSeasonalMarkerApprox(int year, ESeasonalMarkerType markerTypeNumber)
    {
        double JDE0 = GetSeasonalMarkerMean(year, markerTypeNumber);
        double T = JulianDateService.JulianCenturiesSinceJ2000(JDE0);
        double W = Angles.DegreesToRadians(35999.373 * T - 2.47);
        double dLambda = 1 + 0.0334 * Cos(W) + 0.0007 * Cos(2 * W);

        // Sum the periodic terms from Table 27.C.
        double S = PeriodicTerms.Sum(term =>
            term.A * Cos(Angles.DegreesToRadians(term.B + term.C * T)));

        // Equation from p178.
        double jdtt = JDE0 + 0.000_01 * S / dLambda;

        // Get the Julian Date in Universal Time.
        double jdut = JulianDateService.JulianDateTerrestrialTimeToUniversalTime(jdtt);

        // Convert to DateTime.
        DateTime dt = JulianDateService.JulianDateToDateTimeUT(jdut);

        // Round off to nearest minute.
        return DateTimeExtensions.RoundToNearestMinute(dt);
    }

    #endregion Static methods
}
