using Galaxon.Astronomy.Algorithms.Services;
using Galaxon.Astronomy.Algorithms.Utilities;
using Galaxon.Time;
using Microsoft.AspNetCore.Mvc;

namespace Galaxon.Astronomy.AstroAPI.Controllers;

/// <summary>
/// Controller for API endpoints relating to Delta-T.
/// </summary>
[ApiController]
public class DeltaTController(DeltaTService deltaTService) : ControllerBase
{
    /// <summary>
    /// Calculate the approximate value of dalta-T in SI seconds for a given year.
    /// </summary>
    /// <param name="year">
    /// The year. A decimal value is supported (e.g. 2025.0 for the start of the year, 2025.5 for
    /// the middle of the year, etc.).
    /// </param>
    /// <param name="method">The method to use: NASA, Meeus, or Galaxon (default).</param>
    /// <returns>
    /// The approximate value of delta-T in SI seconds at the specified time.
    /// </returns>
    [HttpGet("api/delta-t")]
    public IActionResult GetDeltaTForYear(double year, string method = "Galaxon")
    {
        try
        {
            // Compute the delta-T for the given time specified as a decimal year.
            double deltaT = method.ToLower() switch
            {
                "nasa" => DeltaTUtility.CalcDeltaTNasa(year),
                "meeus" => DeltaTUtility.CalcDeltaTMeeus(year),
                "galaxon" => deltaTService.CalcDeltaTInterpolate(year),
                _ => throw new InvalidOperationException("Invalid method specified.")
            };

            // Construct the result.
            object result = new { year, deltaT = Round(deltaT, 4) };

            // Log it.
            Slog.Information(
                "Delta-T computed for year {Year} using the method from {Method} is {DeltaT} seconds.",
                year, method, deltaT);

            // Return the delta-T value as HTTP response in JSON.
            return Ok(result);
        }
        catch (Exception ex)
        {
            string error = $"Error computing delta-T for year {year}.";
            return Program.ReturnException(this, error, ex);
        }
    }

    /// <summary>
    /// Calculate delta-T for a range of years.
    /// </summary>
    /// <param name="startYear">The start year.</param>
    /// <param name="endYear">The end year.</param>
    /// <param name="step">The step size (day, week, month, quarter, or year).</param>
    /// <param name="method">The method to use: NASA, Meeus, or Galaxon (default).</param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    [HttpGet("api/delta-t-range")]
    public IActionResult GetDeltaTForYearRange(int startYear, int endYear, string step = "year",
        string method = "Galaxon")
    {
        // Get the step size in years.
        double stepSize = step.ToLower() switch
        {
            "day" => 1.0 / TimeConstants.DAYS_PER_YEAR,
            "week" => 1.0 / TimeConstants.WEEKS_PER_YEAR,
            "month" => 1.0 / TimeConstants.MONTHS_PER_YEAR,
            "quarter" => 0.25,
            "year" => 1.0,
            _ => throw new InvalidOperationException("Invalid step size specified.")
        };

        // Get the method to use.
        Func<double, double> func = method.ToLower() switch
        {
            "nasa" => DeltaTUtility.CalcDeltaTNasa,
            "meeus" => DeltaTUtility.CalcDeltaTMeeus,
            "galaxon" => deltaTService.CalcDeltaTInterpolate,
            _ => throw new InvalidOperationException("Invalid method specified.")
        };

        try
        {
            // Get the results.
            List<object> results = new ();
            for (double year = startYear; year <= endYear; year += stepSize)
            {
                results.Add(new
                {
                    year = Round(year, 3),
                    deltaT = Round(func(year), 4)
                });
            }
            return Ok(results);
        }
        catch (Exception ex)
        {
            string error = $"Error computing delta-T for years {startYear}-{endYear}.";
            return Program.ReturnException(this, error, ex);
        }
    }
}
