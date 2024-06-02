namespace Galaxon.Astronomy.Algorithms.Utilities;

/// <summary>
/// This service contains useful methods and constants relating to Earth.
/// </summary>
public static class EarthUtility
{
    /// <summary>
    /// Calculate the Earth Rotation Angle from the Julian Date in UT1.
    /// See: <see href="https://en.wikipedia.org/wiki/Sidereal_time#ERA"/>
    /// </summary>
    /// <param name="jdut">The Julian Date in UT1.</param>
    /// <returns>The Earth Rotation </returns>
    public static double CalcEarthRotationAngle(double jdut)
    {
        double t = JulianDateUtility.JulianDaysSinceJ2000(jdut);
        double radians = Tau * (0.779_057_273_264 + 1.002_737_811_911_354_48 * t);
        return WrapRadians(radians);
    }

    /// <summary>
    /// Calculate the Earth Rotation Angle from a UTC DateTime.
    /// </summary>
    /// <param name="dt">The instant.</param>
    /// <returns>The ERA at the given instant.</returns>
    public static double CalcEarthRotationAngle(DateTime dt)
    {
        double jdut = JulianDateUtility.FromDateTime(dt);
        return CalcEarthRotationAngle(jdut);
    }
}
