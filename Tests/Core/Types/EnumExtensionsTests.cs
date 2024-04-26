using Galaxon.Core.Types;
using Microsoft.OpenApi.Attributes;

namespace Galaxon.Tests.Core.Types;

[TestClass]
public class EnumExtensionsTests
{
    private enum Animal
    {
        Cat,

        [Display("Good boy")]
        Dog
    }

    #region TryParse

    [TestMethod]
    public void TryParse_EnumName_ReturnsTrue()
    {
        // Arrange
        string name = "Cat";
        Animal expected = Animal.Cat;

        // Act
        bool result = EnumExtensions.TryParse(name, out Animal actual);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void TryParse_EnumDisplay_ReturnsTrue()
    {
        // Arrange
        string description = "Good boy";
        Animal expected = Animal.Dog;

        // Act
        bool result = EnumExtensions.TryParse(description, out Animal actual);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void TryParse_InvalidDisplayName_ReturnsFalse()
    {
        // Arrange
        string invalid = "Invalid";
        Animal defaultValue = default;

        // Act
        bool result = EnumExtensions.TryParse(invalid, out Animal actual);

        // Assert
        Assert.IsFalse(result);
        Assert.AreEqual(defaultValue, actual);
    }

    #endregion TryParse
}
