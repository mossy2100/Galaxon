namespace Galaxon.Time;

public static class TimeScales
{
    #region Decimal year methods

    /// <summary>
    /// Convert a decimal year to a DateTime (UTC).
    /// </summary>
    /// <param name="decimalYear">The decimal year.</param>
    /// <returns>The equivalent DateTime.</returns>
    public static DateTime DecimalYearToDateTime(double decimalYear)
    {
        long ticks = (long)((decimalYear - 1) * TimeConstants.TICKS_PER_YEAR);
        return new DateTime(ticks, DateTimeKind.Utc);
    }

    /// <summary>
    /// Convert a DateTime (UTC) to a decimal year.
    /// </summary>
    /// <param name="dt">The DateTime.</param>
    /// <returns>The equivalent decimal year.</returns>
    public static double DateTimeToDecimalYear(DateTime dt)
    {
        return 1 + (double)dt.Ticks / TimeConstants.TICKS_PER_YEAR;
    }

    #endregion Decimal year methods

    #region Time scale conversion methods

    /// <summary>
    /// Convert a value in Terrestrial Time (TT) to International Atomic Time (TAI).
    /// </summary>
    /// <param name="TT">Terrestrial Time (TT) in ticks.</param>
    /// <returns>International Atomic Time in ticks.</returns>
    public static ulong TerrestrialTimeToInternationalAtomicTime(ulong TT)
    {
        return TT - TimeConstants.TT_MINUS_TAI_MILLISECONDS * TimeSpan.TicksPerMillisecond;
    }

    /// <summary>
    /// Convert a value in International Atomic Time (TAI) to Terrestrial Time (TT).
    /// </summary>
    /// <param name="TAI">International Atomic Time (TAI) in ticks.</param>
    /// <returns>Terrestrial Time in ticks.</returns>
    public static ulong InternationalAtomicTimeToTerrestrialTime(ulong TAI)
    {
        return TAI + TimeConstants.TT_MINUS_TAI_MILLISECONDS * TimeSpan.TicksPerMillisecond;
    }

    #endregion Time scale conversion methods
}
