using Galaxon.Astronomy.Data.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Galaxon.Astronomy.Data.Converters;

public class SeasonalMarkerConverter() : ValueConverter<ESeasonalMarkerType, int>(
    // Convert from enum to int.
    eSeasonalMarkerType => (int)eSeasonalMarkerType,

    // Convert from int to enum.
    iSeasonalMarkerType => (ESeasonalMarkerType)iSeasonalMarkerType
);
