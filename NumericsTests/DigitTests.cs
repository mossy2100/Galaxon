using Galaxon.Numerics.Integers;

namespace Galaxon.Numerics.Tests;

[TestClass]
public class DigitTests
{
    [TestMethod]
    public void TestNumDigits1()
    {
        Assert.AreEqual(1, Digits.NumDigits(0));
        Assert.AreEqual(1, Digits.NumDigits(1));
        Assert.AreEqual(1, Digits.NumDigits(2));
        Assert.AreEqual(1, Digits.NumDigits(9));
        Assert.AreEqual(2, Digits.NumDigits(10));
        Assert.AreEqual(2, Digits.NumDigits(11));
        Assert.AreEqual(2, Digits.NumDigits(99));
        Assert.AreEqual(3, Digits.NumDigits(100));
    }

    [TestMethod]
    public void TestNumDigits2()
    {
        Assert.AreEqual(1, Digits.NumDigits(0));
        Assert.AreEqual(1, Digits.NumDigits(1));
        Assert.AreEqual(1, Digits.NumDigits(-1));
        Assert.AreEqual(1, Digits.NumDigits(9));
        Assert.AreEqual(1, Digits.NumDigits(-9));
        Assert.AreEqual(2, Digits.NumDigits(10));
        Assert.AreEqual(2, Digits.NumDigits(99));
        Assert.AreEqual(3, Digits.NumDigits(100));
        Assert.AreEqual(3, Digits.NumDigits(101));
        Assert.AreEqual(10, Digits.NumDigits(1000000000));
    }
}
