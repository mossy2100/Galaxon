using Galaxon.Core.Numbers;

namespace Galaxon.Core.Tests;

// ReSharper disable CompareOfFloatsByEqualityOperator
[TestClass]
public class XDoubleTests
{
    [TestMethod]
    public void TestFuzzyEquals()
    {
        double d1, d2;

        // Test examples of double equals comparison failures.
        d1 = 0.1 * 3;
        d2 = 0.3;
        Assert.IsFalse(d1 == d2);
        Assert.IsTrue(d1.FuzzyEquals(d2));

        d1 = 0.1 + 0.1 + 0.1 + 0.1 + 0.1 + 0.1 + 0.1 + 0.1 + 0.1 + 0.1 + 0.1;
        d2 = 0.1 * 11;
        Assert.IsFalse(d1 == d2);
        Assert.IsTrue(d1.FuzzyEquals(d2));

        // Make sure the function works as expected with +0 and -0.
        d1 = 0d;
        d2 = -0d;
        Assert.IsTrue(d1 == d2);
        Assert.IsTrue(d1.FuzzyEquals(d2));

        // Make sure the function works as expected with limit values.
        d1 = double.MinValue;
        d2 = double.MaxValue;
        Assert.IsFalse(d1 == d2);
        Assert.IsFalse(d1.FuzzyEquals(d2));

        d1 = double.MinValue;
        d2 = -double.MinValue;
        Assert.IsFalse(d1 == d2);
        Assert.IsFalse(d1.FuzzyEquals(d2));

        d1 = double.MaxValue;
        d2 = -double.MaxValue;
        Assert.IsFalse(d1 == d2);
        Assert.IsFalse(d1.FuzzyEquals(d2));

        // Make sure the function works as expected with infinities.
        d1 = double.NegativeInfinity;
        d2 = double.PositiveInfinity;
        Assert.IsFalse(d1 == d2);
        Assert.IsFalse(d1.FuzzyEquals(d2));
    }
}
