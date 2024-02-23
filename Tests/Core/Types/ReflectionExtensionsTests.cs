using Galaxon.Core.Types;

namespace Galaxon.Tests.Core.Types;

[TestClass]
public class ReflectionExtensionsTests
{
    class TestClass
    {
        public static readonly int SomeField = 10;

        public static string SomeProperty => "Hello, world!";
    }

    [TestMethod]
    public void GetStaticFieldValueWorks()
    {
        int expected = TestClass.SomeField;
        int actual = ReflectionExtensions.GetStaticFieldValue<TestClass, int>("SomeField");
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void GetStaticFieldValueExceptionOnUnknownField()
    {
        try
        {
            ReflectionExtensions.GetStaticFieldValue<TestClass, int>("nonexistentField");
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
        string expected = TestClass.SomeProperty;
        string actual = ReflectionExtensions.GetStaticPropertyValue<TestClass, string>("SomeProperty");
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void GetStaticPropertyValueExceptionOnUnknownProperty()
    {
        try
        {
            ReflectionExtensions.GetStaticPropertyValue<TestClass, int>("nonexistentProperty");
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
        int expected = TestClass.SomeField;
        int actual = ReflectionExtensions.GetStaticFieldOrPropertyValue<TestClass, int>("SomeField");
        Assert.AreEqual(expected, actual);

        string expected2 = TestClass.SomeProperty;
        string actual2 = ReflectionExtensions.GetStaticFieldOrPropertyValue<TestClass, string>("SomeProperty");
        Assert.AreEqual(expected2, actual2);
    }

    [TestMethod]
    public void GetStaticFieldOrPropertyValueExceptionOnUnknownField()
    {
        try
        {
            ReflectionExtensions.GetStaticFieldOrPropertyValue<TestClass, int>("nonexistentField");
            Assert.Fail("A MissingFieldException should be thrown.");
        }
        catch (MissingMemberException ex)
        {
            Console.WriteLine(ex.Message);
        }

        try
        {
            ReflectionExtensions.GetStaticFieldOrPropertyValue<TestClass, int>("nonexistentProperty");
            Assert.Fail("A MissingMemberException should be thrown.");
        }
        catch (MissingMemberException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
