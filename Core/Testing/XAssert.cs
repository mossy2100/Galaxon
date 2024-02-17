using Galaxon.Core.Numbers;
using Galaxon.Core.Numbers.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Galaxon.Core.Testing;

/// <summary>
/// Container for my own custom Assert methods.
/// </summary>
public static class XAssert
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
        static double DmsToDeg((double d, double m, double s) angle) =>
            angle.d + 60 * angle.m + 3600 * angle.s;

        var aDeg = DmsToDeg(a);
        var bDeg = DmsToDeg(b);
        var deltaDeg = DmsToDeg(delta);

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
    /// Helper function to test if a double equals a decimal.
    /// </summary>
    /// <param name="expected">Expected double value</param>
    /// <param name="actual">Actual decimal value</param>
    public static void AreEqual(double expected, decimal actual)
    {
        // Doubles and decimals are only equal to a limited number of significant figures, so scale
        // larger values to the range [0..10) before comparing.
        Console.WriteLine($"Comparing {expected} with {actual}");
        var a = actual;
        var e = (decimal)expected;
        if (actual != 0)
        {
            var m = (int)Math.Floor(Math.Log10(Math.Abs(expected)));
            if (m > 0)
            {
                var scaleFactor = XDecimal.Exp10(m);
                a /= scaleFactor;
                e /= scaleFactor;
            }
        }

        // Compare decimals.
        Assert.AreEqual(e, a, 1e-13m);
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
