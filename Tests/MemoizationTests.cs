using Galaxon.Core.Functional;

namespace Galaxon.Core.Tests;

[TestClass]
public class MemoizationTests
{
    [TestMethod]
    public void Memoize_Unary_FunctionIsMemoized()
    {
        // Arrange
        Memoization.DebugMode = true;
        Func<int, int> addOne = x => x + 1;
        var memoizedAddOne = Memoization.Memoize(addOne);

        // Act
        var result1 = memoizedAddOne(5);
        // Should be retrieved from cache
        var result2 = memoizedAddOne(5);

        // Assert
        Assert.AreEqual(6, result1);
        Assert.AreEqual(6, result2);
    }

    [TestMethod]
    public void Memoize_Binary_FunctionIsMemoized()
    {
        // Arrange
        Memoization.DebugMode = true;
        Func<int, int, int> add = (x, y) => x + y;
        var memoizedAdd = Memoization.Memoize(add);

        // Act
        var result1 = memoizedAdd(3, 4);
        // Should be retrieved from cache
        var result2 = memoizedAdd(3, 4);

        // Assert
        Assert.AreEqual(7, result1);
        Assert.AreEqual(7, result2);
    }

    [TestMethod]
    public void Memoize_ExceptionThrownInFunction_ExceptionIsPropagated()
    {
        // Arrange
        Func<int, int> divideByZero = x => 10 / x; // This will throw DivideByZeroException
        var memoizedDivideByZero = Memoization.Memoize(divideByZero);

        // Act & Assert
        Assert.ThrowsException<DivideByZeroException>(() => memoizedDivideByZero(0));
    }
}
