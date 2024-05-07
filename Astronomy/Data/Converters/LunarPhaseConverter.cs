using Galaxon.Astronomy.Data.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Galaxon.Astronomy.Data.Converters;

public class LunarPhaseConverter() : ValueConverter<ELunarPhase, string>(
    // Convert from ELunarPhase enum to string.
    v => v.ToString(),

    // Convert from string back to ELunarPhase enum.
    v => (ELunarPhase)Enum.Parse(typeof(ELunarPhase), v)
);
