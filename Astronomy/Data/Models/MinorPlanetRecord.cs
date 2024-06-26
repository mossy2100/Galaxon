﻿using System.ComponentModel.DataAnnotations;

namespace Galaxon.Astronomy.Data.Models;

public class MinorPlanetRecord : DatabaseRecord
{
    // Link to owner.
    public int AstroObjectId { get; set; }

    public virtual AstroObjectRecord? AstroObject { get; set; }

    // The designation in packed form.
    [MaxLength(30)]
    public string PackedDesignation { get; set; } = "";

    // The designation in readable form.
    [MaxLength(30)]
    public string ReadableDesignation { get; set; } = "";

    // Date/time of periapsis (minor planets).
    public DateTime? DatePeriapsis { get; set; }

    // Orbit type (minor planets).
    public byte? OrbitType { get; set; }

    // Is 1-opposition object seen at earlier opposition
    public bool? Is1Opp { get; set; }

    // Is it a critical-list numbered object?
    public bool? IsCriticalListNumberedObject { get; set; }

    // The object a trojan or quasi-satellite is co-orbital with.
    public int? CoOrbitalObjectId { get; set; }

    public virtual AstroObjectRecord? CoOrbitalObject { get; set; }

    // Tholen spectral type.
    [MaxLength(10)]
    public string? Tholen { get; set; }

    // SMASS spectral type.
    [MaxLength(10)]
    public string? SMASS { get; set; }
}
