using Microsoft.Extensions.Options;

namespace DependencyInjectionWithPackages.Options;

public class SpeedLimitOptions : IOptions<SpeedLimitOptions>
{
    /// <summary>
    ///     Name of the section to find the options in appsettings
    /// </summary>
    public static readonly string SectionName = "SpeedLimitOptions";

    /// <summary>
    ///     The speed limit in ports.
    /// </summary>
    public double PortSpeedLimitInKm { get; set; }

    /// <summary>
    ///     The speed limit in environmentally protected areas.
    /// </summary>
    public double EnvironmentalSpeedLimitInKm { get; set; }

    /// <summary>
    ///     The speed limit in areas populated with whales.
    /// </summary>
    public double WhaleSpeedLimitInKm { get; set; }

    SpeedLimitOptions IOptions<SpeedLimitOptions>.Value => this;
}
