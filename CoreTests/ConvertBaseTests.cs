using System.Numerics;
using Galaxon.Core.Numbers;
using Galaxon.Core.Strings;

namespace Galaxon.Core.Tests;

[TestClass]
public class ConvertBaseTests
{
    [TestMethod]
    public void TesIntToBaseZero()
    {
        var i = 0;
        var s = i.ToBase(2);
        Assert.AreEqual("0", s);

        s = i.ToBase(12);
        Assert.AreEqual("0", s);

        s = i.ToBase(36);
        Assert.AreEqual("0", s);
    }

    [TestMethod]
    public void TestIntToBase16Positive()
    {
        var i = 1;
        var s = i.ToBase(16);
        Assert.AreEqual("1", s);

        i = 10;
        s = i.ToBase(16);
        Assert.AreEqual("a", s);

        i = 1000;
        s = i.ToBase(16);
        Assert.AreEqual("3e8", s);

        i = 1_000_000;
        s = i.ToBase(16);
        Assert.AreEqual("f4240", s);

        i = int.MaxValue;
        s = i.ToBase(16);
        Assert.AreEqual("7fffffff", s);
    }

    [TestMethod]
    public void TestIntToBase16Negative()
    {
        int i;
        string s;

        i = -1;
        s = i.ToBase(16);
        Assert.AreEqual("-1", s);

        i = -10;
        s = i.ToBase(16);
        Assert.AreEqual("-a", s);

        i = -1000;
        s = i.ToBase(16);
        Assert.AreEqual("-3e8", s);

        i = -1_000_000;
        s = i.ToBase(16);
        Assert.AreEqual("-f4240", s);

        i = int.MinValue;
        s = i.ToBase(16);
        Assert.AreEqual("-80000000", s);
    }

    /// <remarks>
    /// Useful converter:
    /// </remarks>
    /// <see href="https://www.rapidtables.com/convert/number/decimal-to-hex.html"/>
    [TestMethod]
    public void TestToBaseDifferentTypes()
    {
        sbyte sb = -100;
        Assert.AreEqual("-1100100", sb.ToBase(2));

        byte b = 224;
        Assert.AreEqual("11100000", b.ToBase(2));

        short sh = -12345;
        Assert.AreEqual("-11000000111001", sh.ToBase(2));

        ushort ush = 23456;
        Assert.AreEqual("101101110100000", ush.ToBase(2));

        var i = -987_654_321;
        Assert.AreEqual("-3ade68b1", i.ToBase(16));

        uint ui = 123_456_789;
        Assert.AreEqual("75bcd15", ui.ToBase(16));

        var l = -111_222_333_444_555L;
        Assert.AreEqual("-6527f7ad11cb", l.ToBase(16));

        var ul = 111_222_333_444_555uL;
        Assert.AreEqual("6527f7ad11cb", ul.ToBase(16));

        var bi = BigInteger.Parse("-98765432109876543210987654321098765432109876543210");
        Assert.AreEqual("-4393fb25a23480e82908ce2957cfb667d751c67eea", bi.ToBase(16));

        bi = BigInteger.Parse("12345678901234567890123456789012345678901234567890");
        Assert.AreEqual("8727f6369aaf83ca15026747af8c7f196ce3f0ad2", bi.ToBase(16));
    }

    [TestMethod]
    public void TestToBase8MinMax()
    {
        Assert.AreEqual("-200", sbyte.MinValue.ToBase(8));
        Assert.AreEqual("177", sbyte.MaxValue.ToBase(8));
        Assert.AreEqual("377", byte.MaxValue.ToBase(8));
        Assert.AreEqual("-100000", short.MinValue.ToBase(8));
        Assert.AreEqual("77777", short.MaxValue.ToBase(8));
        Assert.AreEqual("177777", ushort.MaxValue.ToBase(8));
        Assert.AreEqual("-20000000000", int.MinValue.ToBase(8));
        Assert.AreEqual("17777777777", int.MaxValue.ToBase(8));
        Assert.AreEqual("37777777777", uint.MaxValue.ToBase(8));
        Assert.AreEqual("-1000000000000000000000", long.MinValue.ToBase(8));
        Assert.AreEqual("777777777777777777777", long.MaxValue.ToBase(8));
        Assert.AreEqual("1777777777777777777777", ulong.MaxValue.ToBase(8));
    }

    [TestMethod]
    public void TestToBase2WithWidth()
    {
        byte x;
        string s;

        x = 0;
        s = x.ToBase(2).ZeroPad(8);
        Assert.AreEqual("00000000", s);

        x = 50;
        s = x.ToBase(2).ZeroPad(8);
        Assert.AreEqual("00110010", s);

        x = 100;
        s = x.ToBase(2).ZeroPad(8);
        Assert.AreEqual("01100100", s);

        x = 200;
        s = x.ToBase(2).ZeroPad(8);
        Assert.AreEqual("11001000", s);
    }

    [TestMethod]
    public void TestToBase16WithWidth()
    {
        long x;
        string s;

        x = 0;
        s = x.ToBase(16).ZeroPad(16);
        Assert.AreEqual("0000000000000000", s);

        x = 12345;
        s = x.ToBase(16).ZeroPad(16);
        Assert.AreEqual("0000000000003039", s);

        x = 987654321;
        s = x.ToBase(16).ZeroPad(16);
        Assert.AreEqual("000000003ade68b1", s);

        x = 20015998341291;
        s = x.ToBase(16).ZeroPad(16);
        Assert.AreEqual("00001234567890ab", s);
    }

    [TestMethod]
    public void TestToBase32WithCase()
    {
        var x = 12345678901234567890;
        Assert.AreEqual("aml59hjlhu2mi", x.ToBase(32));
        Assert.AreEqual("aml59hjlhu2mi", x.ToBase(32));
        Assert.AreEqual("AML59HJLHU2MI", x.ToBase(32, 1, true));
    }

    [TestMethod]
    public void TestToBaseInvalidBase()
    {
        var x = 123456;
        string s;
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => s = x.ToBase(0));
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => s = x.ToBase(1));
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => s = x.ToBase(65));
    }

    /**
     * What tests are needed for FromBase()?
     *   - Converting to int from strings in different bases.
     *   - Converting to different integer types from binary and hex.
     *   - ArgumentNullException
     *   - ArgumentFormatException
     *   - OverflowException
     */
}
