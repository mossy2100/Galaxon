using Galaxon.Astronomy.Algorithms.Records;
using Galaxon.Numerics.Algebra;
using Galaxon.Numerics.Geometry;
using Galaxon.Time;

namespace Galaxon.Astronomy.Algorithms.Services;

public class NutationService
{
    /// <summary>
    /// Copy of the data in Table 22.A from "Astronomical Algorithms, 2nd Ed." by Jeean Meeus
    /// (pp145-146).
    /// </summary>
    private static readonly List<(int, int, int, int, int, int, double, int, double)> Table22A =
    [
        (0, 0, 0, 0, 1, -171996, -174.2, 92025, 8.9),
        (-2, 0, 0, 2, 2, -13187, -1.6, 5736, -3.1),
        (0, 0, 0, 2, 2, -2274, -0.2, 977, -0.5),
        (0, 0, 0, 0, 2, 2062, 0.2, -895, 0.5),
        (0, 1, 0, 0, 0, 1426, -3.4, 54, -0.1),
        (0, 0, 1, 0, 0, 712, 0.1, -7, 0),
        (-2, 1, 0, 2, 2, -517, 1.2, 224, -0.6),
        (0, 0, 0, 2, 1, -386, -0.4, 200, 0),
        (0, 0, 1, 2, 2, -301, 0, 129, -0.1),
        (-2, -1, 0, 2, 2, 217, -0.5, -95, 0.3),
        (-2, 0, 1, 0, 0, -158, 0, 0, 0),
        (-2, 0, 0, 2, 1, 129, 0.1, -70, 0),
        (0, 0, -1, 2, 2, 123, 0, -53, 0),
        (2, 0, 0, 0, 0, 63, 0, 0, 0),
        (0, 0, 1, 0, 1, 63, 0.1, -33, 0),
        (2, 0, -1, 2, 2, -59, 0, 26, 0),
        (0, 0, -1, 0, 1, -58, -0.1, 32, 0),
        (0, 0, 1, 2, 1, -51, 0, 27, 0),
        (-2, 0, 2, 0, 0, 48, 0, 0, 0),
        (0, 0, -2, 2, 1, 46, 0, -24, 0),
        (2, 0, 0, 2, 2, -38, 0, 16, 0),
        (0, 0, 2, 2, 2, -31, 0, 13, 0),
        (0, 0, 2, 0, 0, 29, 0, 0, 0),
        (-2, 0, 1, 2, 2, 29, 0, -12, 0),
        (0, 0, 0, 2, 0, 26, 0, 0, 0),
        (-2, 0, 0, 2, 0, -22, 0, 0, 0),
        (0, 0, -1, 2, 1, 21, 0, -10, 0),
        (0, 2, 0, 0, 0, 17, -0.1, 0, 0),
        (2, 0, -1, 0, 1, 16, 0, -8, 0),
        (-2, 2, 0, 2, 2, -16, 0.1, 7, 0),
        (0, 1, 0, 0, 1, -15, 0, 9, 0),
        (-2, 0, 1, 0, 1, -13, 0, 7, 0),
        (0, -1, 0, 0, 1, -12, 0, 6, 0),
        (0, 0, 2, -2, 0, 11, 0, 0, 0),
        (2, 0, -1, 2, 1, -10, 0, 5, 0),
        (2, 0, 1, 2, 2, -8, 0, 3, 0),
        (0, 1, 0, 2, 2, 7, 0, -3, 0),
        (-2, 1, 1, 0, 0, -7, 0, 0, 0),
        (0, -1, 0, 2, 2, -7, 0, 3, 0),
        (2, 0, 0, 2, 1, -7, 0, 3, 0),
        (2, 0, 1, 0, 0, 6, 0, 0, 0),
        (-2, 0, 2, 2, 2, 6, 0, -3, 0),
        (-2, 0, 1, 2, 1, 6, 0, -3, 0),
        (2, 0, -2, 0, 1, -6, 0, 3, 0),
        (2, 0, 0, 0, 1, -6, 0, 3, 0),
        (0, -1, 1, 0, 0, 5, 0, 0, 0),
        (-2, -1, 0, 2, 1, -5, 0, 3, 0),
        (-2, 0, 0, 0, 1, -5, 0, 3, 0),
        (0, 0, 2, 2, 1, -5, 0, 3, 0),
        (-2, 0, 2, 0, 1, 4, 0, 0, 0),
        (-2, 1, 0, 2, 1, 4, 0, 0, 0),
        (0, 0, 1, -2, 0, 4, 0, 0, 0),
        (-1, 0, 1, 0, 0, -4, 0, 0, 0),
        (-2, 1, 0, 0, 0, -4, 0, 0, 0),
        (1, 0, 0, 0, 0, -4, 0, 0, 0),
        (0, 0, 1, 2, 0, 3, 0, 0, 0),
        (0, 0, -2, 2, 2, -3, 0, 0, 0),
        (-1, -1, 1, 0, 0, -3, 0, 0, 0),
        (0, 1, 1, 0, 0, -3, 0, 0, 0),
        (0, -1, 1, 2, 2, -3, 0, 0, 0),
        (2, -1, -1, 2, 2, -3, 0, 0, 0),
        (0, 0, 3, 2, 2, -3, 0, 0, 0),
        (2, -1, 0, 2, 2, -3, 0, 0, 0)
    ];

    /// <summary>
    /// Calculate the nutation for a point in time, defined by Julian Date in Terrestrial Time.
    /// </summary>
    /// <param name="jdtt">The Julian Date in Terrestrial Time.</param>
    /// <returns>The components of the nutation as a tuple.</returns>
    public static Nutation CalcNutation(double jdtt)
    {
        // Calculate the number of Julian centuries since J2000.0.
        double T = TimeScales.JulianCenturiesSinceJ2000(jdtt);

        // Calculate mean elongation of the Moon from the Sun.
        double D = Polynomials.EvaluatePolynomial([297.850_36, 445_267.111_480, -0.001_9142,
            1.0 / 189_474], T);
        D = WrapDegrees(D, false);

        // Calculate mean anomaly of the Sun (Earth).
        double M = Polynomials.EvaluatePolynomial([357.527_72, 35_999.050_340, -0.000_1603,
            -1.0 / 300_000], T);
        M = WrapDegrees(M, false);

        // Calculate mean anomaly of the Moon.
        double N = Polynomials.EvaluatePolynomial([134.962_98, 477_198.867_398, 0.008_6972,
            1.0 / 56_250], T);
        N = WrapDegrees(N, false);

        // Calculate Moon's argument of latitude.
        double F = Polynomials.EvaluatePolynomial([93.271_91, 483_202.017_538, -0.003_6825,
            1.0 / 327_270], T);
        F = WrapDegrees(F, false);

        // Calculate longitude of the ascending node of the Moon's mean orbit on the ecliptic,
        // measured from the mean equinox of the date.
        double G = Polynomials.EvaluatePolynomial([125.044_52, -1934.136_261, 0.002_0708,
            1.0 / 450_000], T);
        G = WrapDegrees(G, false);

        double nutLong = 0;
        double nutOb = 0;
        foreach ((int multD, int multM, int multN, int multF, int multG, int s0, double s1, int c0,
                double c1) in Table22A)
        {
            double arg = multD * D + multM * M + multN * N + multF * F + multG * G;
            nutLong += (s0 + s1 * T) * SinDegrees(arg);
            nutOb += (c0 + c1 * T) * CosDegrees(arg);
        }

        // Convert from units of 0.0001 arcseconds to radians.
        double k = 10000 * ARCSECONDS_PER_RADIAN;
        nutLong /= k;
        nutOb /= k;

        return new Nutation(nutLong, nutOb);
    }
}
