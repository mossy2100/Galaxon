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
            double deltaT = TimeScales.CalcDeltaT(year);

            Log.Information("Delta-T computed for year {Year} is {DeltaT} seconds.", year, deltaT);

            // Return the delta-T value as HTTP response in JSON.
            return Ok($"{deltaT:F2} seconds");
        }
        catch (Exception ex)
        {
            string error = $"Error computing delta-T for year {year}.";
            return Program.ReturnException(this, error, ex);
        }
    }
}
