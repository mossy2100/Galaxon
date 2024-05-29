using System.ComponentModel.DataAnnotations;

namespace Galaxon.Astronomy.Data.Models;

// Main class for astronomical objects.
// All physical quantities are in SI units.
public class AstroObjectRecord : DatabaseRecord
{
    public AstroObjectRecord(string? name = null, uint? number = null)
    {
        Name = name;
        Number = number;
    }

    // ---------------------------------------------------------------------------------------------
    // Core data

    /// <summary>
    /// Object name, e.g. "Sun", "Earth", "Ceres".
    /// These are not assumed to be unique, and in some cases (e.g. minor planets) can be null.
    /// </summary>
    [MaxLength(20)]
    public string? Name { get; set; }

    /// <summary>
    /// Object number, e.g. 2 (Venus), 1 (Luna), 1 (Ceres).
    /// These numbers also aren't unique, and in some cases (stars, major planets) can be null.
    /// </summary>
    public uint? Number { get; set; }

    /// <summary>
    /// URL of the Wikipedia page for this object.
    /// </summary>
    [Column(TypeName = "varchar(100)")]
    public string? WikipediaUrl { get; set; }

    // ---------------------------------------------------------------------------------------------
    // Relationships

    /// <summary>
    /// Foreign key reference to parent object (i.e. the object being orbited).
    /// </summary>
    public int? ParentId { get; set; }

    /// <summary>
    /// Parent object (i.e. the object being orbited).
    /// </summary>
    public virtual AstroObjectRecord? Parent { get; set; }

    /// <summary>
    /// Child objects (i.e. the objects orbiting this object).
    /// </summary>
    public virtual List<AstroObjectRecord>? Children { get; set; }

    /// <summary>
    /// Groups (i.e. populations, categories, families) this object belongs to.
    /// </summary>
    public virtual List<AstroObjectGroupRecord>? Groups { get; set; }

    // ---------------------------------------------------------------------------------------------
    // Additional components

    /// <summary>
    /// Orbital properties.
    /// </summary>
    public virtual OrbitRecord? Orbit { get; set; }

    /// <summary>
    /// Physical properties (size, shape, mass, density, etc.)
    /// </summary>
    public virtual PhysicalRecord? Physical { get; set; }

    /// <summary>
    /// Rotational properties.
    /// </summary>
    public virtual RotationRecord? Rotation { get; set; }

    /// <summary>
    /// Observational properties.
    /// </summary>
    public virtual ObservationRecord? Observation { get; set; }

    /// <summary>
    /// Object's atmosphere.
    /// </summary>
    public virtual AtmosphereRecord? Atmosphere { get; set; }

    /// <summary>
    /// Stellar properties.
    /// </summary>
    public virtual StellarRecord? Stellar { get; set; }

    // Link to Minor Planet Center record.
    // public MinorPlanetRecord? MinorPlanet { get; set; }
}
