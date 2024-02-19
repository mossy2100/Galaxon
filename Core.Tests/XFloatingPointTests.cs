using System.Diagnostics;
using Galaxon.Core.Numbers.Extensions;

namespace Galaxon.Core.Tests;

[TestClass]
public class XFloatingPointTests
{
    [TestMethod]
    public void TestHalfMinMaxPosNormalSubnormalValues()
    {
        var minPosSubnormal = XFloatingPoint.GetMinPosSubnormalValue<Half>();
        Trace.WriteLine($"XHalf.MinPosSubnormalValue = {minPosSubnormal:E10}");
        var maxPosSubnormal = XFloatingPoint.GetMaxPosSubnormalValue<Half>();
        Trace.WriteLine($"XHalf.MaxPosSubnormalValue = {maxPosSubnormal:E10}");
        var minPosNormal = XFloatingPoint.GetMinPosNormalValue<Half>();
        Trace.WriteLine($"XHalf.MinPosNormalValue = {minPosNormal:E10}");
        var maxPosNormal = XFloatingPoint.GetMaxPosNormalValue<Half>();
        Trace.WriteLine($"XHalf.MaxPosNormalValue = {maxPosNormal:E10}");
    }

    [TestMethod]
    public void TestFloatMinMaxPosNormalSubnormalValues()
    {
        var minPosSubnormal = XFloatingPoint.GetMinPosSubnormalValue<float>();
        Trace.WriteLine($"XFloat.MinPosSubnormalValue = {minPosSubnormal:E10}");
        var maxPosSubnormal = XFloatingPoint.GetMaxPosSubnormalValue<float>();
        Trace.WriteLine($"XFloat.MaxPosSubnormalValue = {maxPosSubnormal:E10}");
        var minPosNormal = XFloatingPoint.GetMinPosNormalValue<float>();
        Trace.WriteLine($"XFloat.MinPosNormalValue = {minPosNormal:E10}");
        var maxPosNormal = XFloatingPoint.GetMaxPosNormalValue<float>();
        Trace.WriteLine($"XFloat.MaxPosNormalValue = {maxPosNormal:E10}");
    }

    [TestMethod]
    public void TestDoubleMinMaxPosNormalSubnormalValues()
    {
        var minPosSubnormal = XFloatingPoint.GetMinPosSubnormalValue<double>();
        Trace.WriteLine($"XDouble.MinPosSubnormalValue = {minPosSubnormal:E10}");
        var maxPosSubnormal = XFloatingPoint.GetMaxPosSubnormalValue<double>();
        Trace.WriteLine($"XDouble.MaxPosSubnormalValue = {maxPosSubnormal:E10}");
        var minPosNormal = XFloatingPoint.GetMinPosNormalValue<double>();
        Trace.WriteLine($"XDouble.MinPosNormalValue = {minPosNormal:E10}");
        var maxPosNormal = XFloatingPoint.GetMaxPosNormalValue<double>();
        Trace.WriteLine($"XDouble.MaxPosNormalValue = {maxPosNormal:E10}");
    }

    [TestMethod]
    public void TestMinMaxExpForHalf()
    {
        Assert.AreEqual(-14, XFloatingPoint.GetMinExp<Half>());
        Assert.AreEqual(15, XFloatingPoint.GetMaxExp<Half>());
        Assert.AreEqual(15, XFloatingPoint.GetExpBias<Half>());
    }

    [TestMethod]
    public void TestMinMaxExpForFloat()
    {
        Assert.AreEqual(-126, XFloatingPoint.GetMinExp<float>());
        Assert.AreEqual(127, XFloatingPoint.GetMaxExp<float>());
        Assert.AreEqual(127, XFloatingPoint.GetExpBias<float>());
    }

    [TestMethod]
    public void TestMinMaxExpForDouble()
    {
        Assert.AreEqual(-1022, XFloatingPoint.GetMinExp<double>());
        Assert.AreEqual(1023, XFloatingPoint.GetMaxExp<double>());
        Assert.AreEqual(1023, XFloatingPoint.GetExpBias<double>());
    }
}
