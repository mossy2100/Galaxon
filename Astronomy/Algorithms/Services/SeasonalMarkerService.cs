using Galaxon.Astronomy.Algorithms.Records;
using Galaxon.Astronomy.Data;
using Galaxon.Astronomy.Data.Enums;
using Galaxon.Astronomy.Data.Models;
using Galaxon.Numerics.Algebra;
using Galaxon.Numerics.Geometry;
using Galaxon.Time;

namespace Galaxon.Astronomy.Algorithms.Services;

public class SeasonalMarkerService(AstroDbContext astroDbContext, SunService sunService)
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
    /// Calculate mean value for a seasonal marker as as a Julian Date (TT).
    /// Algorithm from AA2 p178.
    /// </summary>
    /// <param name="year">The year (Gregorian) in the range -1000..3000.</param>
    /// <param name="marker">The seasonal marker (as enum).</param>
    /// <returns>The result as a Julian Date (TT).</returns>
    public double GetSeasonalMarkerMean(int year, ESeasonalMarker marker)
    {
        // Check year is in valid range.
        if (year is < -1000 or > 3000)
        {
            throw new ArgumentOutOfRangeException(nameof(year),
                "Must be in the range -1000..3000.");
        }

        // Check seasonal marker is in valid range.
        if (marker is < ESeasonalMarker.NorthwardEquinox
            or > ESeasonalMarker.SouthernSolstice)
        {
            throw new ArgumentOutOfRangeException(nameof(marker), "Invalid value.");
        }

        // Find instant of mean seasonal marker.
        double Y;
        if (year <= 1000)
        {
            Y = year / 1000.0;
            return marker switch
            {
                ESeasonalMarker.NorthwardEquinox => Polynomials.EvaluatePolynomial([
                    1721139.29189, 365242.13740, 0.06134, 0.00111, -0.00071
                ], Y),
                ESeasonalMarker.NorthernSolstice => Polynomials.EvaluatePolynomial([
                    1721233.25401, 365241.72562, -0.05323, 0.00907, 0.00025
                ], Y),
                ESeasonalMarker.SouthwardEquinox => Polynomials.EvaluatePolynomial([
                    1721325.70455, 365242.49558, -0.11677, -0.00297, 0.00074
                ], Y),
                ESeasonalMarker.SouthernSolstice => Polynomials.EvaluatePolynomial([
                    1721414.39987, 365242.88257, -0.00769, -0.00933, -0.00006
                ], Y),
                _ => throw new ArgumentOutOfRangeException(nameof(marker),
                    "Invalid value.")
            };
        }
        else
        {
            Y = (year - 2000) / 1000.0;
            return marker switch
            {
                ESeasonalMarker.NorthwardEquinox => Polynomials.EvaluatePolynomial([
                    2451623.80984, 365242.37404, 0.05169, -0.00411, -0.00057
                ], Y),
                ESeasonalMarker.NorthernSolstice => Polynomials.EvaluatePolynomial([
                    2451716.56767, 365241.62603, 0.00325, 0.00888, -0.0003
                ], Y),
                ESeasonalMarker.SouthwardEquinox => Polynomials.EvaluatePolynomial([
                    2451810.21715, 365242.01767, -0.11575, 0.00337, 0.00078
                ], Y),
                ESeasonalMarker.SouthernSolstice => Polynomials.EvaluatePolynomial([
                    2451900.05952, 365242.74049, -0.06223, -0.00823, 0.00032
                ], Y),
                _ => throw new ArgumentOutOfRangeException(nameof(marker),
                    "Invalid value.")
            };
        }
    }

    // /// <summary>
    // /// Expands the range of GetSeasonalMarkerMean() by using the
    // /// GetTropicalYearLengthInEphemerisDays() method.
    // /// </summary>
    // /// <param name="year">The year to get the seasonal marker for.</param>
    // /// <param name="marker">The marker type.</param>
    // /// <returns>The approximate Julian date (TT) of the event.</returns>
    // private double GetSeasonalMarkerMeanExpanded(int year, ESeasonalMarker marker)
    // {
    //     // Check year is in valid range.
    //     if (year is < -7999 or > 12000)
    //     {
    //         throw new ArgumentOutOfRangeException(nameof(year),
    //             "Must be in the range 8000 BCE to 12000 CE (-7999..12000).");
    //     }
    //
    //     // Get the approximate result as a Julian Date (TT).
    //     double jdtt;
    //     if (year < -1000)
    //     {
    //         jdtt = GetSeasonalMarkerMean(-1000, marker);
    //         for (int i = -1000; i > year; i--)
    //         {
    //             jdtt -= EarthService.GetTropicalYearInEphemerisDaysForYear(year);
    //         }
    //     }
    //     else if (year > 3000)
    //     {
    //         jdtt = GetSeasonalMarkerMean(3000, marker);
    //         for (int i = 3000; i < year; i++)
    //         {
    //             jdtt += EarthService.GetTropicalYearInEphemerisDaysForYear(year);
    //         }
    //     }
    //     else
    //     {
    //         jdtt = GetSeasonalMarkerMean(year, marker);
    //     }
    //
    //     return jdtt;
    // }

    /// <summary>
    /// Calculate approximate datetime of a seasonal marker as as a Julian Date (TT).
    /// The algorithm is from "Astronomical Algorithms, 2nd Ed." by Jean Meeus, Chapter 27
    /// "Equinoxes and Solstices" (pp177-180).
    /// This method is accurate to within 51 seconds for years 1951-2050 (see the book).
    /// For improved accuracy <see cref="GetSeasonalMarker"/>
    /// </summary>
    /// <param name="year">The year (-1000..3000)</param>
    /// <param name="marker">The seasonal marker (as enum)</param>
    /// <returns>The result as a Julian Date (TT).</returns>
    public double GetSeasonalMarkerApprox(int year, ESeasonalMarker marker)
    {
        double JDE0 = GetSeasonalMarkerMean(year, marker);
        double T = TimeScales.JulianCenturiesSinceJ2000(JDE0);
        double W = Angles.DegreesToRadians(35999.373 * T - 2.47);
        double dLambda = 1 + 0.0334 * Cos(W) + 0.0007 * Cos(2 * W);

        // Sum the periodic terms from Table 27.C.
        double S = PeriodicTerms.Sum(term =>
            term.A * Cos(Angles.DegreesToRadians(term.B + term.C * T)));

        // Equation from p178.
        return JDE0 + 0.000_01 * S / dLambda;
    }

    /// <summary>
    /// Loop until sufficient accuracy is achieved between the supplied Julian Date (TT) and the
    /// desired value for Ls (longitude of the Sun).
    /// </summary>
    /// <param name="jdtt">Initial estimate for Julian Date (TT).</param>
    /// <param name="targetLs">Target longitude of the Sun, in radians.</param>
    /// <returns>Improved value for the Julian Date (TT)</returns>
    private double _LoopUntilDesiredPrecision(double jdtt, double targetLs)
    {
        // Get the maximum difference in radians. As we want precision of 1 second, set this equal
        // to the angular distance the mean Sun moves through the ecliptic in half a second. This
        // might be excessively precise given other imprecisions in the calculation method, but the
        // method doesn't take too long with this value.
        const double sunMovementRadiansPerHalfSecond =
            Angles.RADIANS_PER_CIRCLE / TimeConstants.SECONDS_PER_TROPICAL_YEAR / 2;

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
    /// High-accuracy method for calculating seasonal marker as as a Julian Date (TT).
    /// The algorithm is from "Astronomical Algorithms, 2nd Ed." by Jean Meeus, Chapter 27
    /// "Equinoxes and Solstices" (p180).
    /// </summary>
    /// <param name="year">The year (-1000..3000)</param>
    /// <param name="marker">The marker number (use enum)</param>
    /// <returns>The result as a Julian Date (TT).</returns>
    public double GetSeasonalMarker(int year, ESeasonalMarker marker)
    {
        // Get an approximate time for the event.
        double jdtt = GetSeasonalMarkerMean(year, marker);

        // Find the target Ls in radians (0, π/2, -π, or -π/2).
        double targetLs = Angles.WrapRadians((int)marker * Angles.RADIANS_PER_QUADRANT);

        // Improve the initial value until the desired accuracy is reached.
        return _LoopUntilDesiredPrecision(jdtt, targetLs);
    }

    /// <summary>
    /// Get a seasonal marker as a DateTime (UT).
    /// </summary>
    /// <param name="year">The year (-1000..3000)</param>
    /// <param name="marker">The marker number (use enum)</param>
    /// <returns>The result as a DateTime (UT).</returns>
    public DateTime GetSeasonalMarkerAsDateTime(int year, ESeasonalMarker marker)
    {
        double jdtt = GetSeasonalMarker(year, marker);
        return TimeScales.JulianDateTerrestrialToDateTimeUniversal(jdtt);
    }

    /// <summary>
    /// Get all four seasonal markers in a year.
    /// </summary>
    /// <param name="year">The year (-1000..3000)</param>
    /// <returns>The result as a collection of SeasonalMarker objects.</returns>
    public List<SeasonalMarkerEvent> GetSeasonalMarkersInYear(int year)
    {
        return Enum.GetValues(typeof(ESeasonalMarker))
            .Cast<ESeasonalMarker>()
            .Select(markerType =>
                new SeasonalMarkerEvent(markerType, GetSeasonalMarkerAsDateTime(year, markerType)))
            .ToList();
    }

    /// <summary>
    /// Get the specified seasonal marker, deferring first to other calculations likely to be better
    /// than mine, i.e. USNO and AstroPixels.
    /// Because the USNO values are DateTime (UT), this method also returns a DateTime (UT) rather
    /// than a Julian Date (TT) like the others.
    /// </summary>
    /// <param name="year">The year (-1000..3000)</param>
    /// <param name="marker">The marker number (use enum)</param>
    /// <returns>The result as a DateTime (UT).</returns>
    public DateTime GetSeasonalMarkerAsDateTimeHumble(int year, ESeasonalMarker marker)
    {
        // Look for a matching seasonal marker in the database with a USNO datetime.
        SeasonalMarkerRecord? seasonalMarker =
            astroDbContext.SeasonalMarkers.FirstOrDefault(sm =>
                sm.Type == marker
                && sm.DateTimeUtcUsno != null
                && sm.DateTimeUtcUsno.Value.Year == year);

        // If a USNO value is found, use that, otherwise use my calculation.
        return seasonalMarker != null
            ? seasonalMarker.DateTimeUtcUsno!.Value
            : GetSeasonalMarkerAsDateTime(year, marker);
    }

    /// <summary>
    /// Calculate the moment of the "Besselian New Year" (Ls=280°) at the end of a given Gregorian
    /// year.
    /// Note, the result could be early in the following year.
    /// </summary>
    /// <param name="year">The year (-1000..3000)</param>
    /// <returns>The result as a Julian Date (TT).</returns>
    public double GetBesselianNewYear(int year)
    {
        // Get the approximate moment of the Southern Solstice (which occurs at Ls=270°) as a
        // Julian Date (TT).
        double jdtt = GetSeasonalMarkerMean(year, ESeasonalMarker.SouthernSolstice);

        // Add 10° (in days) to get the approximate moment when Ls=280°.
        jdtt += TimeConstants.DAYS_PER_TROPICAL_YEAR / 36;

        // Find the target Ls in radians.
        double targetLs = Angles.DegreesToRadiansWithWrap(280);

        // Improve the initial value until the desired accuracy is reached.
        return _LoopUntilDesiredPrecision(jdtt, targetLs);
    }

    #endregion Instance methods
}
