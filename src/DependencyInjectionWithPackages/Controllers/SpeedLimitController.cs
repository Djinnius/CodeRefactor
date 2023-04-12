using DependencyInjectionWithPackages.Services.CircleTracker;
using DependencyInjectionWithPackages.Services.SpeedLimitCalculator;
using Microsoft.AspNetCore.Mvc;

namespace Monolith.Controllers;

/// <summary>
///     Single Responsibility: Return speed limits under various conditions.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class SpeedLimitController : ControllerBase
{
    private readonly ISpeedLimitCalculator _speedLimitCalculator;
    private readonly ICircleTracker _circleTracker;

    public SpeedLimitController(ISpeedLimitCalculator speedLimitCalculator, ICircleTracker circleTracker)
    {
        _speedLimitCalculator = speedLimitCalculator;
        _circleTracker = circleTracker;
    }

    [HttpGet]
    public IActionResult GetSpeedLimitInKilometersPerHour()
    {
        _circleTracker.IncrementAngle(10 * Math.PI / 180); // increment by 10 degrees
        var speedLimit = _speedLimitCalculator.GetSpeedLimitForCurrentPosition();
        return speedLimit.Match(m => Ok(m), _ => Ok("No Speed Limit"));
    }
}
