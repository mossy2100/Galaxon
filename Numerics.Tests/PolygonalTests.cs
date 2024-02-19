using Galaxon.Numerics.Integers;

namespace Galaxon.Numerics.Tests;

[TestClass]
public class PolygonalTests
{
    [TestMethod]
    public void TestLargeNumber()
    {
        ulong n = 40755;
        Assert.IsTrue(Polygonal.IsTriangular(n));
        Assert.IsTrue(Polygonal.IsPentagonal(n));
        Assert.IsTrue(Polygonal.IsHexagonal(n));

        n = 1533776805;
        Assert.IsTrue(Polygonal.IsTriangular(n));
        Assert.IsTrue(Polygonal.IsPentagonal(n));
        Assert.IsTrue(Polygonal.IsHexagonal(n));
    }

    #region Triangular numbers

    [TestMethod]
    public void TestGetTriangular1()
    {
        Assert.AreEqual(1u, Polygonal.GetTriangular(1));
    }

    [TestMethod]
    public void TestGetTriangular5()
    {
        Assert.AreEqual(15u, Polygonal.GetTriangular(5));
    }

    [TestMethod]
    public void TestGetAllTriangularUpTo()
    {
        ulong[] nums = Polygonal.GetAllTriangularUpTo(25);
        Assert.AreEqual(1u, nums[1]);
        Assert.AreEqual(3u, nums[2]);
        Assert.AreEqual(6u, nums[3]);
        Assert.AreEqual(10u, nums[4]);
        Assert.AreEqual(15u, nums[5]);
        Assert.AreEqual(21u, nums[6]);
    }

    [TestMethod]
    public void TestIsTriangular()
    {
        Assert.IsTrue(Polygonal.IsTriangular(1));
        Assert.IsTrue(Polygonal.IsTriangular(3));
        Assert.IsTrue(Polygonal.IsTriangular(6));
        Assert.IsTrue(Polygonal.IsTriangular(10));
        Assert.IsTrue(Polygonal.IsTriangular(15));
        Assert.IsTrue(Polygonal.IsTriangular(21));

        Assert.IsFalse(Polygonal.IsTriangular(2));
        Assert.IsFalse(Polygonal.IsTriangular(4));
        Assert.IsFalse(Polygonal.IsTriangular(9));
        Assert.IsFalse(Polygonal.IsTriangular(12));
        Assert.IsFalse(Polygonal.IsTriangular(20));
        Assert.IsFalse(Polygonal.IsTriangular(25));
    }

    #endregion Triangular numbers

    #region Pentagonal numbers

    [TestMethod]
    public void TestGetPentagonal1()
    {
        Assert.AreEqual(1u, Polygonal.GetPentagonal(1));
    }

    [TestMethod]
    public void TestGetPentagonal5()
    {
        Assert.AreEqual(35u, Polygonal.GetPentagonal(5));
    }

    [TestMethod]
    public void TestGetAllPentagonalUpTo()
    {
        ulong[] nums = Polygonal.GetAllPentagonalUpTo(60);
        Assert.AreEqual(1u, nums[1]);
        Assert.AreEqual(5u, nums[2]);
        Assert.AreEqual(12u, nums[3]);
        Assert.AreEqual(22u, nums[4]);
        Assert.AreEqual(35u, nums[5]);
        Assert.AreEqual(51u, nums[6]);
    }

    [TestMethod]
    public void TestIsPentagonal()
    {
        Assert.IsTrue(Polygonal.IsPentagonal(1));
        Assert.IsTrue(Polygonal.IsPentagonal(5));
        Assert.IsTrue(Polygonal.IsPentagonal(12));
        Assert.IsTrue(Polygonal.IsPentagonal(22));
        Assert.IsTrue(Polygonal.IsPentagonal(35));
        Assert.IsTrue(Polygonal.IsPentagonal(51));

        Assert.IsFalse(Polygonal.IsPentagonal(2));
        Assert.IsFalse(Polygonal.IsPentagonal(7));
        Assert.IsFalse(Polygonal.IsPentagonal(15));
        Assert.IsFalse(Polygonal.IsPentagonal(27));
        Assert.IsFalse(Polygonal.IsPentagonal(38));
        Assert.IsFalse(Polygonal.IsPentagonal(55));
    }

    #endregion Pentagonal numbers

    #region Hexagonal numbers

    [TestMethod]
    public void TestGetHexagonal1()
    {
        Assert.AreEqual(1u, Polygonal.GetHexagonal(1));
    }

    [TestMethod]
    public void TestGetHexagonal5()
    {
        Assert.AreEqual(45u, Polygonal.GetHexagonal(5));
    }

    [TestMethod]
    public void TestGetAllHexagonalUpTo()
    {
        ulong[] nums = Polygonal.GetAllHexagonalUpTo(100);
        Assert.AreEqual(1u, nums[1]);
        Assert.AreEqual(6u, nums[2]);
        Assert.AreEqual(15u, nums[3]);
        Assert.AreEqual(28u, nums[4]);
        Assert.AreEqual(45u, nums[5]);
        Assert.AreEqual(66u, nums[6]);
        Assert.AreEqual(91u, nums[7]);
    }

    [TestMethod]
    public void TestIsHexagonal()
    {
        Assert.IsTrue(Polygonal.IsHexagonal(1));
        Assert.IsTrue(Polygonal.IsHexagonal(6));
        Assert.IsTrue(Polygonal.IsHexagonal(15));
        Assert.IsTrue(Polygonal.IsHexagonal(28));
        Assert.IsTrue(Polygonal.IsHexagonal(45));
        Assert.IsTrue(Polygonal.IsHexagonal(66));

        Assert.IsFalse(Polygonal.IsHexagonal(2));
        Assert.IsFalse(Polygonal.IsHexagonal(7));
        Assert.IsFalse(Polygonal.IsHexagonal(18));
        Assert.IsFalse(Polygonal.IsHexagonal(33));
        Assert.IsFalse(Polygonal.IsHexagonal(56));
        Assert.IsFalse(Polygonal.IsHexagonal(71));
    }

    #endregion Hexagonal numbers
}
