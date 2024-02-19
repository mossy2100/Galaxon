using Galaxon.Core.Collections;

namespace Galaxon.Tests.Collections;

[TestClass]
public class EnumerableExtensionsTests
{
    public class TestItem
    {
        public int Id { get; set; }

        public string? Name { get; set; }
    }

    public class TestItemWithoutId
    {
        public string? Name { get; set; }
    }

    [TestMethod]
    public void Diff_RemovesValuesFromList2FromList1()
    {
        // Arrange
        List<int> list1 = new () { 1, 2, 3, 4, 5 };
        List<int> list2 = new () { 2, 4 };

        // Act
        var result = list1.Diff(list2).ToList();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(3, result.Count);
        Assert.AreEqual(1, result[0]);
        Assert.AreEqual(3, result[1]);
        Assert.AreEqual(5, result[2]);
    }

    [TestMethod]
    public void Diff_SupportsDuplicates()
    {
        // Arrange
        List<string> list1 = new () { "cat", "dog", "cat", "bird" };
        List<string> list2 = new () { "cat", "bird" };

        // Act
        var result = list1.Diff(list2).ToList();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Count);
        Assert.AreEqual("dog", result[0]);
        Assert.AreEqual("cat", result[1]);
    }

    [TestMethod]
    public void ToIndex_WithValidItems_ReturnsDictionary()
    {
        // Arrange
        var items = new List<TestItem>
        {
            new TestItem { Id = 5, Name = "Item 1" },
            new TestItem { Id = 42, Name = "Item 2" },
            new TestItem { Id = 777, Name = "Item 3" }
        };

        // Act
        var result = items.ToIndex();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(3, result.Count);
        Assert.IsTrue(result.ContainsKey(5));
        Assert.IsTrue(result.ContainsKey(42));
        Assert.IsTrue(result.ContainsKey(777));
        Assert.AreEqual("Item 1", result[5].Name);
        Assert.AreEqual("Item 2", result[42].Name);
        Assert.AreEqual("Item 3", result[777].Name);
    }

    [TestMethod]
    public void ToIndex_WithEmptyItems_ReturnsEmptyDictionary()
    {
        // Arrange
        var items = new List<TestItem>();

        // Act
        var result = items.ToIndex();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ToIndex_WithMissingIdProperty_ThrowsException()
    {
        // Arrange
        var items = new List<TestItemWithoutId>
        {
            new TestItemWithoutId { Name = "Item 1" }
        };

        // Act
        _ = items.ToIndex();
    }
}
