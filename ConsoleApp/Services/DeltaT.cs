using Galaxon.Astronomy.Algorithms.Utilities;

namespace Galaxon.ConsoleApp.Services;

public static class DeltaT
{
    /// <summary>
    /// The goal here is to generate a chart to show the difference between the 2 methods.
    /// </summary>
    public static void CompareDeltaTCalcs()
    {
        // Open a file.
        using StreamWriter writer = new ("CalcDeltaTComparison.txt");
        writer.WriteLine(
            $"{"Year",-10}{"Delta-T NASA",-25}{"Delta-T Meeus",-25}{"Difference",-25}");

        // Range of years is from https://eclipse.gsfc.nasa.gov/SEcat5/deltatpoly.html
        for (int y = -1999; y <= 3000; y++)
        {
            double deltaTNasa = DeltaTUtility.CalcDeltaTNasa(y);
            double deltaTMeeus = DeltaTUtility.CalcDeltaTMeeus(y);
            double diff = Abs(deltaTMeeus - deltaTNasa);

            string grade = diff switch
            {
                < 1 => "VERY GOOD",
                < 10 => "GOOD",
                < 100 => "MEH",
                _ => "BAD"
            };

            writer.WriteLine($"{y,-10}{deltaTNasa,-25}{deltaTMeeus,-25}{diff,-25}{grade,-25}");
        }
    }

    /// <summary>
    /// Generate a CSV file for importing into Excel.
    /// </summary>
    public static void GenerateDeltaTCsvFile()
    {
        // Open a file.
        using StreamWriter writer = new ("DeltaT.csv");

        // Range of years is from https://eclipse.gsfc.nasa.gov/SEcat5/deltatpoly.html
        for (int y = -1999; y <= 3000; y++)
        {
            double deltaTNasa = DeltaTUtility.CalcDeltaTNasa(y);
            double deltaTMeeus = DeltaTUtility.CalcDeltaTMeeus(y);
            double diff = Abs(deltaTMeeus - deltaTNasa);
            writer.WriteLine($"{y},{deltaTNasa},{deltaTMeeus},{diff}");
        }
    }
}
