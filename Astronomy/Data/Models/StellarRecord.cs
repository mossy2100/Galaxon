using System.ComponentModel.DataAnnotations;

namespace Galaxon.Astronomy.Data.Models;

/// <summary>
/// Represents information about a stellar object.
/// </summary>
public class StellarRecord : DatabaseRecord
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
    /// Spectral classification of the star.
    /// </summary>
    [MaxLength(5)]
    public string? SpectralClass { get; set; }

    /// <summary>
    /// Metallicity of the star.
    /// </summary>
    public double? Metallicity { get; set; }

    /// <summary>
    /// luminosity of the star in watts (W).
    /// </summary>
    public double? Luminosity_W { get; set; }

    /// <summary>
    /// Mean radiance of the star in watts per steradian per square meter (W/sr/m²).
    /// </summary>
    public double? Radiance_W_sr_m2 { get; set; }
}
