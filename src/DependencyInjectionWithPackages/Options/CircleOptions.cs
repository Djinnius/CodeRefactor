using DependencyInjectionWithPackages.Objects;
using Microsoft.Extensions.Options;

namespace DependencyInjectionWithPackages.Options;

public class CircleOptions : IOptions<CircleOptions>
{
    /// <summary>
    ///     Name of the section to find the options in appsettings
    /// </summary>
    public static readonly string SectionName = "CircleOptions";

    /// <summary>
    ///     The radius of the circle in kilometers.
    /// </summary>
    public double CircleRadiusInKm { get; set; }

    /// <summary>
    ///     The centre coordinate of the circle.
    /// </summary>
    public Coordinate CircleCentreCoordinate { get; set; }

    CircleOptions IOptions<CircleOptions>.Value => this;
}
