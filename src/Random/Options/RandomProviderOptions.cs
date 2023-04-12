using Microsoft.Extensions.Options;

namespace PseudoRandom.Package.Options;

/// <summary>
///     Options class containing the <see cref="Random"/> configuration.
/// </summary>
public sealed class RandomProviderOptions : IOptions<RandomProviderOptions>
{
    /// <summary>
    ///     Name of the section to find the options in the appsettings file
    /// </summary>
    public const string SectionName = "Providers:Random";

    /// <summary>
    ///     The seed value for random.
    /// </summary>
    public int? Seed { get; set; }


    RandomProviderOptions IOptions<RandomProviderOptions>.Value => this;
}
