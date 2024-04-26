using Galaxon.Astronomy.Algorithms.Records;
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
    public LunarPhaseEvent GetPhaseNearDateTimeHumble(DateTime dt)
    {
        // Look for a lunar phase in the database with a USNO datetime within 24 hours.
        LunarPhaseRecord? lunarPhase = astroDbContext.LunarPhases.FirstOrDefault(lp =>
            lp.DateTimeUtcUsno != null
            && Abs(EF.Functions.DateDiffHour(lp.DateTimeUtcUsno, dt)!.Value)
            <= TimeConstants.HOURS_PER_DAY);
        if (lunarPhase != null)
        {
            // We found a USNO calculation, so return that.
            return new LunarPhaseEvent(lunarPhase.LunationNumber,
                (ELunarPhase)lunarPhase.PhaseNumber, lunarPhase.DateTimeUtcUsno!.Value);
        }

        // Look for a lunar phase in the database with an AstroPixels datetime within 24 hours.
        lunarPhase = astroDbContext.LunarPhases.FirstOrDefault(lp =>
            lp.DateTimeUtcAstroPixels != null
            && Abs(EF.Functions.DateDiffHour(lp.DateTimeUtcAstroPixels, dt)!.Value)
            <= TimeConstants.HOURS_PER_DAY);
        if (lunarPhase != null)
        {
            // We found an AstroPixels calculation, so return that.
            return new LunarPhaseEvent(lunarPhase.LunationNumber,
                (ELunarPhase)lunarPhase.PhaseNumber, lunarPhase.DateTimeUtcAstroPixels!.Value);
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
    /// An object containing information about what type of phase it is, and the approximate
    /// datetime of the event in UTC.
    /// </returns>
    public static LunarPhaseEvent GetPhaseNearDateTime(DateTime dt)
    {
        // Calculate k, rounded off to nearest 0.25.
        TimeSpan timeSinceLunation0 = dt - TimeConstants.LUNATION_0_START;
        int phaseCount =
            (int)Round((double)timeSinceLunation0.Ticks / TimeConstants.TICKS_PER_LUNAR_PHASE);
        double k = phaseCount / 4.0;

        // Get the lunation number and phase.
        int lunationNumber = (int)Floor(k);
        // Have to use Mod() here instead of % because phaseNumber can be negative and we want a
        // positive result.
        ELunarPhase phase = (ELunarPhase)NumberExtensions.Mod(phaseCount, 4);

        // Calculate T and powers of T.
        DateTime dtPhaseApprox =
            TimeConstants.LUNATION_0_START.AddDays(k * TimeConstants.DAYS_PER_LUNATION);
        double jdtt = TimeScales.DateTimeUniversalToJulianDateTerrestrial(dtPhaseApprox);
        double T = TimeScales.JulianCenturiesSinceJ2000(jdtt);
        double T2 = T * T;
        double T3 = T * T2;
        double T4 = T * T3;

        // Calculate Julian Date (TT).
        jdtt = 2_451_550.097_66
            + 29.530_588_861 * k
            + 0.000_154_37 * T2
            + 0.000_000_150 * T3
            + 0.000_000_000_73 * T4;

        // Calculate E.
        double E = 1 - 0.002_516 * T - 0.000_0074 * T2;
        double E2 = E * E;

        // Calculate Sun's mean anomaly (radians).
        double M = Angles.DegreesToRadiansWithWrap(2.5534
            + 29.105_356_70 * k
            - 0.000_001_4 * T2
            - 0.000_000_11 * T3);

        // Calculate Luna's mean anomaly (radians).
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

        // I'm using Mod() here instead of the modulo operator (%) because the phaseNumber can
        // be negative and we want a non-negative result.
        double C1;
        if (phase is ELunarPhase.NewMoon or ELunarPhase.FullMoon)
        {
            if (phase == ELunarPhase.NewMoon)
            {
                // Phase is New Moon.
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
                // Phase is Full Moon.
                C1 = -0.40614 * Sin(L)
                    + 0.17302 * E * Sin(M)
                    + 0.01614 * Sin(2 * L)
                    + 0.01043 * Sin(2 * F)
                    + 0.00734 * E * Sin(L - M)
                    - 0.00515 * E * Sin(L + M)
                    + 0.00209 * E2 * Sin(2 * M);
            }

            // Phase is New Moon or Full Moon.
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
            // Phase is First Quarter or Third Quarter.
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
            jdtt += phase == ELunarPhase.FirstQuarter ? W : -W;
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

        // Convert the Julian Date (TT) to a DateTime (UT).
        DateTime dtPhase = TimeScales.JulianDateTerrestrialToDateTimeUniversal(jdtt);

        // Construct and return the LunarPhase object.
        return new LunarPhaseEvent(lunationNumber, phase, dtPhase);
    }

    /// <summary>
    /// Find the DateTime (UTC) of the next specified lunar phase closest to the given date.
    /// </summary>
    /// <param name="d">Approximate date of the phase.</param>
    /// <returns>
    /// An object containing information about what type of phase it is, and the approximate
    /// datetime of the event in UTC.
    /// </returns>
    public static LunarPhaseEvent GetPhaseNearDate(DateOnly d)
    {
        return GetPhaseNearDateTime(d.ToDateTime());
    }

    /// <summary>
    /// Get the DateTimes of all lunar phases in a given period.
    /// </summary>
    /// <param name="start">The start of the period.</param>
    /// <param name="end">The end of the period.</param>
    /// <param name="phase">The phase to find, or null for all.</param>
    /// <returns></returns>
    public static List<LunarPhaseEvent> GetPhasesInPeriod(DateTime start, DateTime end,
        ELunarPhase? phase = null)
    {
        List<LunarPhaseEvent> result = [];

        // Find the phase nearest to start.
        LunarPhaseEvent phaseEvent = GetPhaseNearDateTime(start);

        // If it's in range, add it.
        if (phaseEvent.DateTimeUtc >= start && phaseEvent.DateTimeUtc <= end)
        {
            result.Add(phaseEvent);
        }

        // Get the average number of days between phases.
        const double daysPerPhase = TimeConstants.DAYS_PER_LUNATION / 4;

        // Iteratively jump forward to the approximate moment of the next phase, and find the
        // exact moment of the phase, until we reach the finish DateTime.
        while (true)
        {
            // Get the next new moon in the series.
            DateTime nextGuess = phaseEvent.DateTimeUtc.AddDays(daysPerPhase);
            phaseEvent = GetPhaseNearDateTime(nextGuess);

            // We done?
            if (phaseEvent.DateTimeUtc > end)
            {
                break;
            }

            // Add it to the result.
            if (phase == null || phaseEvent.Phase == phase)
            {
                result.Add(phaseEvent);
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
    /// <param name="phase">The phase to find, or null for all.</param>
    /// <returns>A list of lunar phases.</returns>
    public static List<LunarPhaseEvent> GetPhasesInMonth(int year, int month,
        ELunarPhase? phase = null)
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
            GregorianCalendarExtensions.GetMonthStart(year, month, DateTimeKind.Utc),
            GregorianCalendarExtensions.GetMonthEnd(year, month, DateTimeKind.Utc), phase);
    }

    /// <summary>
    /// Get the DateTimes of all occurrences of lunar phases (optionally restricted to a specific
    /// phase) in a given Gregorian calendar year (UTC).
    /// </summary>
    /// <param name="year">The year number.</param>
    /// <param name="phase">The phase to find, or null for all.</param>
    /// <returns>A list of lunar phases.</returns>
    public static List<LunarPhaseEvent> GetPhasesInYear(int year, ELunarPhase? phase = null)
    {
        // Check year is valid. Valid range matches DateTime.IsLeapYear().
        if (year is < 1 or > 9999)
        {
            throw new ArgumentOutOfRangeException(nameof(year),
                "Year must be in the range 1..9999");
        }

        return GetPhasesInPeriod(GregorianCalendarExtensions.GetYearStart(year, DateTimeKind.Utc),
            GregorianCalendarExtensions.GetYearEnd(year, DateTimeKind.Utc), phase);
    }

    /// <summary>
    /// Calculate the approximate average length of a lunation in ephemeris days at a point in time.
    /// The formula is taken from Wikipedia:
    /// <see href="https://en.wikipedia.org/wiki/Lunar_month#Synodic_month"/>
    /// </summary>
    /// <param name="T">The number of Julian centuries since noon, January 1, 2000.</param>
    /// <returns>The average lunation length in seconds at that point in time.</returns>
    public static double GetLunationInEphemerisDays(double T)
    {
        return Polynomials.EvaluatePolynomial([29.530_588_8531, 0.000_000_216_21, -3.64e-10], T);
    }

    /// <summary>
    /// Calculate the approximate average length of a lunation in ephemeris days for a given year.
    /// The year can have a fractional part.
    /// </summary>
    /// <param name="year">The year as a decimal.</param>
    /// <returns>The average lunation length in days at that point in time.</returns>
    public static double GetLunationInEphemerisDaysForYear(double year)
    {
        // Calculate T, the number of Julian centuries since noon, January 1, 2000 (TT).
        double jdut = TimeScales.DecimalYearToJulianDateUniversal(year);
        double deltaT = TimeScales.CalcDeltaT(year);
        double jdtt = jdut + (deltaT / TimeConstants.SECONDS_PER_DAY);
        double T = TimeScales.JulianCenturiesSinceJ2000(jdtt);
        // Evaluate the polynomial.
        return GetLunationInEphemerisDays(T);
    }

    /// <summary>
    /// Calculate the mean lunation length in solar days for a given year.
    /// </summary>
    /// <param name="year">The year as a decimal.</param>
    /// <returns>The lunation length in solar days at that point in time.</returns>
    public static double GetLunationLengthInSolarDays(double year)
    {
        return GetLunationInEphemerisDaysForYear(year)
            * TimeConstants.SECONDS_PER_DAY
            / EarthService.GetSolarDayInSeconds(year);
    }

    #endregion Static methods
}
