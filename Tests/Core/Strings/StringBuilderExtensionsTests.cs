using System.Text;
using Galaxon.Core.Strings;

namespace Galaxon.Tests.Core.Strings;

[TestClass]
public class StringBuilderExtensionsTests
{
    [TestMethod]
    public void Prepend_NullValue_ReturnsOriginalStringBuilder()
    {
        // Arrange
        StringBuilder sb = new StringBuilder("existing");

        // Act
        sb.Prepend(null);

        // Assert
        Assert.AreEqual("existing", sb.ToString());
    }

    [TestMethod]
    public void Prepend_StringValue_PrependsValue()
    {
        // Arrange
        StringBuilder sb = new StringBuilder("world");

        // Act
        sb.Prepend("Hello, ");

        // Assert
        Assert.AreEqual("Hello, world", sb.ToString());
    }

    [TestMethod]
    public void Prepend_IntegerValue_PrependsValue()
    {
        // Arrange
        StringBuilder sb =
            new StringBuilder(
                " shall be the number of the counting and the number of the counting shall be ");
        int theNumberOfTheCounting = 3;

        // Act
        sb.Prepend(theNumberOfTheCounting);
        sb.Append(theNumberOfTheCounting);

        // Assert
        Assert.AreEqual(
            "3 shall be the number of the counting and the number of the counting shall be 3",
            sb.ToString());
    }

    [TestMethod]
    public void Prepend_BooleanValue_PrependsValue()
    {
        // Arrange
        StringBuilder sb = new StringBuilder(
            " happiness arises, in the first place, from the enjoyment of one's self, and in the next, from the friendship and conversation of a few select companions.");

        // Act
        sb.Prepend(true);

        // Assert
        Assert.AreEqual(
            "True happiness arises, in the first place, from the enjoyment of one's self, and in the next, from the friendship and conversation of a few select companions.",
            sb.ToString());
    }
}
