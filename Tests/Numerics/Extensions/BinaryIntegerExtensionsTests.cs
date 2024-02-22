using System.Diagnostics;
using Galaxon.Numerics.Extensions;

namespace Galaxon.Tests.Core.Numbers;

[TestClass]
public class BinaryIntegerExtensionsTests
{
    #region Superscript and subscript

    [TestMethod]
    public void TestIntToSuperscript()
    {
        int n;
        string s;

        n = 0;
        s = n.ToSuperscript();
        Assert.AreEqual("⁰", s);

        n = 12345;
        s = n.ToSuperscript();
        Assert.AreEqual("¹²³⁴⁵", s);

        n = -12345;
        s = n.ToSuperscript();
        Assert.AreEqual("⁻¹²³⁴⁵", s);
    }

    [TestMethod]
    public void TestIntToSubscript()
    {
        int n;
        string s;

        n = 0;
        s = n.ToSubscript();
        Assert.AreEqual("₀", s);

        n = 12345;
        s = n.ToSubscript();
        Assert.AreEqual("₁₂₃₄₅", s);

        n = -12345;
        s = n.ToSubscript();
        Assert.AreEqual("₋₁₂₃₄₅", s);
    }

    [TestMethod]
    public void DigitsToSuperscript_ReturnsCorrectValues()
    {
        string s1;
        string s2;

        s1 = "x2";
        s2 = BinaryIntegerExtensions.DigitsToSuperscript(s1);
        Assert.AreEqual("x²", s2);

        s1 = "m/s2";
        s2 = BinaryIntegerExtensions.DigitsToSuperscript(s1);
        Assert.AreEqual("m/s²", s2);

        s1 = "23";
        s2 = "6.02 * 10" + BinaryIntegerExtensions.DigitsToSuperscript(s1);
        Assert.AreEqual("6.02 * 10²³", s2);
    }

    [TestMethod]
    public void DigitsToSubscript_ReturnsCorrectValues()
    {
        string s1;
        string s2;

        s1 = "CH4";
        s2 = BinaryIntegerExtensions.DigitsToSubscript(s1);
        Assert.AreEqual("CH₄", s2);

        s1 = "CH3OH";
        s2 = BinaryIntegerExtensions.DigitsToSubscript(s1);
        Assert.AreEqual("CH₃OH", s2);

        s1 = "v0";
        s2 = BinaryIntegerExtensions.DigitsToSubscript(s1);
        Assert.AreEqual("v₀", s2);
    }

    #endregion Superscript and subscript

    #region Sqrt

    [TestMethod]
    public void TestSqrt()
    {
        for (var i = 0; i < 100; i++)
        {
            var expected = (int)double.Sqrt(i);
            var actual = (int)BigIntegerExtensions.TruncSqrt(i);
            Trace.WriteLine($"The truncated square root of {i} is {actual}");
            Assert.AreEqual(expected, actual);
        }
    }

    [TestMethod]
    public void TestSqrtRandom()
    {
        var rnd = new Random();
        for (var i = 0; i < 100; i++)
        {
            var n = rnd.Next();
            var sqrt = double.Sqrt(n);
            var expected = (int)sqrt;
            var actual = (int)BigIntegerExtensions.TruncSqrt(n);
            Trace.WriteLine($"The square root of {n} is {sqrt}. XBigInteger.Sqrt() = {actual}");
            Assert.AreEqual(expected, actual);
        }
    }

    #endregion Sqrt
}
