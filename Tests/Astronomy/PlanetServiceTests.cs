using Galaxon.Astronomy.Algorithms.Services;
using Galaxon.Astronomy.Algorithms.Utilities;
using Galaxon.Astronomy.Data.Models;
using Galaxon.Astronomy.Data.Repositories;
using Galaxon.Numerics.Geometry;

namespace Galaxon.Tests.Astronomy;

[TestClass]
public class PlanetServiceTests
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

    /// <summary>
    /// Test the example given in Wikipedia.
    /// <see href="https://en.wikipedia.org/wiki/Sidereal_time#ERA"/>
    /// </summary>
    [TestMethod]
    public void CalcEarthRotationAngle_ReturnsCorrectResult()
    {
        DateTime dt = new (2017, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        double expected = Angles.DMSToRadians(100, 37, 12.4365);
        double actual = EarthService.CalcEarthRotationAngle(dt);
        double delta = Angles.DMSToRadians(0, 0, 1e-3);
        Assert.AreEqual(expected, actual, delta);
    }

    /// <summary>
    /// Test Example 32.a from AA2 p219.
    /// </summary>
    [TestMethod]
    public void TestCalcPositionVenus()
    {
        // Arrange.
        AstroObjectRepository astroObjectRepository =
            ServiceManager.GetService<AstroObjectRepository>();
        PlanetService planetService = ServiceManager.GetService<PlanetService>();
        AstroObjectRecord venus = astroObjectRepository.LoadByName("Venus", "Planet");

        double expectedL = Angles.WrapRadians(-68.659_258_2);
        double expectedB = Angles.WrapRadians(-0.045_739_9);
        double expectedR = 0.724_603;

        // Act.
        (double actualL, double actualB, double actualR) =
            planetService.CalcPlanetPosition(venus, 2_448_976.5);

        // Assert.
        // I assume larger delta values are needed here because Meeus uses a
        // subset of terms from VSOP87 whereas this library uses all of them,
        // thereby producing a more accurate result.
        Assert.AreEqual(expectedL, actualL, 1e-5);
        Assert.AreEqual(expectedB, actualB, 1e-5);
        Assert.AreEqual(expectedR, actualR, 1e-5);
    }

    /// <summary>
    /// Test Example 32.b from AA2 p219.
    /// </summary>
    [TestMethod]
    public void CalcPlanetPosition_Example32b()
    {
        // Arrange.
        AstroObjectRepository astroObjectRepository =
            ServiceManager.GetService<AstroObjectRepository>();
        PlanetService planetService = ServiceManager.GetService<PlanetService>();
        AstroObjectRecord saturn = astroObjectRepository.LoadByName("Saturn", "Planet");
        DateTime dttt = new (1999, 7, 26, 0, 0, 0, DateTimeKind.Utc);
        double jdtt = JulianDateUtility.DateTimeToJulianDate(dttt);

        // Act.
        (double actualL, double _, double _) = planetService.CalcPlanetPosition(saturn, jdtt);

        // Assert.
        double expectedL = Angles.DegreesToRadians(39.972_3901);
        Assert.AreEqual(expectedL, actualL, 1e-6);
    }
}
