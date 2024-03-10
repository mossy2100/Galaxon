using Galaxon.Numerics.Bases;

namespace Galaxon.Numerics.Geometry;

/// <summary>
/// Stuff related to angles.
/// </summary>
public static class Angles
{
    #region Constants

    public const long DEGREES_PER_CIRCLE = 360L;

    public const long DEGREES_PER_SEMICIRCLE = 180L;

    public const long DEGREES_PER_QUADRANT = 90L;

    public const long ARCMINUTES_PER_DEGREE = 60L;

    public const long ARCSECONDS_PER_ARCMINUTE = 60L;

    public const long ARCSECONDS_PER_DEGREE = 3600L;

    public const long ARCSECONDS_PER_CIRCLE = 1_296_000L;

    public const double RADIANS_PER_CIRCLE = Tau;

    public const double RADIANS_PER_SEMICIRCLE = PI;

    public const double RADIANS_PER_QUADRANT = PI / 2;

    public const double RADIANS_PER_DEGREE = RADIANS_PER_CIRCLE / DEGREES_PER_CIRCLE;

    public const double DEGREES_PER_RADIAN = DEGREES_PER_CIRCLE / RADIANS_PER_CIRCLE;

    public const double ARCSECONDS_PER_RADIAN = ARCSECONDS_PER_CIRCLE / RADIANS_PER_CIRCLE;

    #endregion Constants

    #region Wrapping methods

    /// <summary>
    /// Add or subtract multiples of τ so the angle fits within a standard range.
    /// <ul>
    ///     <li>For signed (default), the range will be [-π..π)</li>
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

    #endregion Wrapping methods

    #region Conversion methods

    /// <summary>
    /// Converts degrees to radians.
    /// </summary>
    public static double DegreesToRadians(double degrees)
    {
        return degrees * RADIANS_PER_DEGREE;
    }

    /// <summary>
    /// Converts degrees to radians, then wraps to range.
    /// </summary>
    public static double DegreesToRadiansWithWrap(double degrees, bool signed = true)
    {
        return WrapRadians(degrees * RADIANS_PER_DEGREE, signed);
    }

    /// <summary>
    /// Converts radians to degrees.
    /// </summary>
    public static double RadiansToDegrees(double radians)
    {
        return radians * DEGREES_PER_RADIAN;
    }

    /// <summary>
    /// Converts radians to degrees, then wraps to range.
    /// </summary>
    public static double RadiansToDegreesWithWrap(double radians, bool signed = true)
    {
        return WrapRadians(radians * DEGREES_PER_RADIAN, signed);
    }

    /// <summary>
    /// Converts degrees, arcminutes, and (optionally) arcseconds to decimal degrees.
    /// </summary>
    public static double DMSToDegrees(double degrees, double arcminutes,
        double arcseconds = 0)
    {
        return Sexagesimal.FromUnitsMinutesSeconds(degrees, arcminutes, arcseconds);
    }

    /// <summary>
    /// Converts decimal degrees to degrees, arcminutes, and arcseconds.
    /// </summary>
    public static (int degrees, int arcminutes, double arcseconds) DegreesToDMS(double degrees)
    {
        return Sexagesimal.ToUnitsMinutesSeconds(degrees);
    }

    public static double DMSToRadians(double degrees, double arcminutes,
        double arcseconds = 0)
    {
        return DegreesToRadians(DMSToDegrees(degrees, arcminutes, arcseconds));
    }

    public static (double degrees, double arcminutes, double arcseconds) RadiansToDMS(
        double radians)
    {
        return DegreesToDMS(RadiansToDegrees(radians));
    }

    #endregion Conversion methods

    #region Trigonometric methods

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

    #endregion Trigonometric methods

    #region ToString methods

    /// <summary>
    /// Converts decimal degrees, arcminutes, and arcseconds to string representation.
    /// </summary>
    public static string DMSToString(double degrees, double arcminutes, double arcseconds,
        byte? precision = null)
    {
        // Check all parts have the same sign (or are zero).
        int sign =
            (degrees >= 0 && arcminutes >= 0 && arcseconds >= 0) ? 1 :
            (degrees <= 0 && arcminutes <= 0 && arcseconds <= 0) ? -1 : 0;

        // If signs are inconsistent, throw.
        if (sign == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(degrees),
                "All parts of the value must have the same sign (unless they are zero).");
        }

        // Handle negative values.
        if (sign == -1)
        {
            return '-' + DMSToString(-degrees, -arcminutes, -arcseconds, precision);
        }

        string sArcSeconds =
            precision == null ? $"{arcseconds}" : arcseconds.ToString($"F{precision}");
        return $"{degrees}°{arcminutes}′{sArcSeconds}″";
    }

    /// <summary>
    /// Converts decimal degrees to degrees, arcminutes, and arcseconds notation.
    /// </summary>
    public static string DegreesToString(double n, byte? precision = null)
    {
        (double degrees, double arcminutes, double arcseconds) = DegreesToDMS(n);
        return DMSToString(degrees, arcminutes, arcseconds, precision);
    }

    #endregion ToString methods
}
