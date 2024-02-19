namespace Galaxon.Time;

/// <summary>
/// Additional members to supplement the TimeSpan class.
/// Mostly constants.
/// For consistency with TimeSpan, I've used `long` as the type for any integer constants.
/// For non-integer constants I've used `double`.
/// The following constants are already provided by the TimeSpan class and not reproduced here:
/// - TicksPerMicrosecond
/// - TicksPerMillisecond
/// - TicksPerSecond
/// - TicksPerMinute
/// - TicksPerHour
/// - TicksPerDay
/// **NOTE: Any constants or methods for converting between time units will become obsolete
/// with the completion of Galaxon.Quantities.**
/// The word Month, Year, Decade, Century, or Millennium in a constant name usually refers to the
/// average length of that time unit in the Gregorian Calendar.
/// </summary>
public static class XTimeSpan
{
    #region Conversion Methods

    /// <summary>
    /// Convert a time value from one unit to another.
    /// TODO Replace with Quantity methods.
    /// </summary>
    /// <param name="amount">The amount.</param>
    /// <param name="fromUnit">The amount argument units.</param>
    /// <param name="toUnit">The result units.</param>
    /// <returns>The amount of the new unit.</returns>
    public static double Convert(double amount, ETimeUnit fromUnit,
        ETimeUnit toUnit = ETimeUnit.Tick)
    {
        var ticks = fromUnit switch
        {
            ETimeUnit.Nanosecond => amount / NANOSECONDS_PER_TICK,
            ETimeUnit.Tick => amount,
            ETimeUnit.Microsecond => amount * TimeSpan.TicksPerMicrosecond,
            ETimeUnit.Millisecond => amount * TimeSpan.TicksPerMillisecond,
            ETimeUnit.Second => amount * TimeSpan.TicksPerSecond,
            ETimeUnit.Minute => amount * TimeSpan.TicksPerMinute,
            ETimeUnit.Hour => amount * TimeSpan.TicksPerHour,
            ETimeUnit.Day => amount * TimeSpan.TicksPerDay,
            ETimeUnit.Week => amount * TICKS_PER_WEEK,
            ETimeUnit.Month => amount * XGregorianCalendar.TICKS_PER_MONTH,
            ETimeUnit.Year => amount * XGregorianCalendar.TICKS_PER_YEAR,
            ETimeUnit.Decade => amount * XGregorianCalendar.TICKS_PER_YEAR * 10,
            ETimeUnit.Century => amount * XGregorianCalendar.TICKS_PER_YEAR * 100,
            ETimeUnit.Millennium => amount * XGregorianCalendar.TICKS_PER_YEAR * 1000,
            _ => throw new ArgumentOutOfRangeException(nameof(fromUnit), "Invalid time unit.")
        };

        return toUnit switch
        {
            ETimeUnit.Nanosecond => ticks * NANOSECONDS_PER_TICK,
            ETimeUnit.Tick => ticks,
            ETimeUnit.Microsecond => ticks / TimeSpan.TicksPerMicrosecond,
            ETimeUnit.Millisecond => ticks / TimeSpan.TicksPerMillisecond,
            ETimeUnit.Second => ticks / TimeSpan.TicksPerSecond,
            ETimeUnit.Minute => ticks / TimeSpan.TicksPerMinute,
            ETimeUnit.Hour => ticks / TimeSpan.TicksPerHour,
            ETimeUnit.Day => ticks / TimeSpan.TicksPerDay,
            ETimeUnit.Week => ticks / TICKS_PER_WEEK,
            ETimeUnit.Month => ticks / XGregorianCalendar.TICKS_PER_MONTH,
            ETimeUnit.Year => ticks / XGregorianCalendar.TICKS_PER_YEAR,
            ETimeUnit.Decade => ticks / (XGregorianCalendar.TICKS_PER_YEAR * 10),
            ETimeUnit.Century => ticks / (XGregorianCalendar.TICKS_PER_YEAR * 100),
            ETimeUnit.Millennium => ticks / (XGregorianCalendar.TICKS_PER_YEAR * 1000),
            _ => throw new ArgumentOutOfRangeException(nameof(toUnit), "Invalid time unit.")
        };
    }

    #endregion Conversion Methods

    #region Miscelleanous conversion factors

    /// <summary>
    /// The number of minutes in an hour.
    /// </summary>
    public const long MINUTES_PER_HOUR = 60L;

    /// <summary>
    /// The number of minutes in a day.
    /// </summary>
    public const long MINUTES_PER_DAY = 1440L;

    /// <summary>
    /// The number of hours in an ephemeris day.
    /// </summary>
    public const long HOURS_PER_DAY = 24L;

    /// <summary>
    /// The number of hours in a week.
    /// </summary>
    public const long HOURS_PER_WEEK = 168L;

    #endregion Miscelleanous conversion factors

    #region Nanoseconds per unit of time

    /// <summary>
    /// The number of nanoseconds in a tick.
    /// </summary>
    public const long NANOSECONDS_PER_TICK = 100L;

    #endregion Nanoseconds per unit of time

    #region Ticks per unit of time

    /// <summary>
    /// The number of ticks in a microsecond.
    /// </summary>
    public const double TICKS_PER_MICROSECOND = TimeSpan.TicksPerMicrosecond;

    /// <summary>
    /// The number of ticks in a millisecond.
    /// </summary>
    public const double TICKS_PER_MILLISECOND = TimeSpan.TicksPerMillisecond;

    /// <summary>
    /// The number of ticks in a second.
    /// </summary>
    public const double TICKS_PER_SECOND = TimeSpan.TicksPerSecond;

    /// <summary>
    /// The number of ticks in a minute.
    /// </summary>
    public const double TICKS_PER_MINUTE = TimeSpan.TicksPerMinute;

    /// <summary>
    /// The number of ticks in an hour.
    /// </summary>
    public const double TICKS_PER_HOUR = TimeSpan.TicksPerHour;

    /// <summary>
    /// The number of ticks in a day.
    /// </summary>
    public const double TICKS_PER_DAY = TimeSpan.TicksPerDay;

    /// <summary>
    /// The number of ticks in a week.
    /// </summary>
    public const long TICKS_PER_WEEK = 6_048_000_000_000L;

    #endregion Seconds per unit of time

    #region Milliseconds per unit of time

    /// <summary>
    /// The number of milliseconds in a second.
    /// </summary>
    public const double MILLISECONDS_PER_SECOND = 1e3;

    /// <summary>
    /// The number of milliseconds in a minute.
    /// </summary>
    public const long MILLISECONDS_PER_MINUTE = 60_000L;

    /// <summary>
    /// The number of milliseconds in an hour.
    /// </summary>
    public const long MILLISECONDS_PER_HOUR = 3_600_000L;

    /// <summary>
    /// The number of milliseconds in an ephemeris day.
    /// </summary>
    public const long MILLISECONDS_PER_DAY = 86_400_000L;

    /// <summary>
    /// The number of milliseconds in a week.
    /// </summary>
    public const long MILLISECONDS_PER_WEEK = 604_800_000L;

    #endregion Milliseconds per unit of time

    #region Seconds per unit of time

    /// <summary>
    /// The number of seconds in a minute.
    /// </summary>
    public const long SECONDS_PER_MINUTE = 60L;

    /// <summary>
    /// The number of seconds in an hour.
    /// </summary>
    public const long SECONDS_PER_HOUR = 3600L;

    /// <summary>
    /// The number of seconds in an ephemeris day.
    /// </summary>
    public const long SECONDS_PER_DAY = 86_400L;

    /// <summary>
    /// The number of seconds in a week.
    /// </summary>
    public const long SECONDS_PER_WEEK = 604_800L;

    #endregion Seconds per unit of time

    #region Years per unit of time

    /// <summary>
    /// Number of years in an olympiad.
    /// </summary>
    public const long YEARS_PER_OLYMPIAD = 4L;

    /// <summary>
    /// The number of years in a decade.
    /// </summary>
    public const long YEARS_PER_DECADE = 10L;

    /// <summary>
    /// The number of years in a century.
    /// </summary>
    public const long YEARS_PER_CENTURY = 100L;

    /// <summary>
    /// The number of years in a millennium.
    /// </summary>
    public const long YEARS_PER_MILLENNIUM = 1000L;

    #endregion Years per unit of time

    #region Astronomical

    /// <summary>
    /// The number of seconds in a solar day (as at 2023).
    /// It is increasing by about 2 milliseconds per century.
    /// TODO make this a function taking year as a parameter.
    /// </summary>
    public const double SECONDS_PER_SOLAR_DAY = 86_400.002;

    /// <summary>
    /// Number of days in a synodic lunar month (a.k.a. "lunation").
    /// </summary>
    public const double DAYS_PER_LUNATION = 29.530_588_861;

    /// <summary>
    /// Number of ticks in a synodic lunar month (a.k.a. "lunation").
    /// </summary>
    public const double TICKS_PER_LUNATION = DAYS_PER_LUNATION * TimeSpan.TicksPerDay;

    /// <summary>
    /// The number of days in the mean tropical year B1900 (days).
    /// This value is taken from the SOFA (Standards of Fundamental Astronomy) library, which is
    /// assumed to be authoritative.
    /// </summary>
    public const double DAYS_PER_TROPICAL_YEAR = 365.242_198_781;

    /// <summary>
    /// Number of ticks in a tropical year.
    /// </summary>
    public const double TICKS_PER_TROPICAL_YEAR = DAYS_PER_TROPICAL_YEAR * TimeSpan.TicksPerDay;

    #endregion Astronomical
}
