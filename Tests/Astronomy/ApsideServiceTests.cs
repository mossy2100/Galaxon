using Galaxon.Astronomy.Algorithms.Records;
using Galaxon.Astronomy.Algorithms.Services;
using Galaxon.Astronomy.Data.Enums;
using Galaxon.Astronomy.Data.Models;
using Galaxon.Astronomy.Data.Repositories;
using Galaxon.Quantities;
using Galaxon.Time;

namespace Galaxon.Tests.Astronomy;

[TestClass]
public class ApsideServiceTests
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
    public void GetClosestApsideApprox_TestExample38a()
    {
        // Arrange.
        // Get the planet.
        AstroObjectRepository astroObjectRepository =
            ServiceManager.GetService<AstroObjectRepository>();
        AstroObject? venus = astroObjectRepository.LoadByName("Venus", "Planet");
        if (venus == null)
        {
            Assert.Fail("Venus could not be found in the database.");
            return;
        }

        // Act.
        ApsideService apsideService = ServiceManager.GetService<ApsideService>();
        DateTime dt0 = new (1978, 10, 15);
        DateTime dt1 = apsideService.GetClosestApsideApprox(venus, EApside.Periapsis, dt0);
        Console.WriteLine(dt1.ToIsoString(true));
        double jdtt1 = TimeScales.DateTimeUniversalToJulianDateTerrestrial(dt1);
        Console.WriteLine(jdtt1);

        // Assert.
        Assert.AreEqual(2443_873.704, jdtt1, 1e-3);
        Assert.AreEqual(1978, dt1.Year);
        Assert.AreEqual(12, dt1.Month);
        Assert.AreEqual(31, dt1.Day);
    }

    [TestMethod]
    public void GetClosestApsideApprox_TestExample38b()
    {
        // Arrange.
        // Get the planet.
        AstroObjectRepository astroObjectRepository =
            ServiceManager.GetService<AstroObjectRepository>();
        AstroObject? mars = astroObjectRepository.LoadByName("Mars", "Planet");
        if (mars == null)
        {
            Assert.Fail("Mars could not be found in the database.");
            return;
        }

        // Act.
        ApsideService apsideService = ServiceManager.GetService<ApsideService>();
        DateTime dt0 = new (2032, 1, 1);
        DateTime dt1 = apsideService.GetClosestApsideApprox(mars, EApside.Apoapsis, dt0);
        Console.WriteLine(dt1.ToIsoString(true));
        double jdtt1 = TimeScales.DateTimeUniversalToJulianDateTerrestrial(dt1);
        Console.WriteLine(jdtt1);

        // Assert.
        Assert.AreEqual(2463_530.456, jdtt1, 1e-3);
        Assert.AreEqual(2032, dt1.Year);
        Assert.AreEqual(10, dt1.Month);
        Assert.AreEqual(24, dt1.Day);
    }

    [DataTestMethod]
    [DataRow("Saturn", EApside.Apoapsis, 1929, 11, 11, 10.0467)]
    [DataRow("Saturn", EApside.Periapsis, 1944, 9, 8, 9.0288)]
    [DataRow("Saturn", EApside.Apoapsis, 1959, 5, 29, 10.0664)]
    [DataRow("Saturn", EApside.Periapsis, 1974, 1, 8, 9.0153)]
    [DataRow("Saturn", EApside.Apoapsis, 1988, 9, 11, 10.0444)]
    [DataRow("Saturn", EApside.Periapsis, 2003, 7, 26, 9.0309)]
    [DataRow("Saturn", EApside.Apoapsis, 2018, 4, 17, 10.0656)]
    [DataRow("Saturn", EApside.Periapsis, 2032, 11, 28, 9.0149)]
    [DataRow("Saturn", EApside.Apoapsis, 2047, 7, 15, 10.0462)]
    public void GetClosestApside_TestChapter38SaturnExample1(string planetName, EApside apside,
        int year, int month, int day, double expectedRadiusInAU)
    {
        // Arrange.
        AstroObjectRepository astroObjectRepository =
            ServiceManager.GetService<AstroObjectRepository>();
        AstroObject? planet = astroObjectRepository.LoadByName(planetName, "Planet");
        if (planet == null)
        {
            Assert.Fail($"{planetName} could not be found in the database.");
            return;
        }

        // Act.
        ApsideService apsideService = ServiceManager.GetService<ApsideService>();
        DateOnly dt0 = new (year, month, day);
        (DateTime dt1, double actualRadiusInMetres) =
            apsideService.GetClosestApside(planet, apside, dt0.ToDateTime());
        double jdtt1 = TimeScales.DateTimeUniversalToJulianDateTerrestrial(dt1);
        double actualRadiusInAU =
            actualRadiusInMetres / LengthConstants.METRES_PER_ASTRONOMICAL_UNIT;

        Console.WriteLine($"Event time = {dt1.ToIsoString(true)} = {jdtt1:F6} Julian Date (TT)");
        Console.WriteLine($"Radius = {actualRadiusInMetres:F0} metres = {actualRadiusInAU:F6} AU");

        // Assert.
        Assert.AreEqual(dt0, dt1.GetDateOnly());
        Assert.AreEqual(expectedRadiusInAU, actualRadiusInAU, 1e-4);
    }
}
