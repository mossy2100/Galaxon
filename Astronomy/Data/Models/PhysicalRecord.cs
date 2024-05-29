namespace Galaxon.Astronomy.Data.Models;

public class PhysicalRecord : DatabaseRecord
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
    /// If the object is ellipsoidal, i.e. gravitationally rounded (meaning, in hydrostatic
    /// equilibrium).
    /// </summary>
    public bool? IsRound { get; set; }

    /// <summary>
    /// The equatorial radius in kilometres (km).
    /// </summary>
    public double? EquatorialRadius_km { get; set; }

    /// <summary>
    /// The second equatorial radius in kilometres (km).
    /// Only relevant for triaxial ellipsoids or non-ellipsoids.
    /// </summary>
    public double? EquatorialRadius2_km { get; set; }

    /// <summary>
    /// The polar radius in kilometres (km).
    /// </summary>
    public double? PolarRadius_km { get; set; }

    /// <summary>
    /// The mean radius in kilometres (km).
    /// </summary>
    public double? MeanRadius_km { get; set; }

    /// <summary>
    /// The flattening. Only relevant for spheroidal objects.
    /// </summary>
    public double? Flattening { get; set; }

    /// <summary>
    /// Surface area in square kilometres (km^2).
    /// </summary>
    public double? SurfaceArea_km2 { get; set; }

    /// <summary>
    /// Volume in cubic kilometres (km^3).
    /// </summary>
    public double? Volume_km3 { get; set; }

    /// <summary>
    /// Mass in kilograms (kg).
    /// </summary>
    public double? Mass_kg { get; set; }

    /// <summary>
    /// Mean density in kilograms per cubic metre (kg/m^3).
    /// </summary>
    public double? Density_kg_m3 { get; set; }

    /// <summary>
    /// Surface gravity in metres per second squared (m/s^2).
    /// </summary>
    public double? SurfaceGravity_m_s2 { get; set; }

    /// <summary>
    /// Escape velocity in km/s.
    /// </summary>
    public double? EscapeVelocity_km_s { get; set; }

    /// <summary>
    /// Standard gravitational parameter in m^3/s^2.
    /// </summary>
    public double? StandardGravitationalParameter_m3_s2 { get; set; }

    /// <summary>
    /// If the object has a global magnetic field.
    /// </summary>
    public bool? HasGlobalMagneticField { get; set; }

    /// <summary>
    /// If the object has rings.
    /// </summary>
    public bool? HasRings { get; set; }

    /// <summary>
    /// Geometric albedo.
    /// </summary>
    public double? GeometricAlbedo { get; set; }

    /// <summary>
    /// Minimum surface temperature (K) (at the 0.1 bar altitude for giant planets).
    /// </summary>
    public double? MinSurfaceTemperature_K { get; set; }

    /// <summary>
    /// Mean surface temperature (K) (at the 0.1 bar altitude for giant planets).
    /// </summary>
    public double? MeanSurfaceTemperature_K { get; set; }

    /// <summary>
    /// Maximum surface temperature (K) (at the 0.1 bar altitude for giant planets).
    /// </summary>
    public double? MaxSurfaceTemperature_K { get; set; }

    public double? SurfaceEquivalentDoseRate_microSv_h { get; set; }

    /// <summary>
    /// Specify the object's size and shape.
    /// This can be used for any object. The other methods are for convenience.
    /// </summary>
    /// <seealso cref="SetSphericalShape"/>
    /// <seealso cref="SetSpheroidalShape"/>
    /// <seealso cref="SetEllipsoidalShape"/>
    /// <seealso cref="SetNonEllipsoidShape"/>
    /// <param name="radiusA">The first radius (or half length).</param>
    /// <param name="radiusB">The second radius (or half width).</param>
    /// <param name="radiusC">The third radius (or half height).</param>
    /// <param name="isRound">
    /// This flag should be:
    ///   - true for round objects (stars, planets, dwarf planets, satellite planets)
    ///   - false for lumpy objects (small bodies, satellite planetoids)
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">If any of the radii are 0 or negative.</exception>
    public void SetSizeAndShape(double radiusA, double radiusB, double radiusC, bool isRound)
    {
        if (radiusA <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(radiusA), "Must be a positive value.");
        }
        if (radiusB <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(radiusB), "Must be a positive value.");
        }
        if (radiusC <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(radiusC), "Must be a positive value.");
        }

        EquatorialRadius_km = radiusA;
        EquatorialRadius2_km = radiusB;
        PolarRadius_km = radiusC;
        IsRound = isRound;
    }

    /// <summary>
    /// Specify the object is a sphere.
    /// </summary>
    /// <param name="radius">The radius.</param>
    public void SetSphericalShape(double radius)
    {
        SetEllipsoidalShape(radius, radius, radius);
    }

    /// <summary>
    /// Specify the object is a spheroid.
    /// </summary>
    /// <param name="radiusEquat">The equatorial radius in km.</param>
    /// <param name="radiusPolar">The polar radius in km.</param>
    public void SetSpheroidalShape(double radiusEquat, double radiusPolar)
    {
        SetEllipsoidalShape(radiusEquat, radiusEquat, radiusPolar);
    }

    /// <summary>
    /// Specify the object as a scalene ellipsoid.
    /// </summary>
    /// <param name="radiusA">The first radius in km.</param>
    /// <param name="radiusB">The second radius in km.</param>
    /// <param name="radiusC">The third radius in km.</param>
    public void SetEllipsoidalShape(double radiusA, double radiusB, double radiusC)
    {
        SetSizeAndShape(radiusA, radiusB, radiusC, true);

        // Calculate some stuff; they can still set the property directly if
        // they want to override this.
        Ellipsoid ellipsoid = new (radiusA, radiusB, radiusC);

        // Volumetric mean radius.
        MeanRadius_km = ellipsoid.VolumetricMeanRadius;

        // Surface area.
        SurfaceArea_km2 = ellipsoid.SurfaceArea;

        // Volume.
        Volume_km3 = ellipsoid.Volume;
    }

    /// <summary>
    /// Specify the object's shape as irregular (not round).
    /// </summary>
    /// <param name="length">The length in km.</param>
    /// <param name="width">The width in km.</param>
    /// <param name="height">The height in km.</param>
    public void SetNonEllipsoidShape(double length, double width, double height)
    {
        SetSizeAndShape(length / 2, width / 2, height / 2, false);
    }
}
