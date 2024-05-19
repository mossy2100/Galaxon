using Galaxon.Numerics.Extensions;

namespace Galaxon.Tests.Numerics.Extensions;

[TestClass]
public class NumberExtensionsTests
{
    [TestMethod]
    public void TestMod_WithPositiveDivisorPositiveDividendPositiveResult()
    {
        Assert.AreEqual(1, NumberExtensions.Mod(10, 3), "10 mod 3 should be 1.");
    }

    [TestMethod]
    public void TestMod_WithPositiveDivisorPositiveDividendZeroResult()
    {
        Assert.AreEqual(0, NumberExtensions.Mod(10, 5), "10 mod 5 should be 0.");
    }

    [TestMethod]
    public void TestMod_WithPositiveDivisorNegativeDividendPositiveResult()
    {
        Assert.AreEqual(2, NumberExtensions.Mod(-10, 3), "-10 mod 3 should be 2.");
    }

    [TestMethod]
    public void TestMod_WithPositiveDivisorNegativeDividendZeroResult()
    {
        Assert.AreEqual(0, NumberExtensions.Mod(-10, 5), "-10 mod 5 should be 0.");
    }

    [TestMethod]
    public void TestMod_WithNegativeDivisorPositiveDividendNegativeResult()
    {
        Assert.AreEqual(-2, NumberExtensions.Mod(10, -3), "10 mod -3 should be -2.");
    }

    [TestMethod]
    public void TestMod_WithNegativeDivisorPositiveDividendZeroResult()
    {
        Assert.AreEqual(0, NumberExtensions.Mod(10, -5), "10 mod -5 should be 0.");
    }

    [TestMethod]
    public void TestMod_WithNegativeDivisorNegativeDividendNegativeResult()
    {
        Assert.AreEqual(-1, NumberExtensions.Mod(-10, -3), "-10 mod -3 should be -1.");
    }

    [TestMethod]
    public void TestMod_WithNegativeDivisorNegativeDividendZeroResult()
    {
        Assert.AreEqual(0, NumberExtensions.Mod(-10, -5), "-10 mod -5 should be 0.");
    }

    [TestMethod]
    public void TestMod_WithZeroDividend()
    {
        Assert.AreEqual(0, NumberExtensions.Mod(0, 3), "0 mod 3 should be 0.");
    }

    [TestMethod]
    [ExpectedException(typeof(DivideByZeroException))]
    public void TestMod_WithZeroDivisor()
    {
        NumberExtensions.Mod(10, 0);
    }
}
