using Galaxon.Time;

namespace Galaxon.Tests.Time;

[TestClass]
public class GregorianCalendarUtilityTests
{
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
    public void GetMonthNames_DefaultEnglish_ReturnsCorrectMonthNames()
    {
        // Arrange
        string[] expectedEnglishMonthNames =
        {
            "January", "February", "March", "April", "May", "June",
            "July", "August", "September", "October", "November", "December"
        };

        // Act
        Dictionary<int, string> result = GregorianCalendarUtility.GetMonthNames();

        // Assert
        Assert.AreEqual(12, result.Count, "Should return 12 months.");
        CollectionAssert.AreEqual(expectedEnglishMonthNames, result.Values.ToArray(),
            "The month names should match the expected English month names.");
    }

    [TestMethod]
    public void GetMonthNames_French_ReturnsCorrectMonthNames()
    {
        // Arrange
        string[] expectedFrenchMonthNames =
        [
            "janvier", "février", "mars", "avril", "mai", "juin",
            "juillet", "août", "septembre", "octobre", "novembre", "décembre"
        ];

        // Act
        Dictionary<int, string> result = GregorianCalendarUtility.GetMonthNames("fr");

        // Assert
        Assert.AreEqual(12, result.Count, "Should return 12 months.");
        CollectionAssert.AreEqual(expectedFrenchMonthNames, result.Values.ToArray(),
            "The month names should match the expected French month names.");
    }
}
