using Galaxon.Numerics.Geometry;

namespace Galaxon.Numerics.Tests;

[TestClass]
public class EllipticIntegralTests
{
    /// <summary>
    /// I had to use a smaller delta for testing RF() and RD() because as yet I haven't found
    /// another online calculator with higher precision to compare results with.
    /// You can test the functions here:
    /// <see href="https://rdrr.io/cran/Carlson/src/R/RF.R"/>
    /// In the textarea where it says "Try the Carlson package in your browser"
    /// e.g. gsl::ellint_RF(3789, 12, 777)
    /// There's also this one, but it's less precise:
    /// <see href="https://calcresource.com/eval-elliptic-carlson.html"/>
    /// </summary>
    private const double _Delta = 1e-7;

    [TestMethod]
    public void TestRF()
    {
        Assert.AreEqual(EllipticIntegrals.RF(1, 1, 1), 1, _Delta);
        Assert.AreEqual(EllipticIntegrals.RF(1, 2, 3), 0.7269459, _Delta);
        Assert.AreEqual(EllipticIntegrals.RF(3, 2, 1), 0.7269459, _Delta);
        Assert.AreEqual(EllipticIntegrals.RF(100, 75, 50), 0.1168166, _Delta);
        Assert.AreEqual(EllipticIntegrals.RF(20, 22, 156), 0.1429193, _Delta);
        Assert.AreEqual(EllipticIntegrals.RF(3789, 12, 777), 0.03460313, _Delta);
    }

    [TestMethod]
    public void TestRD()
    {
        Assert.AreEqual(EllipticIntegrals.RD(1, 1, 1), 1, _Delta);
        Assert.AreEqual(EllipticIntegrals.RD(1, 2, 3), 0.2904603, _Delta);
        Assert.AreEqual(EllipticIntegrals.RD(3, 2, 1), 0.5591122, _Delta);
        Assert.AreEqual(EllipticIntegrals.RD(100, 75, 50), 0.001999792, _Delta);
        Assert.AreEqual(EllipticIntegrals.RD(20, 22, 156), 0.001396684, _Delta);
        Assert.AreEqual(EllipticIntegrals.RD(3789, 12, 777), 5.002079e-5, _Delta);
    }

    [TestMethod]
    public void TestFE()
    {
        double F, E;

        (F, E) = EllipticIntegrals.FE(1, 1);
        Assert.AreEqual(1.226191170883517070813, F, _Delta);
        Assert.AreEqual(0.8414709848078965066525, E, _Delta);

        (F, E) = EllipticIntegrals.FE(1.5, 0.8);
        Assert.AreEqual(1.877483256321042436684, F, _Delta);
        Assert.AreEqual(1.233809211535887646124, E, _Delta);

        (F, E) = EllipticIntegrals.FE(0.01, 0.256);
        Assert.AreEqual(0.01000001092248042626032, F, _Delta);
        Assert.AreEqual(0.009999989077541047654094, E, _Delta);

        (F, E) = EllipticIntegrals.FE(PI / 2, 0.9);
        Assert.AreEqual(2.280549138422770204614, F, _Delta);
        Assert.AreEqual(1.171697052781614141186, E, _Delta);
    }
}
