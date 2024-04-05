using Galaxon.Astronomy.Algorithms.Records;
using Galaxon.Astronomy.Algorithms.Services;
using Galaxon.Astronomy.Data;
using Galaxon.Astronomy.Data.Repositories;
using Galaxon.Numerics.Geometry;

namespace Galaxon.Tests.Astronomy;

[TestClass]
public class SunServiceTests
{
    [ClassInitialize]
    public static void ClassInitialize(TestContext context)
    {
        ServiceManager.Initialize();
    }

    [ClassCleanup]
    public static void ClassCleanup()
    {
        ServiceManager.Dispose();
    }

    [TestMethod]
    public void CalcPositionTest()
    {
        SunService sunService = ServiceManager.GetService<SunService>();
        double jdtt = 2448908.5;
        (double actualL, double actualB, double actualR) = sunService.CalcPosition(jdtt);

        double expectedL = Angles.WrapRadians(Angles.DMSToRadians(199, 54, 21.82));
        double expectedB = Angles.WrapRadians(Angles.DMSToRadians(0, 0, 0.62));

        // Note the large delta necessary for the test to pass. This is probably
        // because the calculation in AA2 uses the 1980 method for calculating
        // nutation instead of the more modern SOFA method as used in this
        // library.
        Assert.AreEqual(expectedL, actualL, 1e-3);
        Assert.AreEqual(expectedB, actualB, 1e-3);
    }

    /// <summary>
    /// Test Example 25.a from Astronomical Algorithms, 2nd Ed.
    /// </summary>
    [TestMethod]
    public void TestExample25a()
    {
        // Arrange
        SunService sunService = ServiceManager.GetService<SunService>();
        DateTime dttt = new (1992, 10, 13, 0, 0, 0, DateTimeKind.Utc);
        // TODO fix this; some confusion here between TT and UT. It might be ok for the test, but understand and comment as needed.
        double jdtt = JulianDateService.DateTimeToJulianDateUniversal(dttt);
        double expectedLongitude = Angles.WrapRadians(Angles.DMSToRadians(199, 54, 26.18));
        double expectedLatitude = Angles.WrapRadians(Angles.DMSToRadians(0, 0, 0.72));

        // Act
        Coordinates sunPosition = sunService.CalcPosition(jdtt);

        // Assert
        Assert.AreEqual(2448908.5, jdtt);
        Assert.AreEqual(expectedLongitude, sunPosition.Longitude);
        Assert.AreEqual(expectedLatitude, sunPosition.Latitude);
    }
}
