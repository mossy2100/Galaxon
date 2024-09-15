using System.Numerics;
using Galaxon.Numerics.Extensions;
using Galaxon.Numerics.Extensions.Integers;

namespace Galaxon.Tests.Numerics.Extensions;

[TestClass]
public class BigIntegerExtensionsTests
{
    [TestMethod]
    public void Reverse_ReturnsCorrectValue()
    {
        // Arrange
        BigInteger n = 123456789;

        // Act
        BigInteger result = n.Reverse();

        // Assert
        Assert.AreEqual(987654321, result);
    }

    [TestMethod]
    public void IsPalindromic_ReturnsTrueForPalindromicNumber()
    {
        // Arrange
        BigInteger n = 123454321;

        // Act
        bool result = n.IsPalindromic();

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void IsPalindromic_ReturnsFalseForNonPalindromicNumber()
    {
        // Arrange
        BigInteger n = 123456789;

        // Act
        bool result = n.IsPalindromic();

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void DigitSum_ReturnsCorrectValue()
    {
        // Arrange
        BigInteger n = 123456789;

        // Act
        BigInteger result = n.DigitSum();

        // Assert
        Assert.AreEqual(45, result);
    }

    [TestMethod]
    public void NumDigits_ReturnsCorrectValue()
    {
        // Arrange
        BigInteger n = 123456789;

        // Act
        int result = n.NumDigits();

        // Assert
        Assert.AreEqual(9, result);
    }

    [TestMethod]
    public void Sum_ReturnsCorrectValue()
    {
        // Arrange
        List<BigInteger> nums = new () { 1, 2, 3, 4, 5 };

        // Act
        BigInteger result = nums.Sum();

        // Assert
        Assert.AreEqual(15, result);
    }

    [TestMethod]
    public void Sum_WithTransform_ReturnsCorrectValue()
    {
        // Arrange
        List<BigInteger> nums = new () { 1, 2, 3, 4, 5 };

        // Act
        BigInteger result = nums.Sum(n => n * n);

        // Assert
        Assert.AreEqual(55, result);
    }

    [TestMethod]
    public void TestNumDigitsZero()
    {
        BigInteger bi;
        int nDigits;

        bi = 0;
        nDigits = bi.NumDigits();
        Assert.AreEqual(1, nDigits);
    }

    [TestMethod]
    public void TestNumDigitsPositive()
    {
        BigInteger bi;
        int nDigits;

        bi = 1;
        nDigits = bi.NumDigits();
        Assert.AreEqual(1, nDigits);

        bi = 9;
        nDigits = bi.NumDigits();
        Assert.AreEqual(1, nDigits);

        bi = 10;
        nDigits = bi.NumDigits();
        Assert.AreEqual(2, nDigits);

        bi = 999;
        nDigits = bi.NumDigits();
        Assert.AreEqual(3, nDigits);

        bi = 1000;
        nDigits = bi.NumDigits();
        Assert.AreEqual(4, nDigits);

        bi = 999_999_999;
        nDigits = bi.NumDigits();
        Assert.AreEqual(9, nDigits);

        bi = 1_000_000_000;
        nDigits = bi.NumDigits();
        Assert.AreEqual(10, nDigits);

        bi = BigInteger.Parse("99999999999999999999");
        nDigits = bi.NumDigits();
        Assert.AreEqual(20, nDigits);
    }

    [TestMethod]
    public void TestNumDigitsNegative()
    {
        BigInteger bi;
        int nDigits;

        bi = -1;
        nDigits = bi.NumDigits();
        Assert.AreEqual(1, nDigits);

        bi = -9;
        nDigits = bi.NumDigits();
        Assert.AreEqual(1, nDigits);

        bi = -10;
        nDigits = bi.NumDigits();
        Assert.AreEqual(2, nDigits);

        bi = -999;
        nDigits = bi.NumDigits();
        Assert.AreEqual(3, nDigits);

        bi = -1000;
        nDigits = bi.NumDigits();
        Assert.AreEqual(4, nDigits);

        bi = -999_999_999;
        nDigits = bi.NumDigits();
        Assert.AreEqual(9, nDigits);

        bi = -1_000_000_000;
        nDigits = bi.NumDigits();
        Assert.AreEqual(10, nDigits);

        bi = BigInteger.Parse("-99999999999999999999");
        nDigits = bi.NumDigits();
        Assert.AreEqual(20, nDigits);
    }

    [TestMethod]
    public void IsPowerOf2_PowersOf2_ReturnsTrue()
    {
        Assert.IsTrue(BigIntegerExtensions.IsPowerOf2(1));
        Assert.IsTrue(BigIntegerExtensions.IsPowerOf2(2));
        Assert.IsTrue(BigIntegerExtensions.IsPowerOf2(4));
        Assert.IsTrue(BigIntegerExtensions.IsPowerOf2(8));
        Assert.IsTrue(BigIntegerExtensions.IsPowerOf2(16));
        Assert.IsTrue(BigIntegerExtensions.IsPowerOf2((BigInteger)sbyte.MaxValue + 1));
        Assert.IsTrue(BigIntegerExtensions.IsPowerOf2((BigInteger)byte.MaxValue + 1));
        Assert.IsTrue(BigIntegerExtensions.IsPowerOf2((BigInteger)short.MaxValue + 1));
        Assert.IsTrue(BigIntegerExtensions.IsPowerOf2((BigInteger)ushort.MaxValue + 1));
        Assert.IsTrue(BigIntegerExtensions.IsPowerOf2((BigInteger)int.MaxValue + 1));
        Assert.IsTrue(BigIntegerExtensions.IsPowerOf2((BigInteger)uint.MaxValue + 1));
        Assert.IsTrue(BigIntegerExtensions.IsPowerOf2((BigInteger)long.MaxValue + 1));
        Assert.IsTrue(BigIntegerExtensions.IsPowerOf2((BigInteger)ulong.MaxValue + 1));
    }

    [TestMethod]
    public void IsPowerOf2_NonPowersOf2_ReturnsFalse()
    {
        Assert.IsFalse(BigIntegerExtensions.IsPowerOf2(-1));
        Assert.IsFalse(BigIntegerExtensions.IsPowerOf2(-2));
        Assert.IsFalse(BigIntegerExtensions.IsPowerOf2(-4));
        Assert.IsFalse(BigIntegerExtensions.IsPowerOf2(-8));
        Assert.IsFalse(BigIntegerExtensions.IsPowerOf2(3));
        Assert.IsFalse(BigIntegerExtensions.IsPowerOf2(5));
        Assert.IsFalse(BigIntegerExtensions.IsPowerOf2(6));
        Assert.IsFalse(BigIntegerExtensions.IsPowerOf2(7));
        Assert.IsFalse(BigIntegerExtensions.IsPowerOf2(sbyte.MaxValue));
        Assert.IsFalse(BigIntegerExtensions.IsPowerOf2(byte.MaxValue));
        Assert.IsFalse(BigIntegerExtensions.IsPowerOf2(short.MaxValue));
        Assert.IsFalse(BigIntegerExtensions.IsPowerOf2(ushort.MaxValue));
        Assert.IsFalse(BigIntegerExtensions.IsPowerOf2(int.MaxValue));
        Assert.IsFalse(BigIntegerExtensions.IsPowerOf2(uint.MaxValue));
        Assert.IsFalse(BigIntegerExtensions.IsPowerOf2(long.MaxValue));
        Assert.IsFalse(BigIntegerExtensions.IsPowerOf2(ulong.MaxValue));
    }
}
