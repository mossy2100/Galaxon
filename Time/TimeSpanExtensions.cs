namespace Galaxon.Time;

public static class TimeSpanExtensions
{
    /// <summary>
    /// Convert a time value from one unit to another.
    /// TODO Remove if not needed.
    /// TODO TimeSpan provides conversion functionality sufficient for most requirements.
    /// </summary>
    /// <param name="amount">The amount.</param>
    /// <param name="fromUnit">The amount argument units.</param>
    /// <param name="toUnit">The result units.</param>
    /// <returns>The amount of the new unit.</returns>
    public static double Convert(double amount, ETimeUnit fromUnit,
        ETimeUnit toUnit = ETimeUnit.Tick)
    {
        double ticks = fromUnit switch
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
            ETimeUnit.Olympiad => amount * TimeConstants.TICKS_PER_OLYMPIAD,
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
            ETimeUnit.Olympiad => ticks / TimeConstants.TICKS_PER_OLYMPIAD,
            ETimeUnit.Decade => ticks / (TimeConstants.TICKS_PER_YEAR * 10),
            ETimeUnit.Century => ticks / (TimeConstants.TICKS_PER_YEAR * 100),
            ETimeUnit.Millennium => ticks / (TimeConstants.TICKS_PER_YEAR * 1000),
            _ => throw new ArgumentOutOfRangeException(nameof(toUnit), "Invalid time unit.")
        };
    }

    /// <summary>
    /// Break a timespan into days (ephemeris), hours, minutes, and seconds.
    /// </summary>
    /// <param name="t">The TimeSpan.</param>
    /// <param name="precision">The unit to round off to.</param>
    /// <returns>An array of time parts.</returns>
    public static Dictionary<ETimeUnit, double> GetTimeParts(this TimeSpan t,
        ETimeUnit precision = ETimeUnit.Millisecond)
    {
        // Round off to the nearest unit specified as precision.
        long units = (long)Round(Convert(t.Ticks, ETimeUnit.Tick, precision));
        // Convert back to ticks for breaking down into parts.
        long ticks = (long)Round(Convert(units, precision));

        Dictionary<ETimeUnit, double> result = new ();

        // Get days.
        double nDays = (double)ticks / TimeConstants.TICKS_PER_DAY;
        int iDays = (int)Truncate(nDays);
        result[ETimeUnit.Day] = iDays;

        // Get hours.
        ticks -= iDays * TimeConstants.TICKS_PER_DAY;
        double nHours = (double)ticks / TimeConstants.TICKS_PER_HOUR;
        int iHours = (int)Truncate(nHours);
        result[ETimeUnit.Hour] = iHours;

        // Get minutes.
        ticks -= iHours * TimeConstants.TICKS_PER_HOUR;
        double nMinutes = (double)ticks / TimeConstants.TICKS_PER_MINUTE;
        int iMinutes = (int)Truncate(nMinutes);
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
    /// <param name="precision">The unit to round off to.</param>
    /// <returns>A string describing the time.</returns>
    public static string GetTimeString(Dictionary<ETimeUnit, double> parts,
        ETimeUnit precision = ETimeUnit.Millisecond)
    {
        List<string> sParts = [];
        bool foundFirstUnit = false;
        foreach (KeyValuePair<ETimeUnit, double> part in parts)
        {
            if (part.Value != 0 || (part.Key == precision && !foundFirstUnit))
            {
                double value = foundFirstUnit ? Math.Abs(part.Value) : part.Value;

                string strValue;
                if (part.Key == ETimeUnit.Second)
                {
                    int decimals = precision switch
                    {
                        ETimeUnit.Nanosecond => 9,
                        ETimeUnit.Tick => 7,
                        ETimeUnit.Microsecond => 6,
                        ETimeUnit.Millisecond => 3,
                        _ => 0
                    };
                    strValue = value.ToString("F" + decimals);
                }
                else
                {
                    strValue = value.ToString("F0");
                }

                string unit = $"{part.Key}".ToLower() + (value == 1 ? "" : "s");
                sParts.Add($"{strValue} {unit}");
                foundFirstUnit = true;
            }
        }

        return string.Join(", ", sParts);
    }

    /// <summary>
    /// Convert a time period (as a TimeSpan) into a descriptive string.
    /// </summary>
    /// <param name="t">The TimeSpan.</param>
    /// <param name="precision">The unit to round off to.</param>
    /// <returns>A string describing the time period.</returns>
    public static string GetTimeString(TimeSpan t, ETimeUnit precision = ETimeUnit.Millisecond)
    {
        return GetTimeString(t.GetTimeParts(precision), precision);
    }

    /// <summary>
    /// Get the absolute value, or magnitude, of a TimeSpan.
    /// </summary>
    /// <param name="t">A TimeSpan.</param>
    /// <returns>The absolute value.</returns>
    public static TimeSpan Abs(TimeSpan t)
    {
        return TimeSpan.FromTicks(Math.Abs(t.Ticks));
    }
}
