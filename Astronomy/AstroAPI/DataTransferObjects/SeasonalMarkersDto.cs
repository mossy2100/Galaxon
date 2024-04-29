using Galaxon.Astronomy.Algorithms.Records;
using Galaxon.Core.Types;
using Galaxon.Time;

namespace Galaxon.Astronomy.AstroAPI.DataTransferObjects;

public class SeasonalMarkersDto : Dictionary<string, string>
{
    public void Add(SeasonalMarkerEvent seasonalMarkerEvent)
    {
        string key = seasonalMarkerEvent.SeasonalMarker.GetJsonPropertyName();
        string value = DateTimeExtensions.RoundToNearestMinute(seasonalMarkerEvent.DateTimeUtc)
            .ToIsoString(true);
        Add(key, value);
    }
}
