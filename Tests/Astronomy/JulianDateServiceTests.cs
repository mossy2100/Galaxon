using Galaxon.Astronomy.Algorithms.Services;
using Galaxon.Time;

namespace Galaxon.Tests.Astronomy;

[TestClass]
public class JulianDateServiceTests
{
    [TestMethod]
    public void TestDateOnlyToJulianDay()
    {
        DateOnly date;

        // Test start of range.
        date = new DateOnly(1, 1, 1);
        Assert.AreEqual(JulianDateService.DateOnlyToJulianDateUniversal(date), 1721425.5);

        // Test current date.
        date = new DateOnly(2022, 6, 8);
        Assert.AreEqual(JulianDateService.DateOnlyToJulianDateUniversal(date), 2459738.5);

        // Test middle of range.
        date = new DateOnly(5000, 7, 2);
        Assert.AreEqual(JulianDateService.DateOnlyToJulianDateUniversal(date), 3547454.5);

        // Test end of range.
        date = new DateOnly(9999, 12, 31);
        Assert.AreEqual(JulianDateService.DateOnlyToJulianDateUniversal(date), 5373483.5);
    }

    [TestMethod]
    public void TestDateOnlyFromJulianDay()
    {
        DateOnly date1, date2;

        // Test start of range.
        date1 = new DateOnly(1, 1, 1);
        date2 = JulianDateService.JulianDateUniversalToDateOnly(1721425.5);
        Assert.AreEqual(date1.GetTicks(), date2.GetTicks());

        // Test current date.
        date1 = new DateOnly(2022, 6, 8);
        date2 = JulianDateService.JulianDateUniversalToDateOnly(2459738.5);
        Assert.AreEqual(date1.GetTicks(), date2.GetTicks());

        // Test middle of range.
        date1 = new DateOnly(5000, 7, 2);
        date2 = JulianDateService.JulianDateUniversalToDateOnly(3547454.5);
        Assert.AreEqual(date1.GetTicks(), date2.GetTicks());

        // Test end of range.
        date1 = new DateOnly(9999, 12, 31);
        date2 = JulianDateService.JulianDateUniversalToDateOnly(5373483.5);
        Assert.AreEqual(date1.GetTicks(), date2.GetTicks());
    }

    // TODO Include a reference to the source of the test data.
    [TestMethod]
    public void TestDateTimeToJulianDay()
    {
        DateTime dt;

        // Test start of range.
        dt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        Assert.AreEqual(JulianDateService.DateTimeToJulianDateUniversal(dt), 1721425.5);

        // Test current date.
        dt = new DateTime(2022, 6, 7, 0, 0, 0, 0, DateTimeKind.Utc);
        Assert.AreEqual(JulianDateService.DateTimeToJulianDateUniversal(dt), 2459737.5);

        // Test middle of range.
        dt = new DateTime(5000, 7, 2, 0, 0, 0, 0, DateTimeKind.Utc);
        Assert.AreEqual(JulianDateService.DateTimeToJulianDateUniversal(dt), 3547454.5);

        // Test end of range.
        dt = new DateTime(9999, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc);
        Assert.AreEqual(JulianDateService.DateTimeToJulianDateUniversal(dt), 5373483.5);

        // Test values from Meeus p62.
        dt = new DateTime(2000, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc);
        Assert.AreEqual(JulianDateService.DateTimeToJulianDateUniversal(dt), 2451545.0);
        dt = new DateTime(1999, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        Assert.AreEqual(JulianDateService.DateTimeToJulianDateUniversal(dt), 2451179.5);
        dt = new DateTime(1987, 1, 27, 0, 0, 0, 0, DateTimeKind.Utc);
        Assert.AreEqual(JulianDateService.DateTimeToJulianDateUniversal(dt), 2446822.5);
        dt = new DateTime(1987, 6, 19, 12, 0, 0, 0, DateTimeKind.Utc);
        Assert.AreEqual(JulianDateService.DateTimeToJulianDateUniversal(dt), 2446966.0);
        dt = new DateTime(1988, 1, 27, 0, 0, 0, 0, DateTimeKind.Utc);
        Assert.AreEqual(JulianDateService.DateTimeToJulianDateUniversal(dt), 2447187.5);
        dt = new DateTime(1988, 6, 19, 12, 0, 0, 0, DateTimeKind.Utc);
        Assert.AreEqual(JulianDateService.DateTimeToJulianDateUniversal(dt), 2447332.0);
        dt = new DateTime(1900, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        Assert.AreEqual(JulianDateService.DateTimeToJulianDateUniversal(dt), 2415020.5);
        dt = new DateTime(1600, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        Assert.AreEqual(JulianDateService.DateTimeToJulianDateUniversal(dt), 2305447.5);
        dt = new DateTime(1600, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc);
        Assert.AreEqual(JulianDateService.DateTimeToJulianDateUniversal(dt), 2305812.5);
    }

    // TODO Include a reference to the source of the test data.
    [TestMethod]
    public void TestDateTimeFromJulianDay()
    {
        DateTime dt1, dt2;

        dt1 = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dt2 = JulianDateService.JulianDateTerrestrialToDateTime(1721425.5);
        Assert.AreEqual(dt1.Ticks, dt2.Ticks);

        dt1 = new DateTime(2022, 6, 7, 0, 0, 0, 0, DateTimeKind.Utc);
        dt2 = JulianDateService.JulianDateTerrestrialToDateTime(2459737.5);
        Assert.AreEqual(dt1.Ticks, dt2.Ticks);

        dt1 = new DateTime(5000, 7, 2, 0, 0, 0, 0, DateTimeKind.Utc);
        dt2 = JulianDateService.JulianDateTerrestrialToDateTime(3547454.5);
        Assert.AreEqual(dt1.Ticks, dt2.Ticks);

        dt1 = new DateTime(9999, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc);
        dt2 = JulianDateService.JulianDateTerrestrialToDateTime(5373483.5);
        Assert.AreEqual(dt1.Ticks, dt2.Ticks);
    }

    [DataTestMethod]
    [DataRow(100, 2, 29, 2, 27)]
    [DataRow(100, 3, 1, 2, 28)]
    [DataRow(100, 3, 2, 3, 1)]
    [DataRow(200, 2, 28, 2, 27)]
    [DataRow(200, 2, 29, 2, 28)]
    [DataRow(200, 3, 1, 3, 1)]
    [DataRow(300, 2, 28, 2, 28)]
    [DataRow(300, 2, 29, 3, 1)]
    [DataRow(300, 3, 1, 3, 2)]
    [DataRow(500, 2, 28, 3, 1)]
    [DataRow(500, 2, 29, 3, 2)]
    [DataRow(500, 3, 1, 3, 3)]
    [DataRow(600, 2, 28, 3, 2)]
    [DataRow(600, 2, 29, 3, 3)]
    [DataRow(600, 3, 1, 3, 4)]
    [DataRow(700, 2, 28, 3, 3)]
    [DataRow(700, 2, 29, 3, 4)]
    [DataRow(700, 3, 1, 3, 5)]
    [DataRow(900, 2, 28, 3, 4)]
    [DataRow(900, 2, 29, 3, 5)]
    [DataRow(900, 3, 1, 3, 6)]
    [DataRow(1000, 2, 28, 3, 5)]
    [DataRow(1000, 2, 29, 3, 6)]
    [DataRow(1000, 3, 1, 3, 7)]
    [DataRow(1100, 2, 28, 3, 6)]
    [DataRow(1100, 2, 29, 3, 7)]
    [DataRow(1100, 3, 1, 3, 8)]
    [DataRow(1300, 2, 28, 3, 7)]
    [DataRow(1300, 2, 29, 3, 8)]
    [DataRow(1300, 3, 1, 3, 9)]
    [DataRow(1400, 2, 28, 3, 8)]
    [DataRow(1400, 2, 29, 3, 9)]
    [DataRow(1400, 3, 1, 3, 10)]
    [DataRow(1500, 2, 28, 3, 9)]
    [DataRow(1500, 2, 29, 3, 10)]
    [DataRow(1500, 3, 1, 3, 11)]
    [DataRow(1582, 10, 4, 10, 14)]
    [DataRow(1582, 10, 5, 10, 15)]
    [DataRow(1582, 10, 6, 10, 16)]
    [DataRow(1700, 2, 18, 2, 28)]
    [DataRow(1700, 2, 19, 3, 1)]
    [DataRow(1700, 2, 28, 3, 10)]
    [DataRow(1700, 2, 29, 3, 11)]
    [DataRow(1700, 3, 1, 3, 12)]
    [DataRow(1800, 2, 17, 2, 28)]
    [DataRow(1800, 2, 18, 3, 1)]
    [DataRow(1800, 2, 28, 3, 11)]
    [DataRow(1800, 2, 29, 3, 12)]
    [DataRow(1800, 3, 1, 3, 13)]
    [DataRow(1900, 2, 16, 2, 28)]
    [DataRow(1900, 2, 17, 3, 1)]
    [DataRow(1900, 2, 28, 3, 12)]
    [DataRow(1900, 2, 29, 3, 13)]
    [DataRow(1900, 3, 1, 3, 14)]
    [DataRow(2100, 2, 15, 2, 28)]
    [DataRow(2100, 2, 16, 3, 1)]
    [DataRow(2100, 2, 28, 3, 13)]
    [DataRow(2100, 2, 29, 3, 14)]
    public void JulianCalendarDateToGregorianDate_ReturnsCorrectResult(int year, int jMonth,
        int jDay, int gMonth, int gDay)
    {
        // Arrange
        DateOnly expected = new (year, gMonth, gDay);

        // Act
        DateOnly actual = JulianDateService.JulianCalendarDateToGregorianDate(year, jMonth, jDay);

        // Assert
        Assert.AreEqual(expected, actual);
    }
}
