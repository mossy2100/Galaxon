namespace Galaxon.Numerics.Geometry;

/// <summary>
/// Stuff related to angles.
/// </summary>
public static class Angles
{
    #region Normalize methods

    /// <summary>
    /// Add or subtract multiples of τ so the angle fits within a standard range.
    /// <ul>
    ///     <li>For signed (default), the range will be [-PI..PI)</li>
    ///     <li>For unsigned, the range will be [0..TAU)</li>
    /// </ul>
    /// </summary>
    public static double NormalizeRadians(double radians, bool signed = true)
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
    public static double NormalizeDegrees(double degrees, bool signed = true)
    {
        degrees -= Floor(degrees / DEGREES_PER_CIRCLE) * DEGREES_PER_CIRCLE;
        if (signed && degrees >= DEGREES_PER_SEMICIRCLE)
        {
            degrees -= DEGREES_PER_CIRCLE;
        }
        return degrees;
    }

    #endregion Normalize methods

    #region Conversion methods

    /// <summary>
    /// Converts degrees to radians.
    /// </summary>
    /// <param name="degrees">An angle size in degrees.</param>
    /// <returns>The angle size in radians.</returns>
    public static double DegToRad(double degrees)
    {
        return degrees * RADIANS_PER_DEGREE;
    }

    /// <summary>
    /// Convert radians to degrees.
    /// </summary>
    /// <param name="radians">An angle size in radians.</param>
    /// <returns>The angle size in degrees.</returns>
    public static double RadToDeg(double radians)
    {
        return radians * DEGREES_PER_RADIAN;
    }

    /// <summary>
    /// Creates a new angle from degrees, arcminutes, and (optionally) arcseconds.
    /// In normal usage all values should have the same sign or be zero.
    /// Consider an angle expressed as -2° 20' 14". This really means -(2° 20' 14").
    /// So, although there's only one minus sign, this value is actually equal to -2° -20' -14".
    /// Therefore, if degrees are supplied as negative, but the arcminutes or arcseconds are
    /// positive, this is probably an error.
    /// However, no exception will be thrown, so beware of that.
    /// </summary>
    /// <param name="degrees">The number of degrees.</param>
    /// <param name="arcminutes">The number of arcminutes.</param>
    /// <param name="arcseconds">The number of arcseconds.</param>
    /// <returns>The angle in degrees.</returns>
    public static double DmsToDeg(double degrees, double arcminutes, double arcseconds = 0)
    {
        return degrees
            + arcminutes / ARCMINUTES_PER_DEGREE
            + arcseconds / ARCSECONDS_PER_DEGREE;
    }

    /// <summary>
    /// TODO> This is the same as Sexagesimal.ToUnitsMinutesSeconds(), except that it uses doubles
    /// TODO> instead of decimals. Pick one and eliminate this method.
    ///
    /// Convert an angle from degrees to degrees, arcminutes, and arcseconds.
    /// The degrees and arcminutes values will be whole numbers, but the arcseconds value could have
    /// a fractional part.
    /// The arcminutes and arcseconds values will have the same sign as the degrees value, or be
    /// zero.
    /// </summary>
    /// <param name="degrees">The angle in degrees.</param>
    /// <returns>
    /// A tuple containing 3 double values representing degrees, arcminutes, and arcseconds.
    /// </returns>
    public static (double degrees, double arcminutes, double arcseconds) DegToDms(double degrees)
    {
        double wholeDegrees = Truncate(degrees);
        double fracDegrees = degrees - wholeDegrees;
        double arcminutes = fracDegrees * ARCMINUTES_PER_DEGREE;
        double wholeArcminutes = Truncate(arcminutes);
        double fracArcminutes = arcminutes - wholeArcminutes;
        double arcseconds = fracArcminutes * ARCSECONDS_PER_ARCMINUTE;
        return (degrees: wholeDegrees, arcminutes: wholeArcminutes, arcseconds);
    }

    #endregion Conversion methods

    #region Trigonometric methods

    /// <summary>
    /// The square of the sine of an angle in radians.
    /// </summary>
    /// <param name="radians">The size of an angle in radians.</param>
    /// <returns></returns>
    public static double SinSqr(double radians)
    {
        return Pow(Sin(radians), 2);
    }

    /// <summary>
    /// The square of the sine of an angle in radians.
    /// </summary>
    /// <param name="radians">The size of an angle in radians.</param>
    /// <returns></returns>
    public static double CosSqr(double radians)
    {
        return Pow(Cos(radians), 2);
    }

    /// <summary>
    /// The square of the tangent of an angle in radians.
    /// </summary>
    /// <param name="radians">The size of an angle in radians.</param>
    /// <returns></returns>
    public static double TanSqr(double radians)
    {
        return Pow(Tan(radians), 2);
    }

    /// <summary>
    /// The sine of an angle in degrees.
    /// </summary>
    public static double SinDeg(double degrees)
    {
        return Sin(DegToRad(degrees));
    }

    /// <summary>
    /// The cosine of an angle in degrees.
    /// </summary>
    public static double CosDeg(double degrees)
    {
        return Cos(DegToRad(degrees));
    }

    /// <summary>
    /// The tangent of an angle in degrees.
    /// </summary>
    public static double TanDeg(double degrees)
    {
        return Tan(DegToRad(degrees));
    }

    #endregion Trigonometric methods

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
}
