﻿using Microsoft.Extensions.Options;

namespace GpsPackage.Options;

/// <summary>
///     Options class containing the globe configuration.
/// </summary>
public class GlobeOptions : IOptions<GlobeOptions>
{
    /// <summary>
    ///     Name of the section to find the options in appsettings
    /// </summary>
    public static readonly string SectionName = "GlobeOptions";

    /// <summary>
    ///     The radius of the globe in kilometers.
    /// </summary>
    public double RadiusOfGlobeInKm { get; set; }

    GlobeOptions IOptions<GlobeOptions>.Value => this;
}
