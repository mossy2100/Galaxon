using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Galaxon.Core.Testing;

/// <summary>
/// Container for my own custom Assert methods.
/// </summary>
public static class AssertExtensions
{
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
    public static void AreEqualWithinPercent(double expected, double actual, double percent)
    {
        var delta = Math.Abs(expected * percent / 100);
        Assert.AreEqual(expected, actual, delta);
    }
}
