using DependencyInjectionWithPackages.Services.GpsInstrumentationProvider;
using GpsPackage;
using GpsPackage.Services.GpsCoordinateAveragingService;

namespace DependencyInjectionWithPackages.Services.CurrentCoordinateProvider;

/// <inheritdoc cref="ICurrentCoordinateProvider"/>
public sealed class CurrentCoordinateProvider : ICurrentCoordinateProvider
{
    private readonly IGpsInstrumentationProvider _gpsInstrumentationProvider;
    private readonly IGpsCoordinateAveragingService _gpsCoordinateAveragingService;

    /// <inheritdoc cref="CurrentCoordinateProvider"/>
    public CurrentCoordinateProvider(IGpsInstrumentationProvider gpsInstrumentationProvider, IGpsCoordinateAveragingService gpsCoordinateAveragingService)
    {
        _gpsInstrumentationProvider = gpsInstrumentationProvider;
        _gpsCoordinateAveragingService = gpsCoordinateAveragingService;
    }

    public Coordinate GetCurrentCoordinate()
    {
        // Read three coordinates from 'instrumentation system' e.g. aeroplanes have multiple sensors in case of a fault.
        var coordinate1 = _gpsInstrumentationProvider.GetSensorCoordinate1();
        var coordinate2 = _gpsInstrumentationProvider.GetSensorCoordinate2();
        var coordinate3 = _gpsInstrumentationProvider.GetSensorCoordinate3();
        return _gpsCoordinateAveragingService.GetAverageCoordinateWithErrorCorrection(coordinate1, coordinate2, coordinate3);
    }
}
