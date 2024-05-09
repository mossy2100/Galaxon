using Galaxon.Astronomy.Data.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Galaxon.Astronomy.Data.Converters;

public class LunarPhaseConverter() : ValueConverter<ELunarPhaseType, string>(
    // Convert from enum to string.
    v => v.ToString(),

    // Convert from string to enum.
    v => (ELunarPhaseType)Enum.Parse(typeof(ELunarPhaseType), v)
);
