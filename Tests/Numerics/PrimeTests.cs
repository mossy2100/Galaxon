using System.Diagnostics;
using Galaxon.Core.Numbers;
using Galaxon.Numerics.Integers;

namespace Galaxon.Tests.Numerics;

[TestClass]
[Ignore]
public class PrimeTests
{
    [TestMethod]
    public void TestIsPrimeSlow()
    {
        long t1 = DateTime.Now.Ticks;

        // Test IsPrimeSlow() is correctly reporting whether or not a value is prime for all values
        // up to Primes.MaxValueChecked.
        for (uint i = 0; i <= Primes.MaxValueChecked; i++)
        {
            bool expected = Primes.Cache.Contains(i);
            bool actual = Primes.IsPrimeSlow(i);
            if (expected != actual)
            {
                Trace.WriteLine($"Error. i is prime? {expected} but the function says {actual}.");
            }
            Assert.AreEqual(expected, actual);
        }

        long t2 = DateTime.Now.Ticks;
        long t = t2 - t1;
        Trace.WriteLine($"Time taken: {t} ticks.");
    }

    [TestMethod]
    public void TestIsPrimeFast()
    {
        long t1 = DateTime.Now.Ticks;

        // Test IsPrimeFast() is correctly reporting whether or not a value is prime for all values
        // up to Primes.MaxValueChecked.
        for (uint i = 0; i <= Primes.MaxValueChecked; i++)
        {
            bool expected = Primes.Cache.Contains(i);
            bool actual = Primes.IsPrime(i);
            if (expected != actual)
            {
                Trace.WriteLine($"Error. i is prime? {expected} but the function says {actual}.");
            }
            Assert.AreEqual(expected, actual);
        }

        long t2 = DateTime.Now.Ticks;
        long t = t2 - t1;
        Trace.WriteLine($"Time taken: {t} ticks.");
    }

    public static void CompareIsPrimeMethods(ulong n)
    {
        // Trace.WriteLine("");

        // Trace.WriteLine($"Testing IsPrimeSlow({n})...");
        Primes.ClearCache();
        // long t3 = DateTime.Now.Ticks / TimeSpan.TicksPerMicrosecond;
        bool isPrimeSlow = Primes.IsPrimeSlow(n);
        // long t4 = DateTime.Now.Ticks / TimeSpan.TicksPerMicrosecond;
        // long tSlow = t4 - t3;
        // Trace.WriteLine($"IsPrimeSlow(): {n} is " + (isPrimeSlow ? "" : "not ") + "prime.");
        // Trace.WriteLine($"IsPrimeSlow() took {tSlow} µs.");

        // Trace.WriteLine($"Testing IsPrime({n})...");
        Primes.ClearCache();
        // long t1 = DateTime.Now.Ticks / TimeSpan.TicksPerMicrosecond;
        bool isPrimeFast = Primes.IsPrime(n);
        // long t2 = DateTime.Now.Ticks / TimeSpan.TicksPerMicrosecond;
        // long tFast = t2 - t1;
        // Trace.WriteLine($"IsPrime(): {n} is " + (isPrimeFast ? "" : "not ") + "prime.");
        // Trace.WriteLine($"IsPrime() took {tFast} µs.");

        Assert.AreEqual(isPrimeSlow, isPrimeFast);
    }

    [TestMethod]
    public void CompareIsPrimeMethods1MillionValues()
    {
        for (ulong i = 0; i < 1_000_000; i++)
        {
            CompareIsPrimeMethods(i);
        }
    }

    [TestMethod]
    public void CompareIsPrimeMethodsMaxShort()
    {
        var n = (ulong)short.MaxValue;
        if (!Primes.IsPrime(n))
        {
            n = Primes.GetPrevious(n);
        }
        Trace.WriteLine($"The largest short prime is {n}");
        CompareIsPrimeMethods(n);
    }

    [TestMethod]
    public void CompareIsPrimeMethodsMaxUShort()
    {
        ulong n = ushort.MaxValue;
        if (!Primes.IsPrime(n))
        {
            n = Primes.GetPrevious(n);
        }
        Trace.WriteLine($"The largest ushort prime is {n}");
        CompareIsPrimeMethods(n);
    }

    [TestMethod]
    public void CompareIsPrimeMethodsMaxInt()
    {
        ulong n = int.MaxValue;
        if (!Primes.IsPrime(n))
        {
            n = Primes.GetPrevious(n);
        }
        Trace.WriteLine($"The largest int prime is {n}");
        CompareIsPrimeMethods(n);
    }

    [TestMethod]
    public void CompareIsPrimeMethodsMaxUInt()
    {
        ulong n = uint.MaxValue;
        if (!Primes.IsPrime(n))
        {
            n = Primes.GetPrevious(n);
        }
        Trace.WriteLine($"The largest uint prime is {n}");
        CompareIsPrimeMethods(n);
    }

    [TestMethod]
    public void CompareIsPrimeMethodsMaxLong()
    {
        ulong n = long.MaxValue;
        if (!Primes.IsPrime(n))
        {
            n = Primes.GetPrevious(n);
        }
        Trace.WriteLine($"The largest long prime is {n}");
        // IsPrimeSlow() takes too long.
        // CompareIsPrimeMethods(n);
    }

    [TestMethod]
    public void CompareIsPrimeMethodsMaxULong()
    {
        ulong n = ulong.MaxValue;
        if (!Primes.IsPrime(n))
        {
            n = Primes.GetPrevious(n);
        }
        Trace.WriteLine($"The largest ulong prime is {n}");
        // IsPrimeSlow() takes too long.
        // CompareIsPrimeMethods(n);
    }

    /// <summary>
    ///     <see href="https://projecteuler.net/problem=10"/>
    /// </summary>
    [TestMethod]
    public void PrimesSumTest()
    {
        IEnumerable<ulong> primes = Primes.GetPrimesUpTo(2_000_000);
        ulong total = primes.Sum();
        Assert.AreEqual(142913828922ul, total);
    }

    /// <summary>
    /// Takes about 3 minutes.
    /// </summary>
    [TestMethod]
    public void TestEratosthenesWithMaxValue()
    {
        Primes.Eratosthenes(uint.MaxValue);
        Assert.AreEqual(203_280_221, Primes.Cache.Count);
        Assert.AreEqual(4_294_967_291, Primes.Cache.Max());
    }

    [TestMethod]
    public void TestTotientTens()
    {
        Assert.AreEqual(4ul, Primes.Totient(10));
        Assert.AreEqual(8ul, Primes.Totient(20));
        Assert.AreEqual(8ul, Primes.Totient(30));
        Assert.AreEqual(16ul, Primes.Totient(40));
        Assert.AreEqual(20ul, Primes.Totient(50));
        Assert.AreEqual(16ul, Primes.Totient(60));
        Assert.AreEqual(24ul, Primes.Totient(70));
        Assert.AreEqual(32ul, Primes.Totient(80));
        Assert.AreEqual(24ul, Primes.Totient(90));
        Assert.AreEqual(40ul, Primes.Totient(100));
    }

    [TestMethod]
    public void TestTotientMillions()
    {
        Assert.AreEqual(400_000ul, Primes.Totient(1_000_000));
        Assert.AreEqual(800_000ul, Primes.Totient(2_000_000));
        Assert.AreEqual(800_000ul, Primes.Totient(3_000_000));
        Assert.AreEqual(1_600_000ul, Primes.Totient(4_000_000));
        Assert.AreEqual(2_000_000ul, Primes.Totient(5_000_000));
        Assert.AreEqual(1_600_000ul, Primes.Totient(6_000_000));
        Assert.AreEqual(2_400_000ul, Primes.Totient(7_000_000));
        Assert.AreEqual(3_200_000ul, Primes.Totient(8_000_000));
        Assert.AreEqual(2_400_000ul, Primes.Totient(9_000_000));
        Assert.AreEqual(4_000_000ul, Primes.Totient(10_000_000));
    }

    [TestMethod]
    public void TestPrimeFactors()
    {
        CollectionAssert.AreEqual(new List<ulong>(), Primes.PrimeFactors(1));
        CollectionAssert.AreEqual(new List<ulong> { 2 }, Primes.PrimeFactors(2));
        CollectionAssert.AreEqual(new List<ulong> { 3 }, Primes.PrimeFactors(3));
        CollectionAssert.AreEqual(new List<ulong> { 2, 2 }, Primes.PrimeFactors(4));
        CollectionAssert.AreEqual(new List<ulong> { 5 }, Primes.PrimeFactors(5));
        CollectionAssert.AreEqual(new List<ulong> { 2, 3 }, Primes.PrimeFactors(6));
        CollectionAssert.AreEqual(new List<ulong> { 7 }, Primes.PrimeFactors(7));
        CollectionAssert.AreEqual(new List<ulong> { 2, 2, 2 }, Primes.PrimeFactors(8));
        CollectionAssert.AreEqual(new List<ulong> { 3, 3 }, Primes.PrimeFactors(9));
        CollectionAssert.AreEqual(new List<ulong> { 2, 5 }, Primes.PrimeFactors(10));
        CollectionAssert.AreEqual(new List<ulong> { 11 }, Primes.PrimeFactors(11));
        CollectionAssert.AreEqual(new List<ulong> { 2, 2, 3 }, Primes.PrimeFactors(12));
        CollectionAssert.AreEqual(new List<ulong> { 13 }, Primes.PrimeFactors(13));
        CollectionAssert.AreEqual(new List<ulong> { 2, 7 }, Primes.PrimeFactors(14));
        CollectionAssert.AreEqual(new List<ulong> { 3, 5 }, Primes.PrimeFactors(15));
        CollectionAssert.AreEqual(new List<ulong> { 2, 2, 2, 2 }, Primes.PrimeFactors(16));
        CollectionAssert.AreEqual(new List<ulong> { 17 }, Primes.PrimeFactors(17));
        CollectionAssert.AreEqual(new List<ulong> { 2, 3, 3 }, Primes.PrimeFactors(18));
        CollectionAssert.AreEqual(new List<ulong> { 19 }, Primes.PrimeFactors(19));
        CollectionAssert.AreEqual(new List<ulong> { 2, 2, 5 }, Primes.PrimeFactors(20));
    }
}
