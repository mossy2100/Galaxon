using System.Globalization;
using Galaxon.Astronomy.Data.Models;

namespace Galaxon.Astronomy.AstroAPI.DataTransferObjects;

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
    public LeapSecondDto(LeapSecond leapSecond)
    {
        Date = leapSecond.LeapSecondDate.ToString("O", CultureInfo.InvariantCulture);
        Value = leapSecond.Value;
    }
}
