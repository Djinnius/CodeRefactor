using OneOf.Types;
using OneOf;

namespace DependencyInjection.Services.SpeedLimitCalculator;

public interface ISpeedLimitCalculator
{
    OneOf<double, None> GetSpeedLimitForCurrentPosition();
}
