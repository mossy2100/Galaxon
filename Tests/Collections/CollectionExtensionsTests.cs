using System.Collections;
using Galaxon.Core.Collections;

namespace Galaxon.Tests.Collections;

[TestClass]
public class CollectionExtensionsTests
{
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
