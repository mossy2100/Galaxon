using Galaxon.Astronomy.Algorithms.Records;
using Galaxon.Astronomy.Data.Models;
using Galaxon.Astronomy.Data.Repositories;
using Galaxon.Core.Exceptions;

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
}
