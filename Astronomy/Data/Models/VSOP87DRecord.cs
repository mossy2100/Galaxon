namespace Galaxon.Astronomy.Data.Models;

/// <summary>
/// Represents a record containing VSOP87D planetary data.
/// </summary>
public class VSOP87DRecord : DatabaseRecord
{
    /// <summary>
    /// The planet number (1-8), called "code of body" in the VSOP87 documentation.
    /// </summary>
    public byte CodeOfBody { get; set; }

    /// <summary>
    /// The index of the coordinate:
    ///     1 = Longitude (L)
    ///     2 = Latitude (B)
    ///     3 = Radius (R)
    /// </summary>
    public byte IndexOfCoordinate { get; set; }

    /// <summary>
    /// The exponent (degree alpha) of time variable T.
    /// </summary>
    public byte Exponent { get; set; }

    /// <summary>
    /// Rank of the term in a series.
    /// </summary>
    public ushort Rank { get; set; }

    /// <summary>
    /// The amplitude, expressed in rad (for lat/long) or AU (for radius) per Julian millennium
    /// (called "tjy" in the documentation, for "thousands of Julian years").
    /// </summary>
    [Column(TypeName = "decimal(18, 11)")]
    public decimal Amplitude { get; set; }

    /// <summary>
    /// The phase (radians).
    /// </summary>
    [Column(TypeName = "decimal(14, 11)")]
    public decimal Phase { get; set; }

    /// <summary>
    /// The frequency (radians per Julian millennium).
    /// </summary>
    [Column(TypeName = "decimal(20, 11)")]
    public decimal Frequency { get; set; }
}
