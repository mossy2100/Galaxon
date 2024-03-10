using Galaxon.Numerics.Extensions;

namespace Galaxon.Tests.Numerics.Extensions;

[TestClass]
public class Int32ExtensionsTests
{
    [TestMethod]
    public void TestSqrt()
    {
        for (var i = 1; i <= 10; i++)
        {
            Assert.AreEqual(i, Int32Extensions.Sqrt(i * i));
        }
    }

    [TestMethod]
    public void TestSqrt2()
    {
        Assert.AreEqual(1, Int32Extensions.Sqrt(1));
        Assert.AreEqual(1, Int32Extensions.Sqrt(2));
        Assert.AreEqual(2, Int32Extensions.Sqrt(3));
        Assert.AreEqual(2, Int32Extensions.Sqrt(4));
        Assert.AreEqual(2, Int32Extensions.Sqrt(5));
        Assert.AreEqual(2, Int32Extensions.Sqrt(6));
        Assert.AreEqual(3, Int32Extensions.Sqrt(7));
        Assert.AreEqual(3, Int32Extensions.Sqrt(8));
        Assert.AreEqual(3, Int32Extensions.Sqrt(9));
        Assert.AreEqual(3, Int32Extensions.Sqrt(10));
    }
}
