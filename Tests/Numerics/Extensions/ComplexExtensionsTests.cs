using System.Numerics;
using Galaxon.Numerics.Extensions;

namespace Galaxon.Tests.Numerics.Extensions;

[TestClass]
public class ComplexExtensionsTests
{
    [TestMethod]
    public void Sort_SortsListOfComplexNumbers()
    {
        // Arrange
        var unsortedList = new List<Complex>
        {
            new Complex(3, 2),
            new Complex(1, 5),
            new Complex(4, 1),
            new Complex(2, 3),
            new Complex(3, -1),
            new Complex(4, -6),
        };

        // Complex numbers ordered by the real part first, then the imaginary part.
        var expectedSortedList = new List<Complex>
        {
            new Complex(1, 5),
            new Complex(2, 3),
            new Complex(3, -1),
            new Complex(3, 2),
            new Complex(4, -6),
            new Complex(4, 1)
        };

        // Act
        List<Complex> sortedList = ComplexExtensions.Sort(unsortedList);

        // Assert
        CollectionAssert.AreEqual(expectedSortedList, sortedList);
    }

    [TestMethod]
    public void Sort_EmptyList_DoesNotThrowException()
    {
        // Arrange
        var emptyList = new List<Complex>();

        // Act & Assert
        var sortedList = ComplexExtensions.Sort(emptyList);
        // If no exception is thrown, the test passes.
    }

    [TestMethod]
    public void Sort_ListWithOneElement_DoesNotChangeOrder()
    {
        // Arrange
        var list = new List<Complex> { new Complex(3, 2) };

        // Act
        var sortedList = ComplexExtensions.Sort(list);

        // Assert
        Assert.AreEqual(1, list.Count);
        Assert.AreEqual(new Complex(3, 2), list[0]);
    }
}
