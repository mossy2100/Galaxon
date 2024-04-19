﻿using System.Globalization;
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
        double t = TimeScaleService.JulianDaysSinceJ2000(jdut);
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
        double jdut = TimeScaleService.DateTimeToJulianDate(dt);
        return CalcEarthRotationAngle(jdut);
    }

    /// <summary>
    /// Calculate the mean tropical year length in ephemeris days at a point in time.
    /// The formula is valid for 8000 BCE to 12000 CE, and comes from:
    /// <see href="https://en.wikipedia.org/wiki/Tropical_year#Mean_tropical_year_current_value"/>
    /// </summary>
    /// <param name="T">The number of Julian centuries since noon, January 1, 2000 (TT).</param>
    /// <returns>The tropical year length in ephemeris days at that point in time.</returns>
    public static double CalcTropicalYearLengthInEphemerisDays(double T)
    {
        // To limit the valid range to 8000 BCE - 12000 CE, convert these value to Julian centuries
        // since 2000. This will be approximate, as we won't factor in delta-T, but then delta-T is
        // small and these limits are approximate anyway.
        const int lowerT = -100;
        const int upperT = 100;
        if (T is < lowerT or > upperT)
        {
            throw new ArgumentOutOfRangeException(nameof(T),
                $"Must be in the range {lowerT}..{upperT}.");
        }

        return Polynomials.EvaluatePolynomial([365.242_189_6698, -6.15359e-6, -7.29e-10, 2.64e-10],
            T);
    }

    /// <summary>
    /// Calculate the mean tropical year length in ephemeris days for a given year.
    /// The formula is valid for 8000 BCE to 12000 CE.
    /// The year can have a fractional part.
    /// </summary>
    /// <param name="year">The year as a decimal.</param>
    /// <returns>The tropical year length in ephemeris days at that point in time.</returns>
    public static double GetTropicalYearLengthInEphemerisDays(double year)
    {
        // Check year is in valid range.
        if (year is < -7999 or > 12000)
        {
            throw new ArgumentOutOfRangeException(nameof(year),
                "Must be in the range -7999..12000.");
        }

        // Calculate T, the number of Julian centuries since noon, January 1, 2000 (TT).
        double jdut = TimeScaleService.DecimalYearToJulianDateUniversal(year);
        double jdtt = TimeScaleService.JulianDateUniversalToTerrestrial(jdut);
        double T = TimeScaleService.JulianCenturiesSinceJ2000(jdtt);

        // Call the method that takes T as a parameter.
        return CalcTropicalYearLengthInEphemerisDays(T);
    }

    /// <summary>
    /// Calculate the approximate length of the solar day in SI seconds at a point in time.
    ///
    /// The formula comes from https://en.wikipedia.org/wiki/%CE%94T_(timekeeping)#Universal_time
    ///
    /// It is similar to:
    /// McCarthy, Dennis D.; Seidelmann, P. Kenneth. "Time: From Earth Rotation to Atomic Physics",
    /// Section 4.5: "Current Understanding of the Earth’s Variable Rotation".
    /// </summary>
    /// <see href="https://www.cnmoc.usff.navy.mil/Our-Commands/United-States-Naval-Observatory/Precise-Time-Department/Global-Positioning-System/USNO-GPS-Time-Transfer/Leap-Seconds"/>
    /// <param name="y">The year as a decimal.</param>
    /// <returns>The day length in seconds at that point in time.</returns>
    public static double GetSolarDayLength(double y)
    {
        // The length of the day has been increasing by about 1.7 ms/d/cy.
        // According to the above link at cnmoc.usff.navy.mil, the solar day was equal to exactly
        // 86,400 seconds in approximately 1820.
        return TimeConstants.SECONDS_PER_DAY + 1.7e-5 * (y - 1820);
    }

    /// <summary>
    /// Get the approximate length of a given day, in seconds.
    /// </summary>
    /// <param name="d">The date of the day.</param>
    /// <returns>The length of that day in seconds.</returns>
    public static double GetSolarDayLength(DateOnly d)
    {
        GregorianCalendar gc = new ();
        int daysInYear = gc.GetDaysInYear(d.Year);
        double frac = (d.Day - 0.5) / daysInYear;
        return GetSolarDayLength(d.Year + frac);
    }

    /// <summary>
    /// Calculate the mean tropical year length in solar days for a given calendar year.
    /// </summary>
    /// <param name="y">The year as a decimal.</param>
    /// <returns>The tropical year length in solar days at that point in time.</returns>
    public static double GetTropicalYearLengthInSolarDays(double y)
    {
        return GetTropicalYearLengthInEphemerisDays(y)
            * TimeConstants.SECONDS_PER_DAY
            / GetSolarDayLength(y);
    }

    #endregion Static methods
}
