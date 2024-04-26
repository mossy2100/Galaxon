using Galaxon.Astronomy.Algorithms.Records;
using Galaxon.Time;
using Microsoft.OpenApi.Extensions;

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
    public string Marker { get; init; }

    /// <summary>
    /// The UTC datetime of the seasonal marker, as a string.
    /// </summary>
    public string DateTimeUtc { get; init; }

    /// <summary>
    /// Construct from internal type.
    /// </summary>
    /// <param name="seasonalMarkerEvent"></param>
    public SeasonalMarkerDto(SeasonalMarkerEvent seasonalMarkerEvent)
    {
        Marker = seasonalMarkerEvent.SeasonalMarker.GetDisplayName();
        // Round off to nearest minute.
        DateTimeUtc = DateTimeExtensions.Round(seasonalMarkerEvent.DateTimeUtc, TimeSpan.FromMinutes(1)).ToString("s");
    }
}
