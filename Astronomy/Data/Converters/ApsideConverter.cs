using Galaxon.Astronomy.Data.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Galaxon.Astronomy.Data.Converters;

public class ApsideConverter() : ValueConverter<EApsideType, string>(
    // Convert from enum to string.
    v => v.ToString(),

    // Convert from string to enum.
    v => (EApsideType)Enum.Parse(typeof(EApsideType), v)
);
