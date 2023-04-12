using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PseudoRandom.Package.Options;
using PseudoRandom.Package.Randomness;

namespace PseudoRandom.Package.DependencyInjection;

/// <summary>
///     Extension methods to register all default services and options for the <see cref="PseudoRandom"/> package.
/// </summary>
public static class ServiceExtensions
{
    /// <summary>
    ///     Registers the Psuedo-Random services and options from app settings for the <see cref="PseudoRandom"/> package.
    /// </summary>
    /// <param name="services"> The service collection to register with. </param>
    /// <param name="configuration"> The configuration to read options from. </param>
    /// <returns> The service collection. </returns>
    public static IServiceCollection AddPseudoRandomServices(this IServiceCollection services, IConfiguration configuration)
    {
        // read default options from app settings json.
        services.Configure<RandomProviderOptions>(configuration.GetSection(RandomProviderOptions.SectionName));

        RegisterServices(services);

        return services;
    }

    private static void RegisterServices(IServiceCollection services)
    {
        services.AddSingleton<IRandomNumberProvider, RandomNumberProvider>();
    }

}
