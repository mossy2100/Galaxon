using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Galaxon.UnitTesting;

/// <summary>
/// Methods relating to DateTime values for use in unit tests.
/// </summary>
public static class DateTimeAssert
{
    /// <summary>
    /// Helper function to compare DateTimes for equality.
    /// </summary>
    /// <param name="dt1">The first DateTime</param>
    /// <param name="dt2">The second DateTime</param>
    /// <param name="delta">Maximum acceptable difference.</param>
    public static void AreEqual(DateTime dt1, DateTime dt2, TimeSpan? delta = null)
    {
        Assert.AreEqual(dt1.Ticks, dt2.Ticks, delta?.Ticks ?? 0);
    }
}
