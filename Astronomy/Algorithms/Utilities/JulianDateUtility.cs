using Galaxon.Time;
using Galaxon.Time.Extensions;

namespace Galaxon.Astronomy.Algorithms.Utilities;

public static class JulianDateUtility
{
    #region Convert between decimal years and Julian dates

    /// <summary>
    /// Convert a decimal year to a Julian Date (UT).
    /// I've re-implemented this to not use DateTime so it isn't limited to the range of years
    /// 1-9999 like the DateTime type.
    /// </summary>
    /// <param name="year">The year number as a decimal.</param>
    /// <returns>The equivalent Julian Date (UT).</returns>
    public static double DecimalYearToJulianDate(double year)
    {
        return TimeConstants.START_GREGORIAN_EPOCH_JDUT + (year - 1) * TimeConstants.DAYS_PER_YEAR;
    }

    /// <summary>
    /// Convert a Julian Date (UT) to a decimal year.
    /// I've re-implemented this to not use DateTime so it isn't limited to the range of years
    /// 1-9999 like the DateTime type.
    /// </summary>
    /// <param name="jdut">A Julian Date (UT).</param>
    /// <returns>The equivalent decimal year.</returns>
    public static double JulianDateToDecimalYear(double jdut)
    {
        return (jdut - TimeConstants.START_GREGORIAN_EPOCH_JDUT) / TimeConstants.DAYS_PER_YEAR + 1;
    }

    #endregion Convert between decimal years and Julian dates

    #region Conversion between Julian dates and other time scales

    /// <summary>
    /// Convert a DateTime to a Julian Date. Either both are UT or both are TT.
    /// The time of day information in the DateTime will be expressed as the fractional part of
    /// the return value. Note, however, a Julian Date begins at 12:00 noon.
    /// </summary>
    /// <param name="dt">The DateTime instance.</param>
    /// <returns>The equivalent Julian Date.</returns>
    public static double DateTimeToJulianDate(DateTime dt)
    {
        return TimeConstants.START_GREGORIAN_EPOCH_JDUT + dt.GetTotalDays();
    }

    /// <summary>
    /// Convert a Julian Date to a DateTime. Either both are UT or both are TT.
    /// </summary>
    /// <param name="jdut">
    /// The Julian Date. May include a fractional part indicating the time of day.
    /// </param>
    /// <returns>The equivalent DateTime.</returns>
    public static DateTime JulianDateToDateTime(double jdut)
    {
        return DateTimeExtensions.FromTotalDays(jdut - TimeConstants.START_GREGORIAN_EPOCH_JDUT);
    }

    /// <summary>
    /// Convert a DateTime (UT) to a Julian Date (TT).
    /// The time of day information in the DateTime will be expressed as the fractional part of
    /// the return value. Note, however, a Julian Date begins at 12:00 noon.
    /// </summary>
    /// <param name="dt">The DateTime in Universal Time.</param>
    /// <returns>The Julian Date in Terrestrial Time.</returns>
    public static double DateTimeUniversalToJulianDateTerrestrial(DateTime dt)
    {
        return JulianDateUniversalToTerrestrial(DateTimeToJulianDate(dt));
    }

    /// <summary>
    /// Convert a Julian Date (TT) to a DateTime (UT).
    /// </summary>
    /// <param name="jdtt">
    /// The Julian Date (TT). May include a fractional part indicating the time of day.
    /// </param>
    /// <returns>The DateTime in Universal Time.</returns>
    public static DateTime JulianDateTerrestrialToDateTimeUniversal(double jdtt)
    {
        return JulianDateToDateTime(JulianDateTerrestrialToUniversal(jdtt));
    }

    /// <summary>
    /// Given a Julian Date in Universal Time (UT), find the equivalent in TT (Terrestrial Time).
    /// This is also known as the Julian Ephemeris Day, or JDE.
    /// ∆T = TT - UT  =>  TT = UT + ∆T
    /// </summary>
    /// <param name="jdut">Julian Date in Universal Time.</param>
    /// <returns>Julian Date in Terrestrial Time.</returns>
    public static double JulianDateUniversalToTerrestrial(double jdut)
    {
        double year = JulianDateToDecimalYear(jdut);
        double deltaT = DeltaTUtility.CalcDeltaTNasa(year);
        return jdut + (deltaT / TimeConstants.SECONDS_PER_DAY);
    }

    /// <summary>
    /// Convert a Julian Date in Terrestrial Time (TT) (also known as the
    /// Julian Ephemeris Day or JDE) to a Julian Date in Universal Time (jdut).
    /// ∆T = TT - UT  =>  UT = TT - ∆T
    /// </summary>
    /// <param name="jdtt">Julian Date in Terrestrial Time</param>
    /// <returns>Julian Date in Universal Time</returns>
    public static double JulianDateTerrestrialToUniversal(double jdtt)
    {
        // We have to convert from a Julian Date to a DateTime in TT, even though the CalcDeltaTNasa()
        // method expects a DateTime in UT. This shouldn't matter, though, as the result should be
        // virtually identical given the lack of precision in delta-T calculations.
        DateTime dt = JulianDateToDateTime(jdtt);
        double deltaT = DeltaTUtility.CalcDeltaTNasa(dt);
        return jdtt - deltaT / TimeConstants.SECONDS_PER_DAY;
    }

    /// <summary>
    /// Convert a Julian Date in Terrestrial Time (TT) to a Julian Date in International Atomic Time
    /// (TAI).
    ///
    /// TT = TAI + 32.184 seconds
    ///
    /// Since the unit of TT used here (and throughout the library) is days, in the form of Julian
    /// Date (TT), we must convert the 32.184 seconds to days to find Julian Date (TAI).
    ///
    /// From the Wikipedia article (see link below): "The offset 32.184 seconds was the 1976
    /// estimate of the difference between Ephemeris Time (ET) and TAI, to provide continuity with
    /// the current values and practice in the use of Ephemeris Time."
    ///
    /// See: <see href="https://en.wikipedia.org/wiki/Terrestrial_Time#TAI"/>
    /// </summary>
    /// <param name="jdtt">Julian Date in Terrestrial Time (TT).</param>
    /// <returns>Julian Date in International Atomic Time (TAI).</returns>
    public static double JulianDateTerrestrialToInternationalAtomic(double jdtt)
    {
        return jdtt
            - (double)TimeConstants.TT_MINUS_TAI_MILLISECONDS / TimeConstants.MILLISECONDS_PER_DAY;
    }

    #endregion Conversion between Julian dates and other time scales

    #region Conversion between DateOnly and Julian Date or Day Number

    /// <summary>
    /// Convert a DateOnly object to a Julian Date equal to start point of that day.
    /// If you want to convert a DateOnly to a Julian Date equal to noon on that day, use
    /// <see cref="DateOnlyToJulianDayNumber"/>.
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
    /// <param name="jd">
    /// The Julian Date. If a fractional part indicating the time of day is included, this
    /// information will be discarded.
    /// </param>
    /// <returns>A new DateOnly object.</returns>
    public static DateOnly JulianDateToDateOnly(double jd)
    {
        return DateOnly.FromDateTime(JulianDateToDateTime(jd));
    }

    /// <summary>
    /// Convert a DateOnly to a Julian Day Number.
    /// </summary>
    /// <param name="date">The DateOnly instance.</param>
    /// <returns>The Julian Day Number.</returns>
    public static int DateOnlyToJulianDayNumber(DateOnly date)
    {
        return (int)DateTimeToJulianDate(date.ToDateTime(new TimeOnly(12, 0)));
    }

    /// <summary>
    /// Convert a Julian Day Number to a Gregorian date.
    /// </summary>
    /// <param name="jdn">The Julian Day Number.</param>
    /// <returns>A new DateOnly object.</returns>
    public static DateOnly JulianDayNumberToDateOnly(int jdn)
    {
        return JulianDateToDateOnly(jdn);
    }

    #endregion Conversion between DateOnly and Julian Day Number

    #region Julian periods since start J2000 epoch.

    /// <summary>
    /// Number of days since beginning of the J2000 epoch (TT).
    /// </summary>
    /// <param name="jdtt">The Julian Ephemeris Day.</param>
    /// <returns></returns>
    public static double JulianDaysSinceJ2000(double jdtt)
    {
        return jdtt - TimeConstants.START_J2000_EPOCH_JDTT;
    }

    /// <summary>
    /// Number of Julian years since beginning of the J2000.0 epoch, in TT.
    /// </summary>
    /// <param name="jdtt">The Julian Ephemeris Day.</param>
    /// <returns></returns>
    public static double JulianYearsSinceJ2000(double jdtt)
    {
        return JulianDaysSinceJ2000(jdtt) / TimeConstants.DAYS_PER_JULIAN_YEAR;
    }

    /// <summary>
    /// Number of Julian centuries since beginning of the J2000.0 epoch (TT).
    /// </summary>
    /// <param name="jdtt">The Julian Ephemeris Day.</param>
    /// <returns></returns>
    public static double JulianCenturiesSinceJ2000(double jdtt)
    {
        return JulianDaysSinceJ2000(jdtt) / TimeConstants.DAYS_PER_JULIAN_CENTURY;
    }

    /// <summary>
    /// Number of Julian millennia since beginning of the J2000.0 epoch (TT).
    /// </summary>
    /// <param name="jdtt">The Julian Ephemeris Day.</param>
    /// <returns></returns>
    public static double JulianMillenniaSinceJ2000(double jdtt)
    {
        return JulianDaysSinceJ2000(jdtt) / TimeConstants.DAYS_PER_JULIAN_MILLENNIUM;
    }

    #endregion Julian periods since start J2000 epoch.
}
