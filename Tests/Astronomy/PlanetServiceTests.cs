using Galaxon.Astronomy.Algorithms.Services;
using Galaxon.Astronomy.Data;
using Galaxon.Astronomy.Data.Models;
using Galaxon.Astronomy.Data.Repositories;
using Galaxon.Numerics.Geometry;
using Galaxon.Quantities;

namespace Galaxon.Tests.Astronomy;

[TestClass]
public class PlanetServiceTests
{
    private AstroDbContext? _astroDbContext;

    private AstroObjectRepository? _astroObjectRepository;

    private AstroObjectGroupRepository? _astroObjectGroupRepository;

    private PlanetService? _planetService;

    [TestInitialize]
    public void Init()
    {
        _astroDbContext = new AstroDbContext();
        _astroObjectGroupRepository = new AstroObjectGroupRepository(_astroDbContext);
        _astroObjectRepository =
            new AstroObjectRepository(_astroDbContext, _astroObjectGroupRepository);
        _planetService = new PlanetService(_astroDbContext);
    }

    /// <summary>
    /// Test the example given in Wikipedia.
    /// <see href="https://en.wikipedia.org/wiki/Sidereal_time#ERA"/>
    /// </summary>
    [TestMethod]
    public void TestERA()
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
        AstroObject? venus = _astroObjectRepository!.Load("Venus", "Planet");
        if (venus == null)
        {
            Assert.Fail("Could not find Venus in the database.");
            return;
        }

        double expectedL = Angles.WrapRadians(-68.659_258_2);
        double expectedB = Angles.WrapRadians(-0.045_739_9);
        var expectedR = 0.724_603;

        // Act.
        (double actualL, double actualB, double actualR) =
            _planetService!.CalcPlanetPosition(venus, 2_448_976.5);

        // Assert.
        // I assume larger delta values are needed here because Meeus uses a
        // subset of terms from VSOP87 whereas this library uses all of them,
        // thereby producing a more accurate result.
        Assert.AreEqual(expectedL, actualL, 1e-5);
        Assert.AreEqual(expectedB, actualB, 1e-5);
        Assert.AreEqual(expectedR, actualR / LengthConstants.METRES_PER_ASTRONOMICAL_UNIT, 1e-5);
    }

    /// <summary>
    /// Test Example 32.b from AA2 p219.
    /// </summary>
    [TestMethod]
    public void TestCalcPositionSaturn()
    {
        // Arrange.
        AstroObject? saturn = _astroObjectRepository?.Load("Saturn", "Planet");
        if (saturn == null)
        {
            Assert.Fail("Could not find Saturn in the database.");
            return;
        }
        // TODO fix this; some confusion here between TT and UT. It might be ok for the test, but understand and comment as needed.
        var dttt = new DateTime(1999, 7, 26, 0, 0, 0, DateTimeKind.Utc);
        double jdtt = JulianDateService.DateTimeToJulianDateUT(dttt);

        // Act.
        (double actualL, double _, double _) = _planetService!.CalcPlanetPosition(saturn, jdtt);

        // Assert.
        double expectedL = Angles.DegreesToRadians(39.972_3901);
        Assert.AreEqual(expectedL, actualL, 1e-6);
    }
}
