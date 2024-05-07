using Galaxon.Core.Exceptions;

namespace Galaxon.Astronomy.Data.Models;

public class AtmosphereRecord : DatabaseRecord
{
    /// <summary>
    /// Default constructor.
    /// </summary>
    public AtmosphereRecord() { }

    /// <summary>
    /// Foreign key of containing Atmosphere object.
    /// </summary>
    public int AstroObjectId { get; set; }

    /// <summary>
    /// Reference to containing Atmosphere object.
    /// </summary>
    public virtual AstroObjectRecord? AstroObject { get; set; }

    /// <summary>
    /// Surface pressure (Pa).
    /// </summary>
    public double? SurfacePressure { get; set; }

    /// <summary>
    /// Scale height (km).
    /// </summary>
    public double? ScaleHeight { get; set; }

    /// <summary>
    /// Atmosphere constituents.
    /// </summary>
    public virtual List<AtmosphereConstituentRecord> Constituents { get; set; } = [];

    /// <summary>
    /// Is it a surface-bounded exosphere?
    /// </summary>
    public bool? IsSurfaceBoundedExosphere { get; set; }

    /// <summary>
    /// Add a constituent to the atmosphere.
    /// Does not save the Atmosphere record.
    /// </summary>
    /// <param name="db"></param>
    /// <param name="symbol">The molecule symbol.</param>
    /// <param name="percentage">The percentage of it in the atmosphere.</param>
    public void AddConstituent(AstroDbContext db, string symbol, double? percentage = null)
    {
        AtmosphereConstituentRecord? constituent =
            Constituents.FirstOrDefault(ac => ac.Molecule.Symbol == symbol);
        if (constituent == null)
        {
            // Get the molecule.
            // TODO maybe use Molecule.Load() here.
            MoleculeRecord? m = db.Molecules.FirstOrDefault(m => m.Symbol == symbol);
            if (m == null)
            {
                throw new DataNotFoundException($"Molecule '{symbol}' not found.");
            }

            // Create the new atmo constituent object.
            constituent = new AtmosphereConstituentRecord
            {
                Molecule = m,
                Percentage = percentage
            };

            // Add it.
            Constituents.Add(constituent);
        }
        else
        {
            // Update the constituent percentage.
            constituent.Percentage = percentage;
        }
    }
}
