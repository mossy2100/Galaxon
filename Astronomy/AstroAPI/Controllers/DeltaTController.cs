using Galaxon.Time;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Galaxon.Astronomy.AstroAPI.Controllers;

/// <summary>
/// Controller for API endpoints relating to Delta-T.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class DeltaTController : ControllerBase
{
    [HttpGet("ForYear")]
    public IActionResult GetDeltaTForYear(double year)
    {
        try
        {
            // Computer the delta-T for the given time specified as a decimal year.
            double deltaT = TimeScales.CalcDeltaT(year);

            // Construct the result.
            Dictionary<string, double> result = new ()
            {
                ["seconds"] = double.Round(deltaT, 6)
            };

            // Log it.
            Log.Information("Delta-T computed for year {Year} is {DeltaT} seconds.", year, deltaT);

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
