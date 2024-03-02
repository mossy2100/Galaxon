using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Galaxon.Numerics.Extensions;

/// <summary>
/// Extensions and bonus methods for Complex values and collections of Complex values.
/// </summary>
public static class ComplexExtensions
{
    #region Miscellaneous

    /// <summary>
    /// Sorts the elements of a list of complex numbers in order as defined by ComplexComparer.
    /// </summary>
    /// <param name="values">The list of complex numbers to be sorted.</param>
    public static List<Complex> Sort(List<Complex> values)
    {
        return values.OrderBy(value => value.Real).ThenBy(value => value.Imaginary).ToList();
    }

    /// <summary>
    /// if the real or imaginary part is -0, change to 0.
    /// </summary>
    /// <param name="z">A Complex value.</param>
    /// <returns>The same value with -0 parts replaced with 0.</returns>
    public static Complex RemoveNegativeZero(this Complex z)
    {
        return new Complex(z.Real.RemoveNegativeZero(), z.Imaginary.RemoveNegativeZero());
    }

    #endregion Miscellaneous

    #region Testing

    /// <summary>
    /// Helper function to test if a Complex equals a Complex, within a certain tolerance.
    /// </summary>
    /// <param name="expected">Expected Complex value</param>
    /// <param name="actual">Actual Complex value</param>
    /// <param name="delta">Maximum acceptable difference between the real parts of each value, and
    /// the imaginary parts of each value.</param>
    public static void AreEqual(Complex expected, Complex actual, double delta = 0)
    {
        Assert.AreEqual(expected.Real, actual.Real, delta);
        Assert.AreEqual(expected.Imaginary, actual.Imaginary, delta);
    }

    #endregion Testing
}
