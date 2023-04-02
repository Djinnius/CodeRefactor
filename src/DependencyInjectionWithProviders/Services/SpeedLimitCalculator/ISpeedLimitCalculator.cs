using OneOf.Types;
using OneOf;

namespace DependencyInjectionWithProviders.Services.SpeedLimitCalculator;

public interface ISpeedLimitCalculator
{
    OneOf<double, None> GetSpeedLimitForCurrentPosition();
}
