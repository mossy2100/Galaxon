using Galaxon.Core.Types;

namespace Galaxon.Tests.Core.Types;

[TestClass]
public class EnumExtensionsTests
{
    private enum Animal
    {
        Cat,

        [System.ComponentModel.Description("Good boy")]
        Dog
    }

    #region GetDescription

    [TestMethod]
    public void GetDescription_FieldWithoutDescription_ReturnsEmptyString()
    {
        // Arrange
        string expected = "";

        // Act
        string actual = Animal.Cat.GetDescription();

        // Assert
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void GetDescription_FieldWithDescription_ReturnsDescription()
    {
        // Arrange
        string expected = "Good boy";

        // Act
        string actual = Animal.Dog.GetDescription();

        // Assert
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void GetDescription_InvalidEnum_ThrowsException()
    {
        // Arrange
        var invalidEnum = (Animal)2;

        // Act
        invalidEnum.GetDescription();

        // Assert is handled by the ExpectedException attribute
    }

    #endregion GetDescription

    #region GetDescriptionOrName

    [TestMethod]
    public void GetDescriptionOrName_FieldWithoutDescription_ReturnsName()
    {
        // Arrange
        string expected = "Cat";

        // Act
        string actual = Animal.Cat.GetDescriptionOrName();

        // Assert
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void GetDescriptionOrName_FieldWithDescription_ReturnsDescription()
    {
        // Arrange
        string expected = "Good boy";

        // Act
        string actual = Animal.Dog.GetDescriptionOrName();

        // Assert
        Assert.AreEqual(expected, actual);
    }

    #endregion GetDescriptionOrName

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
    public void TryParse_EnumDescription_ReturnsTrue()
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
    public void TryParse_InvalidNameOrDescription_ReturnsFalse()
    {
        // Arrange
        string invalid = "Invalid";
        Animal defaultValue = default(Animal);

        // Act
        bool result = EnumExtensions.TryParse(invalid, out Animal actual);

        // Assert
        Assert.IsFalse(result);
        Assert.AreEqual(defaultValue, actual);
    }

    #endregion TryParse
}
