using GpsPackage.DataObjects;
using GpsPackage.Extensions;

namespace GpsPackage.Test.Extensions;

/// <summary>
///     Unit Tests for the <see cref="CoordinateExtensions.AverageWith(Coordinate, Coordinate)"/> 
///     <see cref="Coordinate"/> extension methods that take an input of two or more coordinates,
///     and average of the Longitude and Latitude to generate a new mean <see cref="Coordinate"/>.
/// </summary>
public sealed class CoordinateExtensionTests
{
    [Fact]
    public void AverageWith_ShouldReturnCorrectAverageCoordinate_WithEnumerableOfCoordinate()
    {
        // Arrange
        var coordinate1 = new Coordinate(50, -100);
        var coordinate2 = new Coordinate(40, -110);
        var coordinate3 = new Coordinate(60, -90);
        var coordinates = new List<Coordinate> { coordinate1, coordinate2, coordinate3 };

        // Act
        var result = coordinate1.AverageWith(coordinates);

        // Assert
        result.Latitude.Should().Be(50.0, because: "average latitude is correct");
        result.Longitude.Should().Be(-100.0, because: "average longitude is correct");
    }
}
