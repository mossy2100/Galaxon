using System.Text;
using Galaxon.Numerics.Types;

namespace Galaxon.Numerics.Tests;

[TestClass]
public class TestRadixNum
{
    public TestRadixNum(TestContext testContext) =>
        TestContext = testContext;

    private TestContext TestContext { get; }

    [TestMethod]
    public void TestValueField()
    {
        for (sbyte radix = RadixNumber.MinRadix; radix <= RadixNumber.MaxRadix; radix++)
        {
            for (ulong i = 0; i < 100; i++)
            {
                // Test basic constructor.
                RadixNumber q = new (0, radix) { Value = i };

                // Get value.
                ulong j = q.Value;

                // Check we get the same value.
                Assert.AreEqual(i, j);
            }
        }
    }

    [TestMethod]
    public void TestConversions()
    {
        for (sbyte radix = RadixNumber.MinRadix; radix <= RadixNumber.MaxRadix; radix++)
        {
            for (ulong i = 0; i < 100; i++)
            {
                // Construct from ulong.
                RadixNumber q = new (i, radix);

                // Convert to string.
                string s = q.ToString();

                // Construct second RadixNum from string.
                RadixNumber q2 = new (s, radix);

                // Convert to long.
                ulong i2 = q2;

                // Compare.
                Assert.AreEqual(i, i2);
            }
        }
    }

    [TestMethod]
    public void TestNot()
    {
        for (sbyte radix = RadixNumber.MinRadix; radix <= RadixNumber.MaxRadix; radix++)
        {
            for (ulong i = 0; i < 100; i++)
            {
                // Test basic constructor from int.
                RadixNumber q = new (i, radix);

                // Test NOT operator.
                RadixNumber q2 = ~q;
                q2 = ~q2;

                // Compare.
                Assert.AreEqual(q, q2);
            }
        }
    }

    [TestMethod]
    public void TestConversionToCommonBases()
    {
        ulong[] values =
        {
            0,
            long.MaxValue,
            (ulong)long.MaxValue + 1,
            ulong.MaxValue
        };

        foreach (ulong value in values)
        {
            RadixNumber q = new (value);

            string bin = q.ToBinString();
            TestContext.WriteLine(bin);
            RadixNumber qBin = RadixNumber.FromBinString(bin);
            Assert.AreEqual(q, qBin);

            string quat = q.ToQuatString();
            TestContext.WriteLine(quat);
            RadixNumber qQuat = RadixNumber.FromQuatString(quat);
            Assert.AreEqual(q, qQuat);

            string oct = q.ToOctString();
            TestContext.WriteLine(oct);
            RadixNumber qOct = RadixNumber.FromOctString(oct);
            Assert.AreEqual(q, qOct);

            string dec = q.ToDecString();
            TestContext.WriteLine(dec);
            RadixNumber qDec = RadixNumber.FromDecString(dec);
            Assert.AreEqual(q, qDec);

            string duo = q.ToDuoString();
            TestContext.WriteLine(duo);
            RadixNumber qDuo = RadixNumber.FromDuoString(duo);
            Assert.AreEqual(q, qDuo);

            string hex = q.ToHexString();
            TestContext.WriteLine(hex);
            RadixNumber qHex = RadixNumber.FromHexString(hex);
            Assert.AreEqual(q, qHex);

            string vig = q.ToVigString();
            TestContext.WriteLine(vig);
            RadixNumber qVig = RadixNumber.FromVigString(vig);
            Assert.AreEqual(q, qVig);

            string tria = q.ToTriaString();
            TestContext.WriteLine(tria);
            RadixNumber qTria = RadixNumber.FromTriaString(tria);
            Assert.AreEqual(q, qTria);
        }
    }

    [TestMethod]
    public void TestConvertDecToOther()
    {
        const int numTests = 10;
        for (int i = 0; i < numTests; i++)
        {
            // Generate a random number.
            ulong x = (ulong)new Random().NextInt64(0, long.MaxValue);

            // Convert to an alternate radix.
            sbyte radix =
                (sbyte)new Random().Next(RadixNumber.MinRadix, RadixNumber.MaxRadix + 1);
            string s = RadixNumber.ValueToDigits(x, radix);

            // Convert back to a number.
            ulong y = RadixNumber.DigitsToValue(s, radix);

            // Result.
            Assert.AreEqual(x, y);
        }
    }

    [TestMethod]
    public void TestConvertOtherToDec()
    {
        const int numTests = 10;
        for (int i = 0; i < numTests; i++)
        {
            // Make a number in an alt radix.
            sbyte radix =
                (sbyte)new Random().Next(RadixNumber.MinRadix, RadixNumber.MaxRadix + 1);
            StringBuilder sb = new ();
            for (int j = 0; j <= 10; j++)
            {
                sb.Append(RadixNumber.Digits[new Random().Next(0, radix)]);
            }
            string s = sb.ToString().TrimStart('0');
            if (s == "")
            {
                s = "0";
            }
            TestContext.WriteLine($"{s}_{radix}");

            // Convert to ulong.
            ulong y = RadixNumber.DigitsToValue(s, radix);

            // Convert back to string.
            string s2 = RadixNumber.ValueToDigits(y, radix);

            // Result of test.
            Assert.AreEqual(s, s2);
        }
    }
}
