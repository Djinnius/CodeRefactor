using GpsPackage.DataObjects;

namespace DependencyInjectionWithPackages.Services.CurrentCoordinateProvider;

public class CurrentCoordinateProviderLoggingDecorator : ICurrentCoordinateProvider
{
    private readonly ICurrentCoordinateProvider _currentCoordinateProvider;
    private readonly ILogger<CurrentCoordinateProviderLoggingDecorator> _logger;

    public CurrentCoordinateProviderLoggingDecorator(ICurrentCoordinateProvider currentCoordinateProvider, ILogger<CurrentCoordinateProviderLoggingDecorator> logger)
    {
        _currentCoordinateProvider = currentCoordinateProvider;
        _logger = logger;
    }

    public Coordinate GetCurrentCoordinate()
    {
        _logger.LogInformation("Retrieving coordinate.");
        var result = _currentCoordinateProvider.GetCurrentCoordinate();
        _logger.LogInformation($"Current coordinate is {result}");
        return result;
    }
}
