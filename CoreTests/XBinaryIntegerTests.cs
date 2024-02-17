using System.Diagnostics;
using Galaxon.Core.Numbers;
using Galaxon.Core.Numbers.Extensions;

namespace Galaxon.Core.Tests;

[TestClass]
public class XBinaryIntegerTests
{
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
    public void TestSqrt()
    {
        for (var i = 0; i < 100; i++)
        {
            var expected = (int)double.Sqrt(i);
            var actual = (int)XBigInteger.TruncSqrt(i);
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
            var actual = (int)XBigInteger.TruncSqrt(n);
            Trace.WriteLine($"The square root of {n} is {sqrt}. XBigInteger.Sqrt() = {actual}");
            Assert.AreEqual(expected, actual);
        }
    }
}
