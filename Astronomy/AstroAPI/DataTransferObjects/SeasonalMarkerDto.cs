using Galaxon.Astronomy.Algorithms.Records;
using Galaxon.Core.Types;
using Galaxon.Time;

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
    public string Type { get; init; }

    /// <summary>
    /// The UTC datetime of the seasonal marker, as a string.
    /// </summary>
    public string DateTimeUTC { get; init; }

    /// <summary>
    /// Construct from internal type.
    /// </summary>
    /// <param name="seasonalMarkerEvent"></param>
    public SeasonalMarkerDto(SeasonalMarkerEvent seasonalMarkerEvent)
    {
        Type = seasonalMarkerEvent.SeasonalMarker.GetDescription();
        // Round off to nearest minute.
        DateTimeUTC = DateTimeExtensions.Round(seasonalMarkerEvent.DateTimeUtc, TimeSpan.FromMinutes(1)).ToString("s");
    }
}
