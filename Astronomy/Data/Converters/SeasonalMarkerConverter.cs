using Galaxon.Astronomy.Data.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Galaxon.Astronomy.Data.Converters;

public class SeasonalMarkerConverter() : ValueConverter<ESeasonalMarkerType, string>(
    // Convert from enum to string.
    v => v.ToString(),

    // Convert from string to enum.
    v => (ESeasonalMarkerType)Enum.Parse(typeof(ESeasonalMarkerType), v)
);
