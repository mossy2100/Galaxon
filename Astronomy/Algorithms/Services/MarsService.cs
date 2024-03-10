using Galaxon.Astronomy.Data.Models;
using Galaxon.Astronomy.Data.Repositories;
using Galaxon.Core.Exceptions;

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
            AstroObject? mars = astroObjectRepository.Load("Mars", "planet");
            _mars = mars
                ?? throw new DataNotFoundException("Could not find planet Mars in the database.");
        }

        return _mars;
    }

    /// <summary>
    /// Calculation position of Mars in heliocentric coordinates (radians).
    /// </summary>
    /// <param name="JD_TT">The Julian Date (TT).</param>
    /// <returns></returns>
    public Coordinates CalcPosition(double JD_TT)
    {
        AstroObject mars = GetPlanet();
        return planetService.CalcPlanetPosition(mars, JD_TT);
    }
}
