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
            AstroObject? earth = astroObjectRepository.Load("Earth", "planet");
            _earth = earth
                ?? throw new DataNotFoundException("Could not find planet Earth in the database.");
        }

        return _earth;
    }

    /// <summary>
    /// Calculate the heliocentric position of Earth at a given point in time.
    /// </summary>
    /// <param name="JDTT">The Julian Date in Terrestrial Time.</param>
    /// <returns>Heliocentric coordinates of Earth.</returns>
    public Coordinates CalcPosition(double JDTT)
    {
        AstroObject earth = GetPlanet();
        return planetService.CalcPlanetPosition(earth, JDTT);
    }

    #endregion Instance methods

    #region Static methods

    /// <summary>
    /// Calculate the Earth Rotation Angle from the Julian Date in UT1.
    /// <see href="https://en.wikipedia.org/wiki/Sidereal_time#ERA"/>
    /// </summary>
    /// <param name="JD">The Julian Date in UT1.</param>
    /// <returns>The Earth Rotation Angles.</returns>
    public static double CalcEarthRotationAngle(double JD)
    {
        double t = JulianDateService.JulianDaysSinceJ2000(JD);
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
        double JD = JulianDateService.DateTime_to_JulianDate(dt);
        return CalcEarthRotationAngle(JD);
    }

    /// <summary>
    /// Calculate the mean tropical year length in ephemeris days at a point in time.
    /// The formula comes from:
    /// <see href="https://en.wikipedia.org/wiki/Tropical_year#Mean_tropical_year_current_value"/>
    /// </summary>
    /// <param name="T">The number of Julian centuries since noon, January 1, 2000.</param>
    /// <returns>The tropical year length in ephemeris days at that point in time.</returns>
    public static double CalcTropicalYearLength(double T)
    {
        return Polynomials.EvaluatePolynomial([365.242_189_6698, -6.15359e-6, -7.29e-10, 2.64e-10],
            T);
    }

    /// <summary>
    /// Calculate the approximate length of the solar day in SI seconds at a point in time.
    /// The formula comes from "The Length of the Day : A Cosmological Perspective" (Arbab I. Arbab, 2009)
    /// </summary>
    /// <param name="T">The number of Julian centuries since noon, January 1, 2000.</param>
    /// <returns>The day length in seconds at that point in time.</returns>
    public static double CalcSolarDayLength(double T)
    {
        // The length of the day increases by about 2ms/century.
        return TimeConstants.SECONDS_PER_DAY + 2e-3 * T;
    }

    #endregion Static methods
}
