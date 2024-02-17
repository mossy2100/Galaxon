using System.Numerics;
using DecimalMath;
using Galaxon.Core.Testing;
using Galaxon.Numerics.Types;

namespace Galaxon.Numerics.Tests;

[TestClass]
public class TestDecimalComplex
{
    /// <summary>
    /// Test the Equals() method and the equality and inequality operators.
    /// </summary>
    [TestMethod]
    public void EqualsTest()
    {
        DecimalComplex? z1, z2;

        z1 = null;
        z2 = null;
        Assert.IsTrue(z1.Equals(z2));
        Assert.IsTrue(z1 == z2);
        Assert.IsFalse(z1 != z2);

        z1 = null;
        z2 = DecimalComplex.I;
        Assert.IsFalse(z1.Equals(z2));
        Assert.IsTrue(z1 != z2);
        Assert.IsFalse(z1 == z2);

        z1 = new DecimalComplex(6, 23);
        z2 = null;
        Assert.IsFalse(z1.Equals(z2));
        Assert.IsTrue(z1 != z2);
        Assert.IsFalse(z1 == z2);

        z1 = DecimalComplex.I;
        z2 = DecimalComplex.I;
        Assert.IsTrue(z1.Equals(z2));
        Assert.IsTrue(z1 == z2);
        Assert.IsFalse(z1 != z2);

        z1 = 3 + 5 * DecimalComplex.I;
        z2 = 2 + 8 * DecimalComplex.I;
        Assert.IsFalse(z1.Equals(z2));
        Assert.IsTrue(z1 != z2);
        Assert.IsFalse(z1 == z2);

        z1 = 3 + 5 * DecimalComplex.I;
        z2 = new DecimalComplex(3, 5);
        Assert.IsTrue(z1.Equals(z2));
        Assert.IsTrue(z1 == z2);
        Assert.IsFalse(z1 != z2);
    }

    [TestMethod]
    public void ToStringTest()
    {
        DecimalComplex z;

        z = DecimalComplex.Zero;
        Assert.AreEqual("0", z.ToString());

        z = DecimalComplex.One;
        Assert.AreEqual("1", z.ToString());

        z = -DecimalComplex.One;
        Assert.AreEqual("-1", z.ToString());

        z = DecimalComplex.I;
        Assert.AreEqual("i", z.ToString());

        z = -DecimalComplex.I;
        Assert.AreEqual("-i", z.ToString());

        z = new DecimalComplex(5.123m, 0);
        Assert.AreEqual("5.123", z.ToString());

        z = new DecimalComplex(-6.789m, 0);
        Assert.AreEqual("-6.789", z.ToString());

        z = new DecimalComplex(0, 5.123m);
        Assert.AreEqual("5.123i", z.ToString());

        z = new DecimalComplex(0, -6.789m);
        Assert.AreEqual("-6.789i", z.ToString());

        z = new DecimalComplex(3, 6);
        Assert.AreEqual("3 + 6i", z.ToString());

        z = new DecimalComplex(3.14m, 6.88m);
        Assert.AreEqual("3.14 + 6.88i", z.ToString());

        z = new DecimalComplex(-2, 5);
        Assert.AreEqual("-2 + 5i", z.ToString());

        z = new DecimalComplex(9, -2);
        Assert.AreEqual("9 - 2i", z.ToString());

        z = new DecimalComplex(-17, -7);
        Assert.AreEqual("-17 - 7i", z.ToString());
    }

    [TestMethod]
    public void MagnitudePhaseTest()
    {
        DecimalComplex z1;
        Complex z2;

        z1 = DecimalComplex.Zero;
        z2 = Complex.Zero;
        XAssert.AreEqual(Complex.Abs(z2), DecimalComplex.Abs(z1));
        XAssert.AreEqual(z2.Magnitude, z1.Magnitude);
        XAssert.AreEqual(z2.Phase, z1.Phase);

        z1 = DecimalComplex.One;
        z2 = Complex.One;
        XAssert.AreEqual(Complex.Abs(z2), DecimalComplex.Abs(z1));
        XAssert.AreEqual(z2.Magnitude, z1.Magnitude);
        XAssert.AreEqual(z2.Phase, z1.Phase);

        z1 = DecimalComplex.ImaginaryOne;
        z2 = Complex.ImaginaryOne;
        XAssert.AreEqual(Complex.Abs(z2), DecimalComplex.Abs(z1));
        XAssert.AreEqual(z2.Magnitude, z1.Magnitude);
        XAssert.AreEqual(z2.Phase, z1.Phase);

        z1 = -DecimalComplex.One;
        z2 = new Complex(-1, 0);
        XAssert.AreEqual(Complex.Abs(z2), DecimalComplex.Abs(z1));
        XAssert.AreEqual(z2.Magnitude, z1.Magnitude);
        XAssert.AreEqual(z2.Phase, z1.Phase);

        z1 = -DecimalComplex.ImaginaryOne;
        z2 = -Complex.ImaginaryOne;
        XAssert.AreEqual(Complex.Abs(z2), DecimalComplex.Abs(z1));
        XAssert.AreEqual(z2.Magnitude, z1.Magnitude);
        XAssert.AreEqual(z2.Phase, z1.Phase);

        z1 = new DecimalComplex(1, 1);
        z2 = new Complex(1, 1);
        XAssert.AreEqual(Complex.Abs(z2), DecimalComplex.Abs(z1));
        XAssert.AreEqual(z2.Magnitude, z1.Magnitude);
        XAssert.AreEqual(z2.Phase, z1.Phase);

        z1 = new DecimalComplex(-1, -1);
        z2 = new Complex(-1, -1);
        XAssert.AreEqual(Complex.Abs(z2), DecimalComplex.Abs(z1));
        XAssert.AreEqual(z2.Magnitude, z1.Magnitude);
        XAssert.AreEqual(z2.Phase, z1.Phase);

        z1 = new DecimalComplex(3, 4);
        z2 = new Complex(3, 4);
        XAssert.AreEqual(Complex.Abs(z2), DecimalComplex.Abs(z1));
        XAssert.AreEqual(z2.Magnitude, z1.Magnitude);
        XAssert.AreEqual(z2.Phase, z1.Phase);

        z1 = new DecimalComplex(-5, 6);
        z2 = new Complex(-5, 6);
        XAssert.AreEqual(Complex.Abs(z2), DecimalComplex.Abs(z1));
        XAssert.AreEqual(z2.Magnitude, z1.Magnitude);
        XAssert.AreEqual(z2.Phase, z1.Phase);

        z1 = new DecimalComplex(3.14m, 2.81m);
        z2 = new Complex(3.14, 2.81);
        XAssert.AreEqual(Complex.Abs(z2), DecimalComplex.Abs(z1));
        XAssert.AreEqual(z2.Magnitude, z1.Magnitude);
        XAssert.AreEqual(z2.Phase, z1.Phase);

        z1 = new DecimalComplex(-3.14m, -2.81m);
        z2 = new Complex(-3.14, -2.81);
        XAssert.AreEqual(Complex.Abs(z2), DecimalComplex.Abs(z1));
        XAssert.AreEqual(z2.Magnitude, z1.Magnitude);
        XAssert.AreEqual(z2.Phase, z1.Phase);
    }

    [TestMethod]
    public void FromPolarCoordinatesTest()
    {
        DecimalComplex z1;
        Complex z2;

        z1 = DecimalComplex.FromPolarCoordinates(0, 0);
        z2 = Complex.FromPolarCoordinates(0, 0);
        DecimalComplex.AssertAreEqual(z2, z1);

        z1 = DecimalComplex.FromPolarCoordinates(1, 0);
        z2 = Complex.FromPolarCoordinates(1, 0);
        DecimalComplex.AssertAreEqual(z2, z1);

        z1 = DecimalComplex.FromPolarCoordinates(1, DecimalEx.PiHalf);
        z2 = Complex.FromPolarCoordinates(1, PI / 2);
        DecimalComplex.AssertAreEqual(z2, z1);

        z1 = DecimalComplex.FromPolarCoordinates(1, -DecimalEx.PiHalf);
        z2 = Complex.FromPolarCoordinates(1, -PI / 2);
        DecimalComplex.AssertAreEqual(z2, z1);

        z1 = DecimalComplex.FromPolarCoordinates(1.23456789m, 1.23456789m);
        z2 = Complex.FromPolarCoordinates(1.23456789, 1.23456789);
        DecimalComplex.AssertAreEqual(z2, z1);

        z1 = DecimalComplex.FromPolarCoordinates(1.23456789m, -1.23456789m);
        z2 = Complex.FromPolarCoordinates(1.23456789, -1.23456789);
        DecimalComplex.AssertAreEqual(z2, z1);
    }
}
