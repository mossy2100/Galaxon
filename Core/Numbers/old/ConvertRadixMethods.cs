public static class ConvertRadixMethods
{
    #region Methods for working with binary and hexadecimal strings

    /// <summary>
    /// Get the ulong as a string of hexadecimal digits.
    /// </summary>
    public static string ToHexString(this ulong value)
    {
        byte[] bytes = BitConverter.GetBytes(value);
        Array.Reverse(bytes);
        return Convert.ToHexString(bytes);
    }

    /// <summary>
    /// Create a ulong from a string of hexadecimal digits.
    /// </summary>
    public static ulong FromHexString(string strDigits) =>
        Convert.ToUInt64(strDigits, 16);

    /// <summary>
    /// Get the ulong as a string of binary digits.
    /// </summary>
    public static string ToBinString(this ulong value)
    {
        byte[] bytes = BitConverter.GetBytes(value);
        Array.Reverse(bytes);
        string str = bytes
            .Aggregate("", (s, b) => s + Convert.ToString(b, 2).PadLeft(8, '0'));
        return str;
    }

    /// <summary>
    /// Create a ulong from a string of binary digits.
    /// </summary>
    public static ulong FromBinString(string strDigits) =>
        Convert.ToUInt64(strDigits, 2);

    #endregion Methods for working with binary and hexadecimal strings
}
