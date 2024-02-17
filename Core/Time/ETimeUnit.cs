namespace Galaxon.Core.Time;

/// <summary>
/// The time units supported by the conversion functions.
/// **Note: This will be deprecated once Galaxon.Quantities is complete.**
/// </summary>
public enum ETimeUnit
{
    /// <summary>
    /// Nanosecond (ns). 1 ns = 10<sup>-9</sup> s = 0.01 ticks
    /// </summary>
    Nanosecond,

    /// <summary>
    /// Shake. 1 shake = 10<sup>-8</sup> s = 10 ns = 0.1 ticks
    /// </summary>
    Shake,

    /// <summary>
    /// The core time unit used by .NET. 1 tick = 100 ns = 0.1 µs = 10<sup>-7</sup> s
    /// </summary>
    Tick,

    /// <summary>
    /// Microsecond (µs). 1 µs = 10<sup>-6</sup> s = 1000 ns = 10 ticks
    /// </summary>
    Microsecond,

    /// <summary>
    /// Millisecond (ms). 1 ms = 10<sup>-3</sup> s = 1000 µs
    /// </summary>
    Millisecond,

    /// <summary>
    /// Second (s). 1 second = 10,000,000 ticks
    /// </summary>
    Second,

    /// <summary>
    /// Minute (min). 1 min = 60 s
    /// </summary>
    Minute,

    /// <summary>
    /// Hour (h). 1 h = 60 min = 3600 sec
    /// </summary>
    Hour,

    /// <summary>
    /// Day (d). 1 d = 24 h = 86,400 min.
    /// Depending on context, this may refers to an ephemeris day (exactly 86,400 SI seconds), or a
    /// solar day, which is slightly longer.
    /// </summary>
    Day,

    /// <summary>
    /// Week (w). 1 w = 7 d = 168 h
    /// </summary>
    Week,

    /// <summary>
    /// Month (mon). Ranges in length from 28-31 days.
    /// The average Gregorian calendar month length is 30.436875 d = 4.348125 w = 31,556,952 s
    /// </summary>
    Month,

    /// <summary>
    /// Year (y). This refers to a Gregorian calendar year, which is 365 or 366 days.
    /// The average is 365.2425 days. 1 y = 12 mon
    /// </summary>
    Year,

    /// <summary>
    /// Period equal to 4 years.
    /// </summary>
    Olympiad,

    /// <summary>
    /// Decade (dec). 1 dec = 10 y
    /// </summary>
    Decade,

    /// <summary>
    /// Century (c). 1 c = 10 dec = 100 y
    /// </summary>
    Century,

    /// <summary>
    /// Millennium (ky). 1 ky = 10 c = 100 dec = 1000 y
    /// </summary>
    Millennium
}
