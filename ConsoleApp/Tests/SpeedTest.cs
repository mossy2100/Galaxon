using System.Diagnostics;

namespace Galaxon.ConsoleApp.Tests;

public delegate double ToJulianDate(DateTime dt);

public class SpeedTest
{
    protected static List<double> DateTimeToJulianDateMethod(string methodName,
        ToJulianDate tjd, List<DateTime> dateTimes)
    {
        Stopwatch sw = new ();

        sw.Start();
        var results = dateTimes.Select(tjd.Invoke).ToList();
        sw.Stop();

        Console.WriteLine(
            $"Method {methodName} took {sw.Elapsed.TotalMilliseconds} ms to complete the test.");

        return results;
    }

    private static bool ApproxEqual(double d1, double d2)
    {
        return Math.Abs(d1 - d2) < 1e-9;
    }
}
