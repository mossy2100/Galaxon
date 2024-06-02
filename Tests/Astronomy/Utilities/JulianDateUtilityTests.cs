using Galaxon.Astronomy.Algorithms.Utilities;
using Galaxon.Time;

namespace Galaxon.Tests.Astronomy.Utilities;

[TestClass]
public class JulianDateUtilityTests
{
    private double _tolerance = 1e-4 / TimeConstants.SECONDS_PER_DAY;

    [TestMethod]
    public void TestUniversalToTerrestrial_WithValidInput()
    {
        // Arrange
        double jdut = 2441253.5;
        double expected = 2441253.5004868642;

        // Act
        double actual = JulianDateUtility.UniversalToTerrestrial(jdut);
        Console.WriteLine(actual);

        // Assert
        Assert.AreEqual(expected, actual, _tolerance);
    }

    [TestMethod]
    public void TestTerrestrialToUniversal_WithValidInput()
    {
        // Arrange
        double jdtt = 2441253.5004868642;
        double expected = 2441253.5;

        // Act
        double actual = JulianDateUtility.TerrestrialToUniversal(jdtt);
        Console.WriteLine(actual);

        // Assert
        Assert.AreEqual(expected, actual, _tolerance);
    }

    [TestMethod]
    public void TestTerrestrialToUniversalAndViceVersa()
    {
        for (int i = 0; i < 100000; i++)
        {
            // Get a random date in valid range for delta-T.
            Random rnd = new ();
            int year = rnd.Next(1, 3000);
            int month = rnd.Next(1, 12);
            int day = rnd.Next(1, DateTime.DaysInMonth(year, month));
            DateOnly d = new (year, month, day);

            // Convert from TT to UT and back again.
            double jdtt = JulianDateUtility.FromDateOnly(d);
            double jdut = JulianDateUtility.TerrestrialToUniversal(jdtt);
            double jdtt1 = JulianDateUtility.UniversalToTerrestrial(jdut);
            // Assert
            Assert.AreEqual(jdtt1, jdtt, _tolerance);

            // Convert from UT to TT and back again.
            jdut = JulianDateUtility.FromDateOnly(d);
            jdtt = JulianDateUtility.UniversalToTerrestrial(jdut);
            double jdut1 = JulianDateUtility.TerrestrialToUniversal(jdtt);
            // Assert
            Assert.AreEqual(jdut1, jdut, _tolerance);
        }
    }
}
