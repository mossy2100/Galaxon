using Galaxon.Astronomy.Algorithms.Records;
using Galaxon.Time;
using Microsoft.OpenApi.Extensions;

namespace Galaxon.Astronomy.AstroAPI.DataTransferObjects;

/// <summary>
/// Encapsulate a lunar phase for encoding as JSON.
/// </summary>
public record struct LunarPhaseDto
{
    /// <summary>
    /// Meeus Lunation Number.
    /// </summary>
    public int LunationNumber { get; init; }

    /// <summary>
    /// Number representing the phase.
    ///   0 = New Moon
    ///   1 = First Quarter
    ///   2 = Full Moon
    ///   3 = Third Quarter
    /// </summary>
    public int PhaseNumber { get; init; }

    /// <summary>
    /// String representing the phase.
    /// </summary>
    public string Phase { get; init; }

    /// <summary>
    /// The UTC datetime of the lunar phase.
    /// </summary>
    public string DateTime { get; init; }

    /// <summary>
    /// Construct from internal type.
    /// </summary>
    public LunarPhaseDto(LunarPhaseEvent lunarPhase)
    {
        LunationNumber = lunarPhase.LunationNumber;
        PhaseNumber = (int)lunarPhase.Type;
        Phase = lunarPhase.Type.GetDisplayName();
        // Round off to nearest minute.
        DateTime = DateTimeExtensions.Round(lunarPhase.DateTimeUtc, TimeSpan.FromMinutes(1)).ToIsoString();
    }
}
