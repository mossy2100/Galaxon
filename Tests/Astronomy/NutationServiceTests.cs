using Galaxon.Astronomy.Algorithms.Records;
using Galaxon.Astronomy.Algorithms.Services;
using Galaxon.Astronomy.Algorithms.Utilities;
using Galaxon.Numerics.Geometry;

namespace Galaxon.Tests.Astronomy;

[TestClass]
public class NutationServiceTests
{
    /// <summary>
    /// Test Example 22.a from Astronomical Algorithms 2nd ed. (p 148).
    /// </summary>
    [TestMethod]
    public void TestExample22a()
    {
        // Arrange.
        DateTime dttt = new (1987, 4, 10, 0, 0, 0, DateTimeKind.Utc);
        double jdtt = JulianDateUtility.DateTimeToJulianDate(dttt);

        double expectedLongitude = Angles.DMSToRadians(0, 0, -3.788);
        double expectedObliquity = Angles.DMSToRadians(0, 0, 9.443);

        // Act.
        Nutation actual = NutationService.CalcNutation(jdtt);

        // Assert.
        Assert.AreEqual(2446895.5, jdtt);
        Assert.AreEqual(expectedLongitude, actual.Longitude, 1e-8);
        Assert.AreEqual(expectedObliquity, actual.Obliquity, 1e-8);
    }
}
