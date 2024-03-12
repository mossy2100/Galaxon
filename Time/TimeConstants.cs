namespace Galaxon.Time;

/// <summary>
/// Constants for converting between units of time and time scales.
///
/// For consistency with TimeSpan, I've used `long` as the type for any integer constants, and for
/// non-integer constants I've used `double`.
///
/// Constants from TimeSpace are reproduced here, for convenience. I find this saves me always
/// having to check which constants are in TimeSpan and which ones are in this class.
///
/// The words WEEK, MONTH, YEAR, DECADE, CENTURY, and MILLENNIUM in a constant name refers to the
/// average length of that time unit in the Gregorian Calendar, unless otherwise specified.
/// </summary>
public static class TimeConstants
{
    #region Ticks per unit of time

    /// <summary>
    /// The number of ticks in a microsecond.
    /// </summary>
    public const long TICKS_PER_MICROSECOND = 10L;

    /// <summary>
    /// The number of ticks in a millisecond.
    /// </summary>
    public const long TICKS_PER_MILLISECOND = 10_000L;

    /// <summary>
    /// The number of ticks in a second.
    /// </summary>
    public const long TICKS_PER_SECOND = 10_000_000L;

    /// <summary>
    /// The number of ticks in a minute.
    /// </summary>
    public const long TICKS_PER_MINUTE = 600_000_000L;

    /// <summary>
    /// The number of ticks in an hour.
    /// </summary>
    public const long TICKS_PER_HOUR = 36_000_000_000L;

    /// <summary>
    /// The number of ticks in a day.
    /// </summary>
    public const long TICKS_PER_DAY = 864_000_000_000L;

    /// <summary>
    /// The number of ticks in a week (7 days).
    /// </summary>
    public const long TICKS_PER_WEEK = 6_048_000_000_000L;

    /// <summary>
    /// The average number of ticks in a Gregorian month.
    /// </summary>
    public const long TICKS_PER_MONTH = 26_297_460_000_000L;

    /// <summary>
    /// The average number of ticks in a Gregorian year.
    /// </summary>
    public const long TICKS_PER_YEAR = 315_569_520_000_000L;

    #endregion Seconds per unit of time

    #region Milliseconds per unit of time

    /// <summary>
    /// The number of milliseconds in a second.
    /// </summary>
    public const long MILLISECONDS_PER_SECOND = 1000L;

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

    /// <summary>
    /// The average number of milliseconds in a Gregorian month.
    /// </summary>
    public const long MILLISECONDS_PER_MONTH = 2_629_746_000L;

    /// <summary>
    /// The average number of milliseconds in a Gregorian year.
    /// </summary>
    public const long MILLISECONDS_PER_YEAR = 31_556_952_000L;

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

    /// <summary>
    /// The average number of seconds in a Gregorian month.
    /// </summary>
    public const long SECONDS_PER_MONTH = 2_629_746L;

    /// <summary>
    /// The average number of seconds in a Gregorian year.
    /// </summary>
    public const long SECONDS_PER_YEAR = 31_556_952L;

    #endregion Seconds per unit of time

    #region Minutes per unit of time

    /// <summary>
    /// The number of minutes in an hour.
    /// </summary>
    public const long MINUTES_PER_HOUR = 60L;

    /// <summary>
    /// The number of minutes in a day.
    /// </summary>
    public const long MINUTES_PER_DAY = 1440L;

    #endregion Minutes per unit of time

    #region Hours per unit of time

    /// <summary>
    /// The number of hours in an ephemeris day.
    /// </summary>
    public const long HOURS_PER_DAY = 24L;

    /// <summary>
    /// The number of hours in a week.
    /// </summary>
    public const long HOURS_PER_WEEK = 168L;

    #endregion Hours per unit of time

    #region Days per unit of time

    /// <summary>
    /// The number of days in a Gregorian week.
    /// </summary>
    public const long DAYS_PER_WEEK = 7L;

    /// <summary>
    /// The average number of days in a Gregorian month.
    /// </summary>
    public const double DAYS_PER_MONTH = 30.436_875;

    /// <summary>
    /// The average number of days in a Gregorian year.
    /// </summary>
    public const double DAYS_PER_YEAR = 365.2425;

    #endregion Days per unit of time

    #region Weeks per unit of time

    /// <summary>
    /// The average number of weeks in a Gregorian month.
    /// </summary>
    public const double WEEKS_PER_MONTH = 4.348_125;

    /// <summary>
    /// The average number of weeks in a Gregorian year.
    /// </summary>
    public const double WEEKS_PER_YEAR = 52.1775;

    #endregion Weeks per unit of time

    #region Months per unit of time

    /// <summary>
    /// The number of months in a Gregorian year.
    /// </summary>
    public const long MONTHS_PER_YEAR = 12L;

    #endregion Months per unit of time

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

    #region Tropical year

    /// <summary>
    /// The number of days in the mean tropical year (epoch B1900).
    /// This value is taken from the SOFA (Standards of Fundamental Astronomy) library, which is
    /// assumed to be authoritative.
    /// </summary>
    public const double DAYS_PER_TROPICAL_YEAR = 365.242_198_781;

    /// <summary>
    /// Number of seconds in a tropical year.
    /// </summary>
    public const double SECONDS_PER_TROPICAL_YEAR = 31_556_925.974_6784;

    /// <summary>
    /// Number of ticks in a tropical year.
    /// </summary>
    public const long TICKS_PER_TROPICAL_YEAR = 315_569_259_746_784L;

    #endregion Tropical year

    #region Gregorian solar cycles

    /// <summary>
    /// The Gregorian Calendar repeats on a 400-year cycle called the "Gregorian solar cycle".
    /// (Not to be confused with the 11-year solar cycle.)
    /// There are 97 leap years in that period, giving an average calendar year length of
    /// 365 + (97/400) = 365.2425 days/year.
    /// 1 Gregorian solar cycle = 400 y = 4800 mon = 20,871 w = 146,097 d
    /// 5 Gregorian solar cycles = 2000 y = 2 ky
    /// Gregorian solar cycles are not ordinarily numbered, nor given a specific start date.
    /// However, within the proleptic Gregorian epoch (the one used by .NET), which began on Monday,
    /// 1 Jan, 1 CE, we are currently in the 6th solar cycle. It began on Monday, 1 Jan, 2001, which
    /// was also the first day of the 3rd millennium AD. It will end on Sunday, 31 Dec, 2400.
    /// See:
    /// - <see href="https://en.wikipedia.org/wiki/Solar_cycle_(calendar)"/>
    /// - <see href="https://en.wikipedia.org/wiki/Proleptic_Gregorian_calendar"/>
    /// </summary>
    public const long YEARS_PER_GREGORIAN_SOLAR_CYCLE = 400L;

    /// <summary>
    /// Number of olympiads in a Gregorian solar cycle.
    /// </summary>
    public const long OLYMPIADS_PER_GREGORIAN_SOLAR_CYCLE = 100L;

    /// <summary>
    /// Number of centuries in a Gregorian solar cycle.
    /// </summary>
    public const long CENTURIES_PER_GREGORIAN_SOLAR_CYCLE = 4L;

    /// <summary>
    /// The number of leap years in a Gregorian solar cycle.
    /// </summary>
    public const long LEAP_YEARS_PER_GREGORIAN_SOLAR_CYCLE = 97L;

    /// <summary>
    /// The number of weeks in a Gregorian solar cycle.
    /// </summary>
    public const long WEEKS_PER_GREGORIAN_SOLAR_CYCLE = 20_871L;

    /// <summary>
    /// The number of days in a Gregorian solar cycle.
    /// </summary>
    public const long DAYS_PER_GREGORIAN_SOLAR_CYCLE = 146_097L;

    /// <summary>
    /// The number of seconds in a Gregorian solar cycle.
    /// </summary>
    public const long SECONDS_PER_GREGORIAN_SOLAR_CYCLE = 12_622_780_800L;

    /// <summary>
    /// The number of ticks in a Gregorian solar cycle.
    /// </summary>
    public const long TICKS_PER_GREGORIAN_SOLAR_CYCLE = 126_227_808_000_000_000L;

    #endregion Gregorian solar cycles

    #region Time scales

    /// <summary>
    /// Julian Date (UT) at the start of the Gregorian epoch, the epoch used by .NET, which began at
    /// 0001-01-01 00:00:00 UTC.
    /// </summary>
    public const double START_GREGORIAN_EPOCH_JD_UT = 1721425.5;

    /// <summary>
    /// Julian Date (TT) of the start point of the J2000 epoch.
    /// Or: the number of ephemeris days difference between the start of the Julian epoch and the
    /// start of the J2000 epoch (in Terrestrial Time).
    /// This is equivalent to noon on 2000-01-01 in Terrestrial Time (not UTC).
    /// <see href="https://en.wikipedia.org/wiki/Epoch_(astronomy)#Julian_years_and_J2000"/>
    /// </summary>
    public const double START_J2000_EPOCH_JD_TT = 2451545.0;

    /// <summary>
    /// The start point of the J2000 epoch in UTC.
    /// This is equal to the Julian Date 2451545.0 TT, i.e. noon on 2000-01-01 in Terrestrial Time.
    /// <see href="https://en.wikipedia.org/wiki/Epoch_(astronomy)#Julian_years_and_J2000"/>
    /// </summary>
    /// <returns>A DateTime object representing the start point of the J2000 epoch in UTC.</returns>
    public static readonly DateTime START_J2000_EPOCH_UTC =
        new (2000, 1, 1, 11, 58, 55, 816, DateTimeKind.Utc);

    /// <summary>
    /// The number of ephemeris days in a Julian Calendar year.
    /// </summary>
    public const double DAYS_PER_JULIAN_YEAR = 365.25;

    /// <summary>
    /// The number of SI seconds in a Julian Calendar year.
    /// </summary>
    public const long SECONDS_PER_JULIAN_YEAR = 31_557_600L;

    /// <summary>
    /// The number of ticks in a Julian Calendar year.
    /// </summary>
    public const long TICKS_PER_JULIAN_YEAR = 315_576_000_000_000L;

    /// <summary>
    /// The number of ephemeris days in a Julian Calendar decade.
    /// </summary>
    public const double DAYS_PER_JULIAN_DECADE = 3652.5;

    /// <summary>
    /// The number of SI seconds in a Julian Calendar decade.
    /// </summary>
    public const long SECONDS_PER_JULIAN_DECADE = 315_576_000L;

    /// <summary>
    /// The number of ticks in a Julian Calendar year.
    /// </summary>
    public const long TICKS_PER_JULIAN_DECADE = 3_155_760_000_000_000L;

    /// <summary>
    /// The number of ephemeris days in a Julian Calendar century.
    /// </summary>
    public const long DAYS_PER_JULIAN_CENTURY = 36_525L;

    /// <summary>
    /// The number of SI seconds in a Julian Calendar century.
    /// </summary>
    public const long SECONDS_PER_JULIAN_CENTURY = 3_155_760_000L;

    /// <summary>
    /// The number of ticks in a Julian Calendar century.
    /// </summary>
    public const long TICKS_PER_JULIAN_CENTURY = 31_557_600_000_000_000L;

    /// <summary>
    /// The number of ephemeris days in a Julian Calendar millennium.
    /// </summary>
    public const long DAYS_PER_JULIAN_MILLENNIUM = 365_250L;

    /// <summary>
    /// The number of SI seconds in a Julian Calendar millennium.
    /// </summary>
    public const long SECONDS_PER_JULIAN_MILLENNIUM = 31_557_600_000L;

    /// <summary>
    /// The number of ticks in a Julian Calendar millennium.
    /// </summary>
    public const long TICKS_PER_JULIAN_MILLENNIUM = 315_576_000_000_000_000L;

    /// <summary>
    /// Number of milliseconds difference between TAI and TT.
    /// TT = TAI + 32,184 ms
    /// </summary>
    public const long TT_MINUS_TAI_MILLISECONDS = 32_184L;

    #endregion Time scales

    #region Moon

    /// <summary>
    /// Start point of Meeus' lunation number (LN) 0.
    /// </summary>
    public static readonly DateTime
        LUNATION_0_START = new (2000, 1, 6, 18, 14, 0, DateTimeKind.Utc);

    /// <summary>
    /// Number of days in a synodic lunar month (a.k.a. "lunation") at J2000.0 epoch.
    /// <see href="https://en.wikipedia.org/wiki/Lunar_month#Cycle_lengths"/>
    /// </summary>
    public const double DAYS_PER_LUNATION = 29.530_588_861;

    /// <summary>
    /// Number of seconds in a lunation.
    /// </summary>
    public const double SECONDS_PER_LUNATION = DAYS_PER_LUNATION * SECONDS_PER_DAY;

    /// <summary>
    /// Number of ticks in a lunation.
    /// </summary>
    public const long TICKS_PER_LUNATION = 25_514_428_775_904L;

    #endregion Moon

    #region Mars

    /// <summary>
    /// Number of days (Earth solar days) per sol (Mars solar day).
    /// </summary>
    public const double DAYS_PER_SOL = 1.02749125;

    #endregion Mars
}
