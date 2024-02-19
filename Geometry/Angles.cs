namespace Galaxon.Numerics.Geometry;

/// <summary>
/// Stuff related to angles.
/// </summary>
public static class Angles
{
    #region Constants

    public const long DEGREES_PER_CIRCLE = 360;

    public const long DEGREES_PER_SEMICIRCLE = DEGREES_PER_CIRCLE / 2;

    public const long DEGREES_PER_QUADRANT = DEGREES_PER_CIRCLE / 4;

    public const long ARCMINUTES_PER_DEGREE = 60;

    public const long ARCMINUTES_PER_CIRCLE = ARCMINUTES_PER_DEGREE * DEGREES_PER_CIRCLE;

    public const long ARCSECONDS_PER_ARCMINUTE = 60;

    public const long ARCSECONDS_PER_DEGREE = ARCSECONDS_PER_ARCMINUTE * ARCMINUTES_PER_DEGREE;

    public const long ARCSECONDS_PER_CIRCLE = ARCSECONDS_PER_DEGREE * DEGREES_PER_CIRCLE;

    public const double RADIANS_PER_CIRCLE = Tau;

    public const double RADIANS_PER_SEMICIRCLE = PI;

    public const double RADIANS_PER_QUADRANT = PI / 2;

    public const double RADIANS_PER_DEGREE = RADIANS_PER_CIRCLE / DEGREES_PER_CIRCLE;

    public const double DEGREES_PER_RADIAN = DEGREES_PER_CIRCLE / RADIANS_PER_CIRCLE;

    public const double RADIANS_PER_ARCSECOND = RADIANS_PER_CIRCLE / ARCSECONDS_PER_CIRCLE;

    public const double ARCSECONDS_PER_RADIAN = ARCSECONDS_PER_CIRCLE / RADIANS_PER_CIRCLE;

    #endregion Constants

    #region Normalize methods

    /// <summary>
    /// Add or subtract multiples of τ so the angle fits within a standard range.
    /// <ul>
    ///     <li>For signed (default), the range will be [-PI..PI)</li>
    ///     <li>For unsigned, the range will be [0..TAU)</li>
    /// </ul>
    /// </summary>
    public static double WrapRadians(double radians, bool signed = true)
    {
        radians -= Floor(radians / RADIANS_PER_CIRCLE) * RADIANS_PER_CIRCLE;
        if (signed && radians >= RADIANS_PER_SEMICIRCLE)
        {
            radians -= RADIANS_PER_CIRCLE;
        }
        return radians;
    }

    /// <summary>
    /// Add or subtract multiples of 360° so the angle fits within a standard range.
    /// <ul>
    ///     <li>For signed (default), the range will be [-180..180)</li>
    ///     <li>For unsigned, the range will be [0..360)</li>
    /// </ul>
    /// </summary>
    public static double WrapDegrees(double degrees, bool signed = true)
    {
        degrees -= Floor(degrees / DEGREES_PER_CIRCLE) * DEGREES_PER_CIRCLE;
        if (signed && degrees >= DEGREES_PER_SEMICIRCLE)
        {
            degrees -= DEGREES_PER_CIRCLE;
        }
        return degrees;
    }

    #endregion Normalize methods

    #region Conversion Methods

    /// <summary>
    /// Converts degrees to radians.
    /// </summary>
    public static double DegreesToRadians(double degrees)
    {
        return degrees * RADIANS_PER_DEGREE;
    }

    /// <summary>
    /// Converts radians to degrees.
    /// </summary>
    public static double RadiansToDegrees(double radians)
    {
        return radians * DEGREES_PER_RADIAN;
    }

    /// <summary>
    /// Converts degrees, arcminutes, and (optionally) arcseconds to decimal degrees.
    /// </summary>
    public static double DegreesMinutesSecondsToDegrees(double degrees, double arcminutes,
        double arcseconds = 0)
    {
        return degrees
            + (arcminutes / ARCMINUTES_PER_DEGREE)
            + (arcseconds / ARCSECONDS_PER_DEGREE);
    }

    /// <summary>
    /// Converts decimal degrees to degrees, arcminutes, and arcseconds.
    /// </summary>
    public static (double degrees, double arcminutes, double arcseconds)
        DegreesToDegreesMinutesSeconds(double degrees)
    {
        double wholeDegrees = Truncate(degrees);
        double fracDegrees = degrees - wholeDegrees;
        double arcminutes = fracDegrees * ARCMINUTES_PER_DEGREE;
        double wholeArcminutes = Truncate(arcminutes);
        double fracArcminutes = arcminutes - wholeArcminutes;
        double arcseconds = fracArcminutes * ARCSECONDS_PER_ARCMINUTE;
        return (wholeDegrees, wholeArcminutes, arcseconds);
    }

    #endregion Conversion Methods

    #region Trigonometric Methods

    /// <summary>
    /// Calculates the square of the sine of an angle in radians.
    /// </summary>
    public static double Sin2(double radians)
    {
        return Pow(Sin(radians), 2);
    }

    /// <summary>
    /// Calculates the square of the cosine of an angle in radians.
    /// </summary>
    public static double Cos2(double radians)
    {
        return Pow(Cos(radians), 2);
    }

    /// <summary>
    /// Calculates the square of the tangent of an angle in radians.
    /// </summary>
    public static double Tan2(double radians)
    {
        return Pow(Tan(radians), 2);
    }

    /// <summary>
    /// Calculates the sine of an angle in degrees.
    /// </summary>
    public static double SinDegrees(double degrees)
    {
        return Sin(DegreesToRadians(degrees));
    }

    /// <summary>
    /// Calculates the cosine of an angle in degrees.
    /// </summary>
    public static double CosDegrees(double degrees)
    {
        return Cos(DegreesToRadians(degrees));
    }

    /// <summary>
    /// Calculates the tangent of an angle in degrees.
    /// </summary>
    public static double TanDegrees(double degrees)
    {
        return Tan(DegreesToRadians(degrees));
    }

    #endregion Trigonometric Methods

    #region ToString Methods

    /// <summary>
    /// Converts decimal degrees, arcminutes, and arcseconds to string representation.
    /// </summary>
    public static string DegreesMinutesSecondsToString(double degrees, double arcminutes,
        double arcseconds, byte precision = 0)
    {
        // Check all parts have the same sign.
        if (!(degrees <= 0 && arcminutes <= 0 && arcseconds <= 0)
            && !(degrees >= 0 && arcminutes >= 0 && arcseconds >= 0))
        {
            throw new ArgumentOutOfRangeException(nameof(degrees),
                "All parts of the value must have the same sign (unless they are zero).");
        }

        // Handle negative values.
        if (degrees < 0)
        {
            return '-'
                + DegreesMinutesSecondsToString(-degrees, -arcminutes, -arcseconds, precision);
        }

        var sArcSeconds = arcseconds.ToString($"F{precision}");
        return $"{degrees}°{arcminutes}′{sArcSeconds}″";
    }

    /// <summary>
    /// Converts decimal degrees to degrees, arcminutes, and arcseconds notation.
    /// </summary>
    public static string DegreesToString(double n, byte precision = 0)
    {
        (double degrees, double arcminutes, double arcseconds) = DegreesToDegreesMinutesSeconds(n);
        return DegreesMinutesSecondsToString(degrees, arcminutes, arcseconds, precision);
    }

    #endregion ToString Methods
}
