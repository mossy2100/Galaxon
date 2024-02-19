using Galaxon.Core.Numbers;
using Galaxon.Numerics.Types;

namespace Galaxon.Numerics.Tests;

[TestClass]
public class TestGigadecimal
{
    // [TestMethod]
    // public void DigitsToSignificandTest()
    // {
    //     byte[] digits = { 1, 2, 3, 4, 5 };
    //     byte[] significand = Gigadecimal.PackDigits(digits);
    //     Assert.AreEqual(18, significand[0]);
    //     Assert.AreEqual(52, significand[1]);
    //     Assert.AreEqual(80, significand[2]);
    // }

    // [TestMethod]
    // public void SignificandToDigitsTest()
    // {
    //     byte[] significand = { 18, 52, 80, 0 };
    //     byte[] digits = Gigadecimal.UnpackDigits(significand);
    //     Assert.AreEqual(1, digits[0]);
    //     Assert.AreEqual(2, digits[1]);
    //     Assert.AreEqual(3, digits[2]);
    //     Assert.AreEqual(4, digits[3]);
    //     Assert.AreEqual(5, digits[4]);
    //     Assert.AreEqual(0, digits[5]);
    //     Assert.AreEqual(0, digits[6]);
    //     Assert.AreEqual(0, digits[7]);
    // }

    [TestMethod]
    public void FromStringPositiveValuesTest()
    {
        string s;
        Gigadecimal gd;

        s = "0";
        gd = new Gigadecimal(s);
        Assert.AreEqual(false, gd.IsNegative);
        Assert.AreEqual(0, gd.Significand.Length);
        Assert.AreEqual(0, gd.Exponent);

        s = "12345";
        gd = new Gigadecimal(s);
        Assert.AreEqual(false, gd.IsNegative);
        Assert.AreEqual(1, gd.Significand.Length);
        Assert.AreEqual(12345, gd.Significand[0]);
        Assert.AreEqual(0, gd.Exponent);

        s = "1234567890";
        gd = new Gigadecimal(s);
        Assert.AreEqual(false, gd.IsNegative);
        Assert.AreEqual(2, gd.Significand.Length);
        Assert.AreEqual(1, gd.Significand[0]);
        Assert.AreEqual(234567890, gd.Significand[1]);
        Assert.AreEqual(1, gd.Exponent);

        s = "1.2345";
        gd = new Gigadecimal(s);
        Assert.AreEqual(false, gd.IsNegative);
        Assert.AreEqual(2, gd.Significand.Length);
        Assert.AreEqual(1, gd.Significand[0]);
        Assert.AreEqual(234500000, gd.Significand[1]);
        Assert.AreEqual(0, gd.Exponent);

        s = "1.2345e10"; // 12_345_000_000
        gd = new Gigadecimal(s);
        Assert.AreEqual(false, gd.IsNegative);
        Assert.AreEqual(2, gd.Significand.Length);
        Assert.AreEqual(12, gd.Significand[0]);
        Assert.AreEqual(345000000, gd.Significand[1]);
        Assert.AreEqual(1, gd.Exponent);

        s = "123.45e10";
        gd = new Gigadecimal(s);
        Assert.AreEqual(false, gd.IsNegative);
        Assert.AreEqual(2, gd.Significand.Length);
        Assert.AreEqual(1234, gd.Significand[0]);
        Assert.AreEqual(500000000, gd.Significand[1]);
        Assert.AreEqual(1, gd.Exponent);

        s = "123.45e-10";
        gd = new Gigadecimal(s);
        Assert.AreEqual(false, gd.IsNegative);
        Assert.AreEqual(2, gd.Significand.Length);
        Assert.AreEqual(12, gd.Significand[0]);
        Assert.AreEqual(345000000, gd.Significand[1]);
        Assert.AreEqual(-1, gd.Exponent);
    }

    [TestMethod]
    public void FromStringNegativeValuesTest()
    {
        string s;
        Gigadecimal gd;

        s = "-12345";
        gd = new Gigadecimal(s);
        Assert.AreEqual(true, gd.IsNegative);
        Assert.AreEqual(1, gd.Significand.Length);
        Assert.AreEqual(-12345, gd.Significand[0]);
        Assert.AreEqual(0, gd.Exponent);

        s = "-1234567890";
        gd = new Gigadecimal(s);
        Assert.AreEqual(true, gd.IsNegative);
        Assert.AreEqual(2, gd.Significand.Length);
        Assert.AreEqual(-1, gd.Significand[0]);
        Assert.AreEqual(-234567890, gd.Significand[1]);
        Assert.AreEqual(1, gd.Exponent);

        s = "-1.2345";
        gd = new Gigadecimal(s);
        Assert.AreEqual(true, gd.IsNegative);
        Assert.AreEqual(2, gd.Significand.Length);
        Assert.AreEqual(-1, gd.Significand[0]);
        Assert.AreEqual(-234500000, gd.Significand[1]);
        Assert.AreEqual(0, gd.Exponent);

        s = "-1.2345e10";
        gd = new Gigadecimal(s);
        Assert.AreEqual(true, gd.IsNegative);
        Assert.AreEqual(2, gd.Significand.Length);
        Assert.AreEqual(-12, gd.Significand[0]);
        Assert.AreEqual(-345000000, gd.Significand[1]);
        Assert.AreEqual(1, gd.Exponent);

        s = "-123.45e10";
        gd = new Gigadecimal(s);
        Assert.AreEqual(true, gd.IsNegative);
        Assert.AreEqual(2, gd.Significand.Length);
        Assert.AreEqual(-1234, gd.Significand[0]);
        Assert.AreEqual(-500000000, gd.Significand[1]);
        Assert.AreEqual(1, gd.Exponent);

        s = "-123.45e-10";
        gd = new Gigadecimal(s);
        Assert.AreEqual(true, gd.IsNegative);
        Assert.AreEqual(2, gd.Significand.Length);
        Assert.AreEqual(-12, gd.Significand[0]);
        Assert.AreEqual(-345000000, gd.Significand[1]);
        Assert.AreEqual(-1, gd.Exponent);
    }

    [TestMethod]
    public void ToStringTest()
    {
        Gigadecimal gd;
        int[] digits = { 111111111, 222222222, 333333333, 444444444, 555555555 };

        gd = new Gigadecimal();
        Assert.AreEqual("0", gd.ToString());

        gd = new Gigadecimal(digits);
        Assert.AreEqual("1.11111111222222222333333333444444444555555555E+8", gd.ToString());

        gd = new Gigadecimal(digits, 1);
        Assert.AreEqual("1.11111111222222222333333333444444444555555555E+17", gd.ToString());

        gd = new Gigadecimal(digits, -1);
        Assert.AreEqual("1.11111111222222222333333333444444444555555555E-1", gd.ToString());
    }

    [TestMethod]
    public void FromUInt64Test()
    {
        ulong ul;
        Gigadecimal gd;

        ul = 0;
        gd = Gigadecimal.FromUInt64(ul);
        Assert.AreEqual(false, gd.IsNegative);
        Assert.AreEqual(0, gd.Significand.Length);
        Assert.AreEqual(0, gd.Exponent);

        ul = 12345;
        gd = Gigadecimal.FromUInt64(ul);
        Assert.AreEqual(false, gd.IsNegative);
        Assert.AreEqual(1, gd.Significand.Length);
        Assert.AreEqual(12345, gd.Significand[0]);
        Assert.AreEqual(0, gd.Exponent);

        ul = 1234567890;
        gd = Gigadecimal.FromUInt64(ul);
        Assert.AreEqual(false, gd.IsNegative);
        Assert.AreEqual(2, gd.Significand.Length);
        Assert.AreEqual(1, gd.Significand[0]);
        Assert.AreEqual(234567890, gd.Significand[1]);
        Assert.AreEqual(1, gd.Exponent);

        // 18,446,744,073,709,551,615
        ul = ulong.MaxValue;
        gd = Gigadecimal.FromUInt64(ul);
        Assert.AreEqual(false, gd.IsNegative);
        Assert.AreEqual(3, gd.Significand.Length);
        Assert.AreEqual(18, gd.Significand[0]);
        Assert.AreEqual(446744073, gd.Significand[1]);
        Assert.AreEqual(709551615, gd.Significand[2]);
        Assert.AreEqual(2, gd.Exponent);
    }

    [TestMethod]
    public void FromInt64Test()
    {
        long l;
        Gigadecimal gd;

        l = 0;
        gd = Gigadecimal.FromInt64(l);
        Assert.AreEqual(false, gd.IsNegative);
        Assert.AreEqual(0, gd.Significand.Length);
        Assert.AreEqual(0, gd.Exponent);

        l = 12345;
        gd = Gigadecimal.FromInt64(l);
        Assert.AreEqual(false, gd.IsNegative);
        Assert.AreEqual(1, gd.Significand.Length);
        Assert.AreEqual(12345, gd.Significand[0]);
        Assert.AreEqual(0, gd.Exponent);

        l = -12345;
        gd = Gigadecimal.FromInt64(l);
        Assert.AreEqual(true, gd.IsNegative);
        Assert.AreEqual(1, gd.Significand.Length);
        Assert.AreEqual(-12345, gd.Significand[0]);
        Assert.AreEqual(0, gd.Exponent);

        l = 1234567890;
        gd = Gigadecimal.FromInt64(l);
        Assert.AreEqual(false, gd.IsNegative);
        Assert.AreEqual(2, gd.Significand.Length);
        Assert.AreEqual(1, gd.Significand[0]);
        Assert.AreEqual(234567890, gd.Significand[1]);
        Assert.AreEqual(1, gd.Exponent);

        l = -1234567890;
        gd = Gigadecimal.FromInt64(l);
        Assert.AreEqual(true, gd.IsNegative);
        Assert.AreEqual(2, gd.Significand.Length);
        Assert.AreEqual(-1, gd.Significand[0]);
        Assert.AreEqual(-234567890, gd.Significand[1]);
        Assert.AreEqual(1, gd.Exponent);

        // 9,223,372,036,854,775,807
        l = long.MaxValue;
        gd = Gigadecimal.FromInt64(l);
        Assert.AreEqual(false, gd.IsNegative);
        Assert.AreEqual(3, gd.Significand.Length);
        Assert.AreEqual(9, gd.Significand[0]);
        Assert.AreEqual(223372036, gd.Significand[1]);
        Assert.AreEqual(854775807, gd.Significand[2]);
        Assert.AreEqual(2, gd.Exponent);

        // -9,223,372,036,854,775,808
        l = long.MinValue;
        gd = Gigadecimal.FromInt64(l);
        Assert.AreEqual(true, gd.IsNegative);
        Assert.AreEqual(3, gd.Significand.Length);
        Assert.AreEqual(-9, gd.Significand[0]);
        Assert.AreEqual(-223372036, gd.Significand[1]);
        Assert.AreEqual(-854775808, gd.Significand[2]);
        Assert.AreEqual(2, gd.Exponent);
    }

    [TestMethod]
    public void FromDecimalPositiveValuesTest()
    {
        decimal d;
        Gigadecimal gd;

        d = 0;
        gd = Gigadecimal.FromDecimal(d);
        Assert.AreEqual(false, gd.IsNegative);
        Assert.AreEqual(0, gd.Significand.Length);
        Assert.AreEqual(0, gd.Exponent);

        d = 1.2345m;
        gd = Gigadecimal.FromDecimal(d);
        Assert.AreEqual(false, gd.IsNegative);
        Assert.AreEqual(1, gd.Significand[0]);
        Assert.AreEqual(234500000, gd.Significand[1]);
        Assert.AreEqual(0, gd.Exponent);

        d = 1.2345e10m;
        gd = Gigadecimal.FromDecimal(d);
        Assert.AreEqual(false, gd.IsNegative);
        Assert.AreEqual(12, gd.Significand[0]);
        Assert.AreEqual(345000000, gd.Significand[1]);
        Assert.AreEqual(1, gd.Exponent);

        d = 123.45e10m;
        gd = Gigadecimal.FromDecimal(d);
        Assert.AreEqual(false, gd.IsNegative);
        Assert.AreEqual(1234, gd.Significand[0]);
        Assert.AreEqual(500000000, gd.Significand[1]);
        Assert.AreEqual(1, gd.Exponent);

        d = 123.45e-10m;
        gd = Gigadecimal.FromDecimal(d);
        Assert.AreEqual(false, gd.IsNegative);
        Assert.AreEqual(12, gd.Significand[0]);
        Assert.AreEqual(345000000, gd.Significand[1]);
        Assert.AreEqual(-1, gd.Exponent);

        // 79,228,162,514,264,337,593,543,950,335
        d = decimal.MaxValue;
        gd = Gigadecimal.FromDecimal(d);
        Assert.AreEqual(false, gd.IsNegative);
        Assert.AreEqual(4, gd.Significand.Length);
        Assert.AreEqual(79, gd.Significand[0]);
        Assert.AreEqual(228162514, gd.Significand[1]);
        Assert.AreEqual(264337593, gd.Significand[2]);
        Assert.AreEqual(543950335, gd.Significand[3]);
        Assert.AreEqual(3, gd.Exponent);

        d = 1e-28m;
        gd = Gigadecimal.FromDecimal(d);
        Assert.AreEqual(false, gd.IsNegative);
        Assert.AreEqual(1, gd.Significand.Length);
        Assert.AreEqual(100000000, gd.Significand[0]);
        Assert.AreEqual(-4, gd.Exponent);
    }

    [TestMethod]
    public void FromDecimalNegativeValuesTest()
    {
        decimal d;
        Gigadecimal gd;

        d = -1.2345m;
        gd = Gigadecimal.FromDecimal(d);
        Assert.AreEqual(true, gd.IsNegative);
        Assert.AreEqual(-1, gd.Significand[0]);
        Assert.AreEqual(-234500000, gd.Significand[1]);
        Assert.AreEqual(0, gd.Exponent);

        d = -1.2345e10m;
        gd = Gigadecimal.FromDecimal(d);
        Assert.AreEqual(true, gd.IsNegative);
        Assert.AreEqual(-12, gd.Significand[0]);
        Assert.AreEqual(-345000000, gd.Significand[1]);
        Assert.AreEqual(1, gd.Exponent);

        d = -123.45e10m;
        gd = Gigadecimal.FromDecimal(d);
        Assert.AreEqual(true, gd.IsNegative);
        Assert.AreEqual(-1234, gd.Significand[0]);
        Assert.AreEqual(-500000000, gd.Significand[1]);
        Assert.AreEqual(1, gd.Exponent);

        d = -123.45e-10m;
        gd = Gigadecimal.FromDecimal(d);
        Assert.AreEqual(true, gd.IsNegative);
        Assert.AreEqual(-12, gd.Significand[0]);
        Assert.AreEqual(-345000000, gd.Significand[1]);
        Assert.AreEqual(-1, gd.Exponent);

        // -79,228,162,514,264,337,593,543,950,335
        d = decimal.MinValue;
        gd = Gigadecimal.FromDecimal(d);
        Assert.AreEqual(true, gd.IsNegative);
        Assert.AreEqual(4, gd.Significand.Length);
        Assert.AreEqual(-79, gd.Significand[0]);
        Assert.AreEqual(-228162514, gd.Significand[1]);
        Assert.AreEqual(-264337593, gd.Significand[2]);
        Assert.AreEqual(-543950335, gd.Significand[3]);
        Assert.AreEqual(3, gd.Exponent);

        d = -1e-28m;
        gd = Gigadecimal.FromDecimal(d);
        Assert.AreEqual(true, gd.IsNegative);
        Assert.AreEqual(1, gd.Significand.Length);
        Assert.AreEqual(-100000000, gd.Significand[0]);
        Assert.AreEqual(-4, gd.Exponent);
    }

    [TestMethod]
    public void FromDoublePositiveValuesTest()
    {
        double d;
        Gigadecimal gd;

        d = 0;
        gd = Gigadecimal.FromDouble(d);
        Assert.AreEqual(false, gd.IsNegative);
        Assert.AreEqual(0, gd.Significand.Length);
        Assert.AreEqual(0, gd.Exponent);

        d = 1.2345;
        gd = Gigadecimal.FromDouble(d);
        Assert.AreEqual(false, gd.IsNegative);
        Assert.AreEqual(1, gd.Significand[0]);
        Assert.AreEqual(234500000, gd.Significand[1], 1);
        Assert.AreEqual(0, gd.Exponent);

        d = 1.2345e10;
        gd = Gigadecimal.FromDouble(d);
        Assert.AreEqual(false, gd.IsNegative);
        Assert.AreEqual(12, gd.Significand[0]);
        Assert.AreEqual(345000000, gd.Significand[1], 1);
        Assert.AreEqual(1, gd.Exponent);

        d = 123.45e10;
        gd = Gigadecimal.FromDouble(d);
        Assert.AreEqual(false, gd.IsNegative);
        Assert.AreEqual(1234, gd.Significand[0]);
        Assert.AreEqual(500000000, gd.Significand[1], 1);
        Assert.AreEqual(1, gd.Exponent);

        d = 123.45e-10;
        gd = Gigadecimal.FromDouble(d);
        Assert.AreEqual(false, gd.IsNegative);
        Assert.AreEqual(12, gd.Significand[0]);
        Assert.AreEqual(345000000, gd.Significand[1], 1);
        Assert.AreEqual(-1, gd.Exponent);

        // 1.7976931348623157E+308
        d = double.MaxValue;
        gd = Gigadecimal.FromDouble(d);
        Assert.AreEqual(false, gd.IsNegative);
        Assert.AreEqual(179, gd.Significand[0]);
        Assert.AreEqual(769313486, gd.Significand[1]);
        Assert.AreEqual(231570000, gd.Significand[2], 10000);
        Assert.AreEqual(34, gd.Exponent);

        // 4.94065645841247E-324
        d = double.Epsilon;
        gd = Gigadecimal.FromDouble(d);
        Assert.AreEqual(false, gd.IsNegative);
        Assert.AreEqual(4, gd.Significand[0]);
        Assert.AreEqual(940656458, gd.Significand[1]);
        Assert.AreEqual(412470000, gd.Significand[2], 10000);
        Assert.AreEqual(-36, gd.Exponent);
    }

    [TestMethod]
    public void ToDoublePositiveValuesTest()
    {
        double d, d2;
        Gigadecimal gd;
        const double percent = 1e-13;

        d = 0;
        gd = Gigadecimal.FromDouble(d);
        d2 = (double)gd;
        Console.WriteLine($"d={d:G17}, gd={gd}, d2={d2:G17}");
        Assert.AreEqual(d, d2, percent);

        d = 1.2345;
        gd = Gigadecimal.FromDouble(d);
        d2 = (double)gd;
        Console.WriteLine($"d={d:G17}, gd={gd}, d2={d2:G17}");
        XAssert.AreEqualPercent(d, d2, percent);

        d = 1.2345e10;
        gd = Gigadecimal.FromDouble(d);
        d2 = (double)gd;
        Console.WriteLine($"d={d:G17}, gd={gd}, d2={d2:G17}");
        XAssert.AreEqualPercent(d, d2, percent);

        d = 123.45e10;
        gd = Gigadecimal.FromDouble(d);
        d2 = (double)gd;
        Console.WriteLine($"d={d:G17}, gd={gd}, d2={d2:G17}");
        XAssert.AreEqualPercent(d, d2, percent);

        d = 123.45e-10;
        gd = Gigadecimal.FromDouble(d);
        d2 = (double)gd;
        Console.WriteLine($"d={d:G17}, gd={gd}, d2={d2:G17}");
        XAssert.AreEqualPercent(d, d2, percent);

        // 1.7976931348623157E+308
        d = double.MaxValue;
        gd = Gigadecimal.FromDouble(d);
        d2 = (double)gd;
        Console.WriteLine($"d={d:G17}, gd={gd}, d2={d2:G17}");
        XAssert.AreEqualPercent(d, d2, percent);

        // // 4.94065645841247E-324
        // d = double.Epsilon;
        // gd = Gigadecimal.FromDouble(d);
        // d2 = (double)gd;
        // Console.WriteLine($"d={d:G17}, gd={gd}, d2={d2:G17}");
        // XAssert.AreEqualPercent(d, d2, percent);
    }

    [TestMethod]
    public void FromDoubleNegativeValuesTest()
    {
        double d;
        Gigadecimal gd;

        d = -1.2345;
        gd = Gigadecimal.FromDouble(d);
        Assert.AreEqual(true, gd.IsNegative);
        Assert.AreEqual(-1, gd.Significand[0]);
        Assert.AreEqual(-234500000, gd.Significand[1], 1);
        Assert.AreEqual(0, gd.Exponent);

        d = -1.2345e10;
        gd = Gigadecimal.FromDouble(d);
        Assert.AreEqual(true, gd.IsNegative);
        Assert.AreEqual(-12, gd.Significand[0]);
        Assert.AreEqual(-345000000, gd.Significand[1], 1);
        Assert.AreEqual(1, gd.Exponent);

        d = -123.45e10;
        gd = Gigadecimal.FromDouble(d);
        Assert.AreEqual(true, gd.IsNegative);
        Assert.AreEqual(-1234, gd.Significand[0]);
        Assert.AreEqual(-500000000, gd.Significand[1], 1);
        Assert.AreEqual(1, gd.Exponent);

        d = -123.45e-10;
        gd = Gigadecimal.FromDouble(d);
        Assert.AreEqual(true, gd.IsNegative);
        Assert.AreEqual(-12, gd.Significand[0]);
        Assert.AreEqual(-345000000, gd.Significand[1], 1);
        Assert.AreEqual(-1, gd.Exponent);

        // -1.7976931348623157E+308
        d = double.MinValue;
        gd = Gigadecimal.FromDouble(d);
        Assert.AreEqual(true, gd.IsNegative);
        Assert.AreEqual(-179, gd.Significand[0]);
        Assert.AreEqual(-769313486, gd.Significand[1]);
        Assert.AreEqual(-231570000, gd.Significand[2], 10000);
        Assert.AreEqual(34, gd.Exponent);

        // -4.94065645841247E-324
        d = -double.Epsilon;
        gd = Gigadecimal.FromDouble(d);
        Assert.AreEqual(true, gd.IsNegative);
        Assert.AreEqual(-4, gd.Significand[0]);
        Assert.AreEqual(-940656458, gd.Significand[1]);
        Assert.AreEqual(-412470000, gd.Significand[2], 10000);
        Assert.AreEqual(-36, gd.Exponent);
    }

    [TestMethod]
    public void AddZeroToNumberReturnsNumber()
    {
        // Arrange.
        Gigadecimal gd1 = new ("5.4321");
        Gigadecimal gd2 = new ("0");

        // Act.
        Gigadecimal gd3 = gd1 + gd2;

        // Assert.
        Assert.AreEqual("5.4321", gd3.ToString());
    }

    [TestMethod]
    public void AddNumberToZeroReturnsNumber()
    {
        // Arrange.
        Gigadecimal gd1 = new ("0");
        Gigadecimal gd2 = new ("5.4321");

        // Act.
        Gigadecimal gd3 = gd1 + gd2;

        // Assert.
        Assert.AreEqual("5.4321", gd3.ToString());
    }

    [TestMethod]
    public void AddTest1()
    {
        // Arrange.
        Gigadecimal gd1 = new ("1");
        Gigadecimal gd2 = new ("1");

        // Act.
        Gigadecimal gd3 = gd1 + gd2;

        // Assert.
        Assert.AreEqual("2", gd3.ToString());
    }

    [TestMethod]
    public void AddTest2()
    {
        // Arrange.
        Gigadecimal gd1 = new ("5.4321");
        Gigadecimal gd2 = new ("6.7894");

        // Act.
        Gigadecimal gd3 = gd1 + gd2;

        // Assert.
        Assert.AreEqual("1.22215E+1", gd3.ToString());
    }

    [TestMethod]
    public void AddTest3()
    {
        // Arrange.
        Gigadecimal gd1 = new ("12345");
        Gigadecimal gd2 = new ("0.67890");

        // Act.
        Gigadecimal gd3 = gd1 + gd2;

        // Assert.
        Assert.AreEqual("1.23456789E+4", gd3.ToString());
    }

    [TestMethod]
    public void AddTest4()
    {
        // Arrange.
        Gigadecimal gd1 = new ("1e108");
        Gigadecimal gd2 = new ("1");

        // Act.
        Gigadecimal gd3 = gd1 + gd2;

        // Assert.
        string s = gd3.ToString();
        Assert.AreEqual("1E+108", s);
    }

    [TestMethod]
    public void AddTest5()
    {
        // Arrange.
        Gigadecimal gd1 = new ("1e107");
        Gigadecimal gd2 = new ("1");
        string expected = "1.0000000000000000000000000000000000000000000000000"
            + "0000000000000000000000000000000000000000000000000"
            + "000000001E+107";

        // Act.
        Gigadecimal gd3 = gd1 + gd2;

        // Assert.
        string s = gd3.ToString();
        Assert.AreEqual(expected, s);
    }

    [TestMethod]
    public void ConvertToAndFromDecimalTest()
    {
        decimal m;
        Gigadecimal gd;

        m = 0;
        gd = m;
        Assert.AreEqual(m, (decimal)gd);

        m = 1234567890;
        gd = m;
        Assert.AreEqual(m, (decimal)gd);

        m = 1.23456789e15m;
        gd = m;
        Assert.AreEqual(m, (decimal)gd);

        m = -9.87654321m;
        gd = m;
        Assert.AreEqual(m, (decimal)gd);

        m = -9.8765432e-20m;
        gd = m;
        Assert.AreEqual(m, (decimal)gd);

        m = decimal.MaxValue;
        gd = m;
        Assert.AreEqual(m, (decimal)gd);

        m = decimal.MinValue;
        gd = m;
        Assert.AreEqual(m, (decimal)gd);
    }

    [TestMethod]
    public void RandomAddTest()
    {
        decimal m1, m2, mSum;
        Gigadecimal gd1, gd2, gdSum;
        Random rng = new ();

        // Create 1000 pairs of random decimals and add them.
        for (int i = 0; i < 1000; i++)
        {
            m1 = rng.NextDecimal();
            m2 = rng.NextDecimal();
            try
            {
                mSum = m1 + m2;
                int scale = (int)Floor(Log10((double)Abs(mSum)));
                // Console.WriteLine($"{m1} + {m2} = {mSum} (scale = {scale})");
                gd1 = m1;
                gd2 = m2;
                gdSum = gd1 + gd2;
                // decimal diff = Abs(mSum - (decimal)gdSum);
                // Check if they agree up to the second-least significant digit.
                Assert.AreEqual(mSum, (decimal)gdSum, XDecimal.Exp10(scale - 26));
                // Console.WriteLine($"Difference = {diff}");
            }
            catch (OverflowException)
            {
                Console.WriteLine("Overflow, continue.");
            }
        }
    }

    [TestMethod]
    public void RoundTest1()
    {
        // Arrange.
        Gigadecimal gd1 = new ("1e107");
        Gigadecimal gd2 = new ("0.5");

        // Act.
        Gigadecimal gd3 = gd1 + gd2;

        string expected = "1E+107";
        // Assert.
        string s = gd3.ToString();
        Assert.AreEqual(expected, s);
    }

    [TestMethod]
    public void RoundTest2()
    {
        // Arrange.
        Gigadecimal gd1 = new ("1E+107");
        Gigadecimal gd2 = new ("0.6");

        // Act.
        Gigadecimal gd3 = gd1 + gd2;

        string expected = "1.0000000000000000000000000000000000000000000000000"
            + "0000000000000000000000000000000000000000000000000000000001E+107";
        // Assert.
        string s = gd3.ToString();
        Assert.AreEqual(expected, s);
    }

    [TestMethod]
    public void RoundTest3()
    {
        // Arrange.
        Gigadecimal gd1 = new ("1E+107");
        Gigadecimal gd2 = new ("1.5");

        // Act.
        Gigadecimal gd3 = gd1 + gd2;

        string expected = "1.0000000000000000000000000000000000000000000000000"
            + "0000000000000000000000000000000000000000000000000000000002E+107";
        // Assert.
        string s = gd3.ToString();
        Assert.AreEqual(expected, s);
    }

    [TestMethod]
    public void SubtractTest1()
    {
        // Arrange.
        Gigadecimal gd1 = new ("1");
        Gigadecimal gd2 = new ("1");

        // Act.
        Gigadecimal gd3 = gd1 - gd2;

        // Assert.
        Assert.AreEqual("0", gd3.ToString());
    }

    [TestMethod]
    public void SubtractTest2()
    {
        // Arrange.
        Gigadecimal gd1 = new ("1");
        Gigadecimal gd2 = new ("0");

        // Act.
        Gigadecimal gd3 = gd1 - gd2;

        // Assert.
        Assert.AreEqual("1", gd3.ToString());
    }

    [TestMethod]
    public void SubtractTest3()
    {
        // Arrange.
        Gigadecimal gd1 = new ("0");
        Gigadecimal gd2 = new ("1");

        // Act.
        Gigadecimal gd3 = gd1 - gd2;

        // Assert.
        Assert.AreEqual("-1", gd3.ToString());
    }

    [TestMethod]
    public void SubtractTest4()
    {
        // Arrange.
        Gigadecimal gd1 = new ("8");
        Gigadecimal gd2 = new ("5");

        // Act.
        Gigadecimal gd3 = gd1 - gd2;

        // Assert.
        Assert.AreEqual("3", gd3.ToString());
    }

    [TestMethod]
    public void SubtractTest5()
    {
        // Arrange.
        Gigadecimal gd1 = new ("5");
        Gigadecimal gd2 = new ("8");

        // Act.
        Gigadecimal gd3 = gd1 - gd2;

        // Assert.
        Assert.AreEqual("-3", gd3.ToString());
    }

    [TestMethod]
    public void SubtractTest6()
    {
        // Arrange.
        Gigadecimal gd1 = new ("-8");
        Gigadecimal gd2 = new ("5");

        // Act.
        Gigadecimal gd3 = gd1 - gd2;

        // Assert.
        Assert.AreEqual("-1.3E+1", gd3.ToString());
    }

    [TestMethod]
    public void SubtractTest7()
    {
        // Arrange.
        Gigadecimal gd1 = new ("5");
        Gigadecimal gd2 = new ("-8");

        // Act.
        Gigadecimal gd3 = gd1 - gd2;

        // Assert.
        Assert.AreEqual("1.3E+1", gd3.ToString());
    }

    [TestMethod]
    public void SubtractTest8()
    {
        // Arrange.
        Gigadecimal gd1 = new ("-5");
        Gigadecimal gd2 = new ("-8");

        // Act.
        Gigadecimal gd3 = gd1 - gd2;

        // Assert.
        Assert.AreEqual("3", gd3.ToString());
    }

    [TestMethod]
    public void SubtractTest9()
    {
        // Arrange.
        Gigadecimal gd1 = new ("-8");
        Gigadecimal gd2 = new ("-5");

        // Act.
        Gigadecimal gd3 = gd1 - gd2;

        // Assert.
        Assert.AreEqual("-3", gd3.ToString());
    }

    [TestMethod]
    public void SubtractTest10()
    {
        // Arrange.
        Gigadecimal gd1 = new ("1");
        Gigadecimal gd2 = new ("1");

        // Act.
        Gigadecimal gd3 = gd1 - gd2;

        // Assert.
        Assert.AreEqual("0", gd3.ToString());
    }

    [TestMethod]
    public void SubtractTest11()
    {
        // Arrange.
        Gigadecimal gd1 = new ("67890");
        Gigadecimal gd2 = new ("12345");

        // Act.
        Gigadecimal gd3 = gd1 - gd2;

        // Assert.
        Assert.AreEqual("5.5545E+4", gd3.ToString());
    }

    [TestMethod]
    public void SubtractTest12()
    {
        // Arrange.
        Gigadecimal gd1 = new ("1234567890");
        Gigadecimal gd2 = new ("9.87654321");

        // Act.
        Gigadecimal gd3 = gd1 - gd2;

        // Assert.
        Assert.AreEqual("1.23456788012345679E+9", gd3.ToString());
    }

    [TestMethod]
    public void SubtractTest13()
    {
        // Arrange.
        Gigadecimal gd1 = new ("1E10");
        Gigadecimal gd2 = new ("1");

        // Act.
        Gigadecimal gd3 = gd1 - gd2;

        // Assert.
        Assert.AreEqual("9.999999999E+9", gd3.ToString());
    }

    [TestMethod]
    public void SubtractTest14()
    {
        // Arrange.
        Gigadecimal gd1 = new ("1");
        Gigadecimal gd2 = new ("1E+10");

        // Act.
        Gigadecimal gd3 = gd1 - gd2;

        // Assert.
        Assert.AreEqual("-9.999999999E+9", gd3.ToString());
    }

    [TestMethod]
    public void MultiplyTest1()
    {
        // Arrange.
        Gigadecimal gd1 = 0;
        Gigadecimal gd2 = 1.234m;

        // Act.
        Gigadecimal gd3 = gd1 * gd2;

        // Assert.
        Assert.AreEqual(0, gd3);
    }

    [TestMethod]
    public void MultiplyTest2()
    {
        // Arrange.
        Gigadecimal gd1 = 6.789m;
        Gigadecimal gd2 = 0;

        // Act.
        Gigadecimal gd3 = gd1 * gd2;

        // Assert.
        Assert.AreEqual(0, gd3);
    }

    [TestMethod]
    public void MultiplyTest4()
    {
        // Arrange.
        Gigadecimal gd1 = 6.789m;
        Gigadecimal gd2 = 1;

        // Act.
        Gigadecimal gd3 = gd1 * gd2;

        // Assert.
        Assert.AreEqual(6.789m, gd3);
    }

    [TestMethod]
    public void MultiplyTest5()
    {
        // Arrange.
        Gigadecimal gd1 = 2350m;
        Gigadecimal gd2 = 15.67m;

        // Act.
        Gigadecimal gd3 = gd1 * gd2;

        // Assert.
        Assert.AreEqual(36824.5m, gd3);
    }

    [TestMethod]
    public void MultiplyTest6()
    {
        // Arrange.
        Gigadecimal gd1 = 1.23e20m;
        Gigadecimal gd2 = 6.78e-12m;

        // Act.
        Gigadecimal gd3 = gd1 * gd2;

        // Assert.
        Assert.AreEqual(833940000m, gd3);
    }

    [TestMethod]
    public void MultiplyTest7()
    {
        // Arrange.
        Gigadecimal gd1 = -10000m;
        Gigadecimal gd2 = 0.5m;

        // Act.
        Gigadecimal gd3 = gd1 * gd2;

        // Assert.
        Assert.AreEqual(-5000m, gd3);
    }

    [TestMethod]
    public void MultiplyTest8()
    {
        // Arrange.
        Gigadecimal gd1 = -9.8712m;
        Gigadecimal gd2 = 481267111m;

        // Act.
        Gigadecimal gd3 = gd1 * gd2;

        // Assert.
        Assert.AreEqual(-4750683906.1032m, gd3);
    }

    [TestMethod]
    public void MultiplyTest9()
    {
        // Arrange.
        Gigadecimal gd1 = -0.0001234m;
        Gigadecimal gd2 = -6789.543m;

        // Act.
        Gigadecimal gd3 = gd1 * gd2;

        // Assert.
        Assert.AreEqual(0.8378296062m, gd3);
    }

    [TestMethod]
    public void MultiplyTest10()
    {
        // Arrange.
        Gigadecimal gd1 = 9.9999m;
        Gigadecimal gd2 = 99999.9m;

        // Act.
        Gigadecimal gd3 = gd1 * gd2;

        // Assert.
        Assert.AreEqual(999989.00001m, gd3);
    }

    [TestMethod]
    public void RandomMultiplyTest()
    {
        double m1, m2, mProd;
        Gigadecimal gd1, gd2, gdProd;
        Random rng = new ();
        double percent = 0.000001;

        // Create 1000 pairs of random decimals and multiply them.
        for (int i = 0; i < 1000; i++)
        {
            m1 = rng.NextDoubleFullRange();
            m2 = rng.NextDoubleFullRange();
            try
            {
                Console.WriteLine($"{m1} * {m2}");

                mProd = m1 * m2;

                if (double.IsInfinity(mProd))
                {
                    throw new OverflowException("Overflow, continue.");
                }

                gd1 = m1;
                gd2 = m2;
                gdProd = gd1 * gd2;

                Console.WriteLine($"Expected = {mProd:G17}");
                Console.WriteLine($"Actual = {gdProd}");
                double actualAsDouble = (double)gdProd;
                Console.WriteLine($"Actual as double = {actualAsDouble:G17}");

                // Check if they agree up to a reasonable number of digits.
                XAssert.AreEqualPercent(mProd, actualAsDouble, percent);
            }
            catch (OverflowException)
            {
                Console.WriteLine("Overflow, continue.");
            }
            Console.WriteLine();
        }
    }

    [TestMethod]
    public void ReciprocalTest1()
    {
        Gigadecimal x, y;

        x = 2;
        y = Gigadecimal.Reciprocal(x);
        Assert.AreEqual(1, y.Length);
        Assert.AreEqual(500000000, y.Significand[0]);
        Assert.AreEqual(-1, y.Exponent);
        Console.WriteLine(y);

        x = 3;
        y = Gigadecimal.Reciprocal(x);
        Assert.AreEqual(12, y.Length);
        Assert.AreEqual(333333333, y.Significand[0]);
        Assert.AreEqual(333333333, y.Significand[1]);
        Assert.AreEqual(333333333, y.Significand[2]);
        Assert.AreEqual(333333333, y.Significand[3]);
        Assert.AreEqual(333333333, y.Significand[4]);
        Assert.AreEqual(333333333, y.Significand[5]);
        Assert.AreEqual(333333333, y.Significand[6]);
        Assert.AreEqual(333333333, y.Significand[7]);
        Assert.AreEqual(333333333, y.Significand[8]);
        Assert.AreEqual(333333333, y.Significand[9]);
        Assert.AreEqual(333333333, y.Significand[10]);
        Assert.AreEqual(333333333, y.Significand[11]);
        Assert.AreEqual(-1, y.Exponent);
        Console.WriteLine(y);

        x = 4;
        y = Gigadecimal.Reciprocal(x);
        Assert.AreEqual(1, y.Length);
        Assert.AreEqual(250000000, y.Significand[0]);
        Assert.AreEqual(-1, y.Exponent);
        Console.WriteLine(y);

        x = 5;
        y = Gigadecimal.Reciprocal(x);
        Assert.AreEqual(1, y.Length);
        Assert.AreEqual(200000000, y.Significand[0]);
        Assert.AreEqual(-1, y.Exponent);
        Console.WriteLine(y);

        x = 6;
        y = Gigadecimal.Reciprocal(x);
        Assert.AreEqual(12, y.Length);
        Assert.AreEqual(166666666, y.Significand[0]);
        Assert.AreEqual(666666666, y.Significand[1]);
        Assert.AreEqual(666666666, y.Significand[2]);
        Assert.AreEqual(666666666, y.Significand[3]);
        Assert.AreEqual(666666666, y.Significand[4]);
        Assert.AreEqual(666666666, y.Significand[5]);
        Assert.AreEqual(666666666, y.Significand[6]);
        Assert.AreEqual(666666666, y.Significand[7]);
        Assert.AreEqual(666666666, y.Significand[8]);
        Assert.AreEqual(666666666, y.Significand[9]);
        Assert.AreEqual(666666666, y.Significand[10]);
        Assert.AreEqual(666666667, y.Significand[11]);
        Assert.AreEqual(-1, y.Exponent);
        Console.WriteLine(y);

        x = 7;
        y = Gigadecimal.Reciprocal(x);
        Assert.AreEqual(12, y.Length);
        Assert.AreEqual(142857142, y.Significand[0]);
        Assert.AreEqual(857142857, y.Significand[1]);
        Assert.AreEqual(142857142, y.Significand[2]);
        Assert.AreEqual(857142857, y.Significand[3]);
        Assert.AreEqual(142857142, y.Significand[4]);
        Assert.AreEqual(857142857, y.Significand[5]);
        Assert.AreEqual(142857142, y.Significand[6]);
        Assert.AreEqual(857142857, y.Significand[7]);
        Assert.AreEqual(142857142, y.Significand[8]);
        Assert.AreEqual(857142857, y.Significand[9]);
        Assert.AreEqual(142857142, y.Significand[10]);
        Assert.AreEqual(857142857, y.Significand[11]);
        Assert.AreEqual(-1, y.Exponent);
        Console.WriteLine(y);

        x = 8;
        y = Gigadecimal.Reciprocal(x);
        Assert.AreEqual(1, y.Length);
        Assert.AreEqual(125000000, y.Significand[0]);
        Assert.AreEqual(-1, y.Exponent);
        Console.WriteLine(y);

        x = 9;
        y = Gigadecimal.Reciprocal(x);
        Assert.AreEqual(12, y.Length);
        Assert.AreEqual(111111111, y.Significand[0]);
        Assert.AreEqual(111111111, y.Significand[1]);
        Assert.AreEqual(111111111, y.Significand[2]);
        Assert.AreEqual(111111111, y.Significand[3]);
        Assert.AreEqual(111111111, y.Significand[4]);
        Assert.AreEqual(111111111, y.Significand[5]);
        Assert.AreEqual(111111111, y.Significand[6]);
        Assert.AreEqual(111111111, y.Significand[7]);
        Assert.AreEqual(111111111, y.Significand[8]);
        Assert.AreEqual(111111111, y.Significand[9]);
        Assert.AreEqual(111111111, y.Significand[10]);
        Assert.AreEqual(111111111, y.Significand[11]);
        Assert.AreEqual(-1, y.Exponent);
        Console.WriteLine(y);
    }

    [TestMethod]
    public void RandomDivideTest()
    {
        double m1, m2, m3;
        Gigadecimal gd1, gd2, gd3;
        Random rng = new ();
        double percent = 0.000001;
        int n = 100;

        // Create pairs of random decimals and multiply them.
        for (int i = 0; i < n; i++)
        {
            m1 = rng.NextDoubleFullRange();
            m2 = rng.NextDoubleFullRange();
            try
            {
                Console.WriteLine($"{m1} / {m2}");

                if (m2 == 0)
                {
                    throw new OverflowException("Division by zero, continue.");
                }

                m3 = m1 / m2;

                if (double.IsInfinity(m3))
                {
                    throw new OverflowException("Overflow, continue.");
                }

                gd1 = m1;
                gd2 = m2;
                gd3 = Gigadecimal.Divide(gd1, gd2);

                Console.WriteLine($"Expected = {m3:G17}");
                Console.WriteLine($"Actual = {gd3}");
                double actualAsDouble = (double)gd3;
                Console.WriteLine($"Actual as double = {actualAsDouble:G17}");

                // Check if they agree up to a reasonable number of digits.
                XAssert.AreEqualPercent(m3, actualAsDouble, percent);
            }
            catch (OverflowException)
            {
                Console.WriteLine("Overflow, continue.");
            }
            Console.WriteLine();
        }
    }

    [TestMethod]
    public void RandomReciprocalTest()
    {
        double m1, m2;
        Gigadecimal gd1, gd2;
        Random rng = new ();
        // double percent = 0.001;
        int n = 100;

        // Create pairs of random decimals and multiply them.
        for (int i = 0; i < n; i++)
        {
            m1 = rng.NextDoubleFullRange();
            try
            {
                Console.WriteLine($"1 / {m1}");

                if (m1 == 0)
                {
                    throw new OverflowException("Division by zero, continue.");
                }

                m2 = 1 / m1;

                if (double.IsInfinity(m2))
                {
                    throw new OverflowException("Overflow, continue.");
                }

                gd1 = m1;
                gd2 = Gigadecimal.Reciprocal(gd1);

                Console.WriteLine($"Expected = {m2:G17}");
                Console.WriteLine($"Actual = {gd2}");
            }
            catch (OverflowException)
            {
                Console.WriteLine("Overflow, continue.");
            }
            Console.WriteLine();
        }
    }

    [TestMethod]
    public void DivideGoldschmidtTest1()
    {
        Gigadecimal gd1, gd2, gdResult;
        for (int j = 1; j <= 10; j++)
        {
            for (int i = 1; i <= 10; i++)
            {
                Console.WriteLine($"{i} / {j} = ");
                gd1 = i;
                gd2 = j;
                gdResult = Gigadecimal.Divide(gd1, gd2);
                Console.WriteLine(gdResult);
                Console.WriteLine();
            }
        }
    }
}
