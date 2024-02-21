// for (int i = 0; i < 128; i++)
// {
//     char c = (char)i;
//     if (!char.IsControl(c) && !char.IsLetterOrDigit(c) && !char.IsWhiteSpace(c))
//         Console.Write(c);
// }

using System.Globalization;
using System.Numerics;

Complex z = new (5.1, 6.34);

// Get all available cultures
CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);

// Output the list of cultures
foreach (CultureInfo culture in cultures)
{
    // Console.WriteLine($"Culture Name: {culture.Name}, DisplayName: {culture.DisplayName}");
    Console.WriteLine(z.ToString(culture));
}
