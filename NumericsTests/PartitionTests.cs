using System.Numerics;
using Galaxon.Numerics.Integers;

namespace Galaxon.Numerics.Tests;

[TestClass]
public class PartitionTests
{
    [TestMethod]
    public void TestPDigits()
    {
        Assert.AreEqual(1, Partitions.P(0));
        Assert.AreEqual(1, Partitions.P(1));
        Assert.AreEqual(2, Partitions.P(2));
        Assert.AreEqual(3, Partitions.P(3));
        Assert.AreEqual(5, Partitions.P(4));
        Assert.AreEqual(7, Partitions.P(5));
        Assert.AreEqual(11, Partitions.P(6));
        Assert.AreEqual(15, Partitions.P(7));
        Assert.AreEqual(22, Partitions.P(8));
        Assert.AreEqual(30, Partitions.P(9));
    }

    [TestMethod]
    public void TestPTens()
    {
        Assert.AreEqual(42, Partitions.P(10));
        Assert.AreEqual(627, Partitions.P(20));
        Assert.AreEqual(5604, Partitions.P(30));
        Assert.AreEqual(37338, Partitions.P(40));
        Assert.AreEqual(204226, Partitions.P(50));
        Assert.AreEqual(966467, Partitions.P(60));
        Assert.AreEqual(4087968, Partitions.P(70));
        Assert.AreEqual(15796476, Partitions.P(80));
        Assert.AreEqual(56634173, Partitions.P(90));
        Assert.AreEqual(190569292, Partitions.P(100));
    }

    /// <summary>
    /// Test the maximum value supported by the method.
    /// </summary>
    [TestMethod]
    public void TestPMax()
    {
        Assert.AreEqual(BigInteger.Parse(
                "335882043162190684244465257601986956188015498728271478568995"
                + "924215333504731714172271223288113412362639732788262640233482"
                + "123630590162653009028693467046249775426411084015701802867888"
                + "208556585961685681557794125511515944466197421436709877965827"
                + "8272116033402923541053858868675003866332"),
            Partitions.P(ushort.MaxValue));
    }
}
