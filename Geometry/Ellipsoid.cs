using Galaxon.Core.Numbers;

namespace Galaxon.Numerics.Geometry;

public class Ellipsoid : IShape3D
{
    #region Constructor

    public Ellipsoid(double radiusA, double radiusB, double radiusC)
    {
        RadiusA = radiusA;
        RadiusB = radiusB;
        RadiusC = radiusC;
    }

    #endregion Constructor

    #region Radii

    private double _radiusA;

    public double RadiusA
    {
        get => _radiusA;

        set
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(RadiusA), "Must be positive.");
            }
            _radiusA = value;
        }
    }

    private double _radiusB;

    public double RadiusB
    {
        get => _radiusB;

        set
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(RadiusB), "Must be positive.");
            }
            _radiusB = value;
        }
    }

    private double _radiusC;

    public double RadiusC
    {
        get => _radiusC;

        set
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(RadiusC), "Must be positive.");
            }
            _radiusC = value;
        }
    }

    /// <summary>
    /// Return the ellipse radii sorted by length.
    /// </summary>
    /// <returns>The array of sorted radii.</returns>
    public double[] SortedRadii()
    {
        double[] r = [RadiusA, RadiusB, RadiusC];
        Array.Sort(r);
        return r;
    }

    #endregion Radii

    #region Ellipsoid Types

    public bool IsSphere()
    {
        return RadiusA.FuzzyEquals(RadiusB) && RadiusB.FuzzyEquals(RadiusC);
    }

    public bool IsOblateSpheroid()
    {
        double[] r = SortedRadii();
        return r[0] < r[1] && r[1].FuzzyEquals(r[2]);
    }

    public bool IsProlateSpheroid()
    {
        double[] r = SortedRadii();
        return r[0].FuzzyEquals(r[1]) && r[1] < r[2];
    }

    public bool IsSpheroid()
    {
        double[] r = SortedRadii();
        return r[0].FuzzyEquals(r[1]) || r[1].FuzzyEquals(r[2]);
    }

    public bool IsScalene()
    {
        double[] r = SortedRadii();
        return r[0] < r[1] && r[1] < r[2];
    }

    #endregion Ellipsoid Types

    #region Calculated properties

    /// <summary>
    /// Get the surface area in square metres.
    /// </summary>
    /// <see href="https://en.wikipedia.org/wiki/Ellipsoid#Surface_area"/>
    /// <see href="https://www.web-formulas.com/Math_Formulas/Geometry_Surface_of_Ellipsoid.aspx"/>
    /// <returns>The surface area.</returns>
    public virtual double SurfaceArea
    {
        get
        {
            // Check for simple cases to get a quicker answer.
            // Calculate intermediate variables as needed.
            double[] r = SortedRadii();
            double a = r[2];
            double b = r[1];
            double c = r[0];

            double a2 = a * a;

            if (IsSphere())
            {
                // Formula: https://en.wikipedia.org/wiki/Sphere#Surface_area
                return 4 * PI * a2;
            }

            double c2 = c * c;
            double e2 = 1 - c2 / a2;
            double e = Sqrt(e2);

            if (IsOblateSpheroid())
            {
                // Formula: https://en.wikipedia.org/wiki/Ellipsoid#Surface_area
                return Tau * a2 * (1 + (1 - e2) / e * Atanh(e));
            }

            double phi = Asin(e);

            if (IsProlateSpheroid())
            {
                // Formula: https://en.wikipedia.org/wiki/Ellipsoid#Surface_area
                return Tau * c2 * (1 + a / (c * e) * phi);
            }

            // The ellipsoid is scalene.
            // (This is more efficient than the formula in Wikipedia).
            double k = Sqrt(1 - c2 / (b * b)) / e;
            (double F, double E) = EllipticIntegrals.FE(phi, k);
            return Tau * (c2 + a * b * e * E + b * c2 / (a * e) * F);
        }
    }

    /// <summary>
    /// The volume in cubic metres.
    /// </summary>
    public double Volume => 4 * PI / 3 * RadiusA * RadiusB * RadiusC;

    /// <summary>
    /// The volumetric mean radius in metres.
    /// </summary>
    public double VolumetricMeanRadius => double.RootN(RadiusA * RadiusB * RadiusC, 3);

    #endregion Calculated properties
}
