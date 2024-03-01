using System.Numerics;

namespace Galaxon.Numerics.Algebra;

public class ComplexComparer : IComparer<Complex>
{
    public int Compare(Complex x, Complex y)
    {
        // First we consider only the real parts.
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
