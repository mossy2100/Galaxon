using Galaxon.Astronomy.Algorithms.Records;
using Galaxon.Astronomy.Data.Models;
using Galaxon.Astronomy.Data.Repositories;
using Galaxon.Core.Exceptions;
using Galaxon.Numerics.Algebra;
using Galaxon.Numerics.Geometry;
using Galaxon.Time;

namespace Galaxon.Astronomy.Algorithms.Services;

/// <summary>
/// This service contains useful methods and constants relating to Earth.
/// </summary>
public class EarthService(AstroObjectRepository astroObjectRepository, PlanetService planetService)
{
    /// <summary>
    /// Cached reference to the AstroObject representing Earth.
    /// </summary>
    private AstroObject? _earth;

    #region Instance methods

    /// <summary>
    /// Get the AstroObject representing Earth.
    /// </summary>
    /// <exception cref="DataNotFoundException">
    /// If the object could not be loaded from the database.
    /// </exception>
    public AstroObject GetPlanet()
    {
        if (_earth == null)
        {
            AstroObject? earth = astroObjectRepository.Load("Earth", "Planet");
            _earth = earth
                ?? throw new DataNotFoundException("Could not find planet Earth in the database.");
        }

        return _earth;
    }

    /// <summary>
    /// Calculate the heliocentric position of Earth at a given point in time.
    /// </summary>
    /// <param name="jdtt">The Julian Date in Terrestrial Time.</param>
    /// <returns>Heliocentric coordinates of Earth.</returns>
    public Coordinates CalcPosition(double jdtt)
    {
        AstroObject earth = GetPlanet();
        return planetService.CalcPlanetPosition(earth, jdtt);
    }

    #endregion Instance methods

    #region Static methods

    /// <summary>
    /// Calculate the Earth Rotation Angle from the Julian Date in UT1.
    /// <see href="https://en.wikipedia.org/wiki/Sidereal_time#ERA"/>
    /// </summary>
    /// <param name="jdut">The Julian Date in UT1.</param>
    /// <returns>The Earth Rotation Angles.</returns>
    public static double CalcEarthRotationAngle(double jdut)
    {
        double t = JulianDateService.JulianDaysSinceJ2000(jdut);
        double radians = Tau * (0.779_057_273_264 + 1.002_737_811_911_354_48 * t);
        return Angles.WrapRadians(radians);
    }

    /// <summary>
    /// Calculate the Earth Rotation Angle from a UTC DateTime.
    /// </summary>
    /// <param name="dt">The instant.</param>
    /// <returns>The ERA at the given instant.</returns>
    public static double CalcEarthRotationAngle(DateTime dt)
    {
        double jdut = JulianDateService.DateTimeToJulianDate(dt);
        return CalcEarthRotationAngle(jdut);
    }

    /// <summary>
    /// Calculate the mean tropical year length in ephemeris days at a point in time.
    /// The formula comes from:
    /// <see href="https://en.wikipedia.org/wiki/Tropical_year#Mean_tropical_year_current_value"/>
    /// </summary>
    /// <param name="T">The number of Julian centuries since noon, January 1, 2000 (TT).</param>
    /// <returns>The tropical year length in ephemeris days at that point in time.</returns>
    public static double GetTropicalYearLengthInEphemerisDays(double T)
    {
        return Polynomials.EvaluatePolynomial([365.242_189_6698, -6.15359e-6, -7.29e-10, 2.64e-10],
            T);
    }

    /// <summary>
    /// Calculate the approximate length of the solar day in SI seconds at a point in time.
    /// The formula comes from https://en.wikipedia.org/wiki/%CE%94T_(timekeeping)#Universal_time
    /// This is similar to:
    /// McCarthy, Dennis D.; Seidelmann, P. Kenneth. "Time: From Earth Rotation to Atomic Physics",
    /// Section 4.5: "Current Understanding of the Earth’s Variable Rotation".
    /// </summary>
    /// <param name="T">The number of Julian centuries since noon, January 1, 2000.</param>
    /// <returns>The day length in seconds at that point in time.</returns>
    public static double GetSolarDayLengthInSeconds(double T)
    {
        // The length of the day has been increasing by about 1.62±0.21 ms/day/cy since 1820.
        return TimeConstants.SECONDS_PER_DAY + 1.7e-3 * (T + 1.80);
    }

    /// <summary>
    /// Calculate the mean tropical year length in solar days at a point in time.
    /// </summary>
    /// <param name="T">The number of Julian centuries since noon, January 1, 2000 (TT).</param>
    /// <returns>The tropical year length in solar days at that point in time.</returns>
    public static double GetTropicalYearLengthInSolarDays(double T)
    {
        return GetTropicalYearLengthInEphemerisDays(T)
            * TimeConstants.SECONDS_PER_DAY
            / GetSolarDayLengthInSeconds(T);
    }

    #endregion Static methods
}
