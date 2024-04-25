using System.Globalization;
using Galaxon.Astronomy.Algorithms.Records;
using Galaxon.Astronomy.Algorithms.Services;
using Galaxon.Astronomy.Data;
using Galaxon.Astronomy.Data.Enums;
using Galaxon.Astronomy.Data.Models;
using Galaxon.Core.Types;
using Galaxon.Time;

namespace Galaxon.Tests.Astronomy;

[TestClass]
public class MoonServiceTests
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

    /// <summary>
    /// Test example 49a from Astronomical Algorithms 2nd ed.
    /// </summary>
    [TestMethod]
    public void TestExample49a()
    {
        // Arrange
        DateTime dtApprox = new (1977, 2, 15);

        // Act
        MoonPhase phase = MoonService.GetPhaseNearDateTime(dtApprox);

        // Assert
        Assert.AreEqual(ELunarPhaseType.NewMoon, phase.Type);
        Assert.AreEqual(1977, phase.DateTimeUtc.Year);
        Assert.AreEqual(2, phase.DateTimeUtc.Month);
        Assert.AreEqual(18, phase.DateTimeUtc.Day);
        Assert.AreEqual(3, phase.DateTimeUtc.Hour);
        Assert.AreEqual(36, phase.DateTimeUtc.Minute);
    }

    /// <summary>
    /// Test example 49b from Astronomical Algorithms 2nd ed.
    /// </summary>
    [TestMethod]
    public void TestExample49b()
    {
        // Arrange
        DateTime dtApprox = new (2044, 1, 20);

        // Act
        MoonPhase phase = MoonService.GetPhaseNearDateTime(dtApprox);

        // Assert
        Assert.AreEqual(ELunarPhaseType.ThirdQuarter, phase.Type);
        Assert.AreEqual(2044, phase.DateTimeUtc.Year);
        Assert.AreEqual(1, phase.DateTimeUtc.Month);
        Assert.AreEqual(21, phase.DateTimeUtc.Day);
        Assert.AreEqual(23, phase.DateTimeUtc.Hour);
        // This differs from the example by 2 minutes, because I'm using NASA's formulae for
        // calculating deltaT instead of Meeus's.
        Assert.AreEqual(46, phase.DateTimeUtc.Minute);
    }

    /// <summary>
    /// Compare my lunar phase algorithm with data from the AstroPixels ephemeris.
    /// </summary>
    [TestMethod]
    public void GetPhasesInYear_CompareWithAstroPixels()
    {
        // Arrange
        AstroDbContext astroDbContext = ServiceManager.GetService<AstroDbContext>();
        List<LunarPhase> phasesFromDb = astroDbContext.LunarPhases
            .Where(lp => lp.DateTimeUtcAstroPixels != null)
            .OrderBy(lp => lp.DateTimeUtcAstroPixels).ToList();
        const double maxDiffSeconds = 320;
        double maxDiffSecondsFound = 0;

        // Check each.
        foreach (LunarPhase phaseFromDb in phasesFromDb)
        {
            // Act.
            MoonPhase phaseFromMethod =
                MoonService.GetPhaseNearDateTime(phaseFromDb.DateTimeUtcAstroPixels!.Value);

            // Report on different type.
            if (phaseFromDb.Type != phaseFromMethod.Type)
            {
                Console.WriteLine(
                    $"{phaseFromDb.Type,15}: {phaseFromDb.DateTimeUtcAstroPixels.Value.ToIsoString()} c.f. {phaseFromMethod.Type,15}: {phaseFromMethod.DateTimeUtc.ToIsoString()}");
            }

            // Calculate the difference in seconds between their and my calculations.
            TimeSpan diff = phaseFromDb.DateTimeUtcAstroPixels.Value - phaseFromMethod.DateTimeUtc;
            // if (diff.TotalSeconds > maxDiffSeconds)
            // {
            //     Console.WriteLine(
            //         $"{phaseFromDb.Type,15}: {phaseFromDb.DateTimeUtcAstroPixels.Value.ToIsoString()} c.f. {phaseFromMethod.DateTimeUtc.ToIsoString()} = {diff.TotalSeconds:F2} s");
            // }
            if (diff.TotalSeconds > maxDiffSecondsFound)
            {
                maxDiffSecondsFound = diff.TotalSeconds;
            }

            // Assert.
            Assert.AreEqual(phaseFromDb.Type, phaseFromMethod.Type);
            Assert.IsTrue(diff.TotalSeconds <= maxDiffSeconds);
        }

        Console.WriteLine($"Maximum difference found was {maxDiffSecondsFound:F2} seconds.");
    }

    /// <summary>
    /// Compare my lunar phase algorithm with data from the USNO API.
    /// </summary>
    [TestMethod]
    public void GetPhasesInYear_CompareWithUsno()
    {
        // Arrange
        AstroDbContext astroDbContext = ServiceManager.GetService<AstroDbContext>();
        List<LunarPhase> phasesFromDb = astroDbContext.LunarPhases
            .Where(lp => lp.DateTimeUtcUsno != null)
            .OrderBy(lp => lp.DateTimeUtcUsno)
            .ToList();
        const double maxDiffSeconds = 121;
        double maxDiffSecondsFound = 0;

        // Check each.
        foreach (LunarPhase phaseFromDb in phasesFromDb)
        {
            // Act.
            MoonPhase phaseFromMethod =
                MoonService.GetPhaseNearDateTime(phaseFromDb.DateTimeUtcUsno!.Value);

            // Report on different type.
            if (phaseFromDb.Type != phaseFromMethod.Type)
            {
                Console.WriteLine(
                    $"{phaseFromDb.Type.GetDescriptionOrName(),15}: {phaseFromDb.DateTimeUtcUsno.Value.ToIsoString()} c.f. {phaseFromMethod.Type,15}: {phaseFromMethod.DateTimeUtc.ToIsoString()}");
            }

            // Calculate the difference in seconds between their and my calculations.
            TimeSpan diff = phaseFromDb.DateTimeUtcUsno.Value - phaseFromMethod.DateTimeUtc;
            if (diff.TotalSeconds > maxDiffSeconds)
            {
                Console.WriteLine(
                    $"{phaseFromDb.Type,15}: {phaseFromDb.DateTimeUtcUsno.Value.ToIsoString()} c.f. {phaseFromMethod.DateTimeUtc.ToIsoString()} = {diff.TotalSeconds:F1} seconds");
            }
            if (diff.TotalSeconds > maxDiffSecondsFound)
            {
                maxDiffSecondsFound = diff.TotalSeconds;
            }

            // Assert.
            Assert.AreEqual(phaseFromDb.Type, phaseFromMethod.Type);
            Assert.IsTrue(diff.TotalSeconds <= maxDiffSeconds);
        }

        Console.WriteLine($"Maximum difference found was {maxDiffSecondsFound:F2} seconds.");
    }

    /// <summary>
    /// Parse a datetime string from the AstroPixels website into a DateTime object.
    /// </summary>
    /// <param name="sDateTime">The datetime as a string, without year.</param>
    /// <param name="year">The year.</param>
    /// <returns></returns>
    private DateTime ParseDateTime(string sDateTime, int year)
    {
        // Get the month, convert to int.
        string monthAbbrev = sDateTime[..3];
        DateTime parsedDate = DateTime.ParseExact(monthAbbrev, "MMM", CultureInfo.InvariantCulture);
        int month = parsedDate.Month;

        // Parse the day, hour, and minute.
        int day = int.Parse(sDateTime[4..6].TrimStart());
        int hour = int.Parse(sDateTime[8..10]);
        int minute = int.Parse(sDateTime[11..13]);

        return new DateTime(year, month, day, hour, minute, 0, DateTimeKind.Utc);
    }
}
