using Galaxon.Core.Types;

namespace Galaxon.Tests.Core;

class Example
{
    public static readonly int someField = 10;

    public static string SomeProperty { get; set; } = "Hello, world!";
}

[TestClass]
public class ReflectionExtensionsTests
{
    [TestMethod]
    public void GetStaticFieldValueWorks()
    {
        int expected = Example.someField;
        int actual = ReflectionExtensions.GetStaticFieldValue<Example, int>("someField");
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void GetStaticFieldValueExceptionOnUnknownField()
    {
        try
        {
            ReflectionExtensions.GetStaticFieldValue<Example, int>("nonexistentField");
            Assert.Fail("A MissingFieldException should be thrown.");
        }
        catch (MissingFieldException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    [TestMethod]
    public void GetStaticPropertyValueWorks()
    {
        string expected = Example.SomeProperty;
        string actual = ReflectionExtensions.GetStaticPropertyValue<Example, string>("SomeProperty");
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void GetStaticPropertyValueExceptionOnUnknownProperty()
    {
        try
        {
            ReflectionExtensions.GetStaticPropertyValue<Example, int>("nonexistentProperty");
            Assert.Fail("A MissingMemberException should be thrown.");
        }
        catch (MissingMemberException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    [TestMethod]
    public void GetStaticFieldOrPropertyValueWorks()
    {
        int expected = Example.someField;
        int actual = ReflectionExtensions.GetStaticFieldOrPropertyValue<Example, int>("someField");
        Assert.AreEqual(expected, actual);

        string expected2 = Example.SomeProperty;
        string actual2 = ReflectionExtensions.GetStaticFieldOrPropertyValue<Example, string>("SomeProperty");
        Assert.AreEqual(expected2, actual2);
    }

    [TestMethod]
    public void GetStaticFieldOrPropertyValueExceptionOnUnknownField()
    {
        try
        {
            ReflectionExtensions.GetStaticFieldOrPropertyValue<Example, int>("nonexistentField");
            Assert.Fail("A MissingFieldException should be thrown.");
        }
        catch (MissingMemberException ex)
        {
            Console.WriteLine(ex.Message);
        }

        try
        {
            ReflectionExtensions.GetStaticFieldOrPropertyValue<Example, int>("nonexistentProperty");
            Assert.Fail("A MissingMemberException should be thrown.");
        }
        catch (MissingMemberException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
