using Microsoft.Extensions.Options;
using Timing.Package.Options;

namespace Timing.Package.Clocks;

/// <inheritdoc cref="IClock"/>
internal class Clock : IClock
{
    protected ClockOptions Options { get; }

    public Clock(IOptions<ClockOptions> options)
    {
        Options = options.Value;
    }

    public virtual DateTime Now => Options.Kind == DateTimeKind.Utc ? DateTime.UtcNow : DateTime.Now;

    public virtual DateTimeKind Kind => Options.GetDateTimeKindWithFallback();

    public virtual bool SupportsMultipleTimezones => Options.Kind == DateTimeKind.Utc;

    public virtual DateTime Normalise(DateTime dateTime)
    {
        if (Kind == DateTimeKind.Unspecified || Kind == dateTime.Kind)
            return dateTime;

        if (Kind == DateTimeKind.Local && dateTime.Kind == DateTimeKind.Utc)
            return dateTime.ToLocalTime();

        if (Kind == DateTimeKind.Utc && dateTime.Kind == DateTimeKind.Local)
            return dateTime.ToUniversalTime();

        return DateTime.SpecifyKind(dateTime, Kind);
    }
}
