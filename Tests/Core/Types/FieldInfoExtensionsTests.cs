using System.Reflection;
using Galaxon.Core.Types;

namespace Galaxon.Tests.Core.Types;

[TestClass]
public class FieldExtensionsTests
{
    public class TestClass
    {
        public static string staticField = "Hello";

        public string instanceField = "World";

        [System.ComponentModel.Description("Coder's breakfast")]
        public string fieldWithDescription = "Coffee";

        public string fieldWithoutDescription = "Pizza";
    }

    #region GetValue

    [TestMethod]
    public void GetValue_StaticField_ReturnsCorrectValue()
    {
        // Arrange
        FieldInfo? fieldInfo = typeof(TestClass).GetField("staticField");

        // Act
        object? value = fieldInfo!.GetValue();

        // Assert
        Assert.AreEqual("Hello", value);
    }

    [TestMethod]
    public void GetValue_InstanceField_ThrowsException()
    {
        // Arrange
        FieldInfo? fieldInfo = typeof(TestClass).GetField("instanceField");

        // Act & Assert
        Assert.ThrowsException<TargetException>(() =>
        {
            object? _ = fieldInfo!.GetValue();
        });
    }

    #endregion GetValue

    #region GetDescription

    [TestMethod]
    public void GetDescription_FieldWithDescriptionAttribute_ReturnsDescription()
    {
        // Arrange
        FieldInfo? fieldInfo = typeof(TestClass).GetField("fieldWithDescription");

        // Act
        string description = fieldInfo!.GetDescription();

        // Assert
        Assert.AreEqual("Coder's breakfast", description);
    }

    [TestMethod]
    public void GetDescription_FieldWithoutDescriptionAttribute_ReturnsEmptyString()
    {
        // Arrange
        FieldInfo? fieldInfo = typeof(TestClass).GetField("fieldWithoutDescription");

        // Act
        string description = fieldInfo!.GetDescription();

        // Assert
        Assert.AreEqual("", description);
    }

    #endregion GetDescription
}
