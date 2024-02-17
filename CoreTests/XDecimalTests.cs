using DecimalMath;
using Galaxon.Core.Numbers;
using Galaxon.Core.Numbers.Extensions;
using Galaxon.Core.Testing;

namespace Galaxon.Core.Tests;

[TestClass]
public class XDecimalTests
{
    [TestMethod]
    public void SinhTest()
    {
        decimal m, actual;
        double expected;

        m = 0;
        actual = XDecimal.Sinh(m);
        expected = Sinh((double)m);
        XAssert.AreEqual(expected, actual);

        m = 1;
        actual = XDecimal.Sinh(m);
        expected = Sinh((double)m);
        XAssert.AreEqual(expected, actual);

        m = DecimalEx.PiQuarter;
        actual = XDecimal.Sinh(m);
        expected = Sinh((double)m);
        XAssert.AreEqual(expected, actual);

        m = DecimalEx.Pi;
        actual = XDecimal.Sinh(m);
        expected = Sinh((double)m);
        XAssert.AreEqual(expected, actual);

        m = -1;
        actual = XDecimal.Sinh(m);
        expected = Sinh((double)m);
        XAssert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void CoshTest()
    {
        decimal m, actual;
        double expected;

        m = 0;
        actual = XDecimal.Cosh(m);
        expected = Cosh((double)m);
        XAssert.AreEqual(expected, actual);

        m = 1;
        actual = XDecimal.Cosh(m);
        expected = Cosh((double)m);
        XAssert.AreEqual(expected, actual);

        m = DecimalEx.PiQuarter;
        actual = XDecimal.Cosh(m);
        expected = Cosh((double)m);
        XAssert.AreEqual(expected, actual);

        m = DecimalEx.Pi;
        actual = XDecimal.Cosh(m);
        expected = Cosh((double)m);
        XAssert.AreEqual(expected, actual);

        m = -1;
        actual = XDecimal.Cosh(m);
        expected = Cosh((double)m);
        XAssert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void TanhTest()
    {
        decimal m, actual;
        double expected;

        m = 0;
        actual = XDecimal.Tanh(m);
        expected = Tanh((double)m);
        XAssert.AreEqual(expected, actual);

        m = 1;
        actual = XDecimal.Tanh(m);
        expected = Tanh((double)m);
        XAssert.AreEqual(expected, actual);

        m = DecimalEx.PiQuarter;
        actual = XDecimal.Tanh(m);
        expected = Tanh((double)m);
        XAssert.AreEqual(expected, actual);

        m = DecimalEx.Pi;
        actual = XDecimal.Tanh(m);
        expected = Tanh((double)m);
        XAssert.AreEqual(expected, actual);

        m = -1;
        actual = XDecimal.Tanh(m);
        expected = Tanh((double)m);
        XAssert.AreEqual(expected, actual);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void LnThrowsIfArgZero()
    {
        XDecimal.Log(0);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void LnThrowsIfArgNegative()
    {
        XDecimal.Log(-1);
    }

    [TestMethod]
    public void LnTest()
    {
        decimal m;

        m = 1;
        XAssert.AreEqual(0, XDecimal.Log(m));

        m = 2;
        XAssert.AreEqual(Log((double)m), XDecimal.Log(m));

        m = 10;
        XAssert.AreEqual(Log((double)m), XDecimal.Log(m));

        m = DecimalEx.E;
        XAssert.AreEqual(1, XDecimal.Log(m));

        m = decimal.MaxValue;
        XAssert.AreEqual(Log((double)m), XDecimal.Log(m));

        m = DecimalEx.SmallestNonZeroDec;
        XAssert.AreEqual(Log((double)m), XDecimal.Log(m));

        m = 1.23456789m;
        XAssert.AreEqual(Log((double)m), XDecimal.Log(m));

        m = 9.87654321m;
        XAssert.AreEqual(Log((double)m), XDecimal.Log(m));

        m = 123456789m;
        XAssert.AreEqual(Log((double)m), XDecimal.Log(m));

        m = 9876543210m;
        XAssert.AreEqual(Log((double)m), XDecimal.Log(m));

        m = 0.00000000000000000123456789m;
        XAssert.AreEqual(Log((double)m), XDecimal.Log(m));

        m = 0.00000000000000000987654321m;
        XAssert.AreEqual(Log((double)m), XDecimal.Log(m));
    }

    [TestMethod]
    public void Log1Base0Returns0()
    {
        XAssert.AreEqual(0, XDecimal.Log(1, 0));
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void LogThrowsIfBase1()
    {
        XDecimal.Log(1.234m, 1);
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
        y = XDecimal.Assemble(signBit, scaleBits, intBits);
        Assert.AreEqual(x, y);

        x = 12345678901234567890m;
        (signBit, scaleBits, intBits) = x.Disassemble();
        y = XDecimal.Assemble(signBit, scaleBits, intBits);
        Assert.AreEqual(x, y);

        x = -12345678901234567890m;
        (signBit, scaleBits, intBits) = x.Disassemble();
        y = XDecimal.Assemble(signBit, scaleBits, intBits);
        Assert.AreEqual(x, y);

        x = decimal.MinValue;
        (signBit, scaleBits, intBits) = x.Disassemble();
        y = XDecimal.Assemble(signBit, scaleBits, intBits);
        Assert.AreEqual(x, y);

        x = decimal.MaxValue;
        (signBit, scaleBits, intBits) = x.Disassemble();
        y = XDecimal.Assemble(signBit, scaleBits, intBits);
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
            var y = XDecimal.Assemble(signBit, scaleBits, intBits);
            Assert.AreEqual(x, y);
        }
    }
}
