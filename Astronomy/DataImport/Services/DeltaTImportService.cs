using Galaxon.Astronomy.Data;

namespace Galaxon.Astronomy.DataImport.Services;

public class DeltaTImportService(AstroDbContext astroDbContext)
{
    /// <summary>
    /// Import all delta-T data from the internet.
    /// </summary>
    public async Task Import() { }

    /// <summary>
    /// Import monthly delta-T data from
    /// <see href="https://maia.usno.navy.mil/ser7/deltat.data"/>
    /// </summary>
    public async Task ImportFromUsnoDeltaTMonthly()
    {
        // Download data file from USNO.
        // Go through each line.
        // Split each line on whitespace to get 4 numbers.
        // The first 3 numbers represent a Gregorian date triplet (year, month, day).
        // Convert this date to a decimal year using TimeScalesUtility.DateTimeToDecimalYear.
        // The 4th number is the value of Delta T.
        // Create a new DeltaTRecord with the decimal year and Delta T value.
        // Add this record to the database.
    }
}
