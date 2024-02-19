using Galaxon.Core.Testing;
using static Galaxon.Numerics.Geometry.Angles;

namespace Galaxon.Tests.Numerics.Geometry;

[TestClass]
public class AnglesTests
{
    private const double _Delta = 1e-9;

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
            XAssert.IsInRange(actual, -PI, PI);
            Assert.AreEqual(expected, actual, _Delta);
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
            XAssert.IsInRange(actual, 0, Tau);
            Assert.AreEqual(expected, actual, _Delta);
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
            XAssert.IsInRange(actual, -180, 180);
            Assert.AreEqual(expected, actual, _Delta);
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
            XAssert.IsInRange(actual, 0, 360);
            Assert.AreEqual(expected, actual, _Delta);
        }
    }

    [TestMethod]
    public void RadiansToDegreesTest()
    {
        Assert.AreEqual(180, RadiansToDegrees(PI), _Delta);
        Assert.AreEqual(90, RadiansToDegrees(PI / 2), _Delta);
        Assert.AreEqual(60, RadiansToDegrees(PI / 3), _Delta);
        Assert.AreEqual(45, RadiansToDegrees(PI / 4), _Delta);
        Assert.AreEqual(360, RadiansToDegrees(Tau), _Delta);
    }

    [TestMethod]
    public void DegreesToRadiansTest()
    {
        Assert.AreEqual(PI, DegreesToRadians(180), _Delta);
        Assert.AreEqual(PI / 2, DegreesToRadians(90), _Delta);
        Assert.AreEqual(PI / 3, DegreesToRadians(60), _Delta);
        Assert.AreEqual(PI / 4, DegreesToRadians(45), _Delta);
        Assert.AreEqual(Tau, DegreesToRadians(360), _Delta);
    }

    [TestMethod]
    public void DegreesToDegreesMinutesSecondsTest()
    {
        // Test 0.
        double deg = 0;
        (double, double, double) deltaAngle = (0, 0, _Delta);
        XAssert.AreEqual((0, 0, 0), DegreesToDegreesMinutesSeconds(deg), deltaAngle);

        // Test whole number of degrees.
        deg = 12;
        XAssert.AreEqual((12, 0, 0), DegreesToDegreesMinutesSeconds(deg), deltaAngle);

        // Test degrees and minutes.
        deg = 12.5666666666667;
        XAssert.AreEqual((12, 34, 0), DegreesToDegreesMinutesSeconds(deg), deltaAngle);

        // Test degrees, minutes, and seconds.
        deg = 12.5822222222222;
        XAssert.AreEqual((12, 34, 56), DegreesToDegreesMinutesSeconds(deg), deltaAngle);

        // Test degrees, minutes, seconds, and milliseconds.
        deg = 12.5824413888889;
        XAssert.AreEqual((12, 34, 56.789), DegreesToDegreesMinutesSeconds(deg), deltaAngle);

        // Test whole negative degrees.
        deg = -12;
        XAssert.AreEqual((-12, 0, 0), DegreesToDegreesMinutesSeconds(deg), deltaAngle);

        // Test negative degrees and minutes.
        deg = -12.5666666666667;
        XAssert.AreEqual((-12, -34, 0), DegreesToDegreesMinutesSeconds(deg), deltaAngle);

        // Test negative degrees, minutes, and seconds.
        deg = -12.5822222222222;
        XAssert.AreEqual((-12, -34, -56), DegreesToDegreesMinutesSeconds(deg), deltaAngle);

        // Test negative degrees, minutes, seconds, and milliseconds.
        deg = -12.5824413888889;
        XAssert.AreEqual((-12, -34, -56.789), DegreesToDegreesMinutesSeconds(deg), deltaAngle);
    }

    [TestMethod]
    public void DegreesMinutesSecondsToDegreesTest()
    {
        // Test 0.
        Assert.AreEqual(0, DegreesMinutesSecondsToDegrees(0, 0), _Delta);

        // Test whole number of degrees.
        Assert.AreEqual(12, DegreesMinutesSecondsToDegrees(12, 0), _Delta);

        // Test degrees and minutes.
        Assert.AreEqual(12.5666666666667, DegreesMinutesSecondsToDegrees(12, 34), _Delta);

        // Test degrees, minutes, and seconds.
        Assert.AreEqual(12.5822222222222, DegreesMinutesSecondsToDegrees(12, 34, 56), _Delta);

        // Test degrees, minutes, seconds, and milliseconds.
        Assert.AreEqual(12.5824413888889, DegreesMinutesSecondsToDegrees(12, 34, 56.789), _Delta);

        // Test negative whole number of degrees.
        Assert.AreEqual(-12, DegreesMinutesSecondsToDegrees(-12, 0), _Delta);

        // Test negative degrees and minutes.
        Assert.AreEqual(-12.5666666666667, DegreesMinutesSecondsToDegrees(-12, -34), _Delta);

        // Test negative degrees, minutes, and seconds.
        Assert.AreEqual(-12.5822222222222, DegreesMinutesSecondsToDegrees(-12, -34, -56), _Delta);

        // Test negative degrees, minutes, seconds, and milliseconds.
        Assert.AreEqual(-12.5824413888889, DegreesMinutesSecondsToDegrees(-12, -34, -56.789), _Delta);
    }

    [TestMethod]
    public void FormatDmsTest()
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
