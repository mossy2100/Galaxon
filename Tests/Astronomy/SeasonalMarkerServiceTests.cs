using Galaxon.Astronomy.Algorithms.Services;
using Galaxon.Astronomy.Data;
using Galaxon.Astronomy.Data.Enums;
using Galaxon.Astronomy.Data.Models;
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
        double jdtt =
            seasonalMarkerService.GetSeasonalMarkerApprox(1962, ESeasonalMarker.NorthernSolstice);
        DateTime dt = TimeScales.JulianDateTerrestrialToDateTimeUniversal(jdtt);

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
        SeasonalMarkerService seasonalMarkerService =
            ServiceManager.GetService<SeasonalMarkerService>();
        List<SeasonalMarkerRecord> seasonalMarkers =
            astroDbContext.SeasonalMarkers.Where(sm => sm.DateTimeUtcUsno != null)
                .OrderBy(sm => sm.DateTimeUtcUsno!.Value).ToList();

        // Check each.
        foreach (SeasonalMarkerRecord seasonalMarker in seasonalMarkers)
        {
            // Arrange.
            DateTime expected = seasonalMarker.DateTimeUtcUsno!.Value;

            // Act.
            DateTime actual = seasonalMarkerService.GetSeasonalMarkerAsDateTime(
                seasonalMarker.DateTimeUtcUsno.Value.Year,
                (ESeasonalMarker)seasonalMarker.MarkerNumber);

            // Assert.
            DateTimeAssert.AreEqual(expected, actual, maxDiff);
            // if (diff > goalMaxDiff)
            // {
            //     Console.WriteLine(
            //         $"{seasonalMarker.Marker.GetDisplayName(),60}: {seasonalMarker.DateTimeUtcUsno.ToIsoString()} c.f. {dt.ToIsoString()} = {diff} minutes");
            // }
        }

        // Console.WriteLine($"Maximum difference = {maxDiff} minutes.");
    }
}
