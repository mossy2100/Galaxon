using Galaxon.Time;

namespace Galaxon.Astronomy.Algorithms.Utilities;

/// <summary>
/// A container for constants and static methods related to Mars.
/// </summary>
public static class MarsUtility
{
    /// <summary>
    /// Calculate the Mars Sol Date for a given point in time, expressed as a Julian Date.
    /// See: <see href="https://en.wikipedia.org/wiki/Timekeeping_on_Mars#Mars_Sol_Date"/>
    /// </summary>
    /// <param name="jdtt">The Julian Date (TT).</param>
    /// <returns>The Mars Sol Date.</returns>
    public static double CalcMarsSolDate(double jdtt)
    {
        double jdtai = JulianDateUtility.JulianDateTerrestrialToInternationalAtomic(jdtt);
        const double k = 1.0 / 4000;
        double MSD = (jdtai - 2451549.5 + k) / TimeConstants.DAYS_PER_SOL + 44796.0;
        return MSD;
    }
}
