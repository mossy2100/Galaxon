using Galaxon.Astronomy.Algorithms.Records;
using Galaxon.Core.Types;
using Galaxon.Time.Extensions;

namespace Galaxon.Astronomy.AstroAPI.DataTransferObjects;

/// <summary>
/// Represents a collection of seasonal markers with key and value pairs.
/// Inherits from the Dictionary class with a key of type string and a value of type string.
/// </summary>
public class SeasonalMarkersDto : Dictionary<string, string>
{
    /// <summary>
    /// Adds a new seasonal marker to the dictionary.
    /// The key is the JsonPropertyName of the seasonal marker from the `SeasonalMarkerEvent`,
    /// e.g. "northwardEquinox".
    /// The value is the DateTimeUtc from the `SeasonalMarkerEvent` rounded to the nearest minute
    /// and converted to an ISO string.
    /// </summary>
    /// <param name="seasonalMarkerEvent">The seasonal marker event to be added.</param>
    public void Add(SeasonalMarkerEvent seasonalMarkerEvent)
    {
        string key = seasonalMarkerEvent.SeasonalMarkerType.GetJsonPropertyName();
        string value = seasonalMarkerEvent.DateTimeUtc.RoundToNearestMinute().ToIsoString();
        Add(key, value);
    }
}
