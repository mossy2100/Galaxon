using Galaxon.Astronomy.Data;

namespace Galaxon.Astronomy.DataImport.Services;

public class DeltaTImportService(AstroDbContext astroDbContext)
{
    public static async Task ImportUsnoDeltaTMonthly()
    {
        // Download data file from https://maia.usno.navy.mil/ser7/deltat.data
        // Go through each line.
        // Split each line on whitespace to get 4 numbers.
        // The first 3 numbers represent a Gregorian date triplet (year, month, day).
        // Convert this date to a decimal year using TimeScalesUtility.DateTimeToDecimalYear.
        // The 4th number is the value of Delta T.
        // Create a new DeltaTRecord with the decimal year and Delta T value.
        // Add this record to the database.
    }
}
