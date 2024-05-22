using Galaxon.Astronomy.Algorithms.Utilities;
using Galaxon.Numerics.Algebra;
using Galaxon.Time;

namespace Galaxon.Astronomy.Algorithms.Services;

/// <summary>
/// This service contains useful methods and constants relating to Earth.
/// </summary>
public class EarthService
{
    /// <summary>
    /// Calculate the Earth Rotation Angle from the Julian Date in UT1.
    /// <see href="https://en.wikipedia.org/wiki/Sidereal_time#ERA"/>
    /// </summary>
    /// <param name="jdut">The Julian Date in UT1.</param>
    /// <returns>The Earth Rotation </returns>
    public static double CalcEarthRotationAngle(double jdut)
    {
        double t = JulianDateUtility.JulianDaysSinceJ2000(jdut);
        double radians = Tau * (0.779_057_273_264 + 1.002_737_811_911_354_48 * t);
        return WrapRadians(radians);
    }

    /// <summary>
    /// Calculate the Earth Rotation Angle from a UTC DateTime.
    /// </summary>
    /// <param name="dt">The instant.</param>
    /// <returns>The ERA at the given instant.</returns>
    public static double CalcEarthRotationAngle(DateTime dt)
    {
        double jdut = JulianDateUtility.DateTimeToJulianDate(dt);
        return CalcEarthRotationAngle(jdut);
    }

    /// <summary>
    /// Calculate the mean tropical year length in ephemeris days at a point in time.
    /// The formula is valid for 8000 BCE to 12000 CE, and comes from:
    /// <see href="https://en.wikipedia.org/wiki/Tropical_year#Mean_tropical_year_current_value"/>
    /// </summary>
    /// <param name="T">The number of Julian centuries since noon, January 1, 2000 (TT).</param>
    /// <returns>The tropical year length in ephemeris days at that point in time.</returns>
    public static double GetTropicalYearInEphemerisDays(double T)
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
    public static double GetTropicalYearInEphemerisDaysForYear(double year)
    {
        // Check year is in valid range.
        if (year is < -7999 or > 12000)
        {
            throw new ArgumentOutOfRangeException(nameof(year),
                "Must be in the range -7999..12000.");
        }

        // Calculate T, the number of Julian centuries since noon, January 1, 2000 (TT).
        double jdtt = JulianDateUtility.DecimalYearToJulianDate(year);
        double T = JulianDateUtility.JulianCenturiesSinceJ2000(jdtt);

        // Call the method that takes T as a parameter.
        return GetTropicalYearInEphemerisDays(T);
    }

    /// <summary>
    /// Calculate the approximate length of the solar day in SI seconds at a point in time specified
    /// by a decimal year.
    ///
    /// The formula comes from
    /// McCarthy, Dennis D.; Seidelmann, P. Kenneth. "Time: From Earth Rotation to Atomic Physics",
    /// Section 4.5: "Current Understanding of the Earth’s Variable Rotation".
    ///
    /// <seealso href="https://en.wikipedia.org/wiki/%CE%94T_(timekeeping)#Universal_time"/>
    /// </summary>
    /// <see href="https://www.cnmoc.usff.navy.mil/Our-Commands/United-States-Naval-Observatory/Precise-Time-Department/Global-Positioning-System/USNO-GPS-Time-Transfer/Leap-Seconds"/>
    /// <param name="year">The year as a decimal.</param>
    /// <returns>The approximate length of the solar day in SI seconds at that time.</returns>
    public static double GetSolarDayInSeconds(double year)
    {
        // The length of the day is increasing by about 1.62 ms/d/cy.
        // This value for the rate of change is more precise than the value given in Wikipedia of
        // 1.7 ms/d/cy, and more likely to be correct, or at least peer-reviewed.
        // According to the above link at cnmoc.usff.navy.mil, the solar day was equal to exactly
        // 86,400 seconds in approximately 1820, which is why the first-order delta-T formula is
        // centred on 1820.
        return TimeConstants.SECONDS_PER_DAY + 1.62e-3 * (year - 1820) / 100;
    }

    /// <summary>
    /// Calculate the mean tropical year length in solar days for a given calendar year.
    /// </summary>
    /// <param name="year">The year as a decimal.</param>
    /// <returns>The tropical year length in solar days at that point in time.</returns>
    public static double GetTropicalYearInSolarDaysForYear(double year)
    {
        return GetTropicalYearInEphemerisDaysForYear(year)
            * TimeConstants.SECONDS_PER_DAY
            / GetSolarDayInSeconds(year);
    }
}
