using Galaxon.Astronomy.Algorithms.Records;
using Galaxon.Astronomy.Algorithms.Services;
using Galaxon.Numerics.Algebra;
using Galaxon.Time;

namespace Galaxon.ConsoleApp.Services;

public class BirthdayService(SunService sunService)
{
    public double CalcLongitudeOfSunAtBirth(DateTime dtBirth)
    {
        double jdtt = TimeScales.DateTimeUniversalToJulianDateTerrestrial(dtBirth);
        Coordinates coords = sunService.CalcPosition(jdtt);
        return coords.Longitude_rad;
    }

    /// <summary>
    /// Calculate someone's birth minute (as opposed to birthday) in the given year.
    /// This is defined as being when the longitude of the Sun (or, equivalently, the point in the
    /// tropical year) is the same as when the person was born.
    /// </summary>
    /// <param name="dtBirth">There moment of birth.</param>
    /// <param name="year">The year.</param>
    /// <returns>The birth minute of the given year.</returns>
    public DateTime CalcBirthMinute(DateTime dtBirth, int year)
    {
        // Calculate longitude of Sun at moment of birth.
        double LsBirth = CalcLongitudeOfSunAtBirth(dtBirth);

        // Calculate their approximate birth minute in the given year. We'll use noon as a
        // reasonable mean value for time of day. This is fine, because it's just an approximate
        // value to give us a starting point for the golden section search.
        DateTime dtBirthday = new (year, dtBirth.Month, dtBirth.Day, 12, 0, 0);

        // Convert to a Julian Date. Don't worry about delta-T; again, this is just an estimate.
        double jdBirthday = TimeScales.DateTimeToJulianDate(dtBirthday);

        // Construct a function to find the difference between the Ls at a certain datetime and the
        // difference from the Ls at birth.
        Func<double, double> func = jdtt =>
        {
            Coordinates coords = sunService.CalcPosition(jdtt);
            return Abs(coords.Longitude_rad - LsBirth);
        };

        // Find the input JD(TT) where the function is at a minimum.

        // We want a result accurate to within one minute. The tolerance relates to inputs; the
        // maximum difference between the result and the true answer (i.e. the input value that
        // produces the desired minimum).
        double tolerance = 1.0 / TimeConstants.MINUTES_PER_DAY;

        // For bounds, we'll assume the precise moment will be within ±2 days of the usual
        // birthday.
        // Testing reveals than ±1 day is too narrow, because the drift from the seasons due to the
        // Gregorian leap year pattern is greater than this.
        (double jdttBirthMinute, double diffFromLs) =
            GoldenSectionSearch.FindMinimum(func, jdBirthday - 2, jdBirthday + 2, tolerance);

        // Check the output is within range.
        // double radiansPerMinute = Double.Tau / TimeConstants.MINUTES_PER_YEAR;
        // if (diffFromLs > radiansPerMinute)
        // {
        //     Console.WriteLine();
        //     Console.WriteLine("ERROR: diff too large.");
        //     Console.WriteLine(year);
        //     Console.WriteLine($"Maximum diff in Ls is {radiansPerMinute} radians.");
        //     Console.WriteLine($"Diff from birthLs = {diffFromLs} radians.");
        // }

        // Convert back to a DateTime.
        return TimeScales.JulianDateTerrestrialToDateTimeUniversal(jdttBirthMinute);
    }
}
