using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Galaxon.Core.Testing;

/// <summary>
/// Container for my own custom Assert methods.
/// </summary>
public static class AssertExtensions
{
    /// <summary>
    /// Helper function to compare DMS (degrees, minutes, seconds) tuples for equality.
    /// </summary>
    /// <param name="a">Angle 1</param>
    /// <param name="b">Angle 2</param>
    /// <param name="delta">Maximum acceptable difference between the two angles.</param>
    public static void AreEqual((double d, double m, double s) a, (double d, double m, double s) b,
        (double d, double m, double s) delta)
    {
        static double dmsToDeg((double d, double m, double s) angle) =>
            angle.d + 60 * angle.m + 3600 * angle.s;

        var aDeg = dmsToDeg(a);
        var bDeg = dmsToDeg(b);
        var deltaDeg = dmsToDeg(delta);

        Assert.AreEqual(aDeg, bDeg, deltaDeg);
    }

    /// <summary>
    /// Helper function to compare DateTimes for equality.
    /// </summary>
    /// <param name="dt1">The first DateTime</param>
    /// <param name="dt2">The second DateTime</param>
    /// <param name="delta">Maximum acceptable difference.</param>
    public static void AreEqual(DateTime dt1, DateTime dt2, TimeSpan? delta = null)
    {
        double deltaTicks = delta?.Ticks ?? 0;
        Assert.AreEqual(dt1.Ticks, dt2.Ticks, deltaTicks);
    }

    /// <summary>
    /// Check if a value is in a given range.
    /// </summary>
    /// <param name="value">The value to test.</param>
    /// <param name="lower">The lower value.</param>
    /// <param name="upper">The upper value.</param>
    /// <param name="includeLower">Include lower value in the range.</param>
    /// <param name="includeUpper">Include upper value in the range.</param>
    public static void IsInRange(double value, double lower, double upper,
        bool includeLower = true, bool includeUpper = false)
    {
        Assert.IsTrue(includeLower ? value >= lower : value > lower);
        Assert.IsTrue(includeUpper ? value <= upper : value < upper);
    }

    /// <summary>
    /// Compare two double values for equality, with the delta expressed as percentage of the
    /// expected value rather than an absolute value.
    /// </summary>
    public static void AreEqualPercent(double expected, double actual, double percent)
    {
        var delta = Math.Abs(expected * percent / 100);
        Assert.AreEqual(expected, actual, delta);
    }
}
