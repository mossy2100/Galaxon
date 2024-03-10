using Galaxon.Time;

namespace Galaxon.Astronomy.Algorithms.Utilities;

public static class MarsUtility
{
    /// <summary>
    /// Calculate the Mars Sol Date for a given point in time, expressed as a Julian Date.
    /// </summary>
    /// <see href="https://en.wikipedia.org/wiki/Timekeeping_on_Mars#Mars_Sol_Date"/>
    /// <param name="JDTT">The Julian Date (TT).</param>
    /// <returns>The Mars Sol Date.</returns>
    public static double CalcMarsSolDate(double JDTT)
    {
        double JD_TAI = JulianDateUtility.JulianDate_TT_to_TAI(JDTT);
        const double k = 1.0 / 4000;
        double MSD = (JD_TAI - 2451549.5 + k) / TimeConstants.DAYS_PER_SOL + 44796.0;
        return MSD;
    }
}
