using Galaxon.Astronomy.Algorithms.Services;
using Galaxon.Astronomy.Data;
using Galaxon.Astronomy.Data.Enums;
using Galaxon.Astronomy.Data.Models;
using Galaxon.Astronomy.Data.Repositories;
using Galaxon.Core.Types;
using Galaxon.Time;
using Galaxon.UnitTesting;

namespace Galaxon.Tests.Astronomy;

[TestClass]
public class SeasonalMarkerServiceTests
{
    private AstroDbContext? _astroDbContext;

    private AstroObjectRepository? _astroObjectRepository;

    private AstroObjectGroupRepository? _astroObjectGroupRepository;

    private PlanetService? _planetService;

    private EarthService? _earthService;

    private SunService? _sunService;

    private SeasonalMarkerService? _seasonalMarkerService;

    [TestInitialize]
    public void Init()
    {
        _astroDbContext = new AstroDbContext();
        _astroObjectGroupRepository = new AstroObjectGroupRepository(_astroDbContext);
        _astroObjectRepository =
            new AstroObjectRepository(_astroDbContext, _astroObjectGroupRepository);
        _planetService = new PlanetService(_astroDbContext);
        _earthService = new EarthService(_astroObjectRepository, _planetService);
        _sunService = new SunService(_earthService);
        _seasonalMarkerService = new SeasonalMarkerService(_sunService);
    }

    [TestMethod]
    public void TestCalcApproxSeasonalMarker()
    {
        // Test Example 27.a from AA2 p180.
        DateTime dt =
            SeasonalMarkerService.CalcSeasonalMarkerApprox(1962,
                ESeasonalMarkerType.NorthernSolstice);
        var dt2 = new DateTime(1962, 6, 21, 21, 25, 8);
        // Check they match within 1 minute.
        var delta = TimeSpan.FromMinutes(1);
        DateTimeAssert.AreEqual(dt, dt2, delta);
    }

    /// <summary>
    /// Compare my seasonal marker algorithm with imported data.
    /// </summary>
    [TestMethod]
    public void CalcSeasonalMarker_CompareWithUsno()
    {
        int goalMaxDiff = 0;
        int maxDiff = 0;

        // Arrange
        List<SeasonalMarker> seasonalMarkers =
            _astroDbContext!.SeasonalMarkers.OrderBy(sm => sm.DateTimeUtcUsno).ToList();

        // Check each.
        foreach (SeasonalMarker seasonalMarker in seasonalMarkers)
        {
            DateTime dt =
                _seasonalMarkerService!.CalcSeasonalMarker(seasonalMarker.DateTimeUtcUsno.Year,
                    seasonalMarker.Type);
            dt = DateTimeExtensions.RoundToNearestMinute(dt);

            int diff =
                (int)Round(
                    Abs(seasonalMarker.DateTimeUtcUsno.GetTotalSeconds() - dt.GetTotalSeconds())
                    / TimeConstants.SECONDS_PER_MINUTE);
            if (diff > goalMaxDiff)
            {
                Console.WriteLine(
                    $"{seasonalMarker.Type.GetDescription(),60}: {seasonalMarker.DateTimeUtcUsno.ToIsoString()} c.f. {dt.ToIsoString()} = {diff} minutes");
                if (diff > maxDiff)
                {
                    maxDiff = diff;
                }
            }
            // else
            // {
            //     Assert.IsTrue(diff <= goalMaxDiff);
            // }
        }

        Console.WriteLine($"Maximum difference = {maxDiff} minutes.");
    }
}
