using Galaxon.Numerics.Integers;

namespace Galaxon.Numerics.Geometry;

public static class EllipticIntegrals
{
    private const double _Delta = 1e-12;

    /// <summary>
    /// Calculate incomplete elliptic integrals of the first and (optionally)
    /// second kinds.
    /// Calculating both F() and E() together is more efficient (if you want
    /// both of them) because RF() will only be called once, and also because
    /// the calculation of F() requires a lot of the same intermediate variables
    /// as the calculation for E().
    /// This function is just for internal use.
    /// </summary>
    /// <see href="https://en.wikipedia.org/wiki/Carlson_symmetric_form#Relation_to_the_Legendre_forms"/>
    /// <see cref="Ellipsoid.SurfaceArea"/>
    /// <param name="phi">The amplitude in radians.</param>
    /// <param name="k">The elliptic modulus (eccentricity)</param>
    /// <param name="getSecondKind">
    /// Whether to also calculate the second
    /// kind.
    /// </param>
    /// <returns>A tuple containing the two results.</returns>
    private static (double F, double? E) HelperIncomplete(double phi, double k,
        bool getSecondKind = true)
    {
        // Calculate m = k^2, a.k.a. "the parameter".
        double m = k * k;
        double x = Sin(phi);
        double y = Cos(phi);
        double g = y * y;
        double h = 1 - m * x * x;
        double F = x * RF(g, h, 1);
        if (!getSecondKind)
        {
            return (F, null);
        }
        double E = F - m / 3 * Pow(x, 3) * RD(g, h, 1);
        return (F, E);
    }

    /// <summary>
    /// Calculate incomplete elliptic integrals of the first and second kinds.
    /// </summary>
    /// <see href="https://en.wikipedia.org/wiki/Carlson_symmetric_form#Relation_to_the_Legendre_forms"/>
    /// <see cref="Ellipsoid.SurfaceArea"/>
    /// <param name="phi">The amplitude in radians.</param>
    /// <param name="k">The elliptic modulus (eccentricity)</param>
    /// <returns>A tuple containing the two results.</returns>
    public static (double F, double E) FE(double phi, double k)
    {
        (double F, double? E) = HelperIncomplete(phi, k);
        return (F, E!.Value);
    }

    /// <summary>
    /// Calculate incomplete elliptic integral of the first kind.
    /// </summary>
    /// <see href="https://en.wikipedia.org/wiki/Carlson_symmetric_form#Relation_to_the_Legendre_forms"/>
    /// <param name="phi">The amplitude in radians.</param>
    /// <param name="k">The elliptic modulus (eccentricity)</param>
    /// <returns>The result.</returns>
    public static double F(double phi, double k)
    {
        (double F, double? E) = HelperIncomplete(phi, k, false);
        return F;
    }

    /// <summary>
    /// Calculate incomplete elliptic integral of the second kind.
    /// </summary>
    /// <see href="https://en.wikipedia.org/wiki/Carlson_symmetric_form#Relation_to_the_Legendre_forms"/>
    /// <param name="phi">The amplitude in radians.</param>
    /// <param name="k">The elliptic modulus (eccentricity)</param>
    /// <returns>The result.</returns>
    public static double E(double phi, double k)
    {
        (double F, double? E) = HelperIncomplete(phi, k);
        return E!.Value;
    }

    /// <summary>
    /// Computes RF() from the Carlson symmetric forms of elliptic integrals.
    /// The code for this function came from The Code Project (see link below).
    /// </summary>
    /// <see href="https://en.wikipedia.org/wiki/Carlson_symmetric_form"/>
    /// <see href="https://www.codeproject.com/Articles/566614/Elliptic-integrals"/>
    /// <see href="http://dlmf.nist.gov/19.36#E1"/>
    /// <see href="https://rdrr.io/cran/Carlson/src/R/RF.R"/>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns>The result of the calculation.</returns>
    public static double RF(double x, double y, double z)
    {
        double A, dx, dy, dz;

        do
        {
            double lambda = Sqrt(x * y) + Sqrt(y * z) + Sqrt(z * x);

            x = (x + lambda) / 4;
            y = (y + lambda) / 4;
            z = (z + lambda) / 4;

            A = (x + y + z) / 3;

            dx = 1 - x / A;
            dy = 1 - y / A;
            dz = 1 - z / A;
        } while (Compare.Max(Abs(dx), Abs(dy), Abs(dz)) >= _Delta);

        double E2 = dx * dy + dy * dz + dz * dx;
        double E3 = dx * dy * dz;
        double E22 = E2 * E2;

        // Integer values rearranged to avoid integer division operations.
        return (1 - E2 / 10 + E3 / 14 + E22 / 24 - 3 * E2 * E3 / 44
                - 5 * E22 * E2 / 208 + 3 * E3 * E3 / 104 + E22 * E3 / 16)
            / Sqrt(A);
    }

    /// <summary>
    /// Computes RD() from the Carlson symmetric forms of elliptic integrals.
    /// The code for this function came from The Code Project (see link below).
    /// </summary>
    /// <see href="https://en.wikipedia.org/wiki/Carlson_symmetric_form"/>
    /// <see href="https://www.codeproject.com/Articles/566614/Elliptic-integrals"/>
    /// <see href="http://dlmf.nist.gov/19.36#E2"/>
    /// <see href="https://rdrr.io/cran/Carlson/src/R/RD.R"/>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns>The result of the calculation.</returns>
    public static double RD(double x, double y, double z)
    {
        double sum = 0;
        double fac = 1;
        double A, dx, dy, dz;

        do
        {
            double lambda = Sqrt(x * y) + Sqrt(y * z) + Sqrt(z * x);
            sum += fac / (Sqrt(z) * (z + lambda));

            fac /= 4;

            x = (x + lambda) / 4;
            y = (y + lambda) / 4;
            z = (z + lambda) / 4;

            A = (x + y + 3 * z) / 5;

            dx = 1 - x / A;
            dy = 1 - y / A;
            dz = 1 - z / A;
        } while (Compare.Max(Abs(dx), Abs(dy), Abs(dz)) >= _Delta);

        double dz2 = dz * dz;
        double dz3 = dz2 * dz;

        double E2 = dx * dy + 3 * dz2 + 3 * dz * dx + 3 * dy * dz;
        double E3 = dz3 + 3 * dx * dy * dz + 3 * dy * dz2 + 3 * dx * dz2;
        double E4 = dy * dz3 + dx * dz3 + 3 * dx * dy * dz2;
        double E5 = dx * dy * dz3;
        double E22 = E2 * E2;

        return 3 * sum + fac * (1 - 3 * E2 / 14 + E3 / 6 + 9 * E22 / 88
                - 3 * E4 / 22 - 9 * E2 * E3 / 52 + 3 * E5 / 26
                - E22 * E2 / 16 + 3 * E3 * E3 / 40 + 3 * E2 * E4 / 20
                + 45 * E22 * E3 / 272 - 9 * (E3 * E4 + E2 * E5) / 68)
            / A / Sqrt(A);
    }
}
