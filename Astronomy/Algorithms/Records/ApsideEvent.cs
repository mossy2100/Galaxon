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
    /// Gets or sets the apside number.
    /// </summary>
    public double ApsideNumber { get; set; }

    /// <summary>
    /// The type of apside.
    /// </summary>
    public readonly EApsideType ApsideType =>
        double.IsInteger(ApsideNumber) ? EApsideType.Periapsis : EApsideType.Apoapsis;

    /// <summary>
    /// Gets or sets the terrestrial Julian date of this apside event.
    /// </summary>
    public double JulianDateTerrestrial { get; set; }

    /// <summary>
    /// Gets or sets the UTC datetime of this apside event.
    /// </summary>
    public DateTime DateTimeUtc { get; set; }

    /// <summary>
    /// Gets or sets the radius of the orbit at the apside event in metres (optional).
    /// </summary>
    public double? Radius_m { get; set; }

    /// <summary>
    /// Gets or sets the radius of the orbit at the apside event in astronomical units (optional).
    /// </summary>
    public double? Radius_AU { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApsideEvent"/> struct.
    /// </summary>
    /// <param name="planet">The celestial object involved in this apside event.</param>
    /// <param name="orbit">The orbit number where the apside event occurs.</param>
    /// <param name="type">The type of the apside event (e.g., perihelion or aphelion).</param>
    /// <param name="julianDateTerrestrial">
    /// The Julian Date (Terrestrial Time) of this apside event.
    /// </param>
    /// <param name="dateTimeUtc">The UTC datetime of this apside event.</param>
    /// <param name="radius_m">
    /// The radius of the orbit (i.e. distance from planet to Sun) at the apside event in metres
    /// (optional).
    /// </param>
    /// <param name="radius_AU">
    /// The radius of the orbit (i.e. distance from planet to Sun) at the apside event in
    /// astronomical units (AU) (optional).</param>
    public ApsideEvent(
        AstroObjectRecord planet,
        double apsideNumber,
        double julianDateTerrestrial,
        DateTime dateTimeUtc,
        double? radius_m = null,
        double? radius_AU = null)
    {
        Planet = planet;
        ApsideNumber = apsideNumber;
        JulianDateTerrestrial = julianDateTerrestrial;
        DateTimeUtc = dateTimeUtc;
        Radius_m = radius_m;
        Radius_AU = radius_AU;
    }
}
