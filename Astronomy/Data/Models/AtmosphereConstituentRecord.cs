namespace Galaxon.Astronomy.Data.Models;

/// <summary>
/// Represents a constituent of the atmosphere.
/// </summary>
public class AtmosphereConstituentRecord : DatabaseRecord
{
    /// <summary>
    /// Foreign key referencing the parent AtmosphereRecord object.
    /// </summary>
    public int AtmosphereId { get; set; }

    /// <summary>
    /// Reference to the parent AtmosphereRecord object.
    /// </summary>
    public virtual AtmosphereRecord Atmosphere { get; set; } = new ();

    /// <summary>
    /// Foreign key of the gas molecule.
    /// </summary>
    public int MoleculeId { get; set; }

    /// <summary>
    /// Reference to the gas molecule.
    /// </summary>
    public virtual MoleculeRecord Molecule { get; set; } = new ();

    /// <summary>
    /// Percentage of the gas in the atmosphere by volume.
    /// </summary>
    /// <remarks>
    /// This property is nullable as the exact percentage is not always known.
    /// </remarks>
    public double? Percentage { get; set; }
}
