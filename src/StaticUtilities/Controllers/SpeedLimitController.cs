using Microsoft.AspNetCore.Mvc;
using StaticUtilities.Utilities;

namespace Monolith.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SpeedLimitController : ControllerBase
{
    [HttpGet]
    public IActionResult GetSpeedLimitInKilometersPerHour()
    {
        var speedLimit = SpeedLimitCalculator.GetSpeedLimitForCurrentPosition();
        return speedLimit.Match(m => Ok(m), _ => Ok("No Speed Limit"));
    }
}
