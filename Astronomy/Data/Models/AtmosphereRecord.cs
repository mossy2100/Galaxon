using Galaxon.Core.Exceptions;

namespace Galaxon.Astronomy.Data.Models;

public class AtmosphereRecord : DatabaseRecord
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
    /// Surface pressure in pascals (Pa).
    /// </summary>
    public double? SurfacePressure_Pa { get; set; }

    /// <summary>
    /// Scale height in kilometres (km).
    /// </summary>
    public double? ScaleHeight_km { get; set; }

    /// <summary>
    /// Atmosphere constituents.
    /// </summary>
    public virtual List<AtmosphereConstituentRecord> Constituents { get; set; } = [];

    /// <summary>
    /// If the atmosphere is only a surface-bounded exosphere.
    /// </summary>
    public bool? IsSurfaceBoundedExosphere { get; set; }

    /// <summary>
    /// Add a constituent to the atmosphere.
    /// NB: This method does not save the Atmosphere record.
    /// </summary>
    /// <param name="db"></param>
    /// <param name="symbol">The molecule symbol.</param>
    /// <param name="percentage">The percentage of it in the atmosphere.</param>
    public void AddConstituent(AstroDbContext db, string symbol, double? percentage = null)
    {
        // Get the constituent from the database.
        AtmosphereConstituentRecord? constituent =
            Constituents.FirstOrDefault(ac => ac.Molecule.Symbol == symbol);

        if (constituent == null)
        {
            // Get the molecule from the database.
            MoleculeRecord? m = db.Molecules.FirstOrDefault(m => m.Symbol == symbol);
            if (m == null)
            {
                throw new DataNotFoundException($"Molecule '{symbol}' not found.");
            }

            // Create a new record.
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
