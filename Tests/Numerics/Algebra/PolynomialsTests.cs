using Galaxon.Numerics.Algebra;

namespace Galaxon.Tests.Numerics.Algebra;

[TestClass]
public class PolynomialsTests
{
    #region ConstructPolynomial

    [TestMethod]
    public void ConstructPolynomial_ConstantFunction_ReturnsCorrectFunction()
    {
        // Arrange
        double[] coeffs = { 5 };

        // Act
        Func<double, double> polynomial = Polynomials.ConstructPolynomial(coeffs);

        // Assert
        Assert.AreEqual(5, polynomial(0));
        Assert.AreEqual(5, polynomial(1));
        Assert.AreEqual(5, polynomial(-10));
    }

    [TestMethod]
    public void ConstructPolynomial_LinearFunction_ReturnsCorrectFunction()
    {
        // Arrange
        double[] coeffs = { 2, 3 }; // 2 + 3x

        // Act
        Func<double, double> polynomial = Polynomials.ConstructPolynomial(coeffs);

        // Assert
        Assert.AreEqual(2, polynomial(0));
        Assert.AreEqual(5, polynomial(1));
        Assert.AreEqual(-1, polynomial(-1));
    }

    [TestMethod]
    public void ConstructPolynomial_QuadraticFunction_ReturnsCorrectFunction()
    {
        // Arrange
        double[] coeffs = { 1, -2, 3 }; // 1 - 2x + 3x^2

        // Act
        Func<double, double> polynomial = Polynomials.ConstructPolynomial(coeffs);

        // Assert
        Assert.AreEqual(1, polynomial(0));
        Assert.AreEqual(2, polynomial(1));
        Assert.AreEqual(9, polynomial(2));
    }

    #endregion ConstructPolynomial
}
