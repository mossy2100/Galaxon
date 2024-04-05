using Galaxon.Astronomy.Algorithms.Records;
using Galaxon.Astronomy.Algorithms.Services;
using Galaxon.Numerics.Geometry;

namespace Galaxon.Tests.Astronomy;

[TestClass]
public class NutationServiceTests
{
    [TestMethod]
    public void TestExample22a()
    {
        // Arrange.
        // TODO fix this; some confusion here between TT and UT. It might be ok for the test, but understand and comment as needed.
        DateTime dttt = new (1987, 4, 10, 0, 0, 0, DateTimeKind.Utc);
        double jdtt = JulianDateService.DateTimeToJulianDateTerrestrial(dttt);
        double expectedLongitude = Angles.DMSToRadians(0, 0, -3.788);
        double expectedObliquity = Angles.DMSToRadians(0, 0, 9.443);

        // Act.
        Nutation actual = NutationService.CalcNutation(jdtt);

        // Assert.
        Assert.AreEqual(expectedLongitude, actual.Longitude, 1e-8);
        Assert.AreEqual(expectedObliquity, actual.Obliquity, 1e-8);
    }
}
