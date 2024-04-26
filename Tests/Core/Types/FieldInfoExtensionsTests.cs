using System.Reflection;
using Galaxon.Core.Types;
using Microsoft.OpenApi.Attributes;

namespace Galaxon.Tests.Core.Types;

[TestClass]
public class FieldExtensionsTests
{
    public class TestClass
    {
        public static string staticField = "Hello";

        public string instanceField = "World";

        [Display("Coder's breakfast")]
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

    #region GetDisplayName

    [TestMethod]
    public void GetDisplayName_FieldWithDescriptionAttribute_ReturnsDescription()
    {
        // Arrange
        FieldInfo? fieldInfo = typeof(TestClass).GetField("fieldWithDescription");

        // Act
        string description = fieldInfo!.GetDisplayName();

        // Assert
        Assert.AreEqual("Coder's breakfast", description);
    }

    [TestMethod]
    public void GetDisplayName_FieldWithoutDescriptionAttribute_ReturnsEmptyString()
    {
        // Arrange
        FieldInfo? fieldInfo = typeof(TestClass).GetField("fieldWithoutDescription");

        // Act
        string description = fieldInfo!.GetDisplayName();

        // Assert
        Assert.AreEqual("", description);
    }

    #endregion GetDisplayName
}
