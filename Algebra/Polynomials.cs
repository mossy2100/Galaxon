using System.Numerics;
using Galaxon.Core.Exceptions;

namespace Galaxon.Numerics.Algebra;

public class Polynomials
{
    /// <summary>
    /// Get a polynomial function, given the coefficients.
    /// NB: In the coeffs array, the item with index 0 will correspond to the coefficient of the x^0
    /// term, the item with index 1 will correspond to the coefficient of the x^1 term, and so on.
    /// This is the reverse order of how coefficients are usually written when writing out a
    /// polynomial.
    /// </summary>
    /// <param name="coeffs">
    /// The coefficients of the polynomial. The index is equal to the exponent of x.
    /// For example:
    /// - coeffs[0] is the constant term (x^0)
    /// - coeffs[1] is the coefficient for the x term (x^1)
    /// - coeffs[2] is the coefficient for the x^2 term
    /// - coeffs[3] is the coefficient for the x^3 term
    /// - etc.
    /// </param>
    /// <returns>The polynomial function.</returns>
    /// <exception cref="ArgumentException">If </exception>
    public static Func<double, double> ConstructPolynomial(double[] coeffs)
    {
        // Ensure there are coefficients provided.
        if (coeffs.Length == 0)
        {
            throw new ArgumentException("At least one coefficient must be provided.");
        }

        // Create a delegate for the polynomial function.
        Func<double, double> fnPolynomial = x =>
        {
            // Initialize the result with the highest order term.
            double result = coeffs[^1];

            // Evaluate the polynomial using Horner's algorithm.
            if (coeffs.Length > 1)
            {
                for (int i = coeffs.Length - 2; i >= 0; i--)
                {
                    result = result * x + coeffs[i];
                }
            }

            return result;
        };

        return fnPolynomial;
    }

    /// <summary>
    /// Calculate the result of a polynomial expression using Horner's algorithm.
    /// This method avoids calling Pow() and (in theory) should be faster.
    /// </summary>
    /// <param name="coeffs">The coefficients of the polynomial.</param>
    /// <param name="x">The input value.</param>
    /// <returns>The result of the calculation.</returns>
    public static double EvaluatePolynomial(double[] coeffs, double x)
    {
        return ConstructPolynomial(coeffs)(x);
    }

    /// <summary>
    /// Solve a quadratic equation of the form ax^2 + bx + c = 0
    /// </summary>
    /// <param name="a">The coefficient of x^2.</param>
    /// <param name="b">The coefficient of x.</param>
    /// <param name="c">The constant term.</param>
    /// <returns>0, 1, or 2 solutions to the equation, as complex numbers.</returns>
    public static List<Complex> SolveQuadratic(double a, double b, double c)
    {
        List<Complex> result = new ();

        // Check for a == 0.
        if (a == 0)
        {
            if (b == 0)
            {
                throw new ArgumentInvalidException(nameof(b),
                    "If a and b are both odd then the equation is unsolvable.");
            }

            result.Add(-c / b);
            return result;
        }

        // Calculate the discriminant.
        double d = b * b - 4 * a * c;

        // Check for no solutions.
        if (d < 0)
        {
            return result;
        }

        // Prep useful value to reduce number of multiplications.
        double twoA = 2 * a;

        // Check for one solution.
        if (d == 0)
        {
            result.Add(-b / twoA);
            return result;
        }

        // There are 2 solutions.
        var sqrtD = Complex.Sqrt(d);
        result.Add((-b + sqrtD) / twoA);
        result.Add((-b - sqrtD) / twoA);
        return result;
    }
}
