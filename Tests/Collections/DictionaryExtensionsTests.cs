using Galaxon.Core.Collections;
using Galaxon.Core.Exceptions;

namespace Galaxon.Tests.Collections;

[TestClass]
public class DictionaryExtensionsTests
{
    [TestMethod]
    public void HasUniqueValues_EmptyDictionary_ReturnsTrue()
    {
        // Arrange
        var dict = new Dictionary<int, string>();

        // Act
        bool result = dict.HasUniqueValues();

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void HasUniqueValues_DictionaryWithUniqueValues_ReturnsTrue()
    {
        // Arrange
        var dict = new Dictionary<int, string>
        {
            { 1, "one" },
            { 2, "two" },
            { 3, "three" }
        };

        // Act
        bool result = dict.HasUniqueValues();

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void HasUniqueValues_DictionaryWithDuplicateValues_ReturnsFalse()
    {
        // Arrange
        var dict = new Dictionary<int, string>
        {
            { 1, "one" },
            { 2, "two" },
            { 3, "two" } // Duplicate value
        };

        // Act
        bool result = dict.HasUniqueValues();

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Flip_DictionaryWithUniqueValues_ReturnsFlippedDictionary()
    {
        // Arrange
        Dictionary<int, string> dict = new ()
        {
            { 1, "one" },
            { 2, "two" },
            { 3, "three" }
        };

        // Act
        var flippedDict = dict.Flip();

        // Assert
        Assert.AreEqual(1, flippedDict["one"]);
        Assert.AreEqual(2, flippedDict["two"]);
        Assert.AreEqual(3, flippedDict["three"]);
    }

    [TestMethod]
    public void Flip_DictionaryWithDuplicateValues_ThrowsArgumentInvalidException()
    {
        // Arrange
        Dictionary<int, string> dict = new ()
        {
            { 1, "one" },
            { 2, "two" },
            { 3, "one" }
        };

        // Act & Assert
        Assert.ThrowsException<ArgumentInvalidException>(() => dict.Flip());
    }
}
