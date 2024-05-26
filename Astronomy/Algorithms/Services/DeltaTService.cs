using Galaxon.Astronomy.Data;
using Galaxon.Astronomy.Data.Models;
using MathNet.Numerics.Interpolation;

namespace Galaxon.Astronomy.Algorithms.Services;

public class DeltaTService(AstroDbContext astroDbContext)
{
    /// <summary>
    /// Cached delta-T records.
    /// </summary>
    private List<DeltaTRecord>? _deltaTRecords;

    /// <summary>
    /// All delta-T records, ordered by decimal year.
    /// </summary>
    private List<DeltaTRecord> _DeltaTRecords =>
        _deltaTRecords ??= astroDbContext.DeltaTRecords.OrderBy(dt => dt.DecimalYear).ToList();

    /// <summary>
    /// Cached field for minimum supported year.
    /// </summary>
    private double? _minSupportedYear;

    /// <summary>
    /// The minimum supported year.
    /// </summary>
    private double _MinSupportedYear =>
        _minSupportedYear ??= (double)_DeltaTRecords.First().DecimalYear;

    /// <summary>
    /// Cached field for maximum supported year.
    /// </summary>
    private double? _maxSupportedYear;

    /// <summary>
    /// The maximum supported year.
    /// </summary>
    private double _MaxSupportedYear =>
        _maxSupportedYear ??= (double)_DeltaTRecords.Last().DecimalYear;

    /// <summary>
    /// Cached spline object.
    /// </summary>
    private CubicSpline? _spline;

    /// <summary>
    /// CubicSpline object to interpolate delta-T values.
    /// </summary>
    private CubicSpline _Spline =>
        _spline ??= CubicSpline.InterpolateAkimaSorted(
            _DeltaTRecords.Select(dt => (double)dt.DecimalYear).ToArray(),
            _DeltaTRecords.Select(dt => (double)dt.DeltaT).ToArray());

    /// <summary>
    /// Compute the delta-T value for the given point in time, given as a decimal year.
    /// </summary>
    /// <param name="decimalYear"></param>
    /// <returns></returns>
    public double CalcDeltaTInterpolate(double decimalYear)
    {
        // Check the year is in range.
        if (decimalYear < _MinSupportedYear || decimalYear > _MaxSupportedYear)
        {
            throw new ArgumentOutOfRangeException(nameof(decimalYear), "Year out of range.");
        }

        // Look for an exact match.
        DeltaTRecord? deltaTRecord =
            _DeltaTRecords.FirstOrDefault(dt => dt.DecimalYear == (decimal)decimalYear);
        if (deltaTRecord != null)
        {
            return (double)deltaTRecord.DeltaT;
        }

        // Find the interpolated delta-T value.
        return _Spline.Interpolate(decimalYear);
    }
}
