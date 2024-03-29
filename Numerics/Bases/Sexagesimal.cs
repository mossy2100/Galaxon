namespace Galaxon.Numerics.Bases;

/// <summary>
/// Utility class for converting floating point values to sexagesimal values.
/// </summary>
public static class Sexagesimal
{
    /// <summary>
    /// The radix or base of sexagesimal numbers.
    /// </summary>
    public const int BASE = 60;

    /// <summary>
    /// Convert a double value into units, minutes, and seconds.
    /// This can be used to convert a time to hours, minutes, and seconds, or an angle to degrees,
    /// arcminutes, and arcseconds.
    /// The units part will be a signed integer, not limited to the range 0-59.
    /// The minutes part will be an integer in the range 0-59.
    /// The seconds part will be a decimal in the range 0-59.999...
    ///
    /// NB: The minutes and seconds values will have the same sign as the whole number, or be
    /// zero.
    /// In this way, FromUnitsMinutesSeconds() serves as the reverse of ToUnitsMinutesSeconds()
    /// without additional complexity.
    /// </summary>
    /// <param name="n">The value to convert.</param>
    /// <returns>
    /// A tuple representing the units (hours or degrees), minutes (or arcminutes), and seconds (or
    /// arcseconds).
    /// </returns>
    public static (int, int, double) ToUnitsMinutesSeconds(double n)
    {
        // Calculate the units part.
        // This will throw an exception if the truncated value of n is outside the valid range for
        // long. We could make units a BigInteger but that seems unnecessary at this stage.
        var units = (int)Math.Truncate(n);

        // Calculate the minutes part.
        double decimalMinutes = (n - units) * BASE;
        var minutes = (int)Math.Truncate(decimalMinutes);

        // Calculate the seconds part.
        double seconds = (decimalMinutes - minutes) * BASE;

        return (units, minutes, seconds);
    }

    /// <summary>
    /// Convert a value provided as units (e.g. hours or degrees), minutes and seconds, to a double
    /// value.
    /// </summary>
    /// <param name="units"></param>
    /// <param name="minutes"></param>
    /// <param name="seconds"></param>
    /// <returns></returns>
    public static double FromUnitsMinutesSeconds(double units, double minutes, double seconds)
    {
        return units + minutes / BASE + seconds / (BASE * BASE);
    }
}
