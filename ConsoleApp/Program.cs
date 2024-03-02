using System.Numerics;

namespace Galaxon.ConsoleApp;

class Program
{
    static void Main()
    {
        Complex z1 = new (0.0, 2);
        Complex z2 = new (-0.0, 2);
        Console.WriteLine($"{z1 == z2}");
    }
}
