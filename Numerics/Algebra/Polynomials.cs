using System.Numerics;
using Galaxon.Numerics.Extensions;

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
        // Guard. Ensure coefficients are provided.
        if (coeffs.Length == 0)
        {
            throw new ArgumentException("At least one coefficient must be provided.");
        }

        // Create a delegate for the polynomial function.
        return x =>
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
    /// Returns solutions as doubles. Does not find complex solutions.
    /// </summary>
    /// <param name="a">The coefficient of x^2.</param>
    /// <param name="b">The coefficient of x.</param>
    /// <param name="c">The constant term.</param>
    /// <returns>0, 1, or 2 solutions to the equation, as doubles.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If a == 0 and b == 0.</exception>
    public static List<double> SolveQuadratic(double a, double b, double c)
    {
        // Guard.
        if (a == 0 && b == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(b),
                "If a and b are both zero then the equation is unsolvable.");
        }

        List<double> result = [];

        // If a == 0, the expression is bx + c = 0, which gives x = -c/b.
        if (a == 0)
        {
            result.Add(-c / b);
            return result;
        }

        // Calculate the discriminant.
        double d = b * b - 4 * a * c;

        // Check for 0 solutions.
        if (d < 0)
        {
            return result;
        }

        // Calculate intermediate value to reduce number of multiplications.
        double twoA = 2.0 * a;

        // Check for 1 solution.
        if (d == 0)
        {
            result.Add(-b / twoA);
            return result;
        }

        // There are 2 solutions.
        double sqrtD = Math.Sqrt(d);
        result.Add((-b + sqrtD) / twoA);
        result.Add((-b - sqrtD) / twoA);
        // Order the solutions so the results are predictable and testable.
        result.Sort();
        return result;
    }

    /// <summary>
    /// Solve a quadratic equation of the form ax^2 + bx + c = 0
    /// Returns complex numbers.
    /// </summary>
    /// <param name="a">The coefficient of x^2.</param>
    /// <param name="b">The coefficient of x.</param>
    /// <param name="c">The constant term.</param>
    /// <returns>0, 1, or 2 solutions to the equation, as complex numbers.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If a == 0 and b == 0.</exception>
    public static List<Complex> SolveQuadraticComplex(double a, double b, double c)
    {
        // Guard.
        if (a == 0 && b == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(b),
                "If a and b are both zero then the equation is unsolvable.");
        }

        List<Complex> result = [];

        // If a == 0, the expression is bx + c = 0, which gives x = -c/b.
        if (a == 0)
        {
            result.Add(-c / b);
            return result;
        }

        // Calculate the discriminant.
        double d = b * b - 4 * a * c;

        // Calculate intermediate value to reduce number of multiplications.
        double twoA = 2.0 * a;

        // Check for 1 solution.
        if (d == 0)
        {
            result.Add(-b / twoA);
            return result;
        }

        // There are 2 solutions.
        // If they are complex, one will be the complex conjugate of the other.
        Complex sqrtD = Complex.Sqrt(d);
        result.Add(((-b + sqrtD) / twoA).RemoveNegativeZero());
        result.Add(((-b - sqrtD) / twoA).RemoveNegativeZero());
        // Order them so the results are predictable and testable.
        result = ComplexExtensions.Sort(result);
        return result;
    }
}
