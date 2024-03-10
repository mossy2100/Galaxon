using Galaxon.Astronomy.Algorithms.Services;
using Galaxon.Time;

namespace Galaxon.Astronomy.Algorithms.Utilities;

public static class JulianDateUtility
{
    #region Conversion between Julian dates and other time scales

    /// <summary>
    /// Express the DateTime as a Julian Date.
    /// The time of day information in the DateTime will be expressed as the fractional part of
    /// the return value. Note, however, a Julian Date begins at 12:00 noon.
    /// </summary>
    /// <param name="dt">The DateTime instance.</param>
    /// <returns>The Julian Date</returns>
    public static double DateTime_to_JulianDate(DateTime dt)
    {
        return TimeConstants.START_GREGORIAN_EPOCH_JD_UT + dt.GetTotalDays();
    }

    /// <summary>
    /// Convert a Julian Date (Universal Time) to a DateTime object.
    /// </summary>
    /// <param name="JD">
    /// The Julian Date in UT. May include a fractional part indicating the time of day.
    /// </param>
    /// <returns>A new DateTime object.</returns>
    public static DateTime JulianDate_to_DateTime(double JD)
    {
        return DateTimeExtensions.FromTotalDays(JD - TimeConstants.START_GREGORIAN_EPOCH_JD_UT);
    }

    /// <summary>
    /// Convert a DateOnly object to a Julian Date.
    /// </summary>
    /// <param name="date">The DateOnly instance.</param>
    /// <returns>The Julian Date.</returns>
    public static double DateOnly_to_JulianDate(DateOnly date)
    {
        return DateTime_to_JulianDate(date.ToDateTime());
    }

    /// <summary>
    /// Convert a Julian Day Number to a Gregorian Date.
    /// </summary>
    /// <param name="JD">
    /// The Julian Date. If a fractional part indicating the time of day is included, this
    /// information will be discarded.
    /// </param>
    /// <returns>A new DateOnly object.</returns>
    public static DateOnly JulianDate_to_DateOnly(double JD)
    {
        return DateOnly.FromDateTime(JulianDate_to_DateTime(JD));
    }

    /// <summary>
    /// Given a Julian Date in Universal Time (JD), find the equivalent in
    /// Terrestrial Time (also known as the Julian Ephemeris Day, or JDE).
    /// ∆T = TT - UT  =>  TT = UT + ∆T
    /// </summary>
    /// <param name="JD">Julian Date in Universal Time</param>
    /// <returns>Julian Date in Terrestrial Time</returns>
    public static double JulianDate_UT_to_TT(double JD)
    {
        DateTime dt = JulianDate_to_DateTime(JD);
        double deltaT = TimeScaleUtility.CalcDeltaTNASA(dt);
        return JD + TimeSpan.FromSeconds(deltaT).TotalDays;
    }

    /// <summary>
    /// Convert a Julian Date in Terrestrial Time (TT) (also known as the
    /// Julian Ephemeris Day or JDE) to a Julian Date in Universal Time (JD).
    /// ∆T = TT - UT  =>  UT = TT - ∆T
    /// </summary>
    /// <param name="JD_TT">Julian Date in Terrestrial Time</param>
    /// <returns>Julian Date in Universal Time</returns>
    public static double JulianDate_TT_to_UT(double JD_TT)
    {
        // Calculate delta-T. For this calculation, we have to use the Julian Date as provided, which
        // is in TT, even though the JulianDate_to_DateTime() method expects a Julian Date in UT.
        // This shouldn't matter though, as the result should be virtually identical to what we
        // would get for the Julian Date in UT, given the inaccuracy in delta-T calculations.
        DateTime dt = JulianDate_to_DateTime(JD_TT);
        double deltaT = TimeScaleUtility.CalcDeltaTNASA(dt);
        return JD_TT - TimeSpan.FromSeconds(deltaT).TotalDays;
    }

    /// <summary>
    /// Convert a Julian Date in Terrestrial Time (TT)  to a Julian Date in International Atomic Time (TAI).
    /// </summary>
    /// <param name="JD_TT">Julian Date in Terrestrial Time</param>
    /// <returns>Julian Date in International Atomic Time</returns>
    public static double JulianDate_TT_to_TAI(double JD_TT)
    {
        return JD_TT
            - ((double)TimeConstants.TT_MINUS_TAI_MS / TimeConstants.SECONDS_PER_DAY / 1000);
    }

    #endregion Conversion between Julian dates and other time scales

    #region Julian periods since start J2000 epoch.

    /// <summary>
    /// Number of days since beginning of the J2000 epoch, in TT.
    /// </summary>
    /// <param name="JD_TT">The Julian Ephemeris Day.</param>
    /// <returns></returns>
    public static double JulianDaysSinceJ2000(double JD_TT)
    {
        return JD_TT - TimeConstants.START_J2000_EPOCH_JD_TT;
    }

    /// <summary>
    /// Number of Julian years since beginning of the J2000.0 epoch, in TT.
    /// </summary>
    /// <param name="JD_TT">The Julian Ephemeris Day.</param>
    /// <returns></returns>
    public static double JulianYearsSinceJ2000(double JD_TT)
    {
        return JulianDaysSinceJ2000(JD_TT) / TimeConstants.DAYS_PER_JULIAN_YEAR;
    }

    /// <summary>
    /// Number of Julian centuries since beginning of the J2000.0 epoch, in TT.
    /// </summary>
    /// <param name="JD_TT">The Julian Ephemeris Day.</param>
    /// <returns></returns>
    public static double JulianCenturiesSinceJ2000(double JD_TT)
    {
        return JulianDaysSinceJ2000(JD_TT) / TimeConstants.DAYS_PER_JULIAN_CENTURY;
    }

    /// <summary>
    /// Number of Julian millennia since beginning of the J2000.0 epoch, in TT.
    /// </summary>
    /// <param name="JD_TT">The Julian Ephemeris Day.</param>
    /// <returns></returns>
    public static double JulianMillenniaSinceJ2000(double JD_TT)
    {
        return JulianDaysSinceJ2000(JD_TT) / TimeConstants.DAYS_PER_JULIAN_MILLENNIUM;
    }

    #endregion Julian periods since start J2000 epoch.
}
