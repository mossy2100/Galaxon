using Galaxon.Astronomy.Data.Enums;
using Galaxon.Astronomy.Data.Models;

namespace Galaxon.Astronomy.Algorithms.Records;

/// <summary>
/// Represents an event of a celestial object reaching an apside (either perihelion or aphelion).
/// </summary>
public struct ApsideEvent
{
    /// <summary>
    /// Gets or sets the celestial object related to this apside event.
    /// </summary>
    public AstroObjectRecord Planet { get; set; }

    /// <summary>
    /// The orbit number. Negative values are pre-J2000.
    /// </summary>
    public int OrbitNumber { get; set; }

    /// <summary>
    /// The type of apside.
    /// </summary>
    public EApsideType ApsideType { get; set; }

    /// <summary>
    /// The Julian Date (Terrestrial Time) of this apside event.
    /// </summary>
    public double JulianDateTerrestrial { get; set; }

    /// <summary>
    /// The UTC datetime of this apside event.
    /// </summary>
    public DateTime DateTimeUtc { get; set; }

    /// <summary>
    /// The radius of the orbit at the apside event in astronomical units (optional).
    /// </summary>
    public double? Radius_AU { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApsideEvent"/> struct.
    /// </summary>
    /// <param name="planet">The celestial object involved in this apside event.</param>
    /// <param name="orbitNumber">The orbit number where the apside event occurs.</param>
    /// <param name="apsideType">The type of the apside event (e.g. perihelion or aphelion).</param>
    /// <param name="julianDateTerrestrial">
    /// The Julian Date (Terrestrial Time) of this apside event.
    /// </param>
    /// <param name="dateTimeUtc">The UTC datetime of this apside event.</param>
    /// <param name="radius_AU">
    /// The radius of the orbit (i.e. distance from planet to Sun) at the apside event in
    /// astronomical units (AU) (optional).
    /// </param>
    public ApsideEvent(
        AstroObjectRecord planet,
        int orbitNumber,
        EApsideType apsideType,
        double julianDateTerrestrial,
        DateTime dateTimeUtc,
        double? radius_AU = null)
    {
        Planet = planet;
        OrbitNumber = orbitNumber;
        ApsideType = apsideType;
        JulianDateTerrestrial = julianDateTerrestrial;
        DateTimeUtc = dateTimeUtc;
        Radius_AU = radius_AU;
    }
}
