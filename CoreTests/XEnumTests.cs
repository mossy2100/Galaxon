using Galaxon.Core.Types;

namespace Galaxon.Core.Tests;

[TestClass]
public class XEnumTests
{
    [TestMethod]
    public void TestToString()
    {
        Assert.AreEqual("Cat", Animal.Cat.ToString());
        Assert.AreEqual("Dog", Animal.Dog.ToString());
    }

    [TestMethod]
    public void TestNoDescriptionAttribute()
    {
        Assert.AreEqual("Cat", Animal.Cat.GetDescription());
    }

    [TestMethod]
    public void TestDescriptionAttribute()
    {
        Assert.AreEqual("canine", Animal.Dog.GetDescription());
    }

    private enum Animal
    {
        Cat,

        [System.ComponentModel.Description("canine")]
        Dog
    }
}
