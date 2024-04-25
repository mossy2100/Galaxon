using Galaxon.Astronomy.Algorithms.Records;
using Galaxon.Core.Types;

namespace Galaxon.Astronomy.AstroAPI.DataTransferObjects;

public record struct LunarPhaseDto
{
    /// <summary>
    /// Meeus Lunation Number.
    /// </summary>
    public int LunationNumber { get; set; }

    /// <summary>
    /// String representing the lunar phase type.
    ///   0 = New Moon
    ///   1 = First Quarter
    ///   2 = Full Moon
    ///   3 = Third Quarter
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// The UTC datetime of the lunar phase.
    /// </summary>
    public string DateTimeUTC { get; set; }

    /// <summary>
    /// Construct from internal type.
    /// </summary>
    public LunarPhaseDto(LunarPhase lunarPhase)
    {
        LunationNumber = lunarPhase.LunationNumber;
        Type = lunarPhase.Type.GetDescription();
        DateTimeUTC = $"{lunarPhase.DateTimeUtc:s}";
    }
}
