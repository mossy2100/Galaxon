using Galaxon.Astronomy.Data.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Galaxon.Astronomy.Data.Converters;

public class SeasonalMarkerConverter() : ValueConverter<ESeasonalMarkerType, string>(
    // Convert from enum to string.
    seasonalMarkerType => seasonalMarkerType.ToString(),

    // Convert from string to enum.
    strSeasonalMarkerType =>
        (ESeasonalMarkerType)Enum.Parse(typeof(ESeasonalMarkerType), strSeasonalMarkerType)
);
