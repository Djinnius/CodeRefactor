using OneOf.Types;
using OneOf;

namespace DependencyInjectionWithOptions.Services.SpeedLimitCalculator;

public interface ISpeedLimitCalculator
{
    OneOf<double, None> GetSpeedLimitForCurrentPosition();
}
