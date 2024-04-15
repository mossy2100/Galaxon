using Galaxon.Numerics.Extensions;

namespace Galaxon.Tests.Numerics.Extensions;

[TestClass]
public class Int64ExtensionsTests
{
    [DataTestMethod]
    [DataRow(1, "st")]
    [DataRow(2, "nd")]
    [DataRow(3, "rd")]
    [DataRow(4, "th")]
    [DataRow(11, "th")]
    [DataRow(12, "th")]
    [DataRow(13, "th")]
    [DataRow(21, "st")]
    [DataRow(22, "nd")]
    [DataRow(23, "rd")]
    [DataRow(24, "th")]
    [DataRow(111, "th")]
    [DataRow(112, "th")]
    [DataRow(113, "th")]
    public void GetOrdinalSuffix_PositiveValues_ReturnsCorrectResult(long number, string expectedSuffix)
    {
        // Act
        string suffix = Int64Extensions.GetOrdinalSuffix(number);

        // Assert
        Assert.AreEqual(expectedSuffix, suffix);
    }
}
