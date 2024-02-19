namespace Galaxon.Time.Tests;

[TestClass]
public class XDateOnlyTests
{
    [TestMethod]
    public void TestDateOnlyGetTotalDays()
    {
        DateOnly date;

        // Test start of range.
        date = new DateOnly(1, 1, 1);
        Assert.AreEqual(date.GetTotalDays(), 0);

        // Test current date.
        date = new DateOnly(2022, 6, 8);
        Assert.AreEqual(date.GetTotalDays(), 738313);

        // Test middle of range.
        date = new DateOnly(5000, 7, 2);
        Assert.AreEqual(date.GetTotalDays(), 1826029);

        // Test end of range.
        date = new DateOnly(9999, 12, 31);
        Assert.AreEqual(date.GetTotalDays(), 3652058);
    }

    [TestMethod]
    public void TestDateOnlyFromTotalDays()
    {
        DateOnly date1, date2;

        date1 = new DateOnly(1, 1, 1);
        date2 = XDateOnly.FromTotalDays(0);
        Assert.AreEqual(date1.GetTicks(), date2.GetTicks());

        date1 = new DateOnly(2022, 6, 8);
        date2 = XDateOnly.FromTotalDays(738313);
        Assert.AreEqual(date1.GetTicks(), date2.GetTicks());

        date1 = new DateOnly(5000, 7, 2);
        date2 = XDateOnly.FromTotalDays(1826029);
        Assert.AreEqual(date1.GetTicks(), date2.GetTicks());

        date1 = new DateOnly(9999, 12, 31);
        date2 = XDateOnly.FromTotalDays(3652058);
        Assert.AreEqual(date1.GetTicks(), date2.GetTicks());
    }

    [TestMethod]
    public void TestSubtract()
    {
        // This uses the dates of Halley's Comet from example 7.d in AA2 p64.
        DateOnly date1 = new (1910, 4, 20);
        DateOnly date2 = new (1986, 2, 9);
        Assert.AreEqual(date2.Subtract(date1), 27689);
    }

    [TestMethod]
    public void TestAdd()
    {
        // From the exercise in AA2 p64.
        DateOnly date1 = new (1991, 7, 11);
        var date2 = date1.AddDays(10000);
        Assert.AreEqual(date2.ToIsoString(), "2018-11-26");
    }

    [TestMethod]
    // Also tests ToIsoString().
    public void TestFromDayOfYear()
    {
        DateOnly date1, date2;
        int n1, n2;

        // Test start of range.
        date1 = new DateOnly(1, 1, 1);
        n1 = date1.DayOfYear;
        date2 = XDateOnly.FromDayOfYear(1, n1);
        Assert.AreEqual(date2.ToIsoString(), "0001-01-01");
        n2 = date2.DayOfYear;
        Assert.AreEqual(n1, n2);

        // Test leap year.
        date1 = new DateOnly(2020, 12, 31);
        n1 = date1.DayOfYear;
        date2 = XDateOnly.FromDayOfYear(2020, n1);
        Assert.AreEqual(date2.ToIsoString(), "2020-12-31");
        n2 = date2.DayOfYear;
        Assert.AreEqual(n1, n2);

        // Test current date.
        date1 = new DateOnly(2022, 6, 8);
        n1 = date1.DayOfYear;
        date2 = XDateOnly.FromDayOfYear(2022, n1);
        Assert.AreEqual(date2.ToIsoString(), "2022-06-08");
        n2 = date2.DayOfYear;
        Assert.AreEqual(n1, n2);

        // Test middle of range.
        date1 = new DateOnly(5000, 7, 2);
        n1 = date1.DayOfYear;
        date2 = XDateOnly.FromDayOfYear(5000, n1);
        Assert.AreEqual(date2.ToIsoString(), "5000-07-02");
        n2 = date2.DayOfYear;
        Assert.AreEqual(n1, n2);

        // Test end of range.
        date1 = new DateOnly(9999, 12, 31);
        n1 = date1.DayOfYear;
        date2 = XDateOnly.FromDayOfYear(9999, n1);
        Assert.AreEqual(date2.ToIsoString(), "9999-12-31");
        n2 = date2.DayOfYear;
        Assert.AreEqual(n1, n2);
    }

    /// <summary>
    /// These examples are from Meeus AA2 p67.
    /// I've also verified the dates at:
    /// <see href="https://www.assa.org.au/edm"/>
    /// </summary>
    [TestMethod]
    public void TestEaster()
    {
        DateOnly date;

        date = XGregorianCalendar.Easter(1991);
        Assert.AreEqual(date.ToIsoString(), "1991-03-31");

        date = XGregorianCalendar.Easter(1992);
        Assert.AreEqual(date.ToIsoString(), "1992-04-19");

        date = XGregorianCalendar.Easter(1993);
        Assert.AreEqual(date.ToIsoString(), "1993-04-11");

        date = XGregorianCalendar.Easter(1954);
        Assert.AreEqual(date.ToIsoString(), "1954-04-18");

        date = XGregorianCalendar.Easter(2000);
        Assert.AreEqual(date.ToIsoString(), "2000-04-23");

        date = XGregorianCalendar.Easter(1818);
        Assert.AreEqual(date.ToIsoString(), "1818-03-22");

        date = XGregorianCalendar.Easter(2285);
        Assert.AreEqual(date.ToIsoString(), "2285-03-22");

        date = XGregorianCalendar.Easter(1886);
        Assert.AreEqual(date.ToIsoString(), "1886-04-25");

        date = XGregorianCalendar.Easter(1943);
        Assert.AreEqual(date.ToIsoString(), "1943-04-25");

        date = XGregorianCalendar.Easter(2038);
        Assert.AreEqual(date.ToIsoString(), "2038-04-25");
    }

    [TestMethod]
    public void TestGetNthWeekdayInMonth()
    {
        DateOnly d;

        // 1st Monday in January, 2023.
        d = XGregorianCalendar.NthWeekdayInMonth(2023, 1, 1, DayOfWeek.Monday);
        Assert.AreEqual("2023-01-02", d.ToIsoString());

        // 2nd Thursday in February, 2023.
        d = XGregorianCalendar.NthWeekdayInMonth(2023, 2, 2, DayOfWeek.Thursday);
        Assert.AreEqual("2023-02-09", d.ToIsoString());

        // 3rd Saturday in April, 2023.
        d = XGregorianCalendar.NthWeekdayInMonth(2023, 4, 3, DayOfWeek.Saturday);
        Assert.AreEqual("2023-04-15", d.ToIsoString());

        // 4th Tuesday in September, 2023.
        d = XGregorianCalendar.NthWeekdayInMonth(2023, 9, 4, DayOfWeek.Tuesday);
        Assert.AreEqual("2023-09-26", d.ToIsoString());

        // 5th Sunday in October, 2023.
        d = XGregorianCalendar.NthWeekdayInMonth(2023, 10, 5, DayOfWeek.Sunday);
        Assert.AreEqual("2023-10-29", d.ToIsoString());

        // 5th Friday in November, 2023 (doesn't exist).
        Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            XGregorianCalendar.NthWeekdayInMonth(2023, 11, 5, DayOfWeek.Friday));

        // 6th Monday in January, 2024 (doesn't exist).
        Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            XGregorianCalendar.NthWeekdayInMonth(2024, 1, 6, DayOfWeek.Monday));

        // Last Wednesday in July, 2023.
        d = XGregorianCalendar.NthWeekdayInMonth(2023, 7, -1, DayOfWeek.Wednesday);
        Assert.AreEqual("2023-07-26", d.ToIsoString());

        // 2nd-last Sunday in March, 2023.
        d = XGregorianCalendar.NthWeekdayInMonth(2023, 3, -2, DayOfWeek.Sunday);
        Assert.AreEqual("2023-03-19", d.ToIsoString());

        // 3rd-last Thursday in June, 2023.
        d = XGregorianCalendar.NthWeekdayInMonth(2023, 6, -3, DayOfWeek.Thursday);
        Assert.AreEqual("2023-06-15", d.ToIsoString());

        // 4th-last Monday in May, 2023.
        d = XGregorianCalendar.NthWeekdayInMonth(2023, 5, -4, DayOfWeek.Monday);
        Assert.AreEqual("2023-05-08", d.ToIsoString());

        // 5th-last Friday in December, 2023.
        d = XGregorianCalendar.NthWeekdayInMonth(2023, 12, -5, DayOfWeek.Friday);
        Assert.AreEqual("2023-12-01", d.ToIsoString());

        // 5th-last Saturday in August, 2023 (doesn't exist).
        Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            XGregorianCalendar.NthWeekdayInMonth(2023, 8, -5, DayOfWeek.Saturday));

        // 6th-last Tuesday in February, 2024 (doesn't exist).
        Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            XGregorianCalendar.NthWeekdayInMonth(2024, 2, -6, DayOfWeek.Monday));
    }

    [TestMethod]
    public void TestGetThanksgiving()
    {
        DateOnly d;

        d = XGregorianCalendar.Thanksgiving(2023);
        Assert.AreEqual("2023-11-23", d.ToIsoString());
    }
}
