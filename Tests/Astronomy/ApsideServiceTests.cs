using Galaxon.Astronomy.Algorithms.Services;
using Galaxon.Astronomy.Data.Enums;
using Galaxon.Astronomy.Data.Models;
using Galaxon.Astronomy.Data.Repositories;
using Galaxon.Numerics.Geometry;
using Galaxon.Quantities;
using Galaxon.Time;
using Galaxon.UnitTesting;

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
        double jdtt0 = TimeScales.DateTimeToJulianDate(dt0);
        double jdtt1 = apsideService.GetClosestApsideApprox(venus, EApside.Periapsis, jdtt0);
        Console.WriteLine(jdtt1);
        DateTime dt1 = TimeScales.JulianDateTerrestrialToDateTimeUniversal(jdtt1);
        Console.WriteLine(dt1.ToIsoString(true));

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
        double jdtt0 = TimeScales.DateTimeToJulianDate(dt0);
        double jdtt1 = apsideService.GetClosestApsideApprox(mars, EApside.Apoapsis, jdtt0);
        Console.WriteLine(jdtt1);
        DateTime dt1 = TimeScales.JulianDateTerrestrialToDateTimeUniversal(jdtt1);
        Console.WriteLine(dt1.ToIsoString(true));

        // Assert.
        Assert.AreEqual(2463_530.456, jdtt1, 1e-3);
        Assert.AreEqual(2032, dt1.Year);
        Assert.AreEqual(10, dt1.Month);
        Assert.AreEqual(24, dt1.Day);
    }

    /// <summary>
    /// Test all the examples for Saturn, Uranus, and Neptune as provided in the table on page 271
    /// of AA2, and the discussion about Neptune which follows.
    /// </summary>
    /// <param name="planetName"></param>
    /// <param name="apside"></param>
    /// <param name="year"></param>
    /// <param name="month"></param>
    /// <param name="day"></param>
    /// <param name="expectedRadiusInAa"></param>
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
    [DataRow("Uranus", EApside.Apoapsis, 1756, 11, 27, 20.0893)]
    [DataRow("Uranus", EApside.Periapsis, 1798, 3, 3, 18.2890)]
    [DataRow("Uranus", EApside.Apoapsis, 1841, 3, 16, 20.0976)]
    [DataRow("Uranus", EApside.Periapsis, 1882, 3, 23, 18.2807)]
    [DataRow("Uranus", EApside.Apoapsis, 1925, 4, 1, 20.0973)]
    [DataRow("Uranus", EApside.Periapsis, 1966, 5, 21, 18.2848)]
    [DataRow("Uranus", EApside.Apoapsis, 2009, 2, 27, 20.0989)]
    [DataRow("Uranus", EApside.Periapsis, 2050, 8, 17, 18.2830)]
    [DataRow("Uranus", EApside.Apoapsis, 2092, 11, 23, 20.0994)]
    [DataRow("Neptune", EApside.Periapsis, 1876, 8, 28, 29.8148)]
    [DataRow("Neptune", EApside.Apoapsis, 1959, 7, 13, 30.3317)]
    [DataRow("Neptune", EApside.Periapsis, 2042, 9, 5, 29.8064)]
    public void GetClosestApside_TestChapter38OuterPlanetExamples(string planetName,
        EApside apside, int year, int month, int day, double expectedRadiusInAa)
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
        double jdtt0 = TimeScales.DateOnlyToJulianDate(dt0);
        (double jdtt1, double actualRadiusInMetres) = apsideService.GetClosestApside(planet, apside, jdtt0);
        DateTime dt1 = TimeScales.JulianDateTerrestrialToDateTimeUniversal(jdtt1);
        double actualRadiusInAu =
            actualRadiusInMetres / LengthConstants.METRES_PER_ASTRONOMICAL_UNIT;

        // Output result.
        Console.WriteLine($"Event time = {dt1.ToIsoString(true)} = {jdtt1:F6} Julian Date (TT)");
        Console.WriteLine($"Radius = {actualRadiusInMetres:F0} metres = {actualRadiusInAu:F6} AU");

        // Assert.
        Assert.AreEqual(dt0, dt1.GetDateOnly());
        Assert.AreEqual(expectedRadiusInAa, actualRadiusInAu, 1e-4);
    }

    /// <summary>
    /// Test the examples for Earth as provided in the table on page 274 of AA2.
    /// </summary>
    /// <param name="apside"></param>
    /// <param name="year"></param>
    /// <param name="month"></param>
    /// <param name="day"></param>
    /// <param name="hour"></param>
    /// <param name="expectedRadiusInAa"></param>
    [DataTestMethod]
    [DataRow(EApside.Periapsis, 1991, 1, 3, 3.0, 0.983_281)]
    public void GetClosestApside_TestChapter38EarthExamples(EApside apside, int year, int month, int day, double hours, double expectedRadiusInAa)
    {
        // Arrange.
        AstroObjectRepository astroObjectRepository =
            ServiceManager.GetService<AstroObjectRepository>();
        AstroObject? planet = astroObjectRepository.LoadByName("Earth", "Planet");
        if (planet == null)
        {
            Assert.Fail("Earth could not be found in the database.");
            return;
        }

        // Act.
        ApsideService apsideService = ServiceManager.GetService<ApsideService>();

        // Get the initial estimate of event date.
        DateTime dt0 = new (year, month, day, 0, 0, 0, DateTimeKind.Utc);
        double jdtt0 = TimeScales.DateTimeToJulianDate(dt0);

        // Get the expected result.
        DateTime dtttExpected = dt0 + TimeSpan.FromHours(hours);
        (double jdtt1, double actualRadiusInMetres) = apsideService.GetClosestApside(planet, apside, jdtt0);

        // Get the computed event DateTime in Terrestrial (Dynamical) Time, matching the results in
        // the table in the book.
        DateTime dtttActual = TimeScales.JulianDateToDateTime(jdtt1);

        // Get the radius in AU.
        double actualRadiusInAu =
            actualRadiusInMetres / LengthConstants.METRES_PER_ASTRONOMICAL_UNIT;

        // Output result.
        Console.WriteLine($"Expected event datetime = {dtttExpected.ToIsoString(true)} (TT)");
        Console.WriteLine($"Computed event datetime = {dtttActual.ToIsoString(true)} (TT) = {jdtt1:F6} Julian Date (TT)");
        Console.WriteLine($"Radius = {actualRadiusInMetres:F0} metres = {actualRadiusInAu:F6} AU");

        // Assert.
        DateTimeAssert.AreEqual(dtttExpected, dtttActual, TimeSpan.FromMinutes(1));
        Assert.AreEqual(expectedRadiusInAa, actualRadiusInAu, 1e-4);
    }
}
