using Microsoft.Extensions.Options;

namespace Timing.Package.Options;

/// <summary>
///     Options class containing the clock configuration.
/// </summary>
public sealed class ClockOptions : IOptions<ClockOptions>
{
    /// <summary>
    ///     Name of the section to find the options in the appsettings file
    /// </summary>
    public const string SectionName = "Providers:Clock";

    /// <summary>
    ///     Default way to interpret date time
    /// </summary>
    public DateTimeKind? Kind { get; set; }

    /// <summary>
    ///     Get the <see cref="DateTimeKind"/> value.
    ///     or returns a default <see cref="DateTimeKind.Utc"/>
    ///     if the options <see cref="Kind"/> was null.
    /// </summary>
    public DateTimeKind GetDateTimeKindWithFallback() => Kind ?? DateTimeKind.Utc;

    ClockOptions IOptions<ClockOptions>.Value => this;
}
