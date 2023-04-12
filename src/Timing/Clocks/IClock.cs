namespace Timing.Package.Clocks;

/// <summary>
///     Provider of current date time and kind, and utilities to normalise provided date times to match kind.
/// </summary>
public interface IClock
{
    /// <summary>
    ///     Gets the current Date Time.
    /// </summary>
    DateTime Now { get; }

    /// <summary>
    ///     Gets the current Kind of Date Time, i.e. local vs UTC.
    /// </summary>
    DateTimeKind Kind { get; }

    /// <summary>
    ///     Does the provider supports multiple time zones, i.e. UTC.
    /// </summary>
    bool SupportsMultipleTimezones { get; }

    /// <summary>
    ///     Normalises given <see cref="DateTime"/> to match the Kind specified in clock options.
    /// </summary>
    /// <param name="dateTime" >DateTime to be normalised. </param>
    /// <returns> A Normalised DateTime. </returns>
    DateTime Normalise(DateTime dateTime);
}
