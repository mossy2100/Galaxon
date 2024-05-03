namespace Galaxon.Time;

/// <summary>
/// Constants for converting between units of time and time scales.
///
/// For consistency with TimeSpan, I've used `int` as the type for any integer constants, and for
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
    public const int TICKS_PER_MICROSECOND = 10;

    /// <summary>
    /// The number of ticks in a millisecond.
    /// </summary>
    public const int TICKS_PER_MILLISECOND = 10_000;

    /// <summary>
    /// The number of ticks in a second.
    /// </summary>
    public const int TICKS_PER_SECOND = 10_000_000;

    /// <summary>
    /// The number of ticks in a minute.
    /// </summary>
    public const int TICKS_PER_MINUTE = 600_000_000;

    /// <summary>
    /// The number of ticks in an hour.
    /// </summary>
    public const long TICKS_PER_HOUR = 36_000_000_000;

    /// <summary>
    /// The number of ticks in a day.
    /// </summary>
    public const long TICKS_PER_DAY = 864_000_000_000;

    /// <summary>
    /// The number of ticks in a week (7 days).
    /// </summary>
    public const long TICKS_PER_WEEK = 6_048_000_000_000;

    /// <summary>
    /// The average number of ticks in a Gregorian month.
    /// </summary>
    public const long TICKS_PER_MONTH = 26_297_460_000_000;

    /// <summary>
    /// The average number of ticks in a Gregorian year.
    /// </summary>
    public const long TICKS_PER_YEAR = 315_569_520_000_000;

    /// <summary>
    /// The average number of ticks in an olympiad
    /// </summary>
    public const long TICKS_PER_OLYMPIAD = 1_262_278_080_000_000;

    #endregion Seconds per unit of time

    #region Milliseconds per unit of time

    /// <summary>
    /// The number of milliseconds in a second.
    /// </summary>
    public const int MILLISECONDS_PER_SECOND = 1000;

    /// <summary>
    /// The number of milliseconds in a minute.
    /// </summary>
    public const int MILLISECONDS_PER_MINUTE = 60_000;

    /// <summary>
    /// The number of milliseconds in an hour.
    /// </summary>
    public const int MILLISECONDS_PER_HOUR = 3_600_000;

    /// <summary>
    /// The number of milliseconds in an ephemeris day.
    /// </summary>
    public const int MILLISECONDS_PER_DAY = 86_400_000;

    /// <summary>
    /// The number of milliseconds in a week.
    /// </summary>
    public const int MILLISECONDS_PER_WEEK = 604_800_000;

    /// <summary>
    /// The average number of milliseconds in a Gregorian month.
    /// </summary>
    public const long MILLISECONDS_PER_MONTH = 2_629_746_000;

    /// <summary>
    /// The average number of milliseconds in a Gregorian year.
    /// </summary>
    public const long MILLISECONDS_PER_YEAR = 31_556_952_000;

    #endregion Milliseconds per unit of time

    #region Seconds per unit of time

    /// <summary>
    /// The number of seconds in a minute.
    /// </summary>
    public const int SECONDS_PER_MINUTE = 60;

    /// <summary>
    /// The number of seconds in an hour.
    /// </summary>
    public const int SECONDS_PER_HOUR = 3600;

    /// <summary>
    /// The number of seconds in an ephemeris day.
    /// </summary>
    public const int SECONDS_PER_DAY = 86_400;

    /// <summary>
    /// The number of seconds in a week.
    /// </summary>
    public const int SECONDS_PER_WEEK = 604_800;

    /// <summary>
    /// The average number of seconds in a Gregorian month.
    /// </summary>
    public const int SECONDS_PER_MONTH = 2_629_746;

    /// <summary>
    /// The average number of seconds in a Gregorian year.
    /// </summary>
    public const int SECONDS_PER_YEAR = 31_556_952;

    #endregion Seconds per unit of time

    #region Minutes per unit of time

    /// <summary>
    /// The number of minutes in an hour.
    /// </summary>
    public const int MINUTES_PER_HOUR = 60;

    /// <summary>
    /// The number of minutes in a day.
    /// </summary>
    public const int MINUTES_PER_DAY = 1440;

    /// <summary>
    /// The number of minutes in a week.
    /// </summary>
    public const int MINUTES_PER_WEEK = 10080;

    #endregion Minutes per unit of time

    #region Hours per unit of time

    /// <summary>
    /// The number of hours in an ephemeris day.
    /// </summary>
    public const int HOURS_PER_DAY = 24;

    /// <summary>
    /// The number of hours in a week.
    /// </summary>
    public const int HOURS_PER_WEEK = 168;

    #endregion Hours per unit of time

    #region Days per unit of time

    /// <summary>
    /// The number of days in a Gregorian week.
    /// </summary>
    public const int DAYS_PER_WEEK = 7;

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
    public const int MONTHS_PER_YEAR = 12;

    #endregion Months per unit of time

    #region Years per unit of time

    /// <summary>
    /// Number of years in an olympiad.
    /// </summary>
    public const int YEARS_PER_OLYMPIAD = 4;

    /// <summary>
    /// The number of years in a decade.
    /// </summary>
    public const int YEARS_PER_DECADE = 10;

    /// <summary>
    /// The number of years in a century.
    /// </summary>
    public const int YEARS_PER_CENTURY = 100;

    /// <summary>
    /// The number of years in a millennium.
    /// </summary>
    public const int YEARS_PER_MILLENNIUM = 1000;

    #endregion Years per unit of time

    #region Tropical year

    /// <summary>
    /// The number of days in the mean tropical year (epoch J2000).
    /// </summary>
    public const double DAYS_PER_TROPICAL_YEAR = 365.242_189;

    /// <summary>
    /// Number of seconds in a tropical year.
    /// </summary>
    public const double SECONDS_PER_TROPICAL_YEAR = DAYS_PER_TROPICAL_YEAR * SECONDS_PER_DAY;

    /// <summary>
    /// Number of ticks in a tropical year.
    /// </summary>
    public const long TICKS_PER_TROPICAL_YEAR = (long)(DAYS_PER_TROPICAL_YEAR * TICKS_PER_DAY);

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
    public const int YEARS_PER_GREGORIAN_SOLAR_CYCLE = 400;

    /// <summary>
    /// Number of olympiads in a Gregorian solar cycle.
    /// </summary>
    public const int OLYMPIADS_PER_GREGORIAN_SOLAR_CYCLE = 100;

    /// <summary>
    /// Number of centuries in a Gregorian solar cycle.
    /// </summary>
    public const int CENTURIES_PER_GREGORIAN_SOLAR_CYCLE = 4;

    /// <summary>
    /// The number of leap years in a Gregorian solar cycle.
    /// </summary>
    public const int LEAP_YEARS_PER_GREGORIAN_SOLAR_CYCLE = 97;

    /// <summary>
    /// The number of weeks in a Gregorian solar cycle.
    /// </summary>
    public const int WEEKS_PER_GREGORIAN_SOLAR_CYCLE = 20_871;

    /// <summary>
    /// The number of days in a Gregorian solar cycle.
    /// </summary>
    public const int DAYS_PER_GREGORIAN_SOLAR_CYCLE = 146_097;

    /// <summary>
    /// The number of seconds in a Gregorian solar cycle.
    /// </summary>
    public const long SECONDS_PER_GREGORIAN_SOLAR_CYCLE = 12_622_780_800;

    /// <summary>
    /// The number of ticks in a Gregorian solar cycle.
    /// </summary>
    public const long TICKS_PER_GREGORIAN_SOLAR_CYCLE = 126_227_808_000_000_000;

    #endregion Gregorian solar cycles

    #region Time scales

    /// <summary>
    /// Number of milliseconds difference between TAI and TT.
    /// TT = TAI + 32,184 ms
    /// </summary>
    public const int TT_MINUS_TAI_MILLISECONDS = 32_184;

    #endregion Time scales

    #region Julian Dates

    /// <summary>
    /// Julian Date (UT) at the start of the Gregorian epoch, equal to 0001-01-01T00:00:00Z
    /// This is the epoch used by .NET.
    /// </summary>
    public const double START_GREGORIAN_EPOCH_JDUT = 1_721_425.5;

    /// <summary>
    /// Julian Date (TT) of the start point of the J2000 epoch.
    /// Or: the number of ephemeris days difference between the start of the Julian epoch and the
    /// start of the J2000 epoch (in Terrestrial Time).
    /// This is equivalent to noon on 2000-01-01 (TT).
    /// <see href="https://en.wikipedia.org/wiki/Epoch_(astronomy)#Julian_years_and_J2000"/>
    /// </summary>
    public const double START_J2000_EPOCH_JDTT = 2_451_545.0;

    /// <summary>
    /// The start point of the J2000 epoch in UTC.
    /// This is equal to the Julian Date 2451545.0 TT, i.e. noon on 2000-01-01 in Terrestrial Time.
    /// <see href="https://en.wikipedia.org/wiki/Epoch_(astronomy)#Julian_years_and_J2000"/>
    /// </summary>
    /// <returns>A DateTime object representing the start point of the J2000 epoch in UTC.</returns>
    public static readonly DateTime START_J2000_EPOCH_UTC =
        new (2000, 1, 1, 11, 58, 55, 816, DateTimeKind.Utc);

    /// <summary>
    /// Julian Date (TT) of the start point of the B1900 epoch.
    /// This is equivalent to 1900 January 0.8135 (TT).
    /// <see href="https://en.wikipedia.org/wiki/Epoch_(astronomy)#Besselian_years"/>
    /// </summary>
    public const double START_B1900_EPOCH_JDTT = 2_415_020.313_5;

    /// <summary>
    /// The number of ephemeris days in a Julian Calendar year.
    /// </summary>
    public const double DAYS_PER_JULIAN_YEAR = 365.25;

    /// <summary>
    /// The number of SI seconds in a Julian Calendar year.
    /// </summary>
    public const int SECONDS_PER_JULIAN_YEAR = 31_557_600;

    /// <summary>
    /// The number of ticks in a Julian Calendar year.
    /// </summary>
    public const long TICKS_PER_JULIAN_YEAR = 315_576_000_000_000;

    /// <summary>
    /// The number of ephemeris days in a Julian Calendar decade.
    /// </summary>
    public const double DAYS_PER_JULIAN_DECADE = 3_652.5;

    /// <summary>
    /// The number of SI seconds in a Julian Calendar decade.
    /// </summary>
    public const int SECONDS_PER_JULIAN_DECADE = 315_576_000;

    /// <summary>
    /// The number of ticks in a Julian Calendar year.
    /// </summary>
    public const long TICKS_PER_JULIAN_DECADE = 3_155_760_000_000_000;

    /// <summary>
    /// The number of ephemeris days in a Julian Calendar century.
    /// </summary>
    public const int DAYS_PER_JULIAN_CENTURY = 36_525;

    /// <summary>
    /// The number of SI seconds in a Julian Calendar century.
    /// </summary>
    public const long SECONDS_PER_JULIAN_CENTURY = 3_155_760_000;

    /// <summary>
    /// The number of ticks in a Julian Calendar century.
    /// </summary>
    public const long TICKS_PER_JULIAN_CENTURY = 31_557_600_000_000_000;

    /// <summary>
    /// The number of ephemeris days in a Julian Calendar millennium.
    /// </summary>
    public const int DAYS_PER_JULIAN_MILLENNIUM = 365_250;

    /// <summary>
    /// The number of SI seconds in a Julian Calendar millennium.
    /// </summary>
    public const long SECONDS_PER_JULIAN_MILLENNIUM = 31_557_600_000;

    /// <summary>
    /// The number of ticks in a Julian Calendar millennium.
    /// </summary>
    public const long TICKS_PER_JULIAN_MILLENNIUM = 315_576_000_000_000_000;

    #endregion Julian Dates

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
    /// Number of ticks in the average lunation.
    /// </summary>
    public const long TICKS_PER_LUNATION = 25_514_428_775_904;

    /// <summary>
    /// Number of ticks between two lunar phases, on average.
    /// </summary>
    public const long TICKS_PER_LUNAR_PHASE = 6_378_607_193_976;

    #endregion Moon

    #region Mars

    /// <summary>
    /// Number of days (Earth solar days) per sol (Mars solar day).
    /// </summary>
    public const double DAYS_PER_SOL = 1.027_491_25;

    #endregion Mars
}
