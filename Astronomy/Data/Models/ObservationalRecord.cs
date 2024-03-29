﻿namespace Galaxon.Astronomy.Data.Models;

/// <summary>
/// Represents an observational record related to an astronomical object.
/// </summary>
public class ObservationalRecord : DataObject
{
    /// <summary>
    /// Gets or sets the link to the astronomical object associated with this record.
    /// </summary>
    public virtual int AstroObjectId { get; set; }

    /// <summary>
    /// Gets or sets the reference to the astronomical object associated with this record.
    /// </summary>
    public virtual AstroObject? AstroObject { get; set; }

    /// <summary>
    /// Gets or sets the absolute magnitude of the astronomical object.
    /// </summary>
    public double? AbsMag { get; set; }

    /// <summary>
    /// Gets or sets the minimum apparent magnitude observed for the astronomical object.
    /// </summary>
    public double? MinApparentMag { get; set; }

    /// <summary>
    /// Gets or sets the maximum apparent magnitude observed for the astronomical object.
    /// </summary>
    public double? MaxApparentMag { get; set; }

    /// <summary>
    /// Gets or sets the minimum angular diameter of the astronomical object in degrees.
    /// </summary>
    public double? MinAngularDiam { get; set; }

    /// <summary>
    /// Gets or sets the maximum angular diameter of the astronomical object in degrees.
    /// </summary>
    public double? MaxAngularDiam { get; set; }
}
