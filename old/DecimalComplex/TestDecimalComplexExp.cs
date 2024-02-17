using System.Numerics;
using DecimalMath;
using Galaxon.Numerics.Types;

namespace Galaxon.Numerics.Tests;

[TestClass]
public class TestDecimalComplexExp
{
    [TestMethod]
    public void SqrtTest()
    {
        DecimalComplex z1;
        Complex z2;

        z1 = DecimalComplex.Zero;
        z2 = Complex.Zero;
        DecimalComplex.AssertAreEqual(Complex.Sqrt(z2), DecimalComplex.Sqrt(z1));

        z1 = DecimalComplex.One;
        z2 = Complex.One;
        DecimalComplex.AssertAreEqual(Complex.Sqrt(z2), DecimalComplex.Sqrt(z1));

        z1 = DecimalComplex.ImaginaryOne;
        z2 = Complex.ImaginaryOne;
        DecimalComplex.AssertAreEqual(Complex.Sqrt(z2), DecimalComplex.Sqrt(z1));

        z1 = new DecimalComplex(1, 1);
        z2 = new Complex(1, 1);
        DecimalComplex.AssertAreEqual(Complex.Sqrt(z2), DecimalComplex.Sqrt(z1));

        z1 = new DecimalComplex(3, 4);
        z2 = new Complex(3, 4);
        DecimalComplex.AssertAreEqual(Complex.Sqrt(z2), DecimalComplex.Sqrt(z1));

        z1 = new DecimalComplex(-5, 6);
        z2 = new Complex(-5, 6);
        DecimalComplex.AssertAreEqual(Complex.Sqrt(z2), DecimalComplex.Sqrt(z1));

        z1 = new DecimalComplex(3.14m, 2.81m);
        z2 = new Complex(3.14, 2.81);
        DecimalComplex.AssertAreEqual(Complex.Sqrt(z2), DecimalComplex.Sqrt(z1));

        z1 = new DecimalComplex(-3.14m, -2.81m);
        z2 = new Complex(-3.14, -2.81);
        DecimalComplex.AssertAreEqual(Complex.Sqrt(z2), DecimalComplex.Sqrt(z1));
    }

    [TestMethod]
    public void ReciprocalTest()
    {
        DecimalComplex z1;
        Complex z2;

        z1 = DecimalComplex.One;
        z2 = Complex.One;
        DecimalComplex.AssertAreEqual(Complex.Reciprocal(z2), DecimalComplex.Reciprocal(z1));

        z1 = DecimalComplex.ImaginaryOne;
        z2 = Complex.ImaginaryOne;
        DecimalComplex.AssertAreEqual(Complex.Reciprocal(z2), DecimalComplex.Reciprocal(z1));

        z1 = -DecimalComplex.One;
        z2 = -Complex.One;
        DecimalComplex.AssertAreEqual(Complex.Reciprocal(z2), DecimalComplex.Reciprocal(z1));

        z1 = -DecimalComplex.ImaginaryOne;
        z2 = -Complex.ImaginaryOne;
        DecimalComplex.AssertAreEqual(Complex.Reciprocal(z2), DecimalComplex.Reciprocal(z1));

        z1 = new DecimalComplex(1, 1);
        z2 = new Complex(1, 1);
        DecimalComplex.AssertAreEqual(Complex.Reciprocal(z2), DecimalComplex.Reciprocal(z1));

        z1 = new DecimalComplex(3, 4);
        z2 = new Complex(3, 4);
        DecimalComplex.AssertAreEqual(Complex.Reciprocal(z2), DecimalComplex.Reciprocal(z1));

        z1 = new DecimalComplex(-5, 6);
        z2 = new Complex(-5, 6);
        DecimalComplex.AssertAreEqual(Complex.Reciprocal(z2), DecimalComplex.Reciprocal(z1));

        z1 = new DecimalComplex(3.14m, 2.81m);
        z2 = new Complex(3.14, 2.81);
        DecimalComplex.AssertAreEqual(Complex.Reciprocal(z2), DecimalComplex.Reciprocal(z1));

        z1 = new DecimalComplex(-3.14m, -2.81m);
        z2 = new Complex(-3.14, -2.81);
        DecimalComplex.AssertAreEqual(Complex.Reciprocal(z2), DecimalComplex.Reciprocal(z1));
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void LnThrowsExceptionIfArgZero() => DecimalComplex.Log(DecimalComplex.Zero);

    [TestMethod]
    public void DecimalComplexLnMatchesComplexLog()
    {
        DecimalComplex dc;
        Complex c;

        // 1
        dc = DecimalComplex.One;
        c = Complex.One;
        DecimalComplex.AssertAreEqual(Complex.Log(c), DecimalComplex.Log(dc));

        // i
        dc = DecimalComplex.ImaginaryOne;
        c = Complex.ImaginaryOne;
        DecimalComplex.AssertAreEqual(Complex.Log(c), DecimalComplex.Log(dc));

        // -1
        dc = -DecimalComplex.One;
        // NB: Setting c = -Complex.One doesn't work here.
        // The Complex unary negation operator negates a 0 real or imaginary
        // part to -0, which is valid for double, but it causes Atan2() to
        // return -π instead of π for the phase, thus causing Log() to return
        // the wrong result.
        c = new Complex(-1, 0);
        DecimalComplex.AssertAreEqual(Complex.Log(c), DecimalComplex.Log(dc));

        // -i
        dc = -DecimalComplex.ImaginaryOne;
        // Cannot use -Complex.ImaginaryOne; See above note.
        c = new Complex(0, -1);
        DecimalComplex.AssertAreEqual(Complex.Log(c), DecimalComplex.Log(dc));

        // 1+i
        dc = new DecimalComplex(1, 1);
        c = new Complex(1, 1);
        DecimalComplex.AssertAreEqual(Complex.Log(c), DecimalComplex.Log(dc));

        // 1-i
        dc = new DecimalComplex(1, -1);
        c = new Complex(1, -1);
        DecimalComplex.AssertAreEqual(Complex.Log(c), DecimalComplex.Log(dc));

        // 3.14+2.81i
        dc = new DecimalComplex(3.14m, 2.81m);
        c = new Complex(3.14, 2.81);
        DecimalComplex.AssertAreEqual(Complex.Log(c), DecimalComplex.Log(dc));

        // 3.14-2.81i
        dc = new DecimalComplex(3.14m, -2.81m);
        c = new Complex(3.14, -2.81);
        DecimalComplex.AssertAreEqual(Complex.Log(c), DecimalComplex.Log(dc));

        // -3.14+2.81i
        dc = new DecimalComplex(-3.14m, 2.81m);
        c = new Complex(-3.14, 2.81);
        DecimalComplex.AssertAreEqual(Complex.Log(c), DecimalComplex.Log(dc));

        // -3.14-2.81i
        dc = new DecimalComplex(-3.14m, -2.81m);
        c = new Complex(-3.14, -2.81);
        DecimalComplex.AssertAreEqual(Complex.Log(c), DecimalComplex.Log(dc));
    }

    [TestMethod]
    public void ExpTest()
    {
        DecimalComplex z;

        z = 0;
        DecimalComplex.AssertAreEqual(Complex.Exp((Complex)z), DecimalComplex.Exp(z));

        z = 1;
        DecimalComplex.AssertAreEqual(Complex.Exp((Complex)z), DecimalComplex.Exp(z));

        z = -1;
        DecimalComplex.AssertAreEqual(Complex.Exp((Complex)z), DecimalComplex.Exp(z));

        z = DecimalComplex.I;
        DecimalComplex.AssertAreEqual(Complex.Exp((Complex)z), DecimalComplex.Exp(z));

        z = -DecimalComplex.I;
        DecimalComplex.AssertAreEqual(Complex.Exp((Complex)z), DecimalComplex.Exp(z));

        z = DecimalEx.Ln2;
        DecimalComplex.AssertAreEqual(Complex.Exp((Complex)z), DecimalComplex.Exp(z));

        z = DecimalEx.Ln10;
        DecimalComplex.AssertAreEqual(Complex.Exp((Complex)z), DecimalComplex.Exp(z));

        // These throw OverflowExceptions.
        // z = decimal.MaxValue;
        // DecimalComplex.AssertAreEqual(Complex.Exp((Complex)z), DecimalComplex.Exp(z));
        // z = decimal.MinValue;
        // DecimalComplex.AssertAreEqual(Complex.Exp((Complex)z), DecimalComplex.Exp(z));

        z = DecimalEx.SmallestNonZeroDec;
        DecimalComplex.AssertAreEqual(Complex.Exp((Complex)z), DecimalComplex.Exp(z));

        z = new DecimalComplex(0, DecimalEx.Pi);
        DecimalComplex.AssertAreEqual(Complex.Exp((Complex)z), DecimalComplex.Exp(z));

        z = new DecimalComplex(1, 1);
        DecimalComplex.AssertAreEqual(Complex.Exp((Complex)z), DecimalComplex.Exp(z));

        z = new DecimalComplex(3, 4);
        DecimalComplex.AssertAreEqual(Complex.Exp((Complex)z), DecimalComplex.Exp(z));

        z = new DecimalComplex(-5, 6);
        DecimalComplex.AssertAreEqual(Complex.Exp((Complex)z), DecimalComplex.Exp(z));

        z = new DecimalComplex(3.14m, 2.81m);
        DecimalComplex.AssertAreEqual(Complex.Exp((Complex)z), DecimalComplex.Exp(z));

        z = new DecimalComplex(-3.14m, -2.81m);
        DecimalComplex.AssertAreEqual(Complex.Exp((Complex)z), DecimalComplex.Exp(z));
    }

    [TestMethod]
    [ExpectedException(typeof(ArithmeticException))]
    public void PowThrowsWhenZeroRaisedToImagNum() => DecimalComplex.Pow(0, DecimalComplex.I);

    [TestMethod]
    [ExpectedException(typeof(ArithmeticException))]
    public void PowThrowsWhenZeroRaisedToNegNum() => DecimalComplex.Pow(0, -1);

    [TestMethod]
    public void PowTest()
    {
        DecimalComplex z1;
        DecimalComplex w1 = new (3, 4);
        Complex z2;
        Complex w2 = new (3, 4);

        z1 = DecimalComplex.Zero;
        z2 = Complex.Zero;
        DecimalComplex.AssertAreEqual(Complex.Pow(z2, 0), DecimalComplex.Pow(z1, 0));
        DecimalComplex.AssertAreEqual(Complex.Pow(z2, 1), DecimalComplex.Pow(z1, 1));

        z1 = DecimalComplex.One;
        z2 = Complex.One;
        DecimalComplex.AssertAreEqual(Complex.Pow(z2, 0), DecimalComplex.Pow(z1, 0));
        DecimalComplex.AssertAreEqual(Complex.Pow(z2, 1), DecimalComplex.Pow(z1, 1));
        DecimalComplex.AssertAreEqual(Complex.Pow(z2, -1), DecimalComplex.Pow(z1, -1));
        DecimalComplex.AssertAreEqual(Complex.Pow(z2, Complex.ImaginaryOne),
            DecimalComplex.Pow(z1, DecimalComplex.I));
        DecimalComplex.AssertAreEqual(Complex.Pow(z2, w2), DecimalComplex.Pow(z1, w1));

        z1 = DecimalComplex.ImaginaryOne;
        z2 = Complex.ImaginaryOne;
        DecimalComplex.AssertAreEqual(Complex.Pow(z2, 0), DecimalComplex.Pow(z1, 0));
        DecimalComplex.AssertAreEqual(Complex.Pow(z2, 1), DecimalComplex.Pow(z1, 1));
        DecimalComplex.AssertAreEqual(Complex.Pow(z2, -1), DecimalComplex.Pow(z1, -1));
        DecimalComplex.AssertAreEqual(Complex.Pow(z2, Complex.ImaginaryOne),
            DecimalComplex.Pow(z1, DecimalComplex.I));
        DecimalComplex.AssertAreEqual(Complex.Pow(z2, w2), DecimalComplex.Pow(z1, w1));

        z1 = -DecimalComplex.One;
        z2 = new Complex(-1, 0);
        DecimalComplex.AssertAreEqual(Complex.Pow(z2, 0), DecimalComplex.Pow(z1, 0));
        DecimalComplex.AssertAreEqual(Complex.Pow(z2, 1), DecimalComplex.Pow(z1, 1));
        DecimalComplex.AssertAreEqual(Complex.Pow(z2, -1), DecimalComplex.Pow(z1, -1));
        DecimalComplex.AssertAreEqual(Complex.Pow(z2, Complex.ImaginaryOne),
            DecimalComplex.Pow(z1, DecimalComplex.I));
        DecimalComplex.AssertAreEqual(Complex.Pow(z2, w2), DecimalComplex.Pow(z1, w1));

        z1 = -DecimalComplex.ImaginaryOne;
        z2 = new Complex(0, -1);
        DecimalComplex.AssertAreEqual(Complex.Pow(z2, 0), DecimalComplex.Pow(z1, 0));
        DecimalComplex.AssertAreEqual(Complex.Pow(z2, 1), DecimalComplex.Pow(z1, 1));
        DecimalComplex.AssertAreEqual(Complex.Pow(z2, -1), DecimalComplex.Pow(z1, -1));
        DecimalComplex.AssertAreEqual(Complex.Pow(z2, Complex.ImaginaryOne),
            DecimalComplex.Pow(z1, DecimalComplex.I));
        DecimalComplex.AssertAreEqual(Complex.Pow(z2, w2), DecimalComplex.Pow(z1, w1));

        z1 = new DecimalComplex(1, 1);
        z2 = new Complex(1, 1);
        DecimalComplex.AssertAreEqual(Complex.Pow(z2, 0), DecimalComplex.Pow(z1, 0));
        DecimalComplex.AssertAreEqual(Complex.Pow(z2, 1), DecimalComplex.Pow(z1, 1));
        DecimalComplex.AssertAreEqual(Complex.Pow(z2, Complex.ImaginaryOne),
            DecimalComplex.Pow(z1, DecimalComplex.I));
        DecimalComplex.AssertAreEqual(Complex.Pow(z2, w2), DecimalComplex.Pow(z1, w1));

        z1 = new DecimalComplex(3, 4);
        z2 = new Complex(3, 4);
        DecimalComplex.AssertAreEqual(Complex.Pow(z2, 0), DecimalComplex.Pow(z1, 0));
        DecimalComplex.AssertAreEqual(Complex.Pow(z2, 1), DecimalComplex.Pow(z1, 1));
        DecimalComplex.AssertAreEqual(Complex.Pow(z2, Complex.ImaginaryOne),
            DecimalComplex.Pow(z1, DecimalComplex.I));
        DecimalComplex.AssertAreEqual(Complex.Pow(z2, w2), DecimalComplex.Pow(z1, w1));

        z1 = new DecimalComplex(-5, 6);
        z2 = new Complex(-5, 6);
        DecimalComplex.AssertAreEqual(Complex.Pow(z2, 0), DecimalComplex.Pow(z1, 0));
        DecimalComplex.AssertAreEqual(Complex.Pow(z2, 1), DecimalComplex.Pow(z1, 1));
        DecimalComplex.AssertAreEqual(Complex.Pow(z2, Complex.ImaginaryOne),
            DecimalComplex.Pow(z1, DecimalComplex.I));
        DecimalComplex.AssertAreEqual(Complex.Pow(z2, w2), DecimalComplex.Pow(z1, w1));

        z1 = new DecimalComplex(3.14m, 2.81m);
        z2 = new Complex(3.14, 2.81);
        DecimalComplex.AssertAreEqual(Complex.Pow(z2, 0), DecimalComplex.Pow(z1, 0));
        DecimalComplex.AssertAreEqual(Complex.Pow(z2, 1), DecimalComplex.Pow(z1, 1));
        DecimalComplex.AssertAreEqual(Complex.Pow(z2, Complex.ImaginaryOne),
            DecimalComplex.Pow(z1, DecimalComplex.I));
        DecimalComplex.AssertAreEqual(Complex.Pow(z2, w2), DecimalComplex.Pow(z1, w1));

        z1 = new DecimalComplex(-3.14m, -2.81m);
        z2 = new Complex(-3.14, -2.81);
        DecimalComplex.AssertAreEqual(Complex.Pow(z2, 0), DecimalComplex.Pow(z1, 0));
        DecimalComplex.AssertAreEqual(Complex.Pow(z2, 1), DecimalComplex.Pow(z1, 1));
        DecimalComplex.AssertAreEqual(Complex.Pow(z2, Complex.ImaginaryOne),
            DecimalComplex.Pow(z1, DecimalComplex.I));
        DecimalComplex.AssertAreEqual(Complex.Pow(z2, w2), DecimalComplex.Pow(z1, w1));
    }
}
