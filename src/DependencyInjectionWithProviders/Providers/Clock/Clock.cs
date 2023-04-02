namespace DependencyInjectionWithProviders.Providers.Clock;

public class Clock : IClock
{
    public virtual DateTime Now => DateTime.UtcNow;
}
