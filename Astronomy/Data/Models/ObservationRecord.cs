namespace Galaxon.Astronomy.Data.Models;

/// <summary>
/// Represents an observational record related to an astronomical object.
/// </summary>
public class ObservationRecord : DatabaseRecord
{
    /// <summary>
    /// Primary key of the astronomical object this component relates to.
    /// </summary>
    public int AstroObjectId { get; set; }

    /// <summary>
    /// Astronomical object this component relates to.
    /// </summary>
    public virtual AstroObjectRecord? AstroObject { get; set; }

    /// <summary>
    /// Absolute magnitude of the astronomical object.
    /// </summary>
    public double? AbsoluteMagnitude { get; set; }

    /// <summary>
    /// Minimum apparent magnitude observed for the astronomical object.
    /// </summary>
    public double? MinApparentMagnitude { get; set; }

    /// <summary>
    /// Maximum apparent magnitude observed for the astronomical object.
    /// </summary>
    public double? MaxApparentMagnitude { get; set; }

    /// <summary>
    /// Minimum angular diameter in degrees.
    /// </summary>
    public double? MinAngularDiameter_deg { get; set; }

    /// <summary>
    /// Maximum angular diameter in degrees.
    /// </summary>
    public double? MaxAngularDiameter_deg { get; set; }
}
