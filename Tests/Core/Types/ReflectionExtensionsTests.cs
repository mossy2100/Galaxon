using System.Reflection;
using Galaxon.Core.Types;
using Galaxon.Numerics.BigNumbers;

namespace Galaxon.Tests.Core.Types;

[TestClass]
public class ReflectionExtensionsTests
{
    class TestClass
    {
        public static readonly int SomeField = 10;

        public static string SomeProperty => "Hello, world!";
    }

    #region GetStaticFieldValue

    [TestMethod]
    public void GetStaticFieldValue_ValidStaticField_ReturnsCorrectValue()
    {
        // Arrange
        int expected = TestClass.SomeField;

        // Act
        int actual = ReflectionExtensions.GetStaticFieldValue<TestClass, int>("SomeField");

        // Assert
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void GetStaticFieldValue_UnknownField_ThrowsException()
    {
        // Act & Assert
        Assert.ThrowsException<MissingFieldException>(() =>
            ReflectionExtensions.GetStaticFieldValue<TestClass, int>("nonexistentField"));
    }

    #endregion GetStaticFieldValue

    #region GetStaticPropertyValue

    [TestMethod]
    public void GetStaticPropertyValue_ValidStaticProperty_ReturnsCorrectValue()
    {
        // Arrange
        string expected = TestClass.SomeProperty;

        // Act
        string actual =
            ReflectionExtensions.GetStaticPropertyValue<TestClass, string>("SomeProperty");

        // Assert
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void GetStaticPropertyValue_UnknownProperty_ThrowsException()
    {
        // Act & Assert
        Assert.ThrowsException<MissingMemberException>(() =>
            ReflectionExtensions.GetStaticPropertyValue<TestClass, int>("nonexistentProperty"));
    }

    #endregion GetStaticPropertyValue

    #region GetStaticFieldOrPropertyValue

    [TestMethod]
    public void GetStaticFieldOrPropertyValue_ValidMembers_ReturnsCorrectValue()
    {
        // Arrange
        int expected = TestClass.SomeField;

        // Act
        int actual =
            ReflectionExtensions.GetStaticFieldOrPropertyValue<TestClass, int>("SomeField");

        // Assert
        Assert.AreEqual(expected, actual);

        // Arrange
        string expected2 = TestClass.SomeProperty;

        // Act
        string actual2 =
            ReflectionExtensions.GetStaticFieldOrPropertyValue<TestClass, string>("SomeProperty");

        // Assert
        Assert.AreEqual(expected2, actual2);
    }

    [TestMethod]
    public void GetStaticFieldOrPropertyValue_InvalidMembers_ThrowsException()
    {
        // Act & Assert
        Assert.ThrowsException<MissingMemberException>(() =>
            ReflectionExtensions.GetStaticFieldOrPropertyValue<TestClass, int>("nonexistentField"));
        Assert.ThrowsException<MissingMemberException>(() =>
            ReflectionExtensions.GetStaticFieldOrPropertyValue<TestClass, int>(
                "nonexistentProperty"));
    }

    #endregion GetStaticFieldOrPropertyValue

    #region GetConversionMethod

    [TestMethod]
    public void GetConversionMethod_WhenToMethodExists_ReturnsCorrectMethod()
    {
        // Arrange
        string methodName = "ToInt64";

        // Act
        MethodInfo? mi = ReflectionExtensions.GetConversionMethod<int, long>();

        // Assert
        Assert.IsNotNull(mi);
        Assert.AreEqual(methodName, mi.Name);
    }

    [TestMethod]
    public void GetConversionMethod_WhenFromMethodExists_ReturnsCorrectMethod()
    {
        // Arrange
        string methodName = "FromDateTime";

        // Act
        MethodInfo? mi = ReflectionExtensions.GetConversionMethod<DateTime, DateOnly>();

        // Assert
        Assert.IsNotNull(mi);
        Assert.AreEqual(methodName, mi.Name);
    }

    [TestMethod]
    public void GetConversionMethod_WhenParseMethodExists_ReturnsCorrectMethod()
    {
        // Arrange
        string methodName = "Parse";

        // Act
        MethodInfo? mi = ReflectionExtensions.GetConversionMethod<string, DateOnly>();

        // Assert
        Assert.IsNotNull(mi);
        Assert.AreEqual(methodName, mi.Name);
    }

    [TestMethod]
    public void GetConversionMethod_WhenImplicitConversionExists_ReturnsCorrectMethod()
    {
        // Arrange
        string methodName = "op_Implicit";

        // Act
        MethodInfo? mi = ReflectionExtensions.GetConversionMethod<int, BigDecimal>();

        // Assert
        Assert.IsNotNull(mi);
        Assert.AreEqual(methodName, mi.Name);
    }

    [TestMethod]
    public void GetConversionMethod_WhenExplicitConversionExists_ReturnsCorrectMethod()
    {
        // Arrange
        string methodName = "op_Explicit";

        // Act
        MethodInfo? mi = ReflectionExtensions.GetConversionMethod<BigDecimal, int>();

        // Assert
        Assert.IsNotNull(mi);
        Assert.AreEqual(methodName, mi.Name);
    }

    [TestMethod]
    public void GetConversionMethod_WhenNoConversionExists_ReturnsNull()
    {
        // Act
        MethodInfo? mi = ReflectionExtensions.GetConversionMethod<bool, DateOnly>();

        // Assert
        Assert.IsNull(mi);
    }

    #endregion GetConversionMethod

    #region CanConvert

    [TestMethod]
    public void CanConvert_WhenToMethodExists_ReturnsTrue()
    {
        // Act
        bool result = ReflectionExtensions.CanConvert<int, long>();

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void CanConvert_WhenFromMethodExists_ReturnsTrue()
    {
        // Act
        bool result = ReflectionExtensions.CanConvert<DateTime, DateOnly>();

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void CanConvert_WhenParseMethodExists_ReturnsTrue()
    {
        // Act
        bool result = ReflectionExtensions.CanConvert<string, DateOnly>();

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void CanConvert_WhenImplicitConversionExists_ReturnsTrue()
    {
        // Act
        bool result = ReflectionExtensions.CanConvert<int, BigDecimal>();

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void CanConvert_WhenExplicitConversionExists_ReturnsTrue()
    {
        // Act
        bool result = ReflectionExtensions.CanConvert<BigDecimal, int>();

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void CanConvert_WhenNoConversionExists_ReturnsFalse()
    {
        // Act
        bool result = ReflectionExtensions.CanConvert<bool, DateOnly>();

        // Assert
        Assert.IsFalse(result);
    }

    #endregion CanConvert

    #region Convert

    [TestMethod]
    public void Convert_WhenToMethodExists_ReturnsCorrectValue()
    {
        // Arrange
        int x = 5;
        long expected = 5L;

        // Act
        long actual = ReflectionExtensions.Convert<int, long>(x);

        // Assert
        Assert.AreEqual(expected, actual);
        Assert.AreEqual(typeof(long), actual.GetType());
    }

    [TestMethod]
    public void Convert_WhenFromMethodExists_ReturnsCorrectValue()
    {
        // Arrange
        DateTime christmasAfternoon = new (2024, 12, 31, 12, 34, 56);
        DateOnly expected = new (2024, 12, 31);

        // Act
        DateOnly actual = ReflectionExtensions.Convert<DateTime, DateOnly>(christmasAfternoon);

        // Assert
        Assert.AreEqual(expected, actual);
        Assert.AreEqual(typeof(DateOnly), actual.GetType());
    }

    [TestMethod]
    public void Convert_WhenParseMethodExists_ReturnsCorrectValue()
    {
        // Arrange
        string christmas = "2024-12-31";
        DateOnly expected = new (2024, 12, 31);

        // Act
        DateOnly actual = ReflectionExtensions.Convert<string, DateOnly>(christmas);

        // Assert
        Assert.AreEqual(expected, actual);
        Assert.AreEqual(typeof(DateOnly), actual.GetType());
    }

    [TestMethod]
    public void Convert_WhenImplicitConversionExists_ReturnsCorrectValue()
    {
        // Arrange
        int x = 123;
        BigDecimal expected = new BigDecimal(x);

        // Act
        BigDecimal actual = ReflectionExtensions.Convert<int, BigDecimal>(x);

        // Assert
        Assert.AreEqual(expected, actual);
        Assert.AreEqual(typeof(BigDecimal), actual.GetType());
    }

    [TestMethod]
    public void Convert_WhenExplicitConversionExists_ReturnsCorrectValue()
    {
        // Arrange
        BigDecimal x = new BigDecimal(123, 4);
        int expected = 1230000;

        // Act
        int actual = ReflectionExtensions.Convert<BigDecimal, int>(x);

        // Assert
        Assert.AreEqual(expected, actual);
        Assert.AreEqual(typeof(int), actual.GetType());
    }

    [TestMethod]
    public void Convert_WhenNoConversionExists_ThrowException()
    {
        // Act & Assert
        Assert.ThrowsException<InvalidCastException>(() =>
            ReflectionExtensions.Convert<bool, DateTime>(true));
    }

    #endregion Convert
}
