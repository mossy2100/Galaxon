using Galaxon.Astronomy.Algorithms.Utilities;
using Galaxon.Astronomy.Data.Enums;

namespace Galaxon.Astronomy.Algorithms.Services;

public class SeasonalMarkerService(SunService sunService)
{
    /// <summary>
    /// Higher-accuracy method for calculating seasonal marker.
    /// Algorithm is from AA2 p180.
    /// </summary>
    /// <param name="year">The year (-1000..3000)</param>
    /// <param name="markerNumber">The marker number (use enum)</param>
    /// <returns>The result in dynamical time.</returns>
    public DateTime CalcSeasonalMarker(int year, ESeasonalMarker markerNumber)
    {
        double JD = SeasonalMarkerUtility.CalcSeasonalMarkerMean(year, markerNumber);
        double k = (int)markerNumber;
        double targetLs = k * PI / 2;
        const double delta = 1E-9;
        do
        {
            (double Ls, double Bs, double Rs) = sunService.CalcPosition(JD);
            double diffLs = targetLs - Ls;

            // Check if we're done.
            if (Abs(diffLs) < delta)
            {
                break;
            }

            double correction = 58 * Sin(diffLs);
            JD += correction;
        }
        while (true);

        return JulianDateUtility.JulianDate_to_DateTime(JD);
    }
}
