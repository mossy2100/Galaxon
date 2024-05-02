using Galaxon.Astronomy.Algorithms.Extensions;
using Galaxon.Astronomy.Data.Models;
using Galaxon.Astronomy.Data.Repositories;
using Galaxon.Numerics.Geometry;
using GeoCoordinatePortable;

namespace Galaxon.Tests.Astronomy;

[TestClass]
public class DistanceServiceTests
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

    #region CalculateShortestDistanceBetween

    /// <summary>
    /// Test the algorithm for finding the distance between 2 locations on Earth's surface.
    /// Compare with example 11.c on page 85 of Astronomical Algorithms 2nd ed. by Jean Meeus.
    /// </summary>
    [TestMethod]
    public void CalculateShortestDistanceBetween_Example11c_ReturnsCorrectResult()
    {
        // Arrange.
        // Calculate distance in metres.
        AstroObjectRepository astroObjectRepository =
            ServiceManager.GetService<AstroObjectRepository>();
        AstroObject? earth = astroObjectRepository.LoadByName("Earth", "Planet");
        if (earth == null)
        {
            Assert.Fail("Earth could not be found in the database.");
            return;
        }

        // Paris.
        double lat1 = Angles.DMSToDegrees(48, 50, 11);
        double long1 = Angles.DMSToDegrees(-2, -20, -14);
        GeoCoordinate paris = new (lat1, long1);

        // Washington.
        double lat2 = Angles.DMSToDegrees(38, 55, 17);
        double long2 = Angles.DMSToDegrees(77, 3, 56);
        GeoCoordinate washington = new (lat2, long2);

        // Act. Calculate distance in metres.
        double dist = earth.CalculateShortestDistanceBetween(paris, washington);

        // Assert.
        // Check it's correct within 5 metres (in the book he's rounded it off to the nearest 10
        // metres).
        Assert.AreEqual(dist, 6_181_630, 5);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void CalculateShortestDistanceBetween_UnknownLocation1_ThrowsException()
    {
        // Arrange.
        // Calculate distance in metres.
        AstroObjectRepository astroObjectRepository =
            ServiceManager.GetService<AstroObjectRepository>();
        AstroObject? earth = astroObjectRepository.LoadByName("Earth", "Planet");

        if (earth == null)
        {
            Assert.Fail("Earth could not be found in the database.");
            return;
        }

        // Unknown.
        GeoCoordinate unknown = new ();

        // Washington.
        double lat2 = Angles.DMSToDegrees(38, 55, 17);
        double long2 = Angles.DMSToDegrees(77, 3, 56);
        GeoCoordinate washington = new (lat2, long2);

        // Act. Calculate distance in metres.
        double dist = earth.CalculateShortestDistanceBetween(unknown, washington);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void CalculateShortestDistanceBetween_UnknownLocation2_ThrowsException()
    {
        // Arrange.
        // Calculate distance in metres.
        AstroObjectRepository astroObjectRepository =
            ServiceManager.GetService<AstroObjectRepository>();
        AstroObject? earth = astroObjectRepository.LoadByName("Earth", "Planet");

        if (earth == null)
        {
            Assert.Fail("Earth could not be found in the database.");
            return;
        }

        // Washington.
        double lat2 = Angles.DMSToDegrees(38, 55, 17);
        double long2 = Angles.DMSToDegrees(77, 3, 56);
        GeoCoordinate washington = new (lat2, long2);

        // Unknown.
        GeoCoordinate unknown = new ();

        // Act. Calculate distance in metres.
        double dist = earth.CalculateShortestDistanceBetween(washington, unknown);
    }

    #endregion CalculateShortestDistanceBetween
}
