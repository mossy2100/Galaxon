using Galaxon.Numerics.Extensions;
using Galaxon.UnitTesting;
using static Galaxon.Numerics.Geometry.Angles;

namespace Galaxon.Tests.Numerics.Geometry;

[TestClass]
public class AnglesTests
{
    [TestMethod]
    public void TestNormalizeRadiansSigned()
    {
        // Arrange.
        double[] inputs =
        [
            -4 * PI, -3.5 * PI, -3 * PI, -2.5 * PI,
            -Tau, -1.5 * PI, -PI, -PI / 2,
            0, PI / 2, PI, 1.5 * PI,
            Tau, 2.5 * PI, 3 * PI, 3.5 * PI,
            4 * PI
        ];
        double[] outputs =
        [
            0, PI / 2, -PI, -PI / 2,
            0, PI / 2, -PI, -PI / 2,
            0, PI / 2, -PI, -PI / 2,
            0, PI / 2, -PI, -PI / 2,
            0
        ];
        for (var i = 0; i < inputs.Length; i++)
        {
            double actual = WrapRadians(inputs[i]);
            double expected = outputs[i];
            DoubleAssert.IsInRange(actual, -PI, PI);
            Assert.AreEqual(expected, actual, DoubleExtensions.DELTA);
        }
    }

    [TestMethod]
    public void TestNormalizeRadiansUnsigned()
    {
        // Arrange.
        double[] inputs =
        [
            -4 * PI, -3.5 * PI, -3 * PI, -2.5 * PI,
            -Tau, -1.5 * PI, -PI, -PI / 2,
            0, PI / 2, PI, 1.5 * PI,
            Tau, 2.5 * PI, 3 * PI, 3.5 * PI,
            4 * PI
        ];
        double[] outputs =
        [
            0, PI / 2, PI, 1.5 * PI,
            0, PI / 2, PI, 1.5 * PI,
            0, PI / 2, PI, 1.5 * PI,
            0, PI / 2, PI, 1.5 * PI,
            0
        ];
        for (var i = 0; i < inputs.Length; i++)
        {
            double actual = WrapRadians(inputs[i], false);
            double expected = outputs[i];
            DoubleAssert.IsInRange(actual, 0, Tau);
            Assert.AreEqual(expected, actual, DoubleExtensions.DELTA);
        }
    }

    [TestMethod]
    public void TestNormalizeDegreesSigned()
    {
        // Arrange.
        double[] inputs =
        [
            -720, -630, -540, -450,
            -360, -270, -180, -90,
            0, 90, 180, 270,
            360, 450, 540, 630,
            720
        ];
        double[] outputs =
        [
            0, 90, -180, -90,
            0, 90, -180, -90,
            0, 90, -180, -90,
            0, 90, -180, -90,
            0
        ];
        for (var i = 0; i < inputs.Length; i++)
        {
            double actual = WrapDegrees(inputs[i]);
            double expected = outputs[i];
            DoubleAssert.IsInRange(actual, -180, 180);
            Assert.AreEqual(expected, actual, DoubleExtensions.DELTA);
        }
    }

    [TestMethod]
    public void TestNormalizeDegreesUnsigned()
    {
        // Arrange.
        double[] inputs =
        [
            -720, -630, -540, -450,
            -360, -270, -180, -90,
            0, 90, 180, 270,
            360, 450, 540, 630,
            720
        ];
        double[] outputs =
        [
            0, 90, 180, 270,
            0, 90, 180, 270,
            0, 90, 180, 270,
            0, 90, 180, 270,
            0
        ];
        for (var i = 0; i < inputs.Length; i++)
        {
            double actual = WrapDegrees(inputs[i], false);
            double expected = outputs[i];
            DoubleAssert.IsInRange(actual, 0, 360);
            Assert.AreEqual(expected, actual, DoubleExtensions.DELTA);
        }
    }

    [TestMethod]
    public void RadiansToDegreesTest()
    {
        Assert.AreEqual(180, RadiansToDegrees(PI), DoubleExtensions.DELTA);
        Assert.AreEqual(90, RadiansToDegrees(PI / 2), DoubleExtensions.DELTA);
        Assert.AreEqual(60, RadiansToDegrees(PI / 3), DoubleExtensions.DELTA);
        Assert.AreEqual(45, RadiansToDegrees(PI / 4), DoubleExtensions.DELTA);
        Assert.AreEqual(360, RadiansToDegrees(Tau), DoubleExtensions.DELTA);
    }

    [TestMethod]
    public void DegreesToRadiansTest()
    {
        Assert.AreEqual(PI, DegreesToRadians(180), DoubleExtensions.DELTA);
        Assert.AreEqual(PI / 2, DegreesToRadians(90), DoubleExtensions.DELTA);
        Assert.AreEqual(PI / 3, DegreesToRadians(60), DoubleExtensions.DELTA);
        Assert.AreEqual(PI / 4, DegreesToRadians(45), DoubleExtensions.DELTA);
        Assert.AreEqual(Tau, DegreesToRadians(360), DoubleExtensions.DELTA);
    }

    [DataTestMethod]
    [DataRow(0, 0, 0, 0)]
    [DataRow(12, 12, 0, 0)]
    [DataRow(12.5666666666667, 12, 34, 0)]
    [DataRow(12.5822222222222, 12, 34, 56)]
    [DataRow(12.5824413888889, 12, 34, 56.789)]
    [DataRow(-12, -12, 0, 0)]
    [DataRow(-12.5666666666667, -12, -34, 0)]
    [DataRow(-12.5822222222222, -12, -34, -56)]
    [DataRow(-12.5824413888889, -12, -34, -56.789)]
    public void DegreesToDMS_ReturnsCorrectValue(double angle, int expectedD, int expectedM,
        double expectedS)
    {
        // Act
        (int actualD, int actualM, double actualS) = DegreesToDMS(angle);

        // Assert
        Assert.AreEqual(expectedD, actualD, DoubleExtensions.DELTA);
        Assert.AreEqual(expectedM, actualM, DoubleExtensions.DELTA);
        Assert.AreEqual(expectedS, actualS, DoubleExtensions.DELTA);
    }

    [DataTestMethod]
    [DataRow(0, 0, 0, 0)]
    [DataRow(12, 12, 0, 0)]
    [DataRow(12.5666666666667, 12, 34, 0)]
    [DataRow(12.5822222222222, 12, 34, 56)]
    [DataRow(12.5824413888889, 12, 34, 56.789)]
    [DataRow(-12, -12, 0, 0)]
    [DataRow(-12.5666666666667, -12, -34, 0)]
    [DataRow(-12.5822222222222, -12, -34, -56)]
    [DataRow(-12.5824413888889, -12, -34, -56.789)]
    public void DMSToDegrees_ReturnsCorrectValue(double expected, int d, int m, double s)
    {
        // Act
        double actual = DMSToDegrees(d, m, s);

        // Assert
        Assert.AreEqual(expected, actual, DoubleExtensions.DELTA);
    }

    [TestMethod]
    public void DegreesToString_ReturnsCorrectValue()
    {
        // Test 0.
        double deg = 0;
        Assert.AreEqual("0°0′0″", DegreesToString(deg));

        // Test whole number of degrees.
        deg = 12;
        Assert.AreEqual("12°0′0″", DegreesToString(deg));

        // Test degrees and arcminutes.
        deg = 12.5666666666667;
        Assert.AreEqual("12°34′0″", DegreesToString(deg));

        // Test degrees, arcminutes, and arcseconds.
        deg = 12.5822222222222;
        Assert.AreEqual("12°34′56″", DegreesToString(deg));

        // Test degrees, arcminutes, arcseconds, and milliarcseconds.
        deg = 12.5824413888889;
        Assert.AreEqual("12°34′56.789″", DegreesToString(deg, 3));

        // Test whole negative degrees.
        deg = -12;
        Assert.AreEqual("-12°0′0″", DegreesToString(deg));

        // Test negative degrees and arcminutes.
        deg = -12.5666666666667;
        Assert.AreEqual("-12°34′0″", DegreesToString(deg));

        // Test negative degrees, arcminutes, and arcseconds.
        deg = -12.5822222222222;
        Assert.AreEqual("-12°34′56″", DegreesToString(deg));

        // Test negative degrees, arcminutes, arcseconds, and milliarcseconds.
        deg = -12.5824413888889;
        Assert.AreEqual("-12°34′56.789″", DegreesToString(deg, 3));
    }
}
