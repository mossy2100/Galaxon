using Galaxon.Astronomy.Data.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Galaxon.Astronomy.Data.Converters;

public class ApsideConverter() : ValueConverter<EApsideType, int>(
    // Convert from enum to int.
    eApsideType => (int)eApsideType,

    // Convert from int to enum.
    iApsideType => (EApsideType)iApsideType
);
