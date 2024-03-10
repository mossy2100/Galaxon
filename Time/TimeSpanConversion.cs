namespace Galaxon.Time;

public static class TimeSpanConversion
{
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
            ETimeUnit.Nanosecond => amount / TimeSpan.NanosecondsPerTick,
            ETimeUnit.Tick => amount,
            ETimeUnit.Microsecond => amount * TimeSpan.TicksPerMicrosecond,
            ETimeUnit.Millisecond => amount * TimeSpan.TicksPerMillisecond,
            ETimeUnit.Second => amount * TimeSpan.TicksPerSecond,
            ETimeUnit.Minute => amount * TimeSpan.TicksPerMinute,
            ETimeUnit.Hour => amount * TimeSpan.TicksPerHour,
            ETimeUnit.Day => amount * TimeSpan.TicksPerDay,
            ETimeUnit.Week => amount * TimeConstants.TICKS_PER_WEEK,
            ETimeUnit.Month => amount * TimeConstants.TICKS_PER_MONTH,
            ETimeUnit.Year => amount * TimeConstants.TICKS_PER_YEAR,
            ETimeUnit.Decade => amount * TimeConstants.TICKS_PER_YEAR * 10,
            ETimeUnit.Century => amount * TimeConstants.TICKS_PER_YEAR * 100,
            ETimeUnit.Millennium => amount * TimeConstants.TICKS_PER_YEAR * 1000,
            _ => throw new ArgumentOutOfRangeException(nameof(fromUnit), "Invalid time unit.")
        };

        return toUnit switch
        {
            ETimeUnit.Nanosecond => ticks * TimeSpan.NanosecondsPerTick,
            ETimeUnit.Tick => ticks,
            ETimeUnit.Microsecond => ticks / TimeSpan.TicksPerMicrosecond,
            ETimeUnit.Millisecond => ticks / TimeSpan.TicksPerMillisecond,
            ETimeUnit.Second => ticks / TimeSpan.TicksPerSecond,
            ETimeUnit.Minute => ticks / TimeSpan.TicksPerMinute,
            ETimeUnit.Hour => ticks / TimeSpan.TicksPerHour,
            ETimeUnit.Day => ticks / TimeSpan.TicksPerDay,
            ETimeUnit.Week => ticks / TimeConstants.TICKS_PER_WEEK,
            ETimeUnit.Month => ticks / TimeConstants.TICKS_PER_MONTH,
            ETimeUnit.Year => ticks / TimeConstants.TICKS_PER_YEAR,
            ETimeUnit.Decade => ticks / (TimeConstants.TICKS_PER_YEAR * 10),
            ETimeUnit.Century => ticks / (TimeConstants.TICKS_PER_YEAR * 100),
            ETimeUnit.Millennium => ticks / (TimeConstants.TICKS_PER_YEAR * 1000),
            _ => throw new ArgumentOutOfRangeException(nameof(toUnit), "Invalid time unit.")
        };
    }

    /// <summary>
    /// Break a timespan into days, hours, minutes, and seconds.
    /// </summary>
    /// <param name="t">The TimeSpan.</param>
    /// <returns>An array of time parts.</returns>
    public static Dictionary<ETimeUnit, double> GetTimeParts(TimeSpan t)
    {
        Dictionary<ETimeUnit, double> result = new ();
        long ticks = t.Ticks;

        // Get days.
        double nDays = (double)ticks / TimeConstants.TICKS_PER_DAY;
        int iDays = (int)Math.Truncate(nDays);
        result[ETimeUnit.Day] = iDays;

        // Get hours.
        ticks -= iDays * TimeConstants.TICKS_PER_DAY;
        double nHours = (double)ticks / TimeConstants.TICKS_PER_HOUR;
        int iHours = (int)Math.Truncate(nHours);
        result[ETimeUnit.Hour] = iHours;

        // Get minutes.
        ticks -= iHours * TimeConstants.TICKS_PER_HOUR;
        double nMinutes = (double)ticks / TimeConstants.TICKS_PER_MINUTE;
        int iMinutes = (int)Math.Truncate(nMinutes);
        result[ETimeUnit.Minute] = iMinutes;

        // Get seconds.
        ticks -= iMinutes * TimeConstants.TICKS_PER_MINUTE;
        double nSeconds = (double)ticks / TimeConstants.TICKS_PER_SECOND;
        result[ETimeUnit.Second] = nSeconds;

        return result;
    }

    /// <summary>
    /// Convert an array of time parts into a descriptive string.
    /// </summary>
    /// <param name="parts">The array of time parts.</param>
    /// <returns>A string describing the time.</returns>
    public static string GetTimeString(Dictionary<ETimeUnit, double> parts)
    {
        List<string> sParts = new ();
        foreach (KeyValuePair<ETimeUnit, double> part in parts)
        {
            if (part.Value != 0)
            {
                string unit = $"{part.Key}".ToLower() + 's';
                sParts.Add($"{part.Value} {unit}");
            }
        }
        return string.Join(", ", sParts);
    }

    /// <summary>
    /// Convert a time period (as a TimeSpan) into a descriptive string.
    /// </summary>
    /// <param name="t">The TimeSpan.</param>
    /// <returns>A string describing the time period.</returns>
    public static string GetTimeString(TimeSpan t)
    {
        return GetTimeString(GetTimeParts(t));
    }
}
