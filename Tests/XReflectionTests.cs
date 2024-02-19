using Galaxon.Core.Types;

namespace Galaxon.Tests;

class Example
{
    public static readonly int someField = 10;

    public static string SomeProperty { get; set; } = "Hello, world!";
}

[TestClass]
public class XReflectionTests
{
    [TestMethod]
    public void GetStaticFieldValueWorks()
    {
        int expected = Example.someField;
        int actual = XReflection.GetStaticFieldValue<Example, int>("someField");
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void GetStaticFieldValueExceptionOnUnknownField()
    {
        try
        {
            XReflection.GetStaticFieldValue<Example, int>("nonexistentField");
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
        string actual = XReflection.GetStaticPropertyValue<Example, string>("SomeProperty");
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void GetStaticPropertyValueExceptionOnUnknownProperty()
    {
        try
        {
            XReflection.GetStaticPropertyValue<Example, int>("nonexistentProperty");
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
        int actual = XReflection.GetStaticFieldOrPropertyValue<Example, int>("someField");
        Assert.AreEqual(expected, actual);

        string expected2 = Example.SomeProperty;
        string actual2 = XReflection.GetStaticFieldOrPropertyValue<Example, string>("SomeProperty");
        Assert.AreEqual(expected2, actual2);
    }

    [TestMethod]
    public void GetStaticFieldOrPropertyValueExceptionOnUnknownField()
    {
        try
        {
            XReflection.GetStaticFieldOrPropertyValue<Example, int>("nonexistentField");
            Assert.Fail("A MissingFieldException should be thrown.");
        }
        catch (MissingMemberException ex)
        {
            Console.WriteLine(ex.Message);
        }

        try
        {
            XReflection.GetStaticFieldOrPropertyValue<Example, int>("nonexistentProperty");
            Assert.Fail("A MissingMemberException should be thrown.");
        }
        catch (MissingMemberException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
