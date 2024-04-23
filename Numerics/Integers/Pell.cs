using System.Numerics;
using Galaxon.Core.Exceptions;
using Galaxon.Numerics.Extensions.FloatingPoint;

namespace Galaxon.Numerics.Integers;

/// <summary>
/// Solution for Pell's Equation.
/// </summary>
public class Pell
{
    public static (BigInteger x, BigInteger y) Solve(int D)
    {
        if (DoubleExtensions.IsPerfectSquare(D))
        {
            throw new ArgumentInvalidException(nameof(D), "Cannot be a perfect square.");
        }

        // Generate an initial intermediate solution with y = 1.
        var x = (BigInteger)Round(Sqrt(D));
        BigInteger x2 = x * x;
        BigInteger y = 1;
        var k = (int)(x2 - D);
        PellIntermediateSolution soln = new (D, x, y, k);

        // Iterate until we have a solution where k = 1.
        while (soln.k != 1)
        {
            // Console.WriteLine($"{soln.x:N0}² - {D}×{soln.y:N0}² = {soln.k}");
            soln = soln.Chakravala();
        }

        // Console.WriteLine($"{soln.x:N0}² - {D}×{soln.y:N0}² = {soln.k}");
        return (soln.x, soln.y);
    }
}

internal class PellIntermediateSolution
{
    public int D;

    public int k;

    public BigInteger x;

    public BigInteger y;

    public PellIntermediateSolution(int D, BigInteger x, BigInteger y, int k)
    {
        this.D = D;
        this.x = x;
        this.y = y;
        this.k = k;
    }

    private void TestM(BigInteger a, BigInteger b, int mToTest, int c, ref int minAbsMSquaredMinusD,
        ref int m)
    {
        if ((a + b * mToTest) % c == 0)
        {
            int absMSquaredMinusD = Abs(mToTest * mToTest - D);
            if (absMSquaredMinusD < minAbsMSquaredMinusD)
            {
                minAbsMSquaredMinusD = absMSquaredMinusD;
                m = mToTest;
            }
        }
    }

    /// <summary>
    /// Perform one iteration of the Chakravala method.
    /// <see href="https://en.wikipedia.org/wiki/Chakravala_method"/>
    /// </summary>
    /// <returns>The new triple.</returns>
    public PellIntermediateSolution Chakravala()
    {
        switch (k)
        {
            // Optimization.
            case 1:
                return (PellIntermediateSolution)MemberwiseClone();

            // Use Brahmagupta's composition method if possible.
            case -1 or 2 or -2 or 4:
                return Brahmagupta();
        }

        BigInteger a = x;
        BigInteger b = y;
        var m = 0;
        int minAbsMSquaredMinusD = int.MaxValue;

        // Try values for m close to sqrt(D) as this will result in minimum for |m^2 - D|.
        var sqrtD = (int)Round(Sqrt(D));

        // Start with an offset of 0 from the sqrt and gradually increase until we find m.
        var offset = 0;

        while (true)
        {
            // Test value below sqrt.
            TestM(a, b, sqrtD - offset, k, ref minAbsMSquaredMinusD, ref m);

            if (offset > 0)
            {
                // Test value above sqrt.
                TestM(a, b, sqrtD + offset, k, ref minAbsMSquaredMinusD, ref m);
            }

            // Check if we found a value for m yet.
            if (m > 0)
            {
                break;
            }

            // Next offset.
            offset++;
        }

        // Get new values.
        int absK = Abs(k);
        BigInteger newX = (a * m + D * b) / absK;
        BigInteger newY = (a + b * m) / absK;
        int newK = (m * m - D) / k;
        return new PellIntermediateSolution(D, newX, newY, newK);
    }

    /// <summary>
    /// Use Brahmagupta's composition method to calculate final x and y for k = 1.
    /// Not supporting Brahmagupta's method for k=-4 as this doesn't produce the smallest integer
    /// pair.
    /// <see href="https://en.wikipedia.org/wiki/Chakravala_method#Brahmagupta's_composition_method"/>
    /// </summary>
    /// <returns>New solution with k = 1.</returns>
    public PellIntermediateSolution Brahmagupta()
    {
        if (k is not (1 or -1 or 2 or -2 or 4))
        {
            throw new ArithmeticException($"k = {k}. k must be ±1, ±2, or 4.");
        }

        BigInteger newX = 0;
        BigInteger newY = 0;
        BigInteger xSqr = x * x;

        switch (k)
        {
            case 1:
                // We already have the final x and y.
                newX = x;
                newY = y;
                break;

            case -1:
                newX = 2 * xSqr + 1;
                newY = 2 * x * y;
                break;

            case 2:
                newX = xSqr - 1;
                newY = x * y;
                break;

            case -2:
                newX = xSqr + 1;
                newY = x * y;
                break;

            case 4:
                if (x % 2 == 0)
                {
                    newX = (xSqr - 2) / 2;
                    newY = x * y / 2;
                }
                else
                {
                    newX = BigInteger.Pow(x / 2 * (xSqr - 3), 2);
                    newY = BigInteger.Pow(y / 2 * (xSqr - 1), 2);
                }
                break;
        }

        return new PellIntermediateSolution(D, newX, newY, 1);
    }
}
