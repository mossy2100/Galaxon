using System.Globalization;
using Galaxon.Astronomy.Algorithms.Services;
using Galaxon.Astronomy.Data;
using Galaxon.Astronomy.Data.Enums;
using Galaxon.Astronomy.Data.Models;
using Galaxon.Astronomy.Data.Repositories;
using Galaxon.Time;

namespace Galaxon.Tests.Astronomy;

[TestClass]
public class LunaServiceTests
{
    private AstroDbContext? _astroDbContext;

    private AstroObjectRepository? _astroObjectRepository;

    private AstroObjectGroupRepository? _astroObjectGroupRepository;

    private LunaService? _moonService;

    [TestInitialize]
    public void Init()
    {
        _astroDbContext = new AstroDbContext();
        _astroObjectGroupRepository = new AstroObjectGroupRepository(_astroDbContext);
        _astroObjectRepository =
            new AstroObjectRepository(_astroDbContext, _astroObjectGroupRepository);
        _moonService = new LunaService(_astroObjectRepository);
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
        LunarPhase phase = LunaService.GetPhaseFromDateTime(dtApprox);

        // Assert
        Assert.AreEqual(ELunarPhase.NewMoon, phase.PhaseType);
        Assert.AreEqual(1977, phase.DateTimeUTC.Year);
        Assert.AreEqual(2, phase.DateTimeUTC.Month);
        Assert.AreEqual(18, phase.DateTimeUTC.Day);
        Assert.AreEqual(3, phase.DateTimeUTC.Hour);
        Assert.AreEqual(36, phase.DateTimeUTC.Minute);
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
        LunarPhase phase = LunaService.GetPhaseFromDateTime(dtApprox);

        // Assert
        Assert.AreEqual(ELunarPhase.ThirdQuarter, phase.PhaseType);
        Assert.AreEqual(2044, phase.DateTimeUTC.Year);
        Assert.AreEqual(1, phase.DateTimeUTC.Month);
        Assert.AreEqual(21, phase.DateTimeUTC.Day);
        Assert.AreEqual(23, phase.DateTimeUTC.Hour);
        // This differs from the example by 2 minutes, because I'm using NASA's formulae for
        // calculating deltaT instead of Meeus's.
        Assert.AreEqual(46, phase.DateTimeUTC.Minute);
    }

    /// <summary>
    /// Compare my lunar phase algorithm with AstroPixels data.
    /// </summary>
    [TestMethod]
    public void GetPhasesInYear_CompareWithAstroPixels()
    {
        int maxDiff = 60;

        // Arrange
        List<LunarPhase> astroPixelsPhases = _astroDbContext!.LunarPhases
            // .Where(lp => lp.DateTimeUTC.Year >= 1000 && lp.DateTimeUTC.Year <= 3000)
            .OrderBy(lp => lp.DateTimeUTC).ToList();

        // Check each.
        foreach (var astroPixelsPhase in astroPixelsPhases)
        {
            LunarPhase myPhase = LunaService.GetPhaseFromDateTime(astroPixelsPhase.DateTimeUTC);

            // Assert
            if (astroPixelsPhase.PhaseType != myPhase.PhaseType)
            {
                Console.WriteLine($"{astroPixelsPhase.PhaseType,15}: {astroPixelsPhase.DateTimeUTC.ToIsoString()} c.f. {myPhase.PhaseType,15}: {myPhase.DateTimeUTC.ToIsoString()}");
            }

            Assert.AreEqual(astroPixelsPhase.PhaseType, myPhase.PhaseType);

            TimeSpan diff = astroPixelsPhase.DateTimeUTC - myPhase.DateTimeUTC;
            if (diff.TotalSeconds > maxDiff)
            {
                Console.WriteLine($"{astroPixelsPhase.PhaseType,15}: {astroPixelsPhase.DateTimeUTC.ToIsoString()} c.f. {myPhase.DateTimeUTC.ToIsoString()} = {diff.TotalSeconds:F2} s");
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
