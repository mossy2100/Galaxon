using Galaxon.Astronomy.Algorithms.Records;
using Galaxon.Core.Types;

namespace Galaxon.Astronomy.AstroAPI.DataTransferObjects;

public record struct SeasonalMarkerDto
{
    /// <summary>
    /// The seasonal marker type as a string.
    ///   0 = Northward Equinox
    ///   1 = Northern Solstice
    ///   2 = Southward Equinox
    ///   3 = Southern Solstice
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// The UTC datetime of the seasonal marker, as a string.
    /// </summary>
    public string DateTimeUTC { get; set; }

    /// <summary>
    /// Construct from internal type.
    /// </summary>
    /// <param name="seasonalMarker"></param>
    public SeasonalMarkerDto(SeasonalMarker seasonalMarker)
    {
        Type = seasonalMarker.Type.GetDescription();
        DateTimeUTC = $"{seasonalMarker.DateTimeUtc:s}";
    }
}
