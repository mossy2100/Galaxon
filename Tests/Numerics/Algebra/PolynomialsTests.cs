using System.Numerics;
using Galaxon.Numerics.Algebra;
using Galaxon.Numerics.Extensions;

namespace Galaxon.Tests.Numerics.Algebra;

[TestClass]
public class PolynomialsTests
{
    private const double _DELTA = 1e-15;

    #region ConstructPolynomial

    [TestMethod]
    public void ConstructPolynomial_ConstantFunction_ReturnsCorrectFunction()
    {
        // Arrange
        double[] coeffs = [5];

        // Act
        Func<double, double> polynomial = Polynomials.ConstructPolynomial(coeffs);

        // Assert
        Assert.AreEqual(5, polynomial(0));
        Assert.AreEqual(5, polynomial(1));
        Assert.AreEqual(5, polynomial(-10));
        Assert.AreEqual(5, polynomial(7.8));
    }

    [TestMethod]
    public void ConstructPolynomial_LinearFunction_ReturnsCorrectFunction()
    {
        // Arrange
        double[] coeffs = [2, 3]; // 2 + 3x

        // Act
        Func<double, double> polynomial = Polynomials.ConstructPolynomial(coeffs);

        // Assert
        Assert.AreEqual(2, polynomial(0));
        Assert.AreEqual(5, polynomial(1));
        Assert.AreEqual(-1, polynomial(-1));
        Assert.AreEqual(17, polynomial(5));
    }

    [TestMethod]
    public void ConstructPolynomial_QuadraticFunction_ReturnsCorrectFunction()
    {
        // Arrange
        double[] coeffs = [1, -2, 3]; // 1 - 2x + 3x^2

        // Act
        Func<double, double> polynomial = Polynomials.ConstructPolynomial(coeffs);

        // Assert
        Assert.AreEqual(1, polynomial(0));
        Assert.AreEqual(2, polynomial(1));
        Assert.AreEqual(9, polynomial(2));
        Assert.AreEqual(22, polynomial(3));
    }

    #endregion ConstructPolynomial

    #region EvaluatePolynomial

    [TestMethod]
    public void EvaluatePolynomial_ConstantFunction_ReturnsCorrectFunction()
    {
        // Arrange
        double[] coeffs = [5];

        // Assert
        Assert.AreEqual(5, Polynomials.EvaluatePolynomial(coeffs, 0));
        Assert.AreEqual(5, Polynomials.EvaluatePolynomial(coeffs, 1));
        Assert.AreEqual(5, Polynomials.EvaluatePolynomial(coeffs, -10));
        Assert.AreEqual(5, Polynomials.EvaluatePolynomial(coeffs, 7.8));
    }

    [TestMethod]
    public void EvaluatePolynomial_LinearFunction_ReturnsCorrectFunction()
    {
        // Arrange
        double[] coeffs = [2, 3]; // 2 + 3x

        // Assert
        Assert.AreEqual(2, Polynomials.EvaluatePolynomial(coeffs, 0));
        Assert.AreEqual(5, Polynomials.EvaluatePolynomial(coeffs, 1));
        Assert.AreEqual(-1, Polynomials.EvaluatePolynomial(coeffs, -1));
        Assert.AreEqual(17, Polynomials.EvaluatePolynomial(coeffs, 5));
    }

    [TestMethod]
    public void EvaluatePolynomial_QuadraticFunction_ReturnsCorrectFunction()
    {
        // Arrange
        double[] coeffs = [1, -2, 3]; // 1 - 2x + 3x^2

        // Assert
        Assert.AreEqual(1, Polynomials.EvaluatePolynomial(coeffs, 0));
        Assert.AreEqual(2, Polynomials.EvaluatePolynomial(coeffs, 1));
        Assert.AreEqual(9, Polynomials.EvaluatePolynomial(coeffs, 2));
        Assert.AreEqual(22, Polynomials.EvaluatePolynomial(coeffs, 3));
    }

    #endregion EvaluatePolynomial

    #region SolveQuadratic

    [TestMethod]
    public void SolveQuadratic_ZeroAZeroB_ThrowsException()
    {
        // Arrange
        double a = 0;
        double b = 0;
        double c = 5;

        // Act & Assert
        Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            Polynomials.SolveQuadratic(a, b, c));
    }

    [TestMethod]
    public void SolveQuadratic_ZeroA_OneSolution()
    {
        // Arrange
        double a = 0;
        double b = 3;
        double c = 6;

        // Act
        List<double> solutions = Polynomials.SolveQuadratic(a, b, c);

        // Assert
        Assert.AreEqual(1, solutions.Count);
        Assert.AreEqual(-2, solutions[0]);

        // Test solutions
        Func<double, double> f = z => a * z * z + b * z + c;
        Assert.AreEqual(0, f(solutions[0]));
    }

    [TestMethod]
    public void SolveQuadratic_NoSolutions()
    {
        // Arrange
        double a = 2;
        double b = 3;
        double c = 6;

        // Act
        List<double> solutions = Polynomials.SolveQuadratic(a, b, c);

        // Assert
        Assert.AreEqual(0, solutions.Count);
    }

    [TestMethod]
    public void SolveQuadratic_OneSolution()
    {
        // Arrange
        double a = 1;
        double b = -2;
        double c = 1;

        // Act
        List<double> solutions = Polynomials.SolveQuadratic(a, b, c);

        // Assert
        Assert.AreEqual(1, solutions.Count);
        Assert.AreEqual(1, solutions[0]);

        // Test solutions
        Func<double, double> f = z => a * z * z + b * z + c;
        Assert.AreEqual(0, f(solutions[0]));
    }

    [TestMethod]
    public void SolveQuadratic_TwoSolutions()
    {
        // Arrange
        double a = 5;
        double b = 6;
        double c = 1;

        // Act
        List<double> solutions = Polynomials.SolveQuadratic(a, b, c);

        // Assert
        Assert.AreEqual(2, solutions.Count);
        Assert.AreEqual(-1, solutions[0]);
        Assert.AreEqual(-0.2, solutions[1]);

        // Test solutions
        Func<double, double> f = z => a * z * z + b * z + c;
        Assert.AreEqual(0, f(solutions[0]), _DELTA);
        Assert.AreEqual(0, f(solutions[1]), _DELTA);
    }

    #endregion SolveQuadratic

    #region SolveQuadraticComplex

    [TestMethod]
    public void SolveQuadraticComplex_ZeroAZeroB_ThrowsException()
    {
        // Arrange
        double a = 0;
        double b = 0;
        double c = 5;

        // Act & Assert
        Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            Polynomials.SolveQuadraticComplex(a, b, c));
    }

    [TestMethod]
    public void SolveQuadraticComplex_ZeroA_OneSolution()
    {
        // Arrange
        double a = 0;
        double b = -10;
        double c = 4;

        // Act
        List<Complex> solutions = Polynomials.SolveQuadraticComplex(a, b, c);

        // Assert
        Assert.AreEqual(1, solutions.Count);
        Assert.AreEqual(0.4, solutions[0].Real);
        Assert.AreEqual(0, solutions[0].Imaginary);

        // Test solutions
        Func<Complex, Complex> f = z => a * z * z + b * z + c;
        Assert.AreEqual(0, f(solutions[0]));
    }

    [TestMethod]
    public void SolveQuadraticComplex_ZeroB_TwoRealSolution()
    {
        // Arrange
        double a = -4;
        double b = 0;
        double c = 16;

        // Act
        List<Complex> solutions = Polynomials.SolveQuadraticComplex(a, b, c);

        // Assert
        Assert.AreEqual(2, solutions.Count);
        Assert.AreEqual(-2, solutions[0].Real);
        Assert.AreEqual(0, solutions[0].Imaginary);
        Assert.AreEqual(2, solutions[1].Real);
        Assert.AreEqual(0, solutions[1].Imaginary);

        // Test solutions
        Func<Complex, Complex> f = z => a * z * z + b * z + c;
        Assert.AreEqual(0, f(solutions[0]));
        Assert.AreEqual(0, f(solutions[1]));
    }

    [TestMethod]
    public void SolveQuadraticComplex_ZeroB_TwoComplexSolutions()
    {
        // Arrange
        double a = 4;
        double b = 0;
        double c = 16;

        // Act
        List<Complex> solutions = Polynomials.SolveQuadraticComplex(a, b, c);

        // Assert
        Assert.AreEqual(2, solutions.Count);
        Assert.AreEqual(0, solutions[0].Real);
        Assert.AreEqual(-2, solutions[0].Imaginary);
        Assert.AreEqual(0, solutions[1].Real);
        Assert.AreEqual(2, solutions[1].Imaginary);

        // Test solutions
        Func<Complex, Complex> f = z => a * z * z + b * z + c;
        Assert.AreEqual(0, f(solutions[0]));
        Assert.AreEqual(0, f(solutions[1]));
    }

    [TestMethod]
    public void SolveQuadraticComplex_OneRealSolution()
    {
        // Arrange
        double a = 1;
        double b = -2;
        double c = 1;

        // Act
        List<Complex> solutions = Polynomials.SolveQuadraticComplex(a, b, c);

        // Assert
        Assert.AreEqual(1, solutions.Count);
        Assert.AreEqual(1, solutions[0].Real);
        Assert.AreEqual(0, solutions[0].Imaginary);

        // Test solution
        Func<Complex, Complex> f = z => a * z * z + b * z + c;
        Assert.AreEqual(0, f(solutions[0]));
    }

    [TestMethod]
    public void SolveQuadraticComplex_TwoRealSolutions()
    {
        // Arrange
        double a = 5;
        double b = 6;
        double c = 1;

        // Act
        List<Complex> solutions = Polynomials.SolveQuadraticComplex(a, b, c);

        // Assert
        Assert.AreEqual(2, solutions.Count);
        Assert.AreEqual(-1, solutions[0].Real);
        Assert.AreEqual(0, solutions[0].Imaginary);
        Assert.AreEqual(-0.2, solutions[1].Real);
        Assert.AreEqual(0, solutions[1].Imaginary);

        // Test solutions
        Func<Complex, Complex> f = z => a * z * z + b * z + c;
        ComplexExtensions.AreEqual(0, f(solutions[0]), _DELTA);
        ComplexExtensions.AreEqual(0, f(solutions[1]), _DELTA);
    }

    [TestMethod]
    public void SolveQuadraticComplex_TwoComplexSolutions()
    {
        // Arrange
        double a = 2;
        double b = 3;
        double c = 6;
        double expectedReal = -3.0 / 4;
        double expectedImag = Sqrt(39) / 4;

        // Act
        List<Complex> solutions = Polynomials.SolveQuadraticComplex(a, b, c);

        // Assert
        Assert.AreEqual(2, solutions.Count);
        Assert.AreEqual(expectedReal, solutions[0].Real);
        Assert.AreEqual(-expectedImag, solutions[0].Imaginary);
        Assert.AreEqual(expectedReal, solutions[1].Real);
        Assert.AreEqual(expectedImag, solutions[1].Imaginary);

        // Test solutions
        Func<Complex, Complex> f = z => a * z * z + b * z + c;
        ComplexExtensions.AreEqual(0, f(solutions[0]), _DELTA);
        ComplexExtensions.AreEqual(0, f(solutions[1]), _DELTA);
    }

    #endregion SolveQuadraticComplex
}
