using Galaxon.Numerics.Integers;

namespace Galaxon.Numerics.Tests;

[TestClass]
public class MinMaxTests
{
    [TestMethod]
    public void TestMax()
    {
        // byte
        Assert.AreEqual(200, Compare.Max(128, 9, 200, 13, 1, 0));
        // int
        Assert.AreEqual(12345, Compare.Max(-100, -45, 1000, 4, 12345, 0, 9020));
        // double
        Assert.AreEqual(123.2, Compare.Max(45.1, 9.333, 123.2));
        // decimal
        Assert.AreEqual(98383m, Compare.Max(-88m, 133, -1231.008m, 98383m, 176.3m));
        // string
        Assert.AreEqual("cat", Compare.Max("apple", "cat", "ball"));
    }

    [TestMethod]
    public void TestMin()
    {
        // byte
        Assert.AreEqual(0, Compare.Min(128, 9, 200, 13, 1, 0));
        // int
        Assert.AreEqual(-100, Compare.Min(-100, -45, 1000, 4, 12345, 0, 9020));
        // double
        Assert.AreEqual(9.333, Compare.Min(45.1, 9.333, 123.2));
        // decimal
        Assert.AreEqual(-1231.008m, Compare.Min(-88m, 133, -1231.008m, 98383m, 176.3m));
        // string
        Assert.AreEqual("apple", Compare.Min("apple", "cat", "ball"));
    }
}
