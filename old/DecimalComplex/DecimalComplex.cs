using System.Numerics;
using DecimalMath;
using Galaxon.Core.Numbers;
using Galaxon.Core.Testing;

namespace Galaxon.Numerics.Types;

public struct DecimalComplex
{
    #region Properties

    public decimal Real { get; set; }

    public decimal Imaginary { get; set; }

    public decimal Magnitude => Abs(this);

    public decimal Phase => DecimalEx.ATan2(Imaginary, Real);

    #endregion Properties

    #region Static properties

    public static DecimalComplex Zero => new (0, 0);

    public static DecimalComplex One => new (1, 0);

    // Same as in System.Numerics.Complex.
    public static DecimalComplex ImaginaryOne => new (0, 1);

    // Convenient shorthand equal to ImaginaryOne.
    public static DecimalComplex I => ImaginaryOne;

    #endregion Static properties

    #region Constructor

    public DecimalComplex(decimal real, decimal imaginary)
    {
        Real = real;
        Imaginary = imaginary;
    }

    #endregion Constructor

    #region Overridden methods

    public override bool Equals(object? obj)
    {
        if (obj is not DecimalComplex z)
        {
            return false;
        }

        return Real == z.Real && Imaginary == z.Imaginary;
    }

    public override int GetHashCode() => HashCode.Combine(Real, Imaginary);

    /// <summary>
    /// Express the complex number as a string in the usual algebraic format.
    /// This differs from Complex.ToString(), which outputs strings like (x, y).
    /// </summary>
    /// <returns>The complex number as a string.</returns>
    public override string ToString()
    {
        var realPart = Real == 0 && Imaginary != 0 ? "" : $"{Real}";

        var sign = "";
        if (Real == 0)
        {
            if (Imaginary < 0)
            {
                sign = "-";
            }
        }
        else
        {
            if (Imaginary < 0)
            {
                sign = " - ";
            }
            else if (Imaginary > 0)
            {
                sign = " + ";
            }
        }

        var absImag = Math.Abs(Imaginary);
        var imagPart = absImag switch
        {
            0 => "",
            1 => "i",
            _ => $"{absImag}i"
        };

        return $"{realPart}{sign}{imagPart}";
    }

    #endregion Overridden methods

    #region Methods relating to magnitude and phase

    /// <summary>
    /// Calculate absolute value (also known as magnitude).
    /// Using:
    /// sqrt(a^2 + b^2) = |a| * sqrt(1 + (b/a)^2)
    /// we can factor out the larger component to dodge overflow even when a^2
    /// would overflow.
    /// </summary>
    /// <param name="z">A DecimalComplex number.</param>
    /// <returns>The magnitude of the argument.</returns>
    public static decimal Abs(DecimalComplex z)
    {
        var a = Math.Abs(z.Real);
        var b = Math.Abs(z.Imaginary);

        decimal small, large;
        if (a < b)
        {
            small = a;
            large = b;
        }
        else
        {
            small = b;
            large = a;
        }

        if (small == 0m)
        {
            return large;
        }

        var ratio = small / large;
        return large * DecimalEx.Sqrt(1.0m + ratio * ratio);
    }

    /// <summary>
    /// Construct a complex number from the magnitude and phase.
    /// <see cref="System.Numerics.Complex.FromPolarCoordinates"/>
    /// </summary>
    /// <param name="magnitude">
    /// The magnitude (or absolute value) of the complex
    /// number.
    /// </param>
    /// <param name="phase">The phase angle in radians.</param>
    /// <returns>The new DecimalComplex number.</returns>
    public static DecimalComplex FromPolarCoordinates(decimal magnitude, decimal phase) =>
        magnitude switch
        {
            < 0 => throw new ArgumentOutOfRangeException(nameof(magnitude),
                "Cannot be less than zero."),
            0 => Zero,
            _ => magnitude * new DecimalComplex(DecimalEx.Cos(phase), DecimalEx.Sin(phase))
        };

    #endregion Methods relating to magnitude and phase

    #region Exponentiation methods

    /// <summary>
    /// Square a complex number.
    /// </summary>
    /// <param name="z">A complex value.</param>
    /// <returns>The square of the supplied value.</returns>
    public static DecimalComplex Sqr(DecimalComplex z)
    {
        if (z.Imaginary == 0)
        {
            return z.Real switch
            {
                0 => 0,
                1 => 1,
                _ => z.Real * z.Real
            };
        }

        if (z.Real == 0)
        {
            return z.Real switch
            {
                1 => -1,
                _ => -z.Imaginary * z.Imaginary
            };
        }

        return z * z;
    }

    /// <summary>
    /// Calculate the square root of a DecimalComplex number.
    /// The second root can be found by the negative of the result, as with
    /// other Sqrt() methods.
    /// You can use this method to get the square root of a negative decimal.
    /// e.g. DecimalComplex z = DecimalComplex.Sqrt(-5);
    /// (There will be an implicit cast of the -5 to a DecimalComplex.)
    /// TODO Test.
    /// <see cref="System.Math.Sqrt"/>
    /// <see cref="System.Numerics.Complex.Sqrt"/>
    /// <see cref="DecimalEx.Sqrt"/>
    /// </summary>
    /// <param name="z">A DecimalComplex number.</param>
    /// <returns>The positive square root as a DecimalComplex number.</returns>
    public static DecimalComplex Sqrt(DecimalComplex z)
    {
        if (z == 0)
        {
            return 0;
        }
        if (z == 1)
        {
            return 1;
        }
        if (z == -1)
        {
            return I;
        }

        var a = z.Real;
        var b = z.Imaginary;
        if (b == 0)
        {
            return a > 0
                ? new DecimalComplex(DecimalEx.Sqrt(a), 0)
                : new DecimalComplex(0, DecimalEx.Sqrt(-a));
        }

        var m = Abs(z);
        var x = DecimalEx.Sqrt((m + a) / 2);
        var y = b / Math.Abs(b) * DecimalEx.Sqrt((m - a) / 2);
        return new DecimalComplex(x, y);
    }

    /// <summary>
    /// Natural logarithm of a complex number.
    /// <see cref="Log(Galaxon.Numerics.Types.DecimalComplex)"/>
    /// <see cref="XDecimal.Log(decimal)"/>
    /// <see cref="Math.Log(double)"/>
    /// <see cref="Complex.Log(Complex)"/>
    /// </summary>
    /// <param name="z">A complex number.</param>
    /// <returns>The natural logarithm of the given value.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If z == 0.</exception>
    public static DecimalComplex Log(DecimalComplex z)
    {
        if (z.Imaginary == 0)
        {
            return z.Real switch
            {
                // Math.Log(0) returns -Infinity, which decimal can't represent.
                0 => throw new ArgumentOutOfRangeException(nameof(z),
                    "Logarithm of 0 is undefined."),

                // ln(x) = ln(|x|) + πi, for x < 0
                < 0 => new DecimalComplex(XDecimal.Log(-z.Real), DecimalEx.Pi),

                // For positive real values, pass to the decimal method.
                _ => XDecimal.Log(z.Real)
            };
        }

        return new DecimalComplex(XDecimal.Log(z.Magnitude), z.Phase);
    }

    /// <summary>
    /// Logarithm of a complex number in a specified base.
    /// <see cref="Log(Galaxon.Numerics.Types.DecimalComplex)"/>
    /// <see cref="XDecimal.Log(decimal, decimal)"/>
    /// </summary>
    /// <param name="z">The complex arg.</param>
    /// <param name="b">The base.</param>
    /// <returns>The logarithm of z in base b.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If the complex value is 0.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// If the base is less than or equal to 0, or equal to 1.
    /// </exception>
    public static DecimalComplex Log(DecimalComplex z, decimal b) =>
        b switch
        {
            1 => throw new ArgumentOutOfRangeException(nameof(b),
                "Logarithms are undefined for a base of 1."),
            _ => Log(z) / XDecimal.Log(b)
        };

    /// <summary>
    /// Logarithm of a complex number in base 2.
    /// <see cref="XDecimal.Log2"/>
    /// </summary>
    /// <param name="z">The complex arg.</param>
    /// <returns>The logarithm of z in base 2.</returns>
    public static DecimalComplex Log2(DecimalComplex z) => Log(z, 2);

    /// <summary>
    /// Logarithm of a complex number in base 10.
    /// <see cref="XDecimal.Log10"/>
    /// </summary>
    /// <param name="z">The complex arg.</param>
    /// <returns>The logarithm of z in base 10.</returns>
    public static DecimalComplex Log10(DecimalComplex z) => Log(z, 10);

    public static DecimalComplex Exp(DecimalComplex z)
    {
        // Optimizations.
        if (z.Real == 0)
        {
            switch (z.Imaginary)
            {
                // e^0 == 1
                case 0:
                    return 1;

                // e^iπ = -1 (Euler's identity)
                case DecimalEx.Pi:
                    return -1;
            }
        }

        if (z.Imaginary == 0)
        {
            switch (z.Real)
            {
                // e^1 == e
                case 1:
                    return DecimalEx.E;

                // e^ln2 == 2
                case DecimalEx.Ln2:
                    return 2;

                // e^ln10 == 10
                case DecimalEx.Ln10:
                    return 10;
            }
        }

        // Euler's formula.
        return DecimalEx.Exp(z.Real) *
            new DecimalComplex(DecimalEx.Cos(z.Imaginary), DecimalEx.Sin(z.Imaginary));
    }

    /// <summary>
    /// Calculate 2 raised to a complex power.
    /// <see cref="XDecimal.Exp2"/>
    /// </summary>
    /// <param name="z">A complex value.</param>
    /// <returns>2^z</returns>
    public static DecimalComplex Exp2(DecimalComplex z) => 2 ^ z;

    /// <summary>
    /// Calculate 10 raised to a complex power.
    /// <see cref="XDecimal.Exp10"/>
    /// </summary>
    /// <param name="z">A complex value.</param>
    /// <returns>10^z</returns>
    public static DecimalComplex Exp10(DecimalComplex z) => 10 ^ z;

    /// <summary>
    /// Complex exponentiation.
    /// Only the principal value is returned.
    /// <see href="https://en.wikipedia.org/wiki/Exponentiation#Complex_exponentiation"/>
    /// </summary>
    /// <param name="z">The base.</param>
    /// <param name="w">The exponent.</param>
    /// <returns>The result.</returns>
    /// <exception cref="ArithmeticException">
    /// If the base is 0 and the exponent
    /// is negative or imaginary.
    /// </exception>
    public static DecimalComplex Pow(DecimalComplex z, DecimalComplex w)
    {
        // Guards.
        if (z == 0)
        {
            // 0 raised to a negative real value is undefined.
            // Math.Pow() returns double.Infinity, but decimal doesn't have this.
            if (w.Imaginary == 0 && w.Real < 0)
            {
                throw new ArithmeticException("0 raised to a negative power is undefined.");
            }

            // 0 to the power of an imaginary number is also undefined.
            if (w.Imaginary != 0)
            {
                throw new ArithmeticException("0 raised to an imaginary power is undefined.");
            }
        }

        // Any non-zero value (real or complex) raised to the 0 power is 1.
        // 0^0 has no agreed-upon value, but some programming languages,
        // including C#, return 1 (i.e. Math.Pow(0, 0) == 1). Rather than throw
        // an exception, we'll do that here, too, for consistency.
        if (w == 0)
        {
            return 1;
        }

        // Any value (real or complex) raised to the power of 1 is itself.
        if (w == 1)
        {
            return z;
        }

        // 1 raised to any real value is 1.
        // 1 raised to any complex value has multiple results, including 1.
        // We'll just return 1 (the principal value) for simplicity and
        // consistency with Complex.Pow().
        if (z == 1)
        {
            return 1;
        }

        // i^2 == -1 by definition.
        if (z == I && w == 2)
        {
            return -1;
        }

        // If the values are both real, pass it to the decimal calculation.
        if (z.Imaginary == 0 && w.Imaginary == 0)
        {
            return DecimalEx.Pow(z.Real, w.Real);
        }

        // Use formula for principal value.
        return Exp(w * Log(z));
    }

    /// <summary>
    /// Exponentiation operator.
    /// </summary>
    /// <param name="z">The base.</param>
    /// <param name="w">The exponent.</param>
    /// <returns>The first operand raised to the power of the second.</returns>
    /// <exception cref="ArithmeticException">
    /// If the base is 0 and the exponent
    /// is negative or imaginary.
    /// </exception>
    public static DecimalComplex operator ^(DecimalComplex z, DecimalComplex w) => Pow(z, w);

    #endregion Exponentiation methods

    #region Trigonometic methods

    public static DecimalComplex Sin(DecimalComplex z)
    {
        var x = z.Real;
        var y = z.Imaginary;
        var a = DecimalEx.Sin(x) * XDecimal.Cosh(y);
        var b = DecimalEx.Cos(x) * XDecimal.Sinh(y);
        return new DecimalComplex(a, b);
    }

    public static DecimalComplex Cos(DecimalComplex z)
    {
        var x = z.Real;
        var y = z.Imaginary;
        var a = DecimalEx.Cos(x) * XDecimal.Cosh(y);
        var b = DecimalEx.Sin(x) * XDecimal.Sinh(y);
        return new DecimalComplex(a, -b);
    }

    public static DecimalComplex Tan(DecimalComplex z)
    {
        var x = z.Real;
        var y = z.Imaginary;
        DecimalComplex a = new (DecimalEx.Sin(2 * x), XDecimal.Sinh(2 * y));
        var b = DecimalEx.Cos(2 * x) + XDecimal.Cosh(2 * y);
        return a / b;
    }

    /// <summary>
    ///     <see href="https://en.wikipedia.org/wiki/Inverse_trigonometric_functions#Logarithmic_forms"/>
    /// </summary>
    /// <param name="z"></param>
    /// <returns></returns>
    public static DecimalComplex Asin(DecimalComplex z) => I * Log(Sqrt(1 - z * z) - I * z);

    /// <summary>
    ///     <see href="https://en.wikipedia.org/wiki/Inverse_trigonometric_functions#Logarithmic_forms"/>
    /// </summary>
    /// <param name="z"></param>
    /// <returns></returns>
    public static DecimalComplex Acos(DecimalComplex z) => -I * Log(z + I * Sqrt(1 - z * z));

    /// <summary>
    ///     <see href="https://en.wikipedia.org/wiki/Inverse_trigonometric_functions#Logarithmic_forms"/>
    /// </summary>
    /// <param name="z"></param>
    /// <returns></returns>
    public static DecimalComplex Atan(DecimalComplex z) => -I / 2 * Log((I - z) / (I + z));

    public static DecimalComplex Sinh(DecimalComplex z)
    {
        var x = z.Real;
        var y = z.Imaginary;
        var a = XDecimal.Sinh(x) * DecimalEx.Cos(y);
        var b = XDecimal.Cosh(x) * DecimalEx.Sin(y);
        return new DecimalComplex(a, b);
    }

    public static DecimalComplex Cosh(DecimalComplex z)
    {
        var x = z.Real;
        var y = z.Imaginary;
        var a = XDecimal.Cosh(x) * DecimalEx.Cos(y);
        var b = XDecimal.Sinh(x) * DecimalEx.Sin(y);
        return new DecimalComplex(a, b);
    }

    public static DecimalComplex Tanh(DecimalComplex z)
    {
        var x = z.Real;
        var y = z.Imaginary;
        var a = XDecimal.Tanh(x);
        var b = DecimalEx.Tan(y);
        return (a + I * b) / (1 + I * a * b);
    }

    #endregion Trigonometic methods

    #region Cast operators

    /// <summary>
    /// Implicit cast of a decimal to a DecimalComplex.
    /// </summary>
    /// <param name="m">A decimal.</param>
    /// <returns>The equivalent DecimalComplex number.</returns>
    public static implicit operator DecimalComplex(decimal m) => new (m, 0);

    /// <summary>
    /// Implicit cast of integer to a DecimalComplex.
    /// </summary>
    /// <param name="j">An integer.</param>
    /// <returns>The equivalent DecimalComplex number.</returns>
    public static implicit operator DecimalComplex(int j) => new (j, 0);

    /// <summary>
    /// Explicit cast of Complex to a DecimalComplex.
    /// </summary>
    /// <param name="z">A Complex value.</param>
    /// <returns>The equivalent DecimalComplex value.</returns>
    public static explicit operator DecimalComplex(Complex z) =>
        new ((decimal)z.Real, (decimal)z.Imaginary);

    /// <summary>
    /// Explicit cast of DecimalComplex to a Complex.
    /// </summary>
    /// <param name="z">A DecimalComplex value.</param>
    /// <returns>The equivalent Complex value.</returns>
    public static explicit operator Complex(DecimalComplex z) =>
        new ((double)z.Real, (double)z.Imaginary);

    #endregion Cast operators

    #region Comparison operators

    /// <summary>
    /// Equality comparison operator.
    /// </summary>
    /// <param name="z1">The left-hand DecimalComplex number.</param>
    /// <param name="z2">The right-hand DecimalComplex number.</param>
    /// <returns>If they are equal.</returns>
    /// TODO Test.
    public static bool operator ==(DecimalComplex z1, DecimalComplex z2) => z1.Equals(z2);

    /// <summary>
    /// Inequality comparison operator.
    /// </summary>
    /// <param name="z1">The left-hand DecimalComplex number.</param>
    /// <param name="z2">The right-hand DecimalComplex number.</param>
    /// <returns>If they are not equal.</returns>
    public static bool operator !=(DecimalComplex z1, DecimalComplex z2) => !(z1 == z2);

    #endregion Comparison operators

    #region Arithmetic operators and methods

    /// <summary>
    /// Negate method.
    /// <see cref="decimal.Negate"/>
    /// </summary>
    /// <returns>The negation of the argument.</returns>
    public static DecimalComplex Negate(DecimalComplex z) => new (-z.Real, -z.Imaginary);

    /// <summary>
    /// Unary negation operator.
    /// </summary>
    /// <param name="z">A DecimalComplex number.</param>
    /// <returns>The negation of the operand.</returns>
    public static DecimalComplex operator -(DecimalComplex z) => Negate(z);

    /// <summary>
    /// Complex conjugate method.
    /// </summary>
    /// <returns>The complex conjugate of the argument.</returns>
    public static DecimalComplex Conjugate(DecimalComplex z) => new (z.Real, -z.Imaginary);

    /// <summary>
    /// Complex conjugate operator.
    /// The use of the tilde (~) for this operator is non-standard, but it seems
    /// a good fit and it could be useful.
    /// </summary>
    /// <returns>The complex conjugate of the operand.</returns>
    public static DecimalComplex operator ~(DecimalComplex z) => Conjugate(z);

    /// <summary>
    /// Calculate reciprocal.
    /// </summary>
    /// <returns>The reciprocal of the argument.</returns>
    public static DecimalComplex Reciprocal(DecimalComplex z) => 1 / z;

    /// <summary>
    /// Addition method.
    /// <see cref="decimal.Add"/>
    /// </summary>
    /// <param name="z1">The left-hand DecimalComplex number.</param>
    /// <param name="z2">The right-hand DecimalComplex number.</param>
    /// <returns>The addition of the arguments.</returns>
    public static DecimalComplex Add(DecimalComplex z1, DecimalComplex z2) =>
        new (z1.Real + z2.Real, z1.Imaginary + z2.Imaginary);

    /// <summary>
    /// Addition operator.
    /// </summary>
    /// <param name="z1">The left-hand DecimalComplex number.</param>
    /// <param name="z2">The right-hand DecimalComplex number.</param>
    /// <returns>The addition of the operands.</returns>
    public static DecimalComplex operator +(DecimalComplex z1, DecimalComplex z2) => Add(z1, z2);

    /// <summary>
    /// Subtraction method.
    /// <see cref="decimal.Subtract"/>
    /// </summary>
    /// <param name="z1">The left-hand DecimalComplex number.</param>
    /// <param name="z2">The right-hand DecimalComplex number.</param>
    /// <returns>The subtraction of the arguments.</returns>
    public static DecimalComplex Subtract(DecimalComplex z1, DecimalComplex z2) =>
        new (z1.Real - z2.Real, z1.Imaginary - z2.Imaginary);

    /// <summary>
    /// Subtraction operator.
    /// </summary>
    /// <param name="z1">The left-hand DecimalComplex number.</param>
    /// <param name="z2">The right-hand DecimalComplex number.</param>
    /// <returns>The subtraction of the operands.</returns>
    public static DecimalComplex operator -(DecimalComplex z1, DecimalComplex z2) =>
        Subtract(z1, z2);

    /// <summary>
    /// Multiply two DecimalComplex values.
    /// <see cref="decimal.Multiply"/>
    /// </summary>
    /// <param name="z1">The left-hand DecimalComplex number.</param>
    /// <param name="z2">The right-hand DecimalComplex number.</param>
    /// <returns>The multiplication of the arguments.</returns>
    public static DecimalComplex Multiply(DecimalComplex z1, DecimalComplex z2)
    {
        var a = z1.Real;
        var b = z1.Imaginary;
        var c = z2.Real;
        var d = z2.Imaginary;
        return new DecimalComplex(a * c - b * d, a * d + b * c);
    }

    /// <summary>
    /// Multiply a DecimalComplex by a decimal.
    /// </summary>
    /// <param name="z">The DecimalComplex number.</param>
    /// <param name="m">The decimal number.</param>
    /// <returns>The multiplication of the arguments.</returns>
    public static DecimalComplex Multiply(DecimalComplex z, decimal m) =>
        new (z.Real * m, z.Imaginary * m);

    /// <summary>
    /// Multiply a decimal by a DecimalComplex.
    /// </summary>
    /// <param name="m">The decimal number.</param>
    /// <param name="z">The DecimalComplex number.</param>
    /// <returns>The multiplication of the arguments.</returns>
    public static DecimalComplex Multiply(decimal m, DecimalComplex z) => Multiply(z, m);

    /// <summary>
    /// Multiplication operator.
    /// </summary>
    /// <param name="z1">The left-hand DecimalComplex number.</param>
    /// <param name="z2">The right-hand DecimalComplex number.</param>
    /// <returns>The multiplication of the operands.</returns>
    public static DecimalComplex operator *(DecimalComplex z1, DecimalComplex z2) =>
        Multiply(z1, z2);

    /// <summary>
    /// Multiply a DecimalComplex by a decimal.
    /// </summary>
    /// <param name="z">The DecimalComplex number.</param>
    /// <param name="m">The decimal number.</param>
    /// <returns>The multiplication of the operands.</returns>
    public static DecimalComplex operator *(DecimalComplex z, decimal m) => Multiply(z, m);

    /// <summary>
    /// Multiply a decimal by a DecimalComplex.
    /// </summary>
    /// <param name="m">The decimal number.</param>
    /// <param name="z">The DecimalComplex number.</param>
    /// <returns>The multiplication of the operands.</returns>
    public static DecimalComplex operator *(decimal m, DecimalComplex z) => Multiply(m, z);

    /// <summary>
    /// Divide a DecimalComplex by a DecimalComplex.
    /// <see cref="decimal.Divide"/>
    /// </summary>
    /// <param name="z1">The left-hand DecimalComplex number.</param>
    /// <param name="z2">The right-hand DecimalComplex number.</param>
    /// <returns>The division of the arguments.</returns>
    /// <exception cref="System.DivideByZeroException">If z2 == 0</exception>
    public static DecimalComplex Divide(DecimalComplex z1, DecimalComplex z2)
    {
        var a = z1.Real;
        var b = z1.Imaginary;
        var c = z2.Real;
        var d = z2.Imaginary;
        if (d == 0)
        {
            return z1 / c;
        }
        return new DecimalComplex(a * c + b * d, b * c - a * d) / (c * c + d * d);
    }

    /// <summary>
    /// Divide a DecimalComplex by a decimal.
    /// <see cref="decimal.Divide"/>
    /// </summary>
    /// <param name="z">The DecimalComplex value.</param>
    /// <param name="m">The decimal value.</param>
    /// <returns>The division of the arguments.</returns>
    /// <exception cref="System.DivideByZeroException">If d == 0.</exception>
    public static DecimalComplex Divide(DecimalComplex z, decimal m) =>
        new (z.Real / m, z.Imaginary / m);

    /// <summary>
    /// Divide a decimal by a DecimalComplex.
    /// <see cref="decimal.Divide"/>
    /// </summary>
    /// <param name="m">The decimal value.</param>
    /// <param name="z">The DecimalComplex value.</param>
    /// <returns>The division of the arguments.</returns>
    /// <exception cref="System.DivideByZeroException">If z == 0.</exception>
    public static DecimalComplex Divide(decimal m, DecimalComplex z)
    {
        var a = z.Real;
        var b = z.Imaginary;
        return new DecimalComplex(m * a, -m * b) / (a * a + b * b);
    }

    /// <summary>
    /// Divide a DecimalComplex by a DecimalComplex.
    /// </summary>
    /// <param name="z1">The left-hand DecimalComplex number.</param>
    /// <param name="z2">The right-hand DecimalComplex number.</param>
    /// <returns>The division of the operands.</returns>
    public static DecimalComplex operator /(DecimalComplex z1, DecimalComplex z2) => Divide(z1, z2);

    /// <summary>
    /// Divide a DecimalComplex by a decimal.
    /// <see cref="decimal.Divide"/>
    /// </summary>
    /// <param name="z">The DecimalComplex value.</param>
    /// <param name="m">The decimal value.</param>
    /// <returns>The division of the operands.</returns>
    /// <exception cref="System.DivideByZeroException">If d == 0.</exception>
    public static DecimalComplex operator /(DecimalComplex z, decimal m) => Divide(z, m);

    /// <summary>
    /// Divide a decimal by a DecimalComplex.
    /// <see cref="decimal.Divide"/>
    /// </summary>
    /// <param name="m">The decimal value.</param>
    /// <param name="z">The DecimalComplex value.</param>
    /// <returns>The division of the operands.</returns>
    /// <exception cref="System.DivideByZeroException">If z == 0.</exception>
    public static DecimalComplex operator /(decimal m, DecimalComplex z) => Divide(m, z);

    #endregion Arithmetic operators and methods

    #region Testing methods

    /// <summary>
    /// Helper function to test if a Complex equals a DecimalComplex.
    /// </summary>
    /// <param name="expected">Expected Complex value</param>
    /// <param name="actual">Actual DecimalComplex value</param>
    public static void AssertAreEqual(Complex expected, DecimalComplex actual)
    {
        XAssert.AreEqual(expected.Real, actual.Real);
        XAssert.AreEqual(expected.Imaginary, actual.Imaginary);
    }

    #endregion Testing methods
}
