using System.Globalization;
using Galaxon.Time;

namespace Galaxon.Astronomy.Algorithms.Services;

public class JulianDateService
{
    #region Conversion between Julian dates and other time scales

    /// <summary>
    /// Convert a DateTime to a Julian Date.
    /// The time of day information in the DateTime will be expressed as the fractional part of
    /// the return value. Note, however, a Julian Date begins at 12:00 noon.
    /// This method can be used when both input and output are UT or both are TT.
    /// </summary>
    /// <param name="dt">The DateTime instance.</param>
    /// <returns>The Julian Date</returns>
    public static double DateTimeToJulianDate(DateTime dt)
    {
        return TimeConstants.START_GREGORIAN_EPOCH_JD_UT + dt.GetTotalDays();
    }

    /// <summary>
    /// Convert a Julian Date to a DateTime object.
    /// This method can be used when both input and output are UT or both are TT.
    /// </summary>
    /// <param name="JD">
    /// The Julian Date. May include a fractional part indicating the time of day.
    /// </param>
    /// <returns>A new DateTime object.</returns>
    public static DateTime JulianDateToDateTime(double JD)
    {
        return DateTimeExtensions.FromTotalDays(JD - TimeConstants.START_GREGORIAN_EPOCH_JD_UT);
    }

    /// <summary>
    /// Convert a DateOnly object to a Julian Date.
    /// </summary>
    /// <param name="date">The DateOnly instance.</param>
    /// <returns>The Julian Date.</returns>
    public static double DateOnlyToJulianDate(DateOnly date)
    {
        return DateTimeToJulianDate(date.ToDateTime());
    }

    /// <summary>
    /// Convert a Julian Date to a Gregorian Calendar date.
    /// </summary>
    /// <param name="JD">
    /// The Julian Date. If a fractional part indicating the time of day is included, this
    /// information will be discarded.
    /// </param>
    /// <returns>A new DateOnly object.</returns>
    public static DateOnly JulianDateToDateOnly(double JD)
    {
        return DateOnly.FromDateTime(JulianDateToDateTime(JD));
    }

    /// <summary>
    /// Given a Julian Date in Universal Time (JD), find the equivalent in
    /// Terrestrial Time (also known as the Julian Ephemeris Day, or JDE).
    /// ∆T = TT - UT  =>  TT = UT + ∆T
    /// </summary>
    /// <param name="JD">Julian Date in Universal Time</param>
    /// <returns>Julian Date in Terrestrial Time</returns>
    public static double JulianDateUniversalTimeToTerrestrialTime(double JD)
    {
        DateTime dt = JulianDateToDateTime(JD);
        double deltaT = TimeScaleService.CalcDeltaT(dt);
        return JD + (deltaT / TimeConstants.SECONDS_PER_DAY);
    }

    /// <summary>
    /// Convert a Julian Date in Terrestrial Time (TT) (also known as the
    /// Julian Ephemeris Day or JDE) to a Julian Date in Universal Time (JD).
    /// ∆T = TT - UT  =>  UT = TT - ∆T
    /// </summary>
    /// <param name="JDTT">Julian Date in Terrestrial Time</param>
    /// <returns>Julian Date in Universal Time</returns>
    public static double JulianDateTerrestrialTimeToUniversalTime(double JDTT)
    {
        // Calculate delta-T. For this calculation, we have to use TT, even though the
        // CalcDeltaT() method expects a DateTime in UT. This shouldn't matter, though,
        // as the result should be virtually identical to what we would get for the value for
        // delta-T calculated from a DateTime in UT, given the lack of precision in delta-T
        // calculations.
        DateTime dtTT = JulianDateToDateTime(JDTT);
        double deltaT = TimeScaleService.CalcDeltaT(dtTT);
        return JDTT - (deltaT / TimeConstants.SECONDS_PER_DAY);
    }

    /// <summary>
    /// Convert a Julian Date in Terrestrial Time (TT)  to a Julian Date in International Atomic Time (TAI).
    /// </summary>
    /// <param name="JDTT">Julian Date in Terrestrial Time</param>
    /// <returns>Julian Date in International Atomic Time</returns>
    public static double JulianDateTerrestrialTimeToInternationalAtomicTime(double JDTT)
    {
        return JDTT
            - ((double)TimeConstants.TT_MINUS_TAI_MILLISECONDS / TimeConstants.MILLISECONDS_PER_DAY);
    }

    /// <summary>
    /// Convert a Julian Calendar date to a Gregorian Calendar date.
    /// </summary>
    /// <param name="year">The year (-44+)</param>
    /// <param name="month">The month (1-12)</param>
    /// <param name="day">The day (1-31)</param>
    /// <returns>The equivalent Gregorian date.</returns>
    public static DateOnly JulianCalendarDateToGregorianDate(int year, int month, int day)
    {
        JulianCalendar jc = new ();
        DateTime dt = jc.ToDateTime(year, month, day, 0, 0, 0, 0);
        return DateOnly.FromDateTime(dt);
    }

    #endregion Conversion between Julian dates and other time scales

    #region Julian periods since start J2000 epoch.

    /// <summary>
    /// Number of days since beginning of the J2000 epoch, in TT.
    /// </summary>
    /// <param name="JDTT">The Julian Ephemeris Day.</param>
    /// <returns></returns>
    public static double JulianDaysSinceJ2000(double JDTT)
    {
        return JDTT - TimeConstants.START_J2000_EPOCH_JD_TT;
    }

    /// <summary>
    /// Number of Julian years since beginning of the J2000.0 epoch, in TT.
    /// </summary>
    /// <param name="JDTT">The Julian Ephemeris Day.</param>
    /// <returns></returns>
    public static double JulianYearsSinceJ2000(double JDTT)
    {
        return JulianDaysSinceJ2000(JDTT) / TimeConstants.DAYS_PER_JULIAN_YEAR;
    }

    /// <summary>
    /// Number of Julian centuries since beginning of the J2000.0 epoch, in TT.
    /// </summary>
    /// <param name="JDTT">The Julian Ephemeris Day.</param>
    /// <returns></returns>
    public static double JulianCenturiesSinceJ2000(double JDTT)
    {
        return JulianDaysSinceJ2000(JDTT) / TimeConstants.DAYS_PER_JULIAN_CENTURY;
    }

    /// <summary>
    /// Number of Julian millennia since beginning of the J2000.0 epoch, in TT.
    /// </summary>
    /// <param name="JDTT">The Julian Ephemeris Day.</param>
    /// <returns></returns>
    public static double JulianMillenniaSinceJ2000(double JDTT)
    {
        return JulianDaysSinceJ2000(JDTT) / TimeConstants.DAYS_PER_JULIAN_MILLENNIUM;
    }

    #endregion Julian periods since start J2000 epoch.
}
