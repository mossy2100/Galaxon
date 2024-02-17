using Galaxon.Numerics.Geometry;

namespace Galaxon.Numerics.Tests;

[TestClass]
public class EllipsoidTests
{
    private const double _Delta = 1e-9;

    #region Invalid Arguments Tests

    [TestMethod]
    [TestCategory("InvalidArguments")]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void TestZeroRadiusA()
    {
        Ellipsoid ellipsoid = new (0, 100, 75);
    }

    [TestMethod]
    [TestCategory("InvalidArguments")]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void TestZeroRadiusB()
    {
        Ellipsoid ellipsoid = new (50, 0, 75);
    }

    [TestMethod]
    [TestCategory("InvalidArguments")]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void TestZeroRadiusC()
    {
        Ellipsoid ellipsoid = new (50, 100, 0);
    }

    [TestMethod]
    [TestCategory("InvalidArguments")]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void TestNegativeRadiusA()
    {
        Ellipsoid ellipsoid = new (-10, 100, 75);
    }

    [TestMethod]
    [TestCategory("InvalidArguments")]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void TestNegativeRadiusB()
    {
        Ellipsoid ellipsoid = new (50, -1.5, 75);
    }

    [TestMethod]
    [TestCategory("InvalidArguments")]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void TestNegativeRadiusC()
    {
        Ellipsoid ellipsoid = new (50, 100, -8);
    }

    #endregion Invalid Arguments Tests

    #region Volume Tests

    [TestMethod]
    [TestCategory("Volume")]
    public void TestVolumeSphere()
    {
        Sphere sphere = new (100);
        Assert.AreEqual(sphere.Volume, 4188790.20478639099, _Delta);
    }

    [TestMethod]
    [TestCategory("Volume")]
    public void TestVolumeOblate()
    {
        Ellipsoid ellipsoid = new (100, 100, 75);
        Assert.AreEqual(ellipsoid.Volume, 3141592.65358979324, _Delta);
    }

    [TestMethod]
    [TestCategory("Volume")]
    public void TestVolumeProlate()
    {
        Ellipsoid ellipsoid = new (100, 75, 75);
        Assert.AreEqual(ellipsoid.Volume, 2356194.49019234493, _Delta);
    }

    [TestMethod]
    [TestCategory("Volume")]
    public void TestVolumeScalene()
    {
        // Basic.
        Ellipsoid ellipsoid = new (100, 75, 50);
        Assert.AreEqual(ellipsoid.Volume, 1570796.32679489662, _Delta);
        // Flat.
        ellipsoid = new Ellipsoid(0.1, 100, 50);
        Assert.AreEqual(ellipsoid.Volume, 2094.395102393195492308, _Delta);
        // Long.
        ellipsoid = new Ellipsoid(0.1, 0.2, 50);
        Assert.AreEqual(ellipsoid.Volume, 4.188790204786390984617, _Delta);
    }

    #endregion Volume Tests

    #region Surface Area Tests

    [TestMethod]
    [TestCategory("SurfaceArea")]
    public void TestSurfaceAreaSphere()
    {
        Sphere sphere = new (100);
        Assert.AreEqual(sphere.SurfaceArea, 125663.70614359173, _Delta);
    }

    [TestMethod]
    [TestCategory("SurfaceArea")]
    public void TestSurfaceAreaOblate()
    {
        Ellipsoid ellipsoid = new (100, 100, 75);
        Assert.AreEqual(ellipsoid.SurfaceArea, 105330.988412017769, _Delta);
    }

    [TestMethod]
    [TestCategory("SurfaceArea")]
    public void TestSurfaceAreaProlate()
    {
        Ellipsoid ellipsoid = new (100, 75, 75);
        Assert.AreEqual(ellipsoid.SurfaceArea, 86833.8475986630866, _Delta);
    }

    [TestMethod]
    [TestCategory("SurfaceArea")]
    public void TestSurfaceAreaScalene()
    {
        // Basic.
        Ellipsoid ellipsoid = new (100, 75, 50);
        Assert.AreEqual(ellipsoid.SurfaceArea, 69716.106183756452, _Delta);
        // Flat.
        ellipsoid = new Ellipsoid(0.1, 100, 50);
        Assert.AreEqual(ellipsoid.SurfaceArea, 31416.4838110008630761, _Delta);
        // Long.
        ellipsoid = new Ellipsoid(0.1, 0.2, 50);
        Assert.AreEqual(ellipsoid.SurfaceArea, 76.0931647132572842069, _Delta);
    }

    #endregion Surface Area Tests
}
