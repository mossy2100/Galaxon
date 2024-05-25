using System.Collections;
using Galaxon.Core.Types;

namespace Galaxon.Tests.Core.Types;

[TestClass]
public class ObjectExtensionsTests
{
    [TestMethod]
    public void IsEmpty_NullString_ReturnsTrue()
    {
        // Arrange
        string? name = null;

        // Act
        bool result = name.IsEmpty();

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void IsEmpty_EmptyString_ReturnsTrue()
    {
        // Arrange
        string name = "";

        // Act
        bool result = name.IsEmpty();

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void IsEmpty_NonEmptyString_ReturnsFalse()
    {
        // Arrange
        string name = "Charles Babbage";

        // Act
        bool result = name.IsEmpty();

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void IsEmpty_NullArray_ReturnsTrue()
    {
        // Arrange
        int[]? numbers = null;

        // Act
        bool result = numbers.IsEmpty();

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void IsEmpty_EmptyArray_ReturnsTrue()
    {
        // Arrange
        int[] numbers = [];

        // Act
        bool result = numbers.IsEmpty();

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void IsEmpty_NonEmptyArray_ReturnsFalse()
    {
        // Arrange
        int[] numbers = [2, 3, 5, 7];

        // Act
        bool result = numbers.IsEmpty();

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void IsEmpty_NullCollection_ReturnsTrue()
    {
        // Arrange
        ICollection? collection = null;

        // Act
        bool result = collection.IsEmpty();

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void IsEmpty_EmptyCollection_ReturnsTrue()
    {
        // Arrange
        ICollection collection = new List<int>();

        // Act
        bool result = collection.IsEmpty();

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void IsEmpty_NonEmptyCollection_ReturnsFalse()
    {
        // Arrange
        ICollection collection = new List<int> { 1, 2, 3 };

        // Act
        bool result = collection.IsEmpty();

        // Assert
        Assert.IsFalse(result);
    }
}
