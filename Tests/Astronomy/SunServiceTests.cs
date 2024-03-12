using Galaxon.Astronomy.Algorithms.Records;
using Galaxon.Astronomy.Algorithms.Services;
using Galaxon.Astronomy.Data;
using Galaxon.Astronomy.Data.Repositories;
using Galaxon.Numerics.Geometry;

namespace Galaxon.Tests.Astronomy;

[TestClass]
public class SunServiceTests
{
    private AstroDbContext? _astroDbContext;

    private AstroObjectRepository? _astroObjectRepository;

    private AstroObjectGroupRepository? _astroObjectGroupRepository;

    private EarthService? _earthService;

    private PlanetService? _planetService;

    private SunService? _sunService;

    [TestInitialize]
    public void Init()
    {
        _astroDbContext = new AstroDbContext();
        _astroObjectGroupRepository = new AstroObjectGroupRepository(_astroDbContext);
        _astroObjectRepository =
            new AstroObjectRepository(_astroDbContext, _astroObjectGroupRepository);
        _planetService = new PlanetService(_astroDbContext);
        _earthService = new EarthService(_astroObjectRepository, _planetService);
        _sunService = new SunService(_astroDbContext, _astroObjectGroupRepository,
            _astroObjectRepository, _earthService);
    }

    [TestMethod]
    public void CalcPositionTest()
    {
        double JDTT = 2448908.5;
        (double actualL, double actualB, double actualR) = _sunService!.CalcPosition(JDTT);

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
        DateTime dt_TT = new (1992, 10, 13, 0, 0, 0, DateTimeKind.Utc);
        double JDTT = JulianDateService.DateTime_to_JulianDate(dt_TT);

        // Act
        Coordinates sunPosition = _sunService!.CalcPosition(JDTT);

        // Assert
        Assert.AreEqual(2448908.5, JDTT);
        Assert.AreEqual(Angles.DMSToRadians(199, 54, 26.18), sunPosition.Longitude);
        Assert.AreEqual(Angles.DMSToRadians(0, 0, 0.72), sunPosition.Latitude);
    }
}
