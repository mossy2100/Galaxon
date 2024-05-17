using Galaxon.Astronomy.Data.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Galaxon.Astronomy.Data.Converters;

public class ApsideConverter() : ValueConverter<EApsideType, char>(
    // Convert from enum to string.
    apsideType => apsideType.ToString()[0],

    // Convert from string to enum.
    chApsideType => Enum.GetValues(typeof(EApsideType)).Cast<EApsideType>()
        .FirstOrDefault(apsideType => apsideType.ToString()[0] == chApsideType)
);
