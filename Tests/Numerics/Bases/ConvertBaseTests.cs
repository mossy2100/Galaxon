using Bases;
using Galaxon.Core.Exceptions;

namespace Galaxon.Tests.Numerics.Bases;

[TestClass]
public class ConvertBaseTests
{
    [TestMethod]
    public void ToBase_ZeroToAnyBase_ReturnsZero()
    {
        ulong i = 0;
        string s = i.ToBase(2);
        Assert.AreEqual("0", s);

        s = i.ToBase(12);
        Assert.AreEqual("0", s);

        s = i.ToBase(36);
        Assert.AreEqual("0", s);
    }

    [TestMethod]
    public void ToBase_ValidInput_ReturnsExpectedValue()
    {
        // Arrange
        ulong value = 123;
        byte toBase = 2;
        string expected = "1111011";

        // Act
        string result = value.ToBase(toBase);

        // Assert
        Assert.AreEqual(expected, result);
    }

    [DataTestMethod]
    [DataRow(1ul, "1")]
    [DataRow(10ul, "a")]
    [DataRow(1000ul, "3e8")]
    [DataRow(1_000_000ul, "f4240")]
    [DataRow((ulong)int.MaxValue, "7fffffff")]
    public void ToBase16_ValidInput_ReturnsExpectedValue(ulong input, string expected)
    {
        // Act
        string result = input.ToBase(16);

        // Assert
        Assert.AreEqual(expected, result);
    }

    /// <remarks>
    /// Useful converter:
    /// </remarks>
    /// <see href="https://www.rapidtables.com/convert/number/decimal-to-hex.html"/>
    [DataTestMethod]
    [DataRow((byte)2, "111010110111100110100010101")]
    [DataRow((byte)8, "726746425")]
    [DataRow((byte)16, "75bcd15")]
    public void ToBase_DifferentBases_ReturnsExpectedValues(byte radix, string expected)
    {
        // Arrange
        ulong i = 123_456_789;

        // Act
        string result = i.ToBase(radix);

        // Assert
        Assert.AreEqual(expected, result);
    }

    [DataTestMethod]
    [DataRow((ulong)sbyte.MaxValue, "177")]
    [DataRow((ulong)byte.MaxValue, "377")]
    [DataRow((ulong)short.MaxValue, "77777")]
    [DataRow((ulong)ushort.MaxValue, "177777")]
    [DataRow((ulong)int.MaxValue, "17777777777")]
    [DataRow((ulong)uint.MaxValue, "37777777777")]
    [DataRow((ulong)long.MaxValue, "777777777777777777777")]
    [DataRow(ulong.MaxValue, "1777777777777777777777")]
    public void ToBase8_DifferentValues_ReturnsExpectedValues(ulong input, string expected)
    {
        // Act
        string result = input.ToBase(8);

        // Assert
        Assert.AreEqual(expected, result);
    }

    [DataTestMethod]
    [DataRow(0ul, "00000000")]
    [DataRow(50ul, "00110010")]
    [DataRow(100ul, "01100100")]
    [DataRow(200ul, "11001000")]
    public void ToBase2_WithWidth_ReturnsExpectedValues(ulong input, string expected)
    {
        // Act
        string result = input.ToBase(2, 8);

        // Assert
        Assert.AreEqual(expected, result);
    }

    [DataTestMethod]
    [DataRow(0ul, "0000000000000000")]
    [DataRow(12345ul, "0000000000003039")]
    [DataRow(987654321ul, "000000003ade68b1")]
    [DataRow(20015998341291ul, "00001234567890ab")]
    public void ToBase16_WithWidth_ReturnsExpectedValues(ulong input, string expected)
    {
        // Act
        string result = input.ToBase(16, 16);

        // Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void ToBase32_WithCase_ReturnsExpectedValues()
    {
        // Arrange
        ulong x = 12345678901234567890;

        // Act
        string resultLower = x.ToBase(32);
        string resultUpper = x.ToBase(32, upper: true);

        // Assert
        Assert.AreEqual("aml59hjlhu2mi", resultLower);
        Assert.AreEqual("AML59HJLHU2MI", resultUpper);
    }

    [TestMethod]
    public void ToBase_InvalidBase_ThrowsException()
    {
        // Arrange
        ulong x = 123456;
        string s;

        // Assert
        Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
        {
            // Act
            s = x.ToBase(0);
        });
        Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
        {
            // Act
            s = x.ToBase(1);
        });
        Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
        {
            // Act
            s = x.ToBase(65);
        });
    }

    [TestMethod]
    public void ToBase_InvalidWidth_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        ulong x = 123456;

        // Assert
        Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
        {
            // Act
            x.ToBase(2, -1);
        });
        Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
        {
            // Act
            x.ToBase(4, 0);
        });
    }

    [TestMethod]
    public void FromBase_ValidInput_ReturnsExpectedValue()
    {
        // Arrange
        ulong expected = 123;
        byte fromBase = 2;
        string digits = "1111011";

        // Act
        ulong result = ConvertBase.FromBase(digits, fromBase);

        // Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void FromBase_InvalidDigits_ThrowsFormatException()
    {
        // Assert
        Assert.ThrowsException<ArgumentInvalidException>(() =>
        {
            // Act
            ConvertBase.FromBase("123", 2);
        });
    }

    [TestMethod]
    public void FromBase_InvalidBase_ThrowsArgumentOutOfRangeException()
    {
        // Assert
        Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
        {
            // Act
            ConvertBase.FromBase("123", 65);
        });
    }
}
