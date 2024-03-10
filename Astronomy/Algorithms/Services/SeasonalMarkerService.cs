using Galaxon.Astronomy.Algorithms.Utilities;
using Galaxon.Astronomy.Data.Enums;
using Galaxon.Numerics.Geometry;
using Galaxon.Time;

namespace Galaxon.Astronomy.Algorithms.Services;

public class SeasonalMarkerService(SunService sunService)
{
    /// <summary>
    /// High-accuracy method for calculating seasonal marker.
    /// The algorithm is from "Astronomical Algorithms, 2nd Ed." by Jean Meeus, Chapter 27
    /// "Equinoxes and Solstices" (p180).
    /// </summary>
    /// <param name="year">The year (-1000..3000)</param>
    /// <param name="markerNumber">The marker number (use enum)</param>
    /// <returns>The result in dynamical time.</returns>
    public DateTime CalcSeasonalMarker(int year, ESeasonalMarker markerNumber)
    {
        // Get the mean value as a Julian Date (TT).
        double JDTT = SeasonalMarkerUtility.CalcSeasonalMarkerMean(year, markerNumber);

        // Find the target Ls in radians (0, π/2, -π, or -π/2).
        double targetLs = Angles.WrapRadians((int)markerNumber * Angles.RADIANS_PER_QUADRANT);

        // Calculate max difference to get within 0.5 second.
        const double sunMovementRadiansPerHalfSecond =
            Angles.RADIANS_PER_CIRCLE / TimeConstants.SECONDS_PER_TROPICAL_YEAR / 2;

        // Loop until sufficient accuracy is achieved.
        do
        {
            // Get the longitude of the Sun in radians.
            (double Ls, double _, double _) = sunService.CalcPosition(JDTT);

            // Calculate the difference between the computed longitude of the Sun at this time, and
            // the target value.
            double diffLs = Angles.WrapRadians(targetLs - Ls);

            // Check if we're done.
            if (Abs(diffLs) < sunMovementRadiansPerHalfSecond)
            {
                break;
            }

            // Make a correction.
            JDTT += 58 * Sin(diffLs);
        }
        while (true);

        // Get the Julian Date in Universal Time.
        double JDUT = JulianDateUtility.JulianDate_TT_to_UT(JDTT);

        // Convert to DateTime.
        DateTime dt = JulianDateUtility.JulianDate_to_DateTime(JDUT);

        // Round off to nearest second.
        return DateTimeExtensions.RoundToNearestSecond(dt);
    }
}
