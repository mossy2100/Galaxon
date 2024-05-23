using Galaxon.Astronomy.Data.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Galaxon.Astronomy.Data.Converters;

public class ApsideConverter() : ValueConverter<EApsideType, string>(
    // Convert from enum to string.
    apsideType => apsideType.ToString(),

    // Convert from string to enum.
    strApsideType => (EApsideType)Enum.Parse(typeof(EApsideType), strApsideType)
);
