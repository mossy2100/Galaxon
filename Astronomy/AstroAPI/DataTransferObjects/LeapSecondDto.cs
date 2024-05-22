using Galaxon.Astronomy.Data.Models;
using Galaxon.Time.Extensions;

namespace Galaxon.Astronomy.AstroAPI.DataTransferObjects;

/// <summary>
/// Encapsulate a leap second for encoding as JSON.
/// </summary>
public record struct LeapSecondDto
{
    /// <summary>
    /// The date of the leap second as an ISO string (YYYY-MM-DD).
    /// </summary>
    public string Date { get; init; }

    /// <summary>
    /// The value of the leap second.
    /// </summary>
    public int Value { get; init; }

    /// <summary>
    /// Construct from internal type.
    /// </summary>
    public LeapSecondDto(LeapSecondRecord leapSecond)
    {
        Date = leapSecond.Date.ToIsoString();
        Value = leapSecond.Value;
    }
}
