namespace StaticUtilities.Objects;

/// <summary>
///     A coordinate representing the position on a sphere in terms of latitude and longitude in decimal degrees.
/// </summary>
public readonly struct Coordinate : IEquatable<Coordinate>
{
    /// <summary>
    ///     The angular distance north or south of the earth's equator, measured in degrees 
    ///     along a meridian, as on a map or globe.
    /// </summary>
    /// <remarks> Values range between -90 and +90 decimal degrees. </remarks>
    public double Latitude { get; }

    /// <summary>
    ///     The angular distance on the earth's surface, measured east or west from the Prime 
    ///     Meridian at Greenwich, England, to the meridian passing through a position, 
    ///     expressed in degrees.
    /// </summary>
    /// <remarks> Values range between -180 and +180 decimal degrees. </remarks>
    public double Longitude { get; }

    /// <inheritdoc cref="Coordinate"/>
    /// <param name="latitude"></param>
    /// <param name="longitude"></param>
    /// <exception cref="ArgumentOutOfRangeException"> 
    ///     Thrown when the latitude is less than -90 or more than +90 decimal degrees 
    ///     -or- when the lonitude is less than -180 or more than +180 decimal degrees.
    /// </exception>
    public Coordinate(double latitude, double longitude)
    {
        if (latitude < -90 || latitude > 90) throw new ArgumentOutOfRangeException(nameof(latitude));
        if (longitude < -180 || longitude > 180) throw new ArgumentOutOfRangeException(nameof(longitude));

        Latitude = latitude;
        Longitude = longitude;
    }

    /// <summary>
    ///     A default coordinate representing the position (0,0) on a sphere.
    /// </summary>
    public Coordinate()
    {
        Latitude = 0;
        Longitude = 0;
    }

    public override bool Equals(object? obj) => obj is Coordinate other && Equals(other);

    public bool Equals(Coordinate other) => Latitude == other.Latitude && Longitude == other.Longitude;

    public override int GetHashCode() => HashCode.Combine(Latitude, Longitude);

    public static bool operator ==(Coordinate left, Coordinate right) => left.Equals(right);

    public static bool operator !=(Coordinate left, Coordinate right) => !left.Equals(right);

    public override string ToString() => $"Latitude: {Latitude}, Longitude: {Longitude}";
}