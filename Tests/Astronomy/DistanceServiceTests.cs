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

    [TestMethod]
    public void TestShortestDistance()
    {
        // Calculate distance in metres.
        AstroObjectRepository astroObjectRepository =
            ServiceManager.GetService<AstroObjectRepository>();
        AstroObject? earth = astroObjectRepository.Load("Earth", "Planet");

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

        // Calculate distance in metres.
        double dist = earth.CalculateShortestDistanceBetween(paris, washington);

        // Assert.
        // Check it's correct within 5 metres (in the book he's rounded it off to the nearest 10
        // metres).
        Assert.AreEqual(dist, 6_181_630, 5);
    }
}
