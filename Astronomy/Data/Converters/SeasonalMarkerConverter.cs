using Galaxon.Astronomy.Data.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Galaxon.Astronomy.Data.Converters;

public class SeasonalMarkerConverter() : ValueConverter<ESeasonalMarker, string>(
    // Convert from ESeasonalMarker enum to string.
    v => v.ToString(),

    // Convert from string back to ESeasonalMarker enum.
    v => (ESeasonalMarker)Enum.Parse(typeof(ESeasonalMarker), v)
);
