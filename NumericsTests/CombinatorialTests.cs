using Galaxon.Numerics.Integers;

namespace Galaxon.Numerics.Tests;

[TestClass]
public class CombinatorialTests
{
    /// <summary>
    /// Test small, valid values.
    /// </summary>
    [TestMethod]
    public void BinomialCoefficient_NonNegativeParameters_ReturnsCorrectResults()
    {
        Assert.AreEqual(1, Combinatorial.BinomialCoefficient(0, 0));

        Assert.AreEqual(1, Combinatorial.BinomialCoefficient(1, 0));
        Assert.AreEqual(1, Combinatorial.BinomialCoefficient(1, 1));

        Assert.AreEqual(1, Combinatorial.BinomialCoefficient(2, 0));
        Assert.AreEqual(2, Combinatorial.BinomialCoefficient(2, 1));
        Assert.AreEqual(1, Combinatorial.BinomialCoefficient(2, 2));

        Assert.AreEqual(1, Combinatorial.BinomialCoefficient(3, 0));
        Assert.AreEqual(3, Combinatorial.BinomialCoefficient(3, 1));
        Assert.AreEqual(3, Combinatorial.BinomialCoefficient(3, 2));
        Assert.AreEqual(1, Combinatorial.BinomialCoefficient(3, 3));

        Assert.AreEqual(1, Combinatorial.BinomialCoefficient(4, 0));
        Assert.AreEqual(4, Combinatorial.BinomialCoefficient(4, 1));
        Assert.AreEqual(6, Combinatorial.BinomialCoefficient(4, 2));
        Assert.AreEqual(4, Combinatorial.BinomialCoefficient(4, 3));
        Assert.AreEqual(1, Combinatorial.BinomialCoefficient(4, 4));

        Assert.AreEqual(1, Combinatorial.BinomialCoefficient(10, 0));
        Assert.AreEqual(10, Combinatorial.BinomialCoefficient(10, 1));
        Assert.AreEqual(45, Combinatorial.BinomialCoefficient(10, 2));
        Assert.AreEqual(120, Combinatorial.BinomialCoefficient(10, 3));
        Assert.AreEqual(210, Combinatorial.BinomialCoefficient(10, 4));
        Assert.AreEqual(252, Combinatorial.BinomialCoefficient(10, 5));
        Assert.AreEqual(210, Combinatorial.BinomialCoefficient(10, 6));
        Assert.AreEqual(120, Combinatorial.BinomialCoefficient(10, 7));
        Assert.AreEqual(45, Combinatorial.BinomialCoefficient(10, 8));
        Assert.AreEqual(10, Combinatorial.BinomialCoefficient(10, 9));
        Assert.AreEqual(1, Combinatorial.BinomialCoefficient(10, 10));
    }

    [TestMethod]
    public void TestGetPermutationsGuards()
    {
        List<int> bag = new () { 2, 3, 5, 7 };
        List<List<int>> perms;

        perms = Combinatorial.GetPermutations(bag, 0);
        Assert.AreEqual(1, perms.Count);
        Assert.AreEqual(0, perms[0].Count);

        perms = Combinatorial.GetPermutations(bag, 1);
        Assert.AreEqual(bag.Count, perms.Count);
        Assert.AreEqual(1, perms[0].Count);
        Assert.AreEqual(2, perms[0][0]);
        Assert.AreEqual(1, perms[1].Count);
        Assert.AreEqual(3, perms[1][0]);
        Assert.AreEqual(1, perms[2].Count);
        Assert.AreEqual(5, perms[2][0]);

        perms = Combinatorial.GetPermutations(bag, 5);
        Assert.AreEqual(0, perms.Count);
    }

    [TestMethod]
    public void TestGetPermutationsSimple()
    {
        List<int> bag = new () { 2, 3, 5, 7 };
        List<List<int>> perms = Combinatorial.GetPermutations(bag, 2);
        Assert.AreEqual(12, perms.Count);

        int[] permsAsStrings = perms.Select(list => list[0] * 10 + list[1]).ToArray();
        Assert.AreEqual(23, permsAsStrings[0]);
        Assert.AreEqual(25, permsAsStrings[1]);
        Assert.AreEqual(27, permsAsStrings[2]);
        Assert.AreEqual(32, permsAsStrings[3]);
        Assert.AreEqual(35, permsAsStrings[4]);
        Assert.AreEqual(37, permsAsStrings[5]);
        Assert.AreEqual(52, permsAsStrings[6]);
        Assert.AreEqual(53, permsAsStrings[7]);
        Assert.AreEqual(57, permsAsStrings[8]);
        Assert.AreEqual(72, permsAsStrings[9]);
        Assert.AreEqual(73, permsAsStrings[10]);
        Assert.AreEqual(75, permsAsStrings[11]);
    }
}
