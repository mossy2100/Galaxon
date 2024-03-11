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
        DateTime dt_TT = new (1987, 4, 10, 0, 0, 0, DateTimeKind.Utc);
        double JDTT = JulianDateService.DateTime_to_JulianDate(dt_TT);
        double expectedLongitude = Angles.DMSToRadians(0, 0, -3.788);
        double expectedObliquity = Angles.DMSToRadians(0, 0, 9.443);

        // Act.
        Nutation actual = NutationService.CalcNutation(JDTT);

        // Assert.
        Assert.AreEqual(expectedLongitude, actual.Longitude, 1e-8);
        Assert.AreEqual(expectedObliquity, actual.Obliquity, 1e-8);
    }
}
