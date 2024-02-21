namespace Galaxon.Core.Numbers;

/// <summary>
/// Extension methods for Random class.
/// </summary>
public static class RandomExtensions
{
    /// <summary>
    /// Get a random signed short.
    /// </summary>
    public static short GetShort(this Random rnd)
    {
        // Get a random non-negative int and cast to short.
        // The int value will wrap to a short within the valid range for the type, without overflow.
        return (short)rnd.Next();
    }

    /// <summary>
    /// Get a random unsigned short.
    /// </summary>
    public static ushort GetUshort(this Random rnd)
    {
        // Get a random non-negative int and cast to ushort.
        // The value will wrap to a ushort within the valid range for the type, without overflow.
        return (ushort)rnd.Next();
    }

    /// <summary>
    /// Get a random signed int.
    /// The Random.Next() method will only return an int in the range 0..int.MaxValue. This method
    /// will return a signed int with a value anywhere in the valid range for that type.
    /// </summary>
    public static int GetInt(this Random rnd)
    {
        // Get a random value in the range 0..int.MaxValue.
        // It's non-negative, so the most significant bit will always be 0.
        int i = rnd.Next();

        // Get a random sign bit.
        var signBit = (byte)rnd.Next(2);

        return (signBit << 31) | i;
    }

    /// <summary>
    /// Get a random unsigned int.
    /// The Random.Next() method will only return an int in the range 0..int.MaxValue. This method
    /// will return an unsigned int with a value anywhere in the valid range for that type.
    /// </summary>
    public static uint GetUint(this Random rnd)
    {
        return (uint)rnd.GetInt();
    }

    /// <summary>
    /// Get a random signed long.
    /// Will return a value in the valid range for long (long.MinValue..long.MaxValue).
    /// The built in NextInt64 method is restricted to non-negative values:
    /// <see cref="Random.NextInt64()"/>
    /// </summary>
    public static long GetLong(this Random rnd)
    {
        // Get a random value in the range 0..long.MaxValue.
        // It's non-negative, so the most significant bit will always be 0.
        long n = rnd.NextInt64();

        // Get a random sign bit.
        var signBit = (byte)rnd.Next(2);

        // Set the sign bit.
        return ((long)signBit << 63) | n;
    }

    /// <summary>
    /// Get a random unsigned long.
    /// </summary>
    public static ulong GetUlong(this Random rnd)
    {
        return (ulong)rnd.GetLong();
    }

    /// <summary>
    /// Get a random Half.
    /// Will not return anything weird like -0, ±∞, or NaN.
    /// </summary>
    public static Half GetHalf(this Random rnd)
    {
        // Loop until we get a valid result.
        while (true)
        {
            // Get a random short (16 bits).
            short i = rnd.GetShort();

            // Convert to a Half.
            Half h = BitConverter.Int16BitsToHalf(i);

            // Check if it's valid.
            if (Half.IsFinite(h) && h != Half.NegativeZero)
            {
                return h;
            }
        }
    }

    /// <summary>
    /// Get a random float.
    /// Will not return anything weird like -0, ±∞, or NaN.
    /// </summary>
    public static float GetFloat(this Random rnd)
    {
        // Loop until we get a valid result.
        while (true)
        {
            // Get a random int (32 bits).
            int i = rnd.GetInt();

            // Convert to a float.
            float f = BitConverter.Int32BitsToSingle(i);

            // Check if it's valid.
            if (float.IsFinite(f) && f != float.NegativeZero)
            {
                return f;
            }
        }
    }

    /// <summary>
    /// Get a random double.
    /// Will not return anything weird like -0, ±∞, or NaN.
    /// The built-in Random.NextDouble() method will only return values in the range 0.0..1.0.
    /// <see cref="Random.NextDouble"/>
    /// </summary>
    public static double GetDouble(this Random rnd)
    {
        // Loop until we get a valid result.
        while (true)
        {
            // Get a random long (64 bits).
            long l = rnd.GetLong();

            // Convert to a double.
            double d = BitConverter.Int64BitsToDouble(l);

            // Check if it's valid.
            if (double.IsFinite(d) && d != double.NegativeZero)
            {
                return d;
            }
        }
    }

    /// <summary>
    /// Get a random decimal.
    /// </summary>
    public static decimal GetDecimal(this Random rnd)
    {
        int lo = rnd.GetInt();
        int mid = rnd.GetInt();
        int hi = rnd.GetInt();
        bool isNegative = rnd.Next(2) == 1;
        var scale = (byte)rnd.Next(29);
        return new decimal(lo, mid, hi, isNegative, scale);
    }

    /// <summary>
    /// Simulate rolling a dice with a given number of sides.
    /// This is designed for board games.
    /// Using DnD notation, if you want to roll, say, 1d20, call <code>rnd.DieRoll(20)</code>
    /// </summary>
    /// <param name="rnd">The Random object.</param>
    /// <param name="sides">The number of sides on the dice.</param>
    /// <returns></returns>
    public static int DieRoll(this Random rnd, int sides)
    {
        return rnd.Next(1, sides + 1);
    }

    /// <summary>
    /// Simulate rolling multiple dice with the same number of sides.
    /// This is designed for board games.
    /// Using DnD notation, if you want to roll, say, 3d6, call <code>rnd.DiceRoll(3, 6)</code>
    /// </summary>
    /// <param name="rnd">The Random object.</param>
    /// <param name="nDice">The number of dice to roll.</param>
    /// <param name="sides">The number of sides on the dice.</param>
    /// <returns></returns>
    public static int DiceRoll(this Random rnd, int nDice, int sides)
    {
        int result = 0;
        for (int i = 0; i < nDice; i++)
        {
            result += rnd.DieRoll(sides);
        }
        return result;
    }
}
