using System.Diagnostics;
using Galaxon.Numerics.Extensions;

namespace Galaxon.Tests.Core.Numbers;

[TestClass]
public class FloatingPointExtensionsTests
{
    [TestMethod]
    public void TestHalfMinMaxPosNormalSubnormalValues()
    {
        var minPosSubnormal = FloatingPointExtensions.GetMinPosSubnormalValue<Half>();
        Trace.WriteLine($"XHalf.MinPosSubnormalValue = {minPosSubnormal:E10}");
        var maxPosSubnormal = FloatingPointExtensions.GetMaxPosSubnormalValue<Half>();
        Trace.WriteLine($"XHalf.MaxPosSubnormalValue = {maxPosSubnormal:E10}");
        var minPosNormal = FloatingPointExtensions.GetMinPosNormalValue<Half>();
        Trace.WriteLine($"XHalf.MinPosNormalValue = {minPosNormal:E10}");
        var maxPosNormal = FloatingPointExtensions.GetMaxPosNormalValue<Half>();
        Trace.WriteLine($"XHalf.MaxPosNormalValue = {maxPosNormal:E10}");
    }

    [TestMethod]
    public void TestFloatMinMaxPosNormalSubnormalValues()
    {
        var minPosSubnormal = FloatingPointExtensions.GetMinPosSubnormalValue<float>();
        Trace.WriteLine($"XFloat.MinPosSubnormalValue = {minPosSubnormal:E10}");
        var maxPosSubnormal = FloatingPointExtensions.GetMaxPosSubnormalValue<float>();
        Trace.WriteLine($"XFloat.MaxPosSubnormalValue = {maxPosSubnormal:E10}");
        var minPosNormal = FloatingPointExtensions.GetMinPosNormalValue<float>();
        Trace.WriteLine($"XFloat.MinPosNormalValue = {minPosNormal:E10}");
        var maxPosNormal = FloatingPointExtensions.GetMaxPosNormalValue<float>();
        Trace.WriteLine($"XFloat.MaxPosNormalValue = {maxPosNormal:E10}");
    }

    [TestMethod]
    public void TestDoubleMinMaxPosNormalSubnormalValues()
    {
        var minPosSubnormal = FloatingPointExtensions.GetMinPosSubnormalValue<double>();
        Trace.WriteLine($"DoubleExtensions.MinPosSubnormalValue = {minPosSubnormal:E10}");
        var maxPosSubnormal = FloatingPointExtensions.GetMaxPosSubnormalValue<double>();
        Trace.WriteLine($"DoubleExtensions.MaxPosSubnormalValue = {maxPosSubnormal:E10}");
        var minPosNormal = FloatingPointExtensions.GetMinPosNormalValue<double>();
        Trace.WriteLine($"DoubleExtensions.MinPosNormalValue = {minPosNormal:E10}");
        var maxPosNormal = FloatingPointExtensions.GetMaxPosNormalValue<double>();
        Trace.WriteLine($"DoubleExtensions.MaxPosNormalValue = {maxPosNormal:E10}");
    }

    [TestMethod]
    public void TestMinMaxExpForHalf()
    {
        Assert.AreEqual(-14, FloatingPointExtensions.GetMinExp<Half>());
        Assert.AreEqual(15, FloatingPointExtensions.GetMaxExp<Half>());
        Assert.AreEqual(15, FloatingPointExtensions.GetExpBias<Half>());
    }

    [TestMethod]
    public void TestMinMaxExpForFloat()
    {
        Assert.AreEqual(-126, FloatingPointExtensions.GetMinExp<float>());
        Assert.AreEqual(127, FloatingPointExtensions.GetMaxExp<float>());
        Assert.AreEqual(127, FloatingPointExtensions.GetExpBias<float>());
    }

    [TestMethod]
    public void TestMinMaxExpForDouble()
    {
        Assert.AreEqual(-1022, FloatingPointExtensions.GetMinExp<double>());
        Assert.AreEqual(1023, FloatingPointExtensions.GetMaxExp<double>());
        Assert.AreEqual(1023, FloatingPointExtensions.GetExpBias<double>());
    }
}
