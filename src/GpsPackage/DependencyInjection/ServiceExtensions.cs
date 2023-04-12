using GpsPackage.Constants;
using GpsPackage.Options;
using GpsPackage.Services.GpsCoordinateAveragingService;
using GpsPackage.Services.GpsCoordinateDistanceService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace GpsPackage.DependencyInjection;

/// <summary>
///     Extension methods to register all default services and options for the <see cref="GpsPackage"/>.
/// </summary>
public static class ServiceExtensions
{

    // see https://learn.microsoft.com/en-us/dotnet/core/extensions/options-library-authors for options configuration guidance.

    public static IServiceCollection AddGpsServices(this IServiceCollection services)
    {
        // Configure default options - user can't specify.
        services.Configure<GlobeOptions>(SetDefaultOptions);

        RegisterServices(services);

        return services;
    }

    /// <summary>
    ///     Registers the GPS services and options from app settings for the <see cref="GpsPackage"/> package.
    /// </summary>
    /// <param name="services"> The service collection to register with. </param>
    /// <param name="configuration"> The configuration to read options from. </param>
    /// <returns> The service collection. </returns>
    public static IServiceCollection AddGpsServices(this IServiceCollection services, IConfiguration configuration)
    {
        // read default options from app settings json.
        services.Configure<GlobeOptions>(configuration.GetSection(GlobeOptions.SectionName));

        RegisterServices(services);

        return services;
    }

    public static IServiceCollection AddGpsServices(this IServiceCollection services, GlobeOptions globeOptions)
    {
        // Copy settings from passed settings object.
        services.AddOptions<GlobeOptions>()
            .Configure(options =>
            {
                SetDefaultOptions(options);
                options.RadiusOfGlobeInKm = globeOptions.RadiusOfGlobeInKm;
            });

        RegisterServices(services);

        return services;
    }

    public static IServiceCollection AddGpsServices(this IServiceCollection services, Action<GlobeOptions> configureOptions)
    {
        // Configure via lambda expression in post.
        services.PostConfigure(configureOptions);

        RegisterServices(services);

        return services;
    }

    private static void SetDefaultOptions(GlobeOptions globeOptions)
    {
        globeOptions.RadiusOfGlobeInKm = StaticPlanetConstants.RadiusOfEarthInKilometers;
    }

    private static void RegisterServices(IServiceCollection services)
    {
        services.AddSingleton<IPlanetRadiusConstants, PlanetRadiusConstants>();
        services.AddSingleton<IGpsCoordinateDistanceService, GpsCoordinateDistanceService>();
        services.AddSingleton<IGpsCoordinateAveragingService, GpsCoordinateAveragingService>();
    }
}
