using Galaxon.Astronomy.Data.Models;

namespace Galaxon.Astronomy.Data.Repositories;

public class LeapSecondRepository(AstroDbContext astroDbContext)
{
    /// <summary>
    /// Cache of the leap seconds so we don't have to keep loading them if they're needed more than
    /// once during a program.
    /// </summary>
    private List<LeapSecond>? _list;

    /// <summary>
    /// Get all leap seconds inserted so far, in date order.
    /// </summary>
    public List<LeapSecond> List =>
        _list ??= astroDbContext.LeapSeconds.OrderBy(ls => ls.LeapSecondDate).ToList();
}
