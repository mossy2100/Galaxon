using Galaxon.Astronomy.Data.Models;
using Galaxon.Astronomy.Data.Repositories;
using Galaxon.Core.Exceptions;

namespace Galaxon.Astronomy.Algorithms.Services;

/// <summary>
/// Calling this class LunaService as there was some earlier confusion with the word "moon"
/// referring to "the Moon" as well as natural satellites.
/// To avoid confusion the code now refers to the Moon as "Luna" and natural satellites as
/// "satellites".
/// </summary>
/// <param name="astroObjectRepository"></param>
public class LunaService(AstroObjectRepository astroObjectRepository)
{
    /// <summary>
    /// Cached reference to the AstroObject representing Luna.
    /// </summary>
    private AstroObject? _luna;

    /// <summary>
    /// Get the AstroObject representing Luna.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="DataNotFoundException"></exception>
    public AstroObject GetPlanet()
    {
        if (_luna == null)
        {
            AstroObject? luna = astroObjectRepository.Load("Luna", "planet");
            _luna = luna
                ?? throw new DataNotFoundException(
                    "Could not find the Moon (Luna) in the database.");
        }

        return _luna;
    }
}
