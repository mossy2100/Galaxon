using Galaxon.Astronomy.Algorithms.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace Galaxon.Astronomy.AstroAPI.Controllers;

/// <summary>
/// Controller for API endpoints relating to Delta-T.
/// </summary>
[ApiController]
public class DeltaTController : ControllerBase
{
    /// <summary>
    /// Calculate the approximate value of dalta-T in SI seconds for a given year.
    /// </summary>
    /// <param name="year">
    /// The year. A decimal value is supported (e.g. 2025.0 for the start of the year, 2025.5 for
    /// the middle of the year, etc.).
    /// </param>
    /// <returns>
    /// The approximate value of delta-T in SI seconds at the specified time.
    /// </returns>
    [HttpGet("api/delta-t")]
    public IActionResult GetDeltaTForYear(double year)
    {
        try
        {
            // Computer the delta-T for the given time specified as a decimal year.
            double deltaT = DeltaTUtility.CalcDeltaT(year);

            // Construct the result.
            Dictionary<string, double> result = new ()
            {
                { "seconds", Round(deltaT, 6) }
            };

            // Log it.
            Slog.Information("Delta-T computed for year {Year} is {DeltaT} seconds.", year, deltaT);

            // Return the delta-T value as HTTP response in JSON.
            return Ok(result);
        }
        catch (Exception ex)
        {
            string error = $"Error computing delta-T for year {year}.";
            return Program.ReturnException(this, error, ex);
        }
    }
}
