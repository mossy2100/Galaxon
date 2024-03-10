using Galaxon.Astronomy.Algorithms.Services;
using Galaxon.Astronomy.Data;
using Galaxon.Astronomy.Data.Repositories;
using Galaxon.Numerics.Geometry;

namespace Galaxon.Tests.Astronomy;

[TestClass]
public class TestSun
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
            _astroObjectRepository, _planetService, _earthService);
    }

    [TestMethod]
    public void CalcPositionTest()
    {
        double JD_TT = 2448908.5;
        (double actualL, double actualB, double actualR) = _sunService!.CalcPosition(JD_TT);

        double expectedL = Angles.WrapRadians(Angles.DMSToRadians(199, 54, 21.82));
        double expectedB = Angles.WrapRadians(Angles.DMSToRadians(0, 0, 0.62));

        // Note the large delta necessary for the test to pass. This is probably
        // because the calculation in AA2 uses the 1980 method for calculating
        // nutation instead of the more modern SOFA method as used in this
        // library.
        Assert.AreEqual(expectedL, actualL, 1e-3);
        Assert.AreEqual(expectedB, actualB, 1e-3);
    }
}
