using Galaxon.Astronomy.Data.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Galaxon.Astronomy.Data.Converters;

public class ApsideConverter() : ValueConverter<EApside, string>(
    // Convert from EApside enum to string.
    v => v.ToString(),

    // Convert from string back to EApside enum.
    v => (EApside)Enum.Parse(typeof(EApside), v)
);
