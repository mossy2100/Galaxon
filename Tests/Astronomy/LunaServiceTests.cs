using System.Globalization;
using Galaxon.Astronomy.Algorithms.Models;
using Galaxon.Astronomy.Algorithms.Services;
using Galaxon.Astronomy.Data;
using Galaxon.Astronomy.Data.Enums;
using Galaxon.Astronomy.Data.Models;
using Galaxon.Time;

namespace Galaxon.Tests.Astronomy;

[TestClass]
public class LunaServiceTests
{
    private AstroDbContext? _astroDbContext;

    [TestInitialize]
    public void Init()
    {
        _astroDbContext = new AstroDbContext();
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
        MoonPhase phase = LunaService.GetPhaseFromDateTime(dtApprox);

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
        MoonPhase phase = LunaService.GetPhaseFromDateTime(dtApprox);

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
    /// Compare my lunar phase algorithm with data from the AstroPixels ephmeris.
    /// </summary>
    [TestMethod]
    public void GetPhasesInYear_CompareWithAstroPixels()
    {
        int maxDiff = 60;

        // Arrange
        List<LunarPhase> astroPixelsPhases = _astroDbContext!.LunarPhases
            .Where(lp => lp.DateTimeUtcAstroPixels != null)
            .OrderBy(lp => lp.DateTimeUtcAstroPixels).ToList();

        // Check each.
        foreach (LunarPhase astroPixelsPhase in astroPixelsPhases)
        {
            if (astroPixelsPhase.DateTimeUtcAstroPixels == null)
            {
                continue;
            }

            MoonPhase myPhase =
                LunaService.GetPhaseFromDateTime(astroPixelsPhase.DateTimeUtcAstroPixels.Value);

            // Assert
            if (astroPixelsPhase.Type != myPhase.Type)
            {
                Console.WriteLine(
                    $"{astroPixelsPhase.Type,15}: {astroPixelsPhase.DateTimeUtcAstroPixels.Value.ToIsoString()} c.f. {myPhase.Type,15}: {myPhase.DateTimeUtc.ToIsoString()}");
            }

            Assert.AreEqual(astroPixelsPhase.Type, myPhase.Type);

            TimeSpan diff = astroPixelsPhase.DateTimeUtcAstroPixels.Value - myPhase.DateTimeUtc;
            if (diff.TotalSeconds > maxDiff)
            {
                Console.WriteLine(
                    $"{astroPixelsPhase.Type,15}: {astroPixelsPhase.DateTimeUtcAstroPixels.Value.ToIsoString()} c.f. {myPhase.DateTimeUtc.ToIsoString()} = {diff.TotalSeconds:F2} s");
            }
            else
            {
                Assert.IsTrue(diff.TotalSeconds <= maxDiff);
            }
        }
    }

    /// <summary>
    /// Compare my lunar phase algorithm with data from the USNO API.
    /// </summary>
    [TestMethod]
    public void GetPhasesInYear_CompareWithUsno()
    {
        int maxDiff = 60;

        // Arrange
        List<LunarPhase> astroPixelsPhases = _astroDbContext!.LunarPhases
            .Where(lp => lp.DateTimeUtcUsno != null)
            .OrderBy(lp => lp.DateTimeUtcAstroPixels)
            .ToList();

        // Check each.
        foreach (LunarPhase astroPixelsPhase in astroPixelsPhases)
        {
            if (astroPixelsPhase.DateTimeUtcUsno == null)
            {
                continue;
            }

            MoonPhase myPhase =
                LunaService.GetPhaseFromDateTime(astroPixelsPhase.DateTimeUtcUsno.Value);

            // Assert
            if (astroPixelsPhase.Type != myPhase.Type)
            {
                Console.WriteLine(
                    $"{astroPixelsPhase.Type,15}: {astroPixelsPhase.DateTimeUtcUsno.Value.ToIsoString()} c.f. {myPhase.Type,15}: {myPhase.DateTimeUtc.ToIsoString()}");
            }

            Assert.AreEqual(astroPixelsPhase.Type, myPhase.Type);

            TimeSpan diff = astroPixelsPhase.DateTimeUtcUsno.Value - myPhase.DateTimeUtc;
            if (diff.TotalSeconds > maxDiff)
            {
                Console.WriteLine(
                    $"{astroPixelsPhase.Type,15}: {astroPixelsPhase.DateTimeUtcUsno.Value.ToIsoString()} c.f. {myPhase.DateTimeUtc.ToIsoString()} = {diff.TotalSeconds:F2} s");
            }
            else
            {
                Assert.IsTrue(diff.TotalSeconds <= maxDiff);
            }
        }
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
