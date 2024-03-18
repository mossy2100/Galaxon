namespace Galaxon.Astronomy.Data.Models;

/// <summary>
/// Represents a constituent of the atmosphere.
/// </summary>
public class AtmosphereConstituent : Entity
{
    /// <summary>
    /// Gets or sets the foreign key referencing the parent AtmosphereRecord object.
    /// </summary>
    public virtual int AtmosphereId { get; set; }

    /// <summary>
    /// Gets or sets the reference to the parent AtmosphereRecord object.
    /// </summary>
    public virtual AtmosphereRecord Atmosphere { get; set; } = new ();

    /// <summary>
    /// Gets or sets the link to the gas molecule.
    /// </summary>
    public virtual int MoleculeId { get; set; }

    /// <summary>
    /// Gets or sets the reference to the gas molecule.
    /// </summary>
    public virtual Molecule Molecule { get; set; } = new ();

    /// <summary>
    /// Gets or sets the percentage of the gas in the atmosphere by volume.
    /// </summary>
    /// <remarks>
    /// This property is nullable as the exact percentage is not always known.
    /// </remarks>
    public double? Percentage { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AtmosphereConstituent"/> class.
    /// </summary>
    public AtmosphereConstituent() { }
}
