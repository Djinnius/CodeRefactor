using Microsoft.Extensions.Options;

namespace DependencyInjectionWithOptions.Options;

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
