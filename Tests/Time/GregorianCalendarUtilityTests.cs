using Galaxon.Time.Utilities;

namespace Galaxon.Tests.Time;

[TestClass]
public class GregorianCalendarUtilityTests
{
    private string[] englishMonthNames =
    {
        "January", "February", "March", "April", "May", "June",
        "July", "August", "September", "October", "November", "December"
    };

    private string[] frenchMonthNames =
    {
        "Janvier", "Février", "Mars", "Avril", "Mai", "Juin",
        "Juillet", "Août", "Septembre", "Octobre", "Novembre", "Décembre"
    };

    [DataTestMethod]
    [DataRow("jan", 1)]
    [DataRow("feb", 2)]
    [DataRow("F", 2)]
    [DataRow("mar", 3)]
    [DataRow("apr", 4)]
    [DataRow("may", 5)]
    [DataRow("jun", 6)]
    [DataRow("jul", 7)]
    [DataRow("aug", 8)]
    [DataRow("AU", 8)]
    [DataRow("sep", 9)]
    [DataRow("s", 9)]
    [DataRow("oct", 10)]
    [DataRow("nov", 11)]
    [DataRow("dec", 12)]
    [DataRow("January", 1)]
    [DataRow("FEBRUARY", 2)]
    [DataRow("MarCh", 3)]
    [DataRow("April", 4)]
    [DataRow("MAY", 5)]
    [DataRow("June", 6)]
    [DataRow("JuLy", 7)]
    [DataRow("AuGuST", 8)]
    [DataRow("SEPTEMBER", 9)]
    [DataRow("October", 10)]
    [DataRow("NOVEMBER", 11)]
    [DataRow("December", 12)]
    public void MonthNameToNumber_ValidInput_ReturnsExpectedNumber(string monthName,
        int expectedNumber)
    {
        // Act
        int actualNumber = GregorianCalendarUtility.MonthNameToNumber(monthName);

        // Assert
        Assert.AreEqual(expectedNumber, actualNumber);
    }

    [TestMethod]
    public void MonthNameToNumber_InvalidInput_ThrowsException()
    {
        // Act & Assert
        Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            GregorianCalendarUtility.MonthNameToNumber("InvalidMonth"));
    }

    [TestMethod]
    public void MonthNameToNumber_AmbiguousInput_ThrowsException()
    {
        // Act & Assert
        Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            GregorianCalendarUtility.MonthNameToNumber("Ju"));
    }

    [TestMethod]
    public void MonthNumberToName_ValidMonths_English()
    {
        // Test the English month names
        for (int i = 1; i <= 12; i++)
        {
            string result = GregorianCalendarUtility.MonthNumberToName(i, "en");
            Assert.AreEqual(englishMonthNames[i - 1], result, true, $"Failed for month number {i}");
        }
    }

    [TestMethod]
    public void MonthNumberToName_ValidMonths_French()
    {
        // Test the French month names
        for (int i = 1; i <= 12; i++)
        {
            string result = GregorianCalendarUtility.MonthNumberToName(i, "fr");
            Assert.AreEqual(frenchMonthNames[i - 1], result, true, $"Failed for month number {i}");
        }
    }

    [TestMethod]
    public void MonthNumberToName_InvalidMonthNumber_ThrowsException()
    {
        // Test invalid month numbers below 1 and above 12
        Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            GregorianCalendarUtility.MonthNumberToName(0, "en"));
        Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            GregorianCalendarUtility.MonthNumberToName(13, "en"));
    }

    [TestMethod]
    public void MonthNumberToName_UnsupportedLanguageCode_ReturnsEnglish()
    {
        // Test with an unsupported language code
        Assert.AreEqual("January", GregorianCalendarUtility.MonthNumberToName(1, "xx"), true, $"Failed for month number 1.");
    }

    [TestMethod]
    public void GetMonthNames_DefaultEnglish_ReturnsCorrectMonthNames()
    {
        // Act
        Dictionary<int, string> result = GregorianCalendarUtility.GetMonthNames();

        // Assert
        Assert.AreEqual(12, result.Count, "Should return 12 months.");
        for (int i = 1; i <= 12; i++)
        {
            Assert.AreEqual(englishMonthNames[i - 1], result[i], true, $"Failed for month number {i}");
        }
    }

    [TestMethod]
    public void GetMonthNames_French_ReturnsCorrectMonthNames()
    {
        // Act
        Dictionary<int, string> result = GregorianCalendarUtility.GetMonthNames("fr");

        // Assert
        Assert.AreEqual(12, result.Count, "Should return 12 months.");
        for (int i = 1; i <= 12; i++)
        {
            Assert.AreEqual(frenchMonthNames[i - 1], result[i], true, $"Failed for month number {i}");
        }
    }
}
