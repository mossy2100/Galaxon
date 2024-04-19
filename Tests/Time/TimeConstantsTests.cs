using Galaxon.Time;
using Galaxon.UnitTesting;

namespace Galaxon.Tests.Time;

[TestClass]
public class TimeConstantsTests
{
    /// <summary>
    /// Check epoch constants match delta-T calculation.
    /// </summary>
    [TestMethod]
    public void J2000EpochDeltaTTest()
    {
        DateTime expected = TimeConstants.START_J2000_EPOCH_UTC;
        DateTime actual = TimeScales.JulianDateTerrestrialToDateTimeUniversal(TimeConstants.START_J2000_EPOCH_JDTT);
        DateTimeAssert.AreEqual(expected, actual, TimeSpan.FromSeconds(0.5));
    }
}
