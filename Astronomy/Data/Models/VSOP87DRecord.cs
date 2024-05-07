﻿namespace Galaxon.Astronomy.Data.Models;

/// <summary>
/// Represents a record containing VSOP87D planetary data.
/// </summary>
public class VSOP87DRecord : DatabaseRecord
{
    /// <summary>
    /// Gets or sets the link to the astronomical object associated with this record.
    /// </summary>
    public int AstroObjectId { get; set; }

    /// <summary>
    /// Gets or sets the reference to the astronomical object associated with this record.
    /// </summary>
    public virtual AstroObjectRecord? AstroObject { get; set; }

    /// <summary>
    /// Gets or sets the variable used in the record.
    /// </summary>
    public char Variable { get; set; }

    /// <summary>
    /// Gets or sets the exponent used in the record.
    /// </summary>
    public byte Exponent { get; set; }

    /// <summary>
    /// Gets or sets the index used in the record.
    /// </summary>
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
