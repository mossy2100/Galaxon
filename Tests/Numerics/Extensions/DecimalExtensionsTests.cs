using DecimalMath;
using Galaxon.Numerics.Extensions;

namespace Galaxon.Tests.Numerics.Extensions;

[TestClass]
public class DecimalExtensionsTests
{
    [TestMethod]
    public void SinhTest()
    {
        decimal m, actual;
        double expected;

        m = 0;
        actual = DecimalExtensions.Sinh(m);
        expected = Sinh((double)m);
        DecimalExtensions.AreEqual(expected, actual);

        m = 1;
        actual = DecimalExtensions.Sinh(m);
        expected = Sinh((double)m);
        DecimalExtensions.AreEqual(expected, actual);

        m = DecimalEx.PiQuarter;
        actual = DecimalExtensions.Sinh(m);
        expected = Sinh((double)m);
        DecimalExtensions.AreEqual(expected, actual);

        m = DecimalEx.Pi;
        actual = DecimalExtensions.Sinh(m);
        expected = Sinh((double)m);
        DecimalExtensions.AreEqual(expected, actual);

        m = -1;
        actual = DecimalExtensions.Sinh(m);
        expected = Sinh((double)m);
        DecimalExtensions.AreEqual(expected, actual);
    }

    [TestMethod]
    public void CoshTest()
    {
        decimal m, actual;
        double expected;

        m = 0;
        actual = DecimalExtensions.Cosh(m);
        expected = Cosh((double)m);
        DecimalExtensions.AreEqual(expected, actual);

        m = 1;
        actual = DecimalExtensions.Cosh(m);
        expected = Cosh((double)m);
        DecimalExtensions.AreEqual(expected, actual);

        m = DecimalEx.PiQuarter;
        actual = DecimalExtensions.Cosh(m);
        expected = Cosh((double)m);
        DecimalExtensions.AreEqual(expected, actual);

        m = DecimalEx.Pi;
        actual = DecimalExtensions.Cosh(m);
        expected = Cosh((double)m);
        DecimalExtensions.AreEqual(expected, actual);

        m = -1;
        actual = DecimalExtensions.Cosh(m);
        expected = Cosh((double)m);
        DecimalExtensions.AreEqual(expected, actual);
    }

    [TestMethod]
    public void TanhTest()
    {
        decimal m, actual;
        double expected;

        m = 0;
        actual = DecimalExtensions.Tanh(m);
        expected = Tanh((double)m);
        DecimalExtensions.AreEqual(expected, actual);

        m = 1;
        actual = DecimalExtensions.Tanh(m);
        expected = Tanh((double)m);
        DecimalExtensions.AreEqual(expected, actual);

        m = DecimalEx.PiQuarter;
        actual = DecimalExtensions.Tanh(m);
        expected = Tanh((double)m);
        DecimalExtensions.AreEqual(expected, actual);

        m = DecimalEx.Pi;
        actual = DecimalExtensions.Tanh(m);
        expected = Tanh((double)m);
        DecimalExtensions.AreEqual(expected, actual);

        m = -1;
        actual = DecimalExtensions.Tanh(m);
        expected = Tanh((double)m);
        DecimalExtensions.AreEqual(expected, actual);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void LnThrowsIfArgZero()
    {
        DecimalExtensions.Log(0);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void LnThrowsIfArgNegative()
    {
        DecimalExtensions.Log(-1);
    }

    [TestMethod]
    public void LnTest()
    {
        decimal m;

        m = 1;
        DecimalExtensions.AreEqual(0, DecimalExtensions.Log(m));

        m = 2;
        DecimalExtensions.AreEqual(Log((double)m), DecimalExtensions.Log(m));

        m = 10;
        DecimalExtensions.AreEqual(Log((double)m), DecimalExtensions.Log(m));

        m = DecimalEx.E;
        DecimalExtensions.AreEqual(1, DecimalExtensions.Log(m));

        m = decimal.MaxValue;
        DecimalExtensions.AreEqual(Log((double)m), DecimalExtensions.Log(m));

        m = DecimalEx.SmallestNonZeroDec;
        DecimalExtensions.AreEqual(Log((double)m), DecimalExtensions.Log(m));

        m = 1.23456789m;
        DecimalExtensions.AreEqual(Log((double)m), DecimalExtensions.Log(m));

        m = 9.87654321m;
        DecimalExtensions.AreEqual(Log((double)m), DecimalExtensions.Log(m));

        m = 123456789m;
        DecimalExtensions.AreEqual(Log((double)m), DecimalExtensions.Log(m));

        m = 9876543210m;
        DecimalExtensions.AreEqual(Log((double)m), DecimalExtensions.Log(m));

        m = 0.00000000000000000123456789m;
        DecimalExtensions.AreEqual(Log((double)m), DecimalExtensions.Log(m));

        m = 0.00000000000000000987654321m;
        DecimalExtensions.AreEqual(Log((double)m), DecimalExtensions.Log(m));
    }

    [TestMethod]
    public void Log1Base0Returns0()
    {
        DecimalExtensions.AreEqual(0, DecimalExtensions.Log(1, 0));
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void LogThrowsIfBase1()
    {
        DecimalExtensions.Log(1.234m, 1);
    }

    [TestMethod]
    public void TestDisassembleAssemble()
    {
        decimal x;
        decimal y;
        byte signBit;
        byte scaleBits;
        UInt128 intBits;

        x = 0m;
        (signBit, scaleBits, intBits) = x.Disassemble();
        y = DecimalExtensions.Assemble(signBit, scaleBits, intBits);
        Assert.AreEqual(x, y);

        x = 12345678901234567890m;
        (signBit, scaleBits, intBits) = x.Disassemble();
        y = DecimalExtensions.Assemble(signBit, scaleBits, intBits);
        Assert.AreEqual(x, y);

        x = -12345678901234567890m;
        (signBit, scaleBits, intBits) = x.Disassemble();
        y = DecimalExtensions.Assemble(signBit, scaleBits, intBits);
        Assert.AreEqual(x, y);

        x = decimal.MinValue;
        (signBit, scaleBits, intBits) = x.Disassemble();
        y = DecimalExtensions.Assemble(signBit, scaleBits, intBits);
        Assert.AreEqual(x, y);

        x = decimal.MaxValue;
        (signBit, scaleBits, intBits) = x.Disassemble();
        y = DecimalExtensions.Assemble(signBit, scaleBits, intBits);
        Assert.AreEqual(x, y);
    }

    /// <summary>
    /// Get some random decimal values and check Disassemble and Assemble work as expected.
    /// </summary>
    [TestMethod]
    public void TestDisassembleAssembleRandom()
    {
        const int N = 100;
        var rnd = new Random();
        for (var i = 0; i < N; i++)
        {
            var x = rnd.GetDecimal();
            var (signBit, scaleBits, intBits) = x.Disassemble();
            var y = DecimalExtensions.Assemble(signBit, scaleBits, intBits);
            Assert.AreEqual(x, y);
        }
    }
}
