using Galaxon.Astronomy.Data.Enums;

namespace Galaxon.Astronomy.Data.Models;

public class SeasonalMarkerRecord : DatabaseRecord
{
    /// <summary>
    /// The id of the astronomical object (planet, minor planet, or moon) for which the seasonal
    /// marker has been computed.
    /// </summary>
    public int AstroObjectId { get; set; }

    /// <summary>
    /// The tropical year number for the given planet.
    /// </summary>
    public int Year { get; set; }

    /// <summary>
    /// The seasonal marker type.
    /// </summary>
    [Column(TypeName = "varchar(20)")]
    public ESeasonalMarker Type { get; set; }

    /// <summary>
    /// The UTC datetime of the seasonal marker as calculated by my code.
    /// </summary>
    public DateTime? DateTimeUtc { get; set; }

    /// <summary>
    /// The UTC datetime of the seasonal marker as obtained from the USNO web service.
    /// <see href="https://aa.usno.navy.mil/data/Earth_Seasons"/>
    /// </summary>
    public DateTime? DateTimeUtcUsno { get; set; }
}
