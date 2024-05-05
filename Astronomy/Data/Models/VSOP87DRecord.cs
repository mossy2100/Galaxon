namespace Galaxon.Astronomy.Data.Models;

/// <summary>
/// Represents a record containing VSOP87D planetary data.
/// </summary>
public class VSOP87DRecord : DataObject
{
    /// <summary>
    /// Gets or sets the link to the astronomical object associated with this record.
    /// </summary>
    public virtual int AstroObjectId { get; set; }

    /// <summary>
    /// Gets or sets the reference to the astronomical object associated with this record.
    /// </summary>
    public virtual AstroObject? AstroObject { get; set; }

    /// <summary>
    /// Gets or sets the variable used in the record.
    /// </summary>
    [Column(TypeName = "CHAR(1)")]
    public char Variable { get; set; }

    /// <summary>
    /// Gets or sets the exponent used in the record.
    /// </summary>
    [Column(TypeName = "TINYINT")]
    public byte Exponent { get; set; }

    /// <summary>
    /// Gets or sets the index used in the record.
    /// </summary>
    [Column(TypeName = "SMALLINT")]
    public ushort Index { get; set; }

    /// <summary>
    /// Gets or sets the amplitude value in the record.
    /// </summary>
    public double Amplitude { get; set; }

    /// <summary>
    /// Gets or sets the phase value in the record.
    /// </summary>
    public double Phase { get; set; }

    /// <summary>
    /// Gets or sets the frequency value in the record.
    /// </summary>
    public double Frequency { get; set; }
}
