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

    /// <summary>
    /// Test Example 27.a from AA2 p180.
    /// </summary>
    [TestMethod]
    public void GetSeasonalMarkerApprox_Example27a()
    {
        // Arrange
        SeasonalMarkerService seasonalMarkerService =
            ServiceManager.GetService<SeasonalMarkerService>();
        var expected = new DateTime(1962, 6, 21, 21, 25, 8);
        var delta = TimeSpan.FromMinutes(1);

        // Act
        double jdtt = seasonalMarkerService.GetSeasonalMarkerApprox(1962, ESeasonalMarkerType.NorthernSolstice);
        DateTime dt = TimeScaleService.JulianDateTerrestrialToDateTimeUniversal(jdtt);

        // Assert
        DateTimeAssert.AreEqual(dt, expected, delta);
    }

    /// <summary>
    /// Compare my seasonal marker algorithm with imported data.
    /// </summary>
    [TestMethod]
    public void CalcSeasonalMarker_CompareWithUsno()
    {
        // Arrange
        TimeSpan maxDiff = TimeSpan.FromMinutes(2);
        AstroDbContext astroDbContext = ServiceManager.GetService<AstroDbContext>();
        SeasonalMarkerService seasonalMarkerService = ServiceManager.GetService<SeasonalMarkerService>();
        List<SeasonalMarker> seasonalMarkers = astroDbContext.SeasonalMarkers.OrderBy(sm => sm.DateTimeUtcUsno).ToList();

        // Check each.
        foreach (SeasonalMarker seasonalMarker in seasonalMarkers)
        {
            // Arrange.
            DateTime expected = seasonalMarker.DateTimeUtcUsno;

            // Act.
            DateTime actual = seasonalMarkerService.GetSeasonalMarkerAsDateTime(seasonalMarker.DateTimeUtcUsno.Year, seasonalMarker.Type);

            // Assert.
            DateTimeAssert.AreEqual(expected, actual, maxDiff);
            // if (diff > goalMaxDiff)
            // {
            //     Console.WriteLine(
            //         $"{seasonalMarker.Type.GetDescription(),60}: {seasonalMarker.DateTimeUtcUsno.ToIsoString()} c.f. {dt.ToIsoString()} = {diff} minutes");
            // }
        }

        // Console.WriteLine($"Maximum difference = {maxDiff} minutes.");
    }
}
