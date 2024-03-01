using Galaxon.Numerics.Algebra;
using Galaxon.Numerics.BigNumbers;

namespace Galaxon.Tests.Numerics.Algebra;

[TestClass]
public class AnalysisTests
{
    #region Bernoulli

    [TestMethod]
    public void Bernoulli_ThrowsException_ForNegativeIndex()
    {
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => Analysis.Bernoulli(-1));
    }

    /// <summary>
    /// Test the Bernoulli method returns the correct value for first 21 Bernoulli numbers.
    /// </summary>
    /// <see href="https://en.wikipedia.org/wiki/Bernoulli_number"/>
    [DataTestMethod]
    [DataRow(0, 1)]
    [DataRow(1, 1, 2)]
    [DataRow(2, 1, 6)]
    [DataRow(3, 0)]
    [DataRow(4, -1, 30)]
    [DataRow(5, 0)]
    [DataRow(6, 1, 42)]
    [DataRow(7, 0)]
    [DataRow(8, -1, 30)]
    [DataRow(9, 0)]
    [DataRow(10, 5, 66)]
    [DataRow(11, 0)]
    [DataRow(12, -691, 2730)]
    [DataRow(13, 0)]
    [DataRow(14, 7, 6)]
    [DataRow(15, 0)]
    [DataRow(16, -3617, 510)]
    [DataRow(17, 0)]
    [DataRow(18, 43867, 798)]
    [DataRow(19, 0)]
    [DataRow(20, -174611, 330)]
    public void Bernoulli_ValidIndices_ReturnsExpectedValue(int index, int numerator, int denominator = 1)
    {
        BigRational expected = new BigRational(numerator, denominator);
        BigRational result = Analysis.Bernoulli(index);
        Assert.AreEqual(expected, result);
    }

    #endregion Bernoulli
}
