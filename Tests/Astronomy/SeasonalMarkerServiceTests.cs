using Galaxon.Astronomy.Algorithms.Services;
using Galaxon.Astronomy.Data;
using Galaxon.Astronomy.Data.Enums;
using Galaxon.Astronomy.Data.Models;
using Galaxon.Core.Types;
using Galaxon.Time;
using Galaxon.UnitTesting;

namespace Galaxon.Tests.Astronomy;

[TestClass]
public class SeasonalMarkerServiceTests
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
    public void TestCalcApproxSeasonalMarker()
    {
        SeasonalMarkerService seasonalMarkerService =
            ServiceManager.GetService<SeasonalMarkerService>();

        // Test Example 27.a from AA2 p180.
        DateTime dt = seasonalMarkerService.GetSeasonalMarkerApprox(1962,
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
        // Arrange
        int goalMaxDiff = 0;
        int maxDiff = 0;
        AstroDbContext astroDbContext = ServiceManager.GetService<AstroDbContext>();
        SeasonalMarkerService seasonalMarkerService =
            ServiceManager.GetService<SeasonalMarkerService>();
        List<SeasonalMarker> seasonalMarkers =
            astroDbContext.SeasonalMarkers.OrderBy(sm => sm.DateTimeUtcUsno).ToList();

        // Check each.
        foreach (SeasonalMarker seasonalMarker in seasonalMarkers)
        {
            DateTime dt = seasonalMarkerService.GetSeasonalMarkerAsDateTime(
                seasonalMarker.DateTimeUtcUsno.Year, seasonalMarker.Type);

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
