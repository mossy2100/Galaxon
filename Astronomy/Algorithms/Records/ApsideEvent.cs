using Galaxon.Astronomy.Data.Enums;
using Galaxon.Astronomy.Data.Models;

namespace Galaxon.Astronomy.Algorithms.Records;

public record struct ApsideEvent(
    AstroObject Planet,
    int OrbitNumber,
    EApside Apside,
    double JulianDateTerrestrial,
    DateTime DateTimeUtc,
    double? RadiusInMetres = null,
    double? RadiusInAstronomicalUnits = null
);
