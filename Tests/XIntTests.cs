using Galaxon.Core.Numbers.Extensions;

namespace Galaxon.Core.Tests;

[TestClass]
public class XIntTests
{
    [TestMethod]
    public void TestSqrt()
    {
        for (var i = 1; i <= 10; i++)
        {
            Assert.AreEqual(i, XInt.Sqrt(i * i));
        }
    }

    [TestMethod]
    public void TestSqrt2()
    {
        Assert.AreEqual(1, XInt.Sqrt(1));
        Assert.AreEqual(1, XInt.Sqrt(2));
        Assert.AreEqual(2, XInt.Sqrt(3));
        Assert.AreEqual(2, XInt.Sqrt(4));
        Assert.AreEqual(2, XInt.Sqrt(5));
        Assert.AreEqual(2, XInt.Sqrt(6));
        Assert.AreEqual(3, XInt.Sqrt(7));
        Assert.AreEqual(3, XInt.Sqrt(8));
        Assert.AreEqual(3, XInt.Sqrt(9));
        Assert.AreEqual(3, XInt.Sqrt(10));
    }
}
