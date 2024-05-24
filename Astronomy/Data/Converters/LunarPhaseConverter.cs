using Galaxon.Astronomy.Data.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Galaxon.Astronomy.Data.Converters;

public class LunarPhaseConverter() : ValueConverter<ELunarPhaseType, int>(
    // Convert from enum to int.
    eLunarPhaseType => (int)eLunarPhaseType,

    // Convert from int to enum.
    iLunarPhaseType => (ELunarPhaseType)iLunarPhaseType
);
