using Galaxon.Astronomy.Data.Enums;
using Galaxon.Astronomy.Data.Models;

namespace Galaxon.Astronomy.Algorithms.Records;

public record struct ApsideEvent(
    AstroObjectRecord Planet,
    int Orbit,
    EApsideType Type,
    double JulianDateTerrestrial,
    DateTime DateTimeUtc,
    double? Radius_m = null,
    double? Radius_AU = null
);
