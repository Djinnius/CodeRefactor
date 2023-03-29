using DependencyInjectionWithOptions.Services.SpeedLimitCalculator;
using Microsoft.AspNetCore.Mvc;

namespace Monolith.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SpeedLimitController : ControllerBase
{
    private readonly ISpeedLimitCalculator _speedLimitCalculator;

    public SpeedLimitController(ISpeedLimitCalculator speedLimitCalculator)
    {
        _speedLimitCalculator = speedLimitCalculator;
    }


    [HttpGet]
    public IActionResult GetSpeedLimitInKilometersPerHour()
    {
        var speedLimit = _speedLimitCalculator.GetSpeedLimitForCurrentPosition();
        return speedLimit.Match(m => Ok(m), _ => Ok("No Speed Limit"));
    }
}
