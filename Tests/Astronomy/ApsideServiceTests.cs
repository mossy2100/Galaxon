using Galaxon.Astronomy.Algorithms.Records;
using Galaxon.Astronomy.Algorithms.Services;
using Galaxon.Astronomy.Data.Enums;
using Galaxon.Astronomy.Data.Models;
using Galaxon.Astronomy.Data.Repositories;
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
        AstroObject venus = astroObjectRepository.LoadByName("Venus", "Planet");

        // Act.
        ApsideService apsideService = ServiceManager.GetService<ApsideService>();
        DateTime dt0 = new (1978, 10, 15);
        double jdtt0 = TimeScales.DateTimeToJulianDate(dt0);
        ApsideEvent apsideEvent = apsideService.GetClosestApsideApprox(venus, jdtt0);
        double jdtt1 = apsideEvent.JulianDateTerrestrial;
        Console.WriteLine(jdtt1);
        DateTime dt1 = apsideEvent.DateTimeUtc;
        Console.WriteLine(dt1.ToIsoString());

        // Assert.
        Assert.AreEqual(2443_873.704, jdtt1, 5e-4);
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
        AstroObject mars = astroObjectRepository.LoadByName("Mars", "Planet");

        // Act.
        ApsideService apsideService = ServiceManager.GetService<ApsideService>();
        DateTime dt0 = new (2032, 1, 1);
        double jdtt0 = TimeScales.DateTimeToJulianDate(dt0);
        ApsideEvent apsideEvent = apsideService.GetClosestApsideApprox(mars, jdtt0);
        double jdtt1 = apsideEvent.JulianDateTerrestrial;
        Console.WriteLine(jdtt1);
        DateTime dt1 = apsideEvent.DateTimeUtc;
        Console.WriteLine(dt1.ToIsoString());

        // Assert.
        Assert.AreEqual(2463_530.456, jdtt1, 5e-4);
        Assert.AreEqual(2032, dt1.Year);
        Assert.AreEqual(10, dt1.Month);
        Assert.AreEqual(24, dt1.Day);
    }

    /// <summary>
    /// Test the example given for Earth on AA2 p273.
    /// </summary>
    [TestMethod]
    public void GetClosestApsideApprox_TestEarthExample()
    {
        // Arrange.
        // Get the planet.
        AstroObjectRepository astroObjectRepository =
            ServiceManager.GetService<AstroObjectRepository>();
        AstroObject planet = astroObjectRepository.LoadByName("Earth", "Planet");

        // Act.
        ApsideService apsideService = ServiceManager.GetService<ApsideService>();
        DateTime dt0 = new (1990, 1, 4);
        double jdtt0 = TimeScales.DateTimeToJulianDate(dt0);
        ApsideEvent apsideEvent = apsideService.GetClosestApsideApprox(planet, jdtt0);
        double jdtt1 = apsideEvent.JulianDateTerrestrial;
        DateTime dt1 = apsideEvent.DateTimeUtc;

        // Output.
        Console.WriteLine(jdtt1);
        Console.WriteLine(dt1.ToIsoString());

        // Assert.
        Assert.AreEqual(2447_896.172, jdtt1, 5e-4);
        Assert.AreEqual(1990, dt1.Year);
        Assert.AreEqual(1, dt1.Month);
        Assert.AreEqual(4, dt1.Day);
        Assert.AreEqual(16, dt1.Hour);
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
        AstroObject planet = astroObjectRepository.LoadByName(planetName, "Planet");

        // Act.
        ApsideService apsideService = ServiceManager.GetService<ApsideService>();
        DateOnly dt0 = new (year, month, day);
        double jdtt0 = TimeScales.DateOnlyToJulianDate(dt0);
        ApsideEvent apsideEvent = apsideService.GetClosestApside(planet, jdtt0);
        double jdtt1 = apsideEvent.JulianDateTerrestrial;
        DateTime dt1 = apsideEvent.DateTimeUtc;
        double actualRadiusInMetres = apsideEvent.RadiusInMetres!.Value;
        double actualRadiusInAu = apsideEvent.RadiusInAstronomicalUnits!.Value;

        // Output result.
        Console.WriteLine($"Event time = {dt1.ToIsoString()} = {jdtt1:F6} Julian Date (TT)");
        Console.WriteLine($"Radius = {actualRadiusInMetres:F0} metres = {actualRadiusInAu:F6} AU");

        // Assert.
        Assert.AreEqual(dt0, dt1.GetDateOnly());
        Assert.AreEqual(expectedRadiusInAa, actualRadiusInAu, 5e-5);
    }

    /// <summary>
    /// Test the examples for Earth as provided in the table on page 274 of AA2.
    /// </summary>
    /// <param name="apside"></param>
    /// <param name="year"></param>
    /// <param name="month"></param>
    /// <param name="day"></param>
    /// <param name="hours"></param>
    /// <param name="expectedRadiusInAa"></param>
    [DataTestMethod]
    [DataRow(EApside.Periapsis, 1991, 1, 3, 3.0, 0.983_281)]
    [DataRow(EApside.Periapsis, 1992, 1, 3, 15.06, 0.983_324)]
    [DataRow(EApside.Periapsis, 1993, 1, 4, 3.08, 0.983_283)]
    [DataRow(EApside.Periapsis, 1994, 1, 2, 5.92, 0.983_301)]
    [DataRow(EApside.Periapsis, 1995, 1, 4, 11.1, 0.983_302)]
    [DataRow(EApside.Periapsis, 1996, 1, 4, 7.43, 0.983_223)]
    [DataRow(EApside.Periapsis, 1997, 1, 1, 23.29, 0.983_267)]
    [DataRow(EApside.Periapsis, 1998, 1, 4, 21.28, 0.983_300)]
    [DataRow(EApside.Periapsis, 1999, 1, 3, 13.02, 0.983_281)]
    [DataRow(EApside.Periapsis, 2000, 1, 3, 5.31, 0.983_321)]
    [DataRow(EApside.Periapsis, 2001, 1, 4, 8.89, 0.983_286)]
    [DataRow(EApside.Periapsis, 2002, 1, 2, 14.17, 0.983_290)]
    [DataRow(EApside.Periapsis, 2003, 1, 4, 5.04, 0.983_320)]
    [DataRow(EApside.Periapsis, 2004, 1, 4, 17.72, 0.983_265)]
    [DataRow(EApside.Periapsis, 2005, 1, 2, 0.61, 0.983_297)]
    [DataRow(EApside.Periapsis, 2006, 1, 4, 15.52, 0.983_327)]
    [DataRow(EApside.Periapsis, 2007, 1, 3, 19.74, 0.983_260)]
    [DataRow(EApside.Periapsis, 2008, 1, 2, 23.87, 0.983_280)]
    [DataRow(EApside.Periapsis, 2009, 1, 4, 15.51, 0.983_273)]
    [DataRow(EApside.Periapsis, 2010, 1, 3, 0.18, 0.983_290)]
    [DataRow(EApside.Apoapsis, 1991, 7, 6, 15.46, 1.016_703)]
    [DataRow(EApside.Apoapsis, 1992, 7, 3, 12.14, 1.016_740)]
    [DataRow(EApside.Apoapsis, 1993, 7, 4, 22.37, 1.016_666)]
    [DataRow(EApside.Apoapsis, 1994, 7, 5, 19.30, 1.016_724)]
    [DataRow(EApside.Apoapsis, 1995, 7, 4, 2.29, 1.016_742)]
    [DataRow(EApside.Apoapsis, 1996, 7, 5, 19.02, 1.016_717)]
    [DataRow(EApside.Apoapsis, 1997, 7, 4, 19.34, 1.016_754)]
    [DataRow(EApside.Apoapsis, 1998, 7, 3, 23.86, 1.016_696)]
    [DataRow(EApside.Apoapsis, 1999, 7, 6, 22.86, 1.016_718)]
    [DataRow(EApside.Apoapsis, 2000, 7, 3, 23.84, 1.016_741)]
    [DataRow(EApside.Apoapsis, 2001, 7, 4, 13.65, 1.016_643)]
    [DataRow(EApside.Apoapsis, 2002, 7, 6, 3.8, 1.016_688)]
    [DataRow(EApside.Apoapsis, 2003, 7, 4, 5.67, 1.016_728)]
    [DataRow(EApside.Apoapsis, 2004, 7, 5, 10.9, 1.016_694)]
    [DataRow(EApside.Apoapsis, 2005, 7, 5, 4.98, 1.016_742)]
    [DataRow(EApside.Apoapsis, 2006, 7, 3, 23.18, 1.016_697)]
    [DataRow(EApside.Apoapsis, 2007, 7, 6, 23.89, 1.016_706)]
    [DataRow(EApside.Apoapsis, 2008, 7, 4, 7.71, 1.016_754)]
    [DataRow(EApside.Apoapsis, 2009, 7, 4, 1.69, 1.016_666)]
    [DataRow(EApside.Apoapsis, 2010, 7, 6, 11.52, 1.016_702)]
    public void GetClosestApside_TestChapter38EarthExamples(EApside apside, int year, int month,
        int day, double hours, double expectedRadiusInAa)
    {
        // Arrange.
        AstroObjectRepository astroObjectRepository =
            ServiceManager.GetService<AstroObjectRepository>();
        AstroObject planet = astroObjectRepository.LoadByName("Earth", "Planet");

        ApsideService apsideService = ServiceManager.GetService<ApsideService>();

        // Get the initial estimate of event date.
        DateTime dt0 = new (year, month, day, 0, 0, 0, DateTimeKind.Utc);
        double jdtt0 = TimeScales.DateTimeToJulianDate(dt0);

        // Get the expected result.
        DateTime dtttExpected = dt0 + TimeSpan.FromHours(hours);

        // Act.
        ApsideEvent apsideEvent = apsideService.GetClosestApside(planet, jdtt0);
        double jdtt1 = apsideEvent.JulianDateTerrestrial;
        double actualRadiusInMetres = apsideEvent.RadiusInMetres!.Value;

        // Get the computed event DateTime in Terrestrial (Dynamical) Time, matching the results in
        // the table in the book.
        DateTime dtttActual = TimeScales.JulianDateToDateTime(jdtt1);

        // Get the radius in AU.
        double actualRadiusInAu = apsideEvent.RadiusInAstronomicalUnits!.Value;

        // Output.
        Console.WriteLine($"Expected event datetime = {dtttExpected.ToIsoString()} (TT)");
        Console.WriteLine(
            $"Computed event datetime = {dtttActual.ToIsoString()} (TT) = {jdtt1:F6} Julian Date (TT)");
        Console.WriteLine($"Radius = {actualRadiusInMetres:F0} metres = {actualRadiusInAu:F6} AU");

        // Assert.
        DateTimeAssert.AreEqual(dtttExpected, dtttActual, TimeSpan.FromMinutes(1));
        Assert.AreEqual(expectedRadiusInAa, actualRadiusInAu, 5e-7);
    }
}
