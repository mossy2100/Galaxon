using Galaxon.Astronomy.Data.Enums;

namespace Galaxon.Astronomy.Algorithms.Records;

public record struct LunarPhaseEvent
{
    /// <summary>
    /// The lunation number of the lunar phase event.
    /// </summary>
    public int LunationNumber { get; init; }

    /// <summary>
    /// The type of the lunar phase.
    /// </summary>
    public ELunarPhaseType PhaseType { get; init; }

    /// <summary>
    /// Calculate the unique lunar phase event number.
    /// </summary>
    public double PhaseNumber => LunationNumber + (int)PhaseType / 4.0;

    /// <summary>
    /// The Julian Date (Terrestrial Time) of the lunar phase event.
    /// </summary>
    public double JulianDateTerrestrial { get; init; }

    /// <summary>
    /// The UTC date and time of the lunar phase event.
    /// </summary>
    public DateTime DateTimeUtc { get; init; }

    /// <summary>
    /// Initializes a new instance of the LunarPhaseEvent record struct.
    /// </summary>
    /// <param name="lunationNumber">The lunation number of the lunar phase event.</param>
    /// <param name="phaseType">The type of the lunar phase.</param>
    /// <param name="julianDateTerrestrial">
    /// The Julian date for the terrestrial time of the lunar phase event.
    /// </param>
    /// <param name="dateTimeUtc">The UTC date and time of the lunar phase event.</param>
    public LunarPhaseEvent(int lunationNumber, ELunarPhaseType phaseType,
        double julianDateTerrestrial, DateTime dateTimeUtc)
    {
        LunationNumber = lunationNumber;
        PhaseType = phaseType;
        JulianDateTerrestrial = julianDateTerrestrial;
        DateTimeUtc = dateTimeUtc;
    }
}
