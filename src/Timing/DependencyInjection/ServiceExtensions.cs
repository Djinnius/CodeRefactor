using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Timing.Package.Clocks;
using Timing.Package.Options;

namespace Timing.Package.DependencyInjection;

/// <summary>
///     Extension methods to register all default services and options for the <see cref="Timing"/> package.
/// </summary>
public static class ServiceExtensions
{
    /// <summary>
    ///     Registers the Timing services and options from app settings for the <see cref="Timing"/> package.
    /// </summary>
    /// <param name="services"> The service collection to register with. </param>
    /// <param name="configuration"> The configuration to read options from. </param>
    /// <returns> The service collection. </returns>
    public static IServiceCollection AddTimingServices(this IServiceCollection services, IConfiguration configuration)
    {
        // read options from app settings json.
        services.Configure<ClockOptions>(configuration.GetSection(ClockOptions.SectionName));

        RegisterServices(services);

        return services;
    }

    private static void RegisterServices(IServiceCollection services)
    {
        services.AddSingleton<IClock, Clock>();
    }
}
