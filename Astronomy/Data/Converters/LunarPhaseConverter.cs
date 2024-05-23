using Galaxon.Astronomy.Data.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Galaxon.Astronomy.Data.Converters;

public class LunarPhaseConverter() : ValueConverter<ELunarPhaseType, string>(
    // Convert from enum to string.
    lunarPhaseType => lunarPhaseType.ToString(),

    // Convert from string to enum.
    strLunarPhaseType => (ELunarPhaseType)Enum.Parse(typeof(ELunarPhaseType), strLunarPhaseType)
);
