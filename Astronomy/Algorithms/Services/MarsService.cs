using Galaxon.Astronomy.Algorithms.Records;
using Galaxon.Astronomy.Data.Models;
using Galaxon.Astronomy.Data.Repositories;
using Galaxon.Core.Exceptions;
using Galaxon.Time;

namespace Galaxon.Astronomy.Algorithms.Services;

/// <summary>
/// A container for constants and static methods related to Mars.
/// </summary>
public class MarsService(AstroObjectRepository astroObjectRepository, PlanetService planetService)
{
    /// <summary>
    /// Cached reference to the AstroObject representing Mars.
    /// </summary>
    private AstroObject? _mars;

    #region Instance methods

    /// <summary>
    /// Get the AstroObject representing Mars.
    /// </summary>
    /// <exception cref="DataNotFoundException">
    /// If the object could not be loaded from the database.
    /// </exception>
    public AstroObject GetPlanet()
    {
        if (_mars == null)
        {
            AstroObject? mars = astroObjectRepository.Load("Mars", "Planet");
            _mars = mars
                ?? throw new DataNotFoundException("Could not find planet Mars in the database.");
        }

        return _mars;
    }

    /// <summary>
    /// Calculation position of Mars in heliocentric coordinates (radians).
    /// </summary>
    /// <param name="jdtt">The Julian Date (TT).</param>
    /// <returns></returns>
    public Coordinates CalcPosition(double jdtt)
    {
        AstroObject mars = GetPlanet();
        return planetService.CalcPlanetPosition(mars, jdtt);
    }

    #endregion Instance methods

    #region Static methods

    /// <summary>
    /// Calculate the Mars Sol Date for a given point in time, expressed as a Julian Date.
    /// </summary>
    /// <see href="https://en.wikipedia.org/wiki/Timekeeping_on_Mars#Mars_Sol_Date"/>
    /// <param name="jdtt">The Julian Date (TT).</param>
    /// <returns>The Mars Sol Date.</returns>
    public static double CalcMarsSolDate(double jdtt)
    {
        double JD_TAI = TimeScaleService.JulianDateTerrestrialToInternationalAtomic(jdtt);
        const double k = 1.0 / 4000;
        double MSD = (JD_TAI - 2451549.5 + k) / TimeConstants.DAYS_PER_SOL + 44796.0;
        return MSD;
    }

    #endregion Static methods
}
