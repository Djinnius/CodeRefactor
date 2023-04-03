using GpsPackage;
using GpsPackage.Options;
using GpsPackage.Services.GpsCoordinateDistanceService;

namespace GpsProject.Test.Services;
public sealed class GpsCoordinateDistanceServiceTests
{
    [Theory]
    [InlineData(0, 0, 0, 0, 0)]
    [InlineData(90, 0, 90, 0, 0)]
    [InlineData(0, 180, 0, -180, 0)]
    [InlineData(37.7749, -122.4194, 40.7128, -74.0060, 4129.09)]
    [InlineData(-33.859972, 151.211111, 51.507222, -0.1275, 16993.29)]
    public void GetDistanceBetweenTwoCoordinatesInKilometers_ShouldCalculateDistanceCorrectly(double latitude1, double longitude1, double latitude2, double longitude2, double expectedDistance)
    {
        // Arrange
        var coordinate1 = new Coordinate(latitude1, longitude1);
        var coordinate2 = new Coordinate(latitude2, longitude2);
        var globeOptions = new GlobeOptions { RadiusOfGlobeInKm = 6371 }; // Earth's radius in km
        var sut = new GpsCoordinateDistanceService(globeOptions);

        // Act
        var distance = sut.GetDistanceBetweenTwoCoordinatesInKilometers(coordinate1, coordinate2);

        // Assert
        distance.Should().BeApproximately(expectedDistance, 0.01);
    }
}
