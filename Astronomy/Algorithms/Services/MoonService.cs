using System.Data;
using Galaxon.Astronomy.Algorithms.Models;
using Galaxon.Astronomy.Data;
using Galaxon.Astronomy.Data.Enums;
using Galaxon.Astronomy.Data.Models;
using Galaxon.Astronomy.Data.Repositories;
using Galaxon.Core.Exceptions;
using Galaxon.Numerics.Algebra;
using Galaxon.Numerics.Extensions;
using Galaxon.Numerics.Geometry;
using Galaxon.Time;
using Microsoft.EntityFrameworkCore;

namespace Galaxon.Astronomy.Algorithms.Services;

/// <summary>
/// Stuff relating to the Moon.
/// </summary>
/// <param name="astroObjectRepository"></param>
public class MoonService(AstroDbContext astroDbContext, AstroObjectRepository astroObjectRepository)
{
    /// <summary>
    /// Cached reference to the AstroObject representing Luna.
    /// </summary>
    private AstroObject? _luna;

    #region Instance methods

    /// <summary>
    /// Get the AstroObject representing Luna.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="DataNotFoundException"></exception>
    public AstroObject GetPlanet()
    {
        if (_luna == null)
        {
            AstroObject? luna = astroObjectRepository.Load("Luna", "Satellite");
            _luna = luna
                ?? throw new DataNotFoundException(
                    "Could not find the Moon (Luna) in the database.");
        }

        return _luna;
    }

    /// <summary>
    /// Get the lunar phase closest to the given DateTime, deferring first to other calculations
    /// likely to be better than mine, i.e. USNO and AstroPixels.
    /// </summary>
    /// <param name="dt">The approximate DateTime of the phase.</param>
    /// <returns>The closest lunar phase.</returns>
    public MoonPhase GetPhaseNearDateTimeHumble(DateTime dt)
    {
        // Get any lunar phases from the database that match within 1 day.
        LunarPhase? lunarPhase = astroDbContext.LunarPhases
            .FirstOrDefault(lp =>
                (lp.DateTimeUtcUsno != null
                    && Abs(EF.Functions.DateDiffDay(lp.DateTimeUtcUsno, dt)!.Value) <= 1)
                || (lp.DateTimeUtcAstroPixels != null
                    && Abs(EF.Functions.DateDiffDay(lp.DateTimeUtcAstroPixels, dt)!.Value) <= 1));

        if (lunarPhase != null)
        {
            if (lunarPhase.DateTimeUtcUsno != null)
            {
                // We found a USNO calculation, so return that.
                return new MoonPhase
                {
                    Type = lunarPhase.Type,
                    DateTimeUtc = lunarPhase.DateTimeUtcUsno!.Value
                };
            }
            if (lunarPhase.DateTimeUtcAstroPixels != null)
            {
                // We found an AstroPixels calculation, so return that.
                return new MoonPhase
                {
                    Type = lunarPhase.Type,
                    DateTimeUtc = lunarPhase.DateTimeUtcAstroPixels!.Value
                };
            }
        }

        // Use my calculation.
        return GetPhaseNearDateTime(dt);
    }

    #endregion Instance methods

    #region Static methods

    /// <summary>
    /// Find the DateTime (UTC) of the next specified lunar phase closest to the given DateTime.
    /// Algorithm taken from Chapter 49 "Phases of the Moon", Astronomical Algorithms 2nd ed. by
    /// Jean Meeus (1998).
    /// </summary>
    /// <param name="dt">Approximate DateTime of the phase.</param>
    /// <returns>
    /// A LunarPhase object, which contains information about which phase it is, and the approximate
    /// datetime of the event.
    /// </returns>
    public static MoonPhase GetPhaseNearDateTime(DateTime dt)
    {
        // Calculate k, rounded off to nearest 0.25.
        TimeSpan timeSinceLunation0 = dt - TimeConstants.LUNATION_0_START;
        double k = (double)timeSinceLunation0.Ticks / TimeConstants.TICKS_PER_LUNATION;
        int phaseNumber = (int)Round(k * 4.0);
        k = phaseNumber / 4.0;

        // Calculate T and powers of T.
        DateTime dtPhaseApprox =
            TimeConstants.LUNATION_0_START.AddDays(k * TimeConstants.DAYS_PER_LUNATION);
        double jdtt = JulianDateService.DateTimeUniversalToJulianDateTerrestrial(dtPhaseApprox);
        double T = JulianDateService.JulianCenturiesSinceJ2000(jdtt);
        double T2 = T * T;
        double T3 = T * T2;
        double T4 = T * T3;

        // Calculate jdtt.
        jdtt = 2_451_550.097_66
            + 29.530_588_861 * k
            + 0.000_154_37 * T2
            + 0.000_000_150 * T3
            + 0.000_000_000_73 * T4;

        // Calculate E.
        double E = 1 - 0.002_516 * T - 0.000_0074 * T2;
        double E2 = E * E;

        // Calculate Sun's mean anomaly at time jdtt (radians).
        double M = Angles.DegreesToRadiansWithWrap(2.5534
            + 29.105_356_70 * k
            - 0.000_001_4 * T2
            - 0.000_000_11 * T3);

        // Calculate Luna's mean anomaly at time jdtt (radians).
        double L = Angles.DegreesToRadiansWithWrap(201.5643
            + 385.816_935_28 * k
            + 0.010_758_2 * T2
            + 0.000_012_38 * T3
            - 0.000_000_058 * T4);

        // Calculate Luna's argument of latitude (radians).
        double F = Angles.DegreesToRadiansWithWrap(160.710_8
            + 390.670_502_84 * k
            - 0.001_6118 * T2
            - 0.000_002_27 * T2
            + 0.000_000_011 * T4);

        // Calculate the longitude of the ascending node of the lunar orbit (radians).
        double Omega = Angles.DegreesToRadiansWithWrap(124.7746
            - 1.563_755_88 * k
            + 0.002_0672 * T2
            + 0.000_002_15 * T3);

        // Calculate planetary arguments (radians).
        double A1 = Angles.DegreesToRadiansWithWrap(299.77 + 0.107_408 * k - 0.009_173 * T2);
        double A2 = Angles.DegreesToRadiansWithWrap(251.88 + 0.016_321 * k);
        double A3 = Angles.DegreesToRadiansWithWrap(251.83 + 26.651_886 * k);
        double A4 = Angles.DegreesToRadiansWithWrap(349.42 + 36.412_478 * k);
        double A5 = Angles.DegreesToRadiansWithWrap(84.66 + 18.206_239 * k);
        double A6 = Angles.DegreesToRadiansWithWrap(141.74 + 53.303_771 * k);
        double A7 = Angles.DegreesToRadiansWithWrap(207.14 + 2.453_732 * k);
        double A8 = Angles.DegreesToRadiansWithWrap(154.84 + 7.306_860 * k);
        double A9 = Angles.DegreesToRadiansWithWrap(34.52 + 27.261_239 * k);
        double A10 = Angles.DegreesToRadiansWithWrap(207.19 + 0.121_824 * k);
        double A11 = Angles.DegreesToRadiansWithWrap(291.34 + 1.844_379 * k);
        double A12 = Angles.DegreesToRadiansWithWrap(161.72 + 24.198_154 * k);
        double A13 = Angles.DegreesToRadiansWithWrap(239.56 + 25.513_099 * k);
        double A14 = Angles.DegreesToRadiansWithWrap(331.55 + 3.592_518 * k);

        // I'm using Mod() here instead of the modulo operator (%) because the phaseNumber can be
        // negative and we want a non-negative result.
        ELunarPhaseType phaseType = (ELunarPhaseType)NumberExtensions.Mod(phaseNumber, 4);
        double C1;
        if (phaseType is ELunarPhaseType.NewMoon or ELunarPhaseType.FullMoon)
        {
            if (phaseType == ELunarPhaseType.NewMoon)
            {
                // Phase type is New Moon.
                C1 = -0.40720 * Sin(L)
                    + 0.17241 * E * Sin(M)
                    + 0.01608 * Sin(2 * L)
                    + 0.01039 * Sin(2 * F)
                    + 0.00739 * E * Sin(L - M)
                    - 0.00514 * E * Sin(L + M)
                    + 0.00208 * E2 * Sin(2 * M);
            }
            else
            {
                // Phase type is Full Moon.
                C1 = -0.40614 * Sin(L)
                    + 0.17302 * E * Sin(M)
                    + 0.01614 * Sin(2 * L)
                    + 0.01043 * Sin(2 * F)
                    + 0.00734 * E * Sin(L - M)
                    - 0.00515 * E * Sin(L + M)
                    + 0.00209 * E2 * Sin(2 * M);
            }

            // Phase type is New Moon or Full Moon.
            C1 += -0.00111 * Sin(L - 2 * F)
                - 0.00057 * Sin(L + 2 * F)
                + 0.00056 * E * Sin(2 * L + M)
                - 0.00042 * Sin(3 * L)
                + 0.00042 * E * Sin(M + 2 * F)
                + 0.00038 * E * Sin(M - 2 * F)
                - 0.00024 * E * Sin(2 * L - M)
                - 0.00017 * Sin(Omega)
                - 0.00007 * Sin(L + 2 * M)
                + 0.00004 * Sin(2 * (L - F))
                + 0.00004 * Sin(3 * M)
                + 0.00003 * Sin(L + M - 2 * F)
                + 0.00003 * Sin(2 * (L + F))
                - 0.00003 * Sin(L + M + 2 * F)
                + 0.00003 * Sin(L - M + 2 * F)
                - 0.00002 * Sin(L - M - 2 * F)
                - 0.00002 * Sin(3 * L + M)
                + 0.00002 * Sin(4 * L);
        }
        else
        {
            // Phase type is First Quarter or Third Quarter.
            C1 = -0.62801 * Sin(L)
                + 0.17172 * E * Sin(M)
                - 0.01183 * E * Sin(L + M)
                + 0.00862 * Sin(2 * L)
                + 0.00804 * Sin(2 * F)
                + 0.00454 * E * Sin(L - M)
                + 0.00204 * E2 * Sin(2 * M)
                - 0.00180 * Sin(L - 2 * F)
                - 0.00070 * Sin(L + 2 * F)
                - 0.00040 * Sin(3 * L)
                - 0.00034 * E * Sin(2 * L - M)
                + 0.00032 * E * Sin(M + 2 * F)
                + 0.00032 * E * Sin(M - 2 * F)
                - 0.00028 * E2 * Sin(L + 2 * M)
                + 0.00027 * E * Sin(2 * L + M)
                - 0.00017 * Sin(Omega)
                - 0.00005 * Sin(L - M - 2 * F)
                + 0.00004 * Sin(2 * (L + F))
                - 0.00004 * Sin(L + M + 2 * F)
                + 0.00004 * Sin(L - 2 * M)
                + 0.00003 * Sin(L + M - 2 * F)
                + 0.00003 * Sin(3 * M)
                + 0.00002 * Sin(2 * (L - F))
                + 0.00002 * Sin(L - M + 2 * F)
                - 0.00002 * Sin(3 * L + M);

            // Calculate additional correction for first and third quarter phases.
            double W = 0.00306
                - 0.00038 * E * Cos(M)
                + 0.00026 * Cos(L)
                - 0.00002 * Cos(L - M)
                + 0.00002 * Cos(L + M)
                + 0.00002 * Cos(2 * F);
            jdtt += phaseType == ELunarPhaseType.FirstQuarter ? W : -W;
        }

        // Additional correction for all phases.
        double C2 = 0.000_325 * Sin(A1)
            + 0.000_165 * Sin(A2)
            + 0.000_164 * Sin(A3)
            + 0.000_126 * Sin(A4)
            + 0.000_110 * Sin(A5)
            + 0.000_062 * Sin(A6)
            + 0.000_060 * Sin(A7)
            + 0.000_056 * Sin(A8)
            + 0.000_047 * Sin(A9)
            + 0.000_042 * Sin(A10)
            + 0.000_040 * Sin(A11)
            + 0.000_037 * Sin(A12)
            + 0.000_035 * Sin(A13)
            + 0.000_023 * Sin(A14);

        // Apply corrections.
        jdtt += C1 + C2;

        // Convert the jdtt to a UTC DateTime.
        DateTime dtPhase = JulianDateService.JulianDateTerrestrialToDateTimeUniversal(jdtt);

        // Construct and return the LunarPhase object.
        return new MoonPhase { Type = phaseType, DateTimeUtc = dtPhase };
    }

    /// <summary>
    /// Get the DateTimes of all lunar phases in a given period.
    /// </summary>
    /// <param name="start">The start of the period.</param>
    /// <param name="end">The end of the period.</param>
    /// <param name="phaseType">The phase type to find, or null for all.</param>
    /// <returns></returns>
    public static List<MoonPhase> GetPhasesInPeriod(DateTime start, DateTime end,
        ELunarPhaseType? phaseType = null)
    {
        List<MoonPhase> result = [];

        // Find the phase nearest to start.
        MoonPhase phase = GetPhaseNearDateTime(start);

        // If it's in range, add it.
        if (phase.DateTimeUtc >= start)
        {
            result.Add(phase);
        }

        // Get the average number of days between phases.
        const double daysPerPhase = TimeConstants.DAYS_PER_LUNATION / 4.0;

        // Iteratively jump forward to the approximate moment of the next phase, and find the
        // exact moment of the phase, until we reach the finish DateTime.
        while (true)
        {
            // Get the next new moon in the series.
            DateTime nextGuess = phase.DateTimeUtc.AddDays(daysPerPhase);
            phase = GetPhaseNearDateTime(nextGuess);

            // We done?
            if (phase.DateTimeUtc > end)
            {
                break;
            }

            // Add it to the result.
            if (phaseType == null || phase.Type == phaseType)
            {
                result.Add(phase);
            }
        }

        return result;
    }

    /// <summary>
    /// Get the DateTimes of all occurrences of the specified phase in a given Gregorian calendar
    /// month (UTC).
    /// </summary>
    /// <param name="year">The year number.</param>
    /// <param name="month">The month number.</param>
    /// <param name="phaseType">The phase type to find, or null for all.</param>
    /// <returns>A list of lunar phases.</returns>
    public static List<MoonPhase> GetPhasesInMonth(int year, int month,
        ELunarPhaseType? phaseType = null)
    {
        // Check year is valid. Valid range matches DateTime.IsLeapYear().
        if (year is < 1 or > 9999)
        {
            throw new ArgumentOutOfRangeException(nameof(year),
                "Year must be in the range 1..9999");
        }

        // Check month is valid. Valid range is 1-12.
        if (month is < 1 or > 12)
        {
            throw new ArgumentOutOfRangeException(nameof(month),
                "Month must be in the range 1..12");
        }

        return GetPhasesInPeriod(
            GregorianCalendarExtensions.MonthStart(year, month, DateTimeKind.Utc),
            GregorianCalendarExtensions.MonthEnd(year, month, DateTimeKind.Utc), phaseType);
    }

    /// <summary>
    /// Get the DateTimes of all occurrences of lunar phases (optionally restricted to the specified
    /// phase) in a given Gregorian calendar year (UTC).
    /// </summary>
    /// <param name="year">The year number.</param>
    /// <param name="phaseType">The phase type to find, or null for all.</param>
    /// <returns>A list of lunar phases.</returns>
    public static List<MoonPhase> GetPhasesInYear(int year, ELunarPhaseType? phaseType = null)
    {
        // Check year is valid. Valid range matches DateTime.IsLeapYear().
        if (year is < 1 or > 9999)
        {
            throw new ArgumentOutOfRangeException(nameof(year),
                "Year must be in the range 1..9999");
        }

        return GetPhasesInPeriod(
            GregorianCalendarExtensions.YearStart(year, DateTimeKind.Utc),
            GregorianCalendarExtensions.YearEnd(year, DateTimeKind.Utc), phaseType);
    }

    /// <summary>
    /// Calculate the approximate average length of a lunation in SI seconds at a point in time.
    /// The formula is taken from Wikipedia:
    /// <see href="https://en.wikipedia.org/wiki/Lunar_month#Synodic_month"/>
    /// </summary>
    /// <param name="T">The number of Julian centuries since noon, January 1, 2000.</param>
    /// <returns>The average lunation length in seconds at that point in time.</returns>
    public static double GetLengthOfLunation1(double T)
    {
        return Polynomials.EvaluatePolynomial([29.530_588_8531, 0.000_000_216_21, -3.64e-10], T);
    }

    /// <summary>
    /// Calculate the approximate average length of a lunation in SI seconds for a given year.
    /// The year can have a fractional part.
    /// </summary>
    /// <param name="y">The year as a decimal.</param>
    /// <returns>The average lunation length in seconds at that point in time.</returns>
    public static double GetLengthOfLunation(double y)
    {
        // Calculate T, the number of Julian centuries since noon, January 1, 2000 (TT).
        double jdtt = TimeScaleService.DecimalYearToJulianDateUniversal(y);
        double T = JulianDateService.JulianCenturiesSinceJ2000(jdtt);
        return GetLengthOfLunation1(T);
    }

    #endregion Static methods
}
