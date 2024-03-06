namespace Galaxon.ConsoleApp;

public static class Division
{
    /// <summary>
    /// Result will have same sign as dividend.
    /// </summary>
    public static int ModuloOperator(int a, int b)
    {
        return a % b;
    }

    /// <summary>
    /// Result will have same sign as divisor.
    /// </summary>
    public static int Mod(int a, int b)
    {
        int r = a % b;
        return r < 0 ? r + b : r;
    }

    /// <summary>
    /// Result will have same sign as dividend.
    /// Equivalent to ModuloOperator().
    /// </summary>
    public static int RemainderAfterTruncatedDivision(int a, int b)
    {
        return a - b * (int)double.Truncate((double)a / b);
    }

    public static int RemainderAfterFlooredDivision(int a, int b)
    {
        return a - b * (int)Math.Floor((double)a / b);
    }

    /// <summary>
    /// Always returns a positive result, even if the divisor is negative.
    /// </summary>
    public static int RemainderAfterEuclideanDivision(int a, int b)
    {
        var c = int.Abs(b);
        return a - c * (int)Math.Floor((double)a / c);
    }
}
