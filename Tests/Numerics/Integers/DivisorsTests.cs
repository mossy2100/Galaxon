using System.Numerics;
using Galaxon.Numerics.Integers;

namespace Galaxon.Tests.Numerics.Integers;

[TestClass]
public class DivisorsTests
{
    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void GetProperDivisors_NegativeArgument_ThrowsException()
    {
        Divisors.GetProperDivisors(-5);
    }

    [TestMethod]
    public void GetProperDivisors_Zero_ReturnsEmptyList()
    {
        List<BigInteger> divisors = Divisors.GetProperDivisors(0);
        Assert.AreEqual(0, divisors.Count);
    }

    [TestMethod]
    public void GetProperDivisors_One_ReturnsEmptyList()
    {
        List<BigInteger> divisors = Divisors.GetProperDivisors(1);
        Assert.AreEqual(0, divisors.Count);
    }

    [TestMethod]
    public void GetProperDivisors_PositiveInteger_NoExceptionThrown()
    {
        List<BigInteger> divisors = Divisors.GetProperDivisors(10);
        CollectionAssert.AreEqual(new BigInteger[] { 1, 2, 5 }, divisors);
    }

    [TestMethod]
    public void GetProperDivisors_PrimeNumber_ReturnsOnlyOneAndItself()
    {
        List<BigInteger> divisors = Divisors.GetProperDivisors(13);
        CollectionAssert.AreEqual(new BigInteger[] { 1 }, divisors);
    }

    [TestMethod]
    public void GetProperDivisors_PerfectSquare_ReturnsAllDivisors()
    {
        List<BigInteger> divisors = Divisors.GetProperDivisors(36);
        CollectionAssert.AreEqual(new BigInteger[] { 1, 2, 3, 4, 6, 9, 12, 18 }, divisors);
    }

    [TestMethod]
    public void GetProperDivisors_SuperiorHighlyCompositeNumber_ReturnsAllDivisors()
    {
        List<BigInteger> divisors = Divisors.GetProperDivisors(60);
        CollectionAssert.AreEqual(new BigInteger[] { 1, 2, 3, 4, 5, 6, 10, 12, 15, 20, 30 }, divisors);
    }

    [DataTestMethod]
    [DataRow(1, 0)]
    [DataRow(2, 1)]
    [DataRow(3, 1)]
    [DataRow(4, 3)]
    [DataRow(5, 1)]
    [DataRow(6, 6)]
    [DataRow(7, 1)]
    [DataRow(8, 7)]
    [DataRow(9, 4)]
    [DataRow(10, 8)]
    public void Aliquot_ReturnsCorrectValue(int n, int expected)
    {
        BigInteger result = Divisors.Aliquot(n);

        // Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void LeastCommonMultiple_BothInputsZero_ReturnsCorrectValue()
    {
        // Arrange
        BigInteger a = 0;
        BigInteger b = 0;

        // Act
        BigInteger result = Divisors.LeastCommonMultiple(a, b);

        // Assert
        Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void LeastCommonMultiple_OneInputZero_ReturnsCorrectValue()
    {
        // Second input zero.
        // Arrange
        BigInteger a = 123;
        BigInteger b = 0;

        // Act
        BigInteger result = Divisors.LeastCommonMultiple(a, b);

        // Assert
        Assert.AreEqual(123, result);

        // First input zero.
        // Arrange
        a = 0;
        b = 456;

        // Act
        result = Divisors.LeastCommonMultiple(a, b);

        // Assert
        Assert.AreEqual(456, result);
    }

    [DataTestMethod]
    [DataRow(4, 6, 12)]
    [DataRow(6, 4, 12)]
    [DataRow(15, 5, 15)]
    [DataRow(5, 15, 15)]
    [DataRow(12, 10, 60)]
    [DataRow(10, 12, 60)]
    public void LeastCommonMultiple_PositiveInputs_ReturnsCorrectValue(int a, int b, int expected)
    {
        BigInteger result = Divisors.LeastCommonMultiple(a, b);

        // Assert
        Assert.AreEqual(expected, result);
    }

    [DataTestMethod]
    [DataRow(-4, -6, 12)]
    [DataRow(-4, 6, 12)]
    [DataRow(4, -6, 12)]
    [DataRow(-15, -5, 15)]
    [DataRow(-15, 5, 15)]
    [DataRow(15, -5, 15)]
    public void LeastCommonMultiple_NegativeInputs_ReturnsCorrectValue(int a, int b, int expected)
    {
        // Act
        BigInteger result = Divisors.LeastCommonMultiple(a, b);

        // Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void GreatestCommonDivisor_ReturnsCorrectValue()
    {
        // Arrange
        BigInteger a = 123456789;
        BigInteger b = 987654321;

        // Act
        BigInteger result = Divisors.GreatestCommonDivisor(a, b);

        // Assert
        Assert.AreEqual(9, result);
    }

    [DataTestMethod]
    [DataRow(0, 0, 0)]
    [DataRow(1, 1, 1)]
    [DataRow(5, 5, 5)]
    [DataRow(10, 10, 10)]
    [DataRow(1, 0, 1)]
    [DataRow(0, 1, 1)]
    [DataRow(5, 0, 5)]
    [DataRow(0, 5, 5)]
    [DataRow(10, 0, 10)]
    [DataRow(0, 10, 10)]
    [DataRow(1, 5, 1)]
    [DataRow(5, 1, 1)]
    [DataRow(1, 10, 1)]
    [DataRow(10, 1, 1)]
    [DataRow(3, 7, 1)]
    [DataRow(7, 3, 1)]
    [DataRow(10, 5, 5)]
    [DataRow(5, 10, 5)]
    [DataRow(6, 5, 1)]
    [DataRow(5, 6, 1)]
    [DataRow(4, 6, 2)]
    [DataRow(6, 4, 2)]
    [DataRow(4, 16, 4)]
    [DataRow(16, 4, 4)]
    [DataRow(4, 9, 1)]
    [DataRow(9, 4, 1)]
    public void TestGreatestCommonDivisor(int a, int b, int expected)
    {
        // Act
        BigInteger result = Divisors.GreatestCommonDivisor(a, b);

        // Assert
        Assert.AreEqual(expected, result);
    }
}
