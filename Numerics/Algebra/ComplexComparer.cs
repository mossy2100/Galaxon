using System.Numerics;

namespace Galaxon.Numerics.Algebra;

public class ComplexComparer : IComparer<Complex>
{
    /// <summary>
    /// Compare 2 Complex values.
    /// This does not compare magnitudes of the complex numbers, as the method would then show
    /// different values as being equal.
    /// So, the method is for the purpose of ordering values in a list, rather than comparing
    /// magnitudes.
    /// First the real parts are compared, then if they are equal, the imaginary parts are compared.
    /// </summary>
    /// <param name="x">The first Complex value.</param>
    /// <param name="y">The second Complex value.</param>
    /// <returns>If one is less than the other for the purpose of ordering.</returns>
    public int Compare(Complex x, Complex y)
    {
        // First we consider the real parts.
        if (x.Real < y.Real)
        {
            return -1;
        }
        else if (x.Real > y.Real)
        {
            return 1;
        }
        // From here, the real parts are equal. Now compare the imaginary parts.
        else if (x.Imaginary < y.Imaginary)
        {
            return -1;
        }
        else if (x.Imaginary > y.Imaginary)
        {
            return 1;
        }
        // The values are equal.
        else
        {
            return 0;
        }
    }
}
