using Microsoft.AspNetCore.Mvc;
using NonStatic.Services;
using LazyCache;

namespace Monolith.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SpeedLimitController : ControllerBase
{
    [HttpGet]
    public IActionResult GetSpeedLimitInKilometersPerHour()
    {
        var appcache = new CachingService();
        var coordinateProvider = new CoordinateProvider(appcache);
        var coordinateAggregator = new CoordinateAggregator(coordinateProvider);
        var speedLimitCalculator = new SpeedLimitCalculator(coordinateAggregator);
        var speedLimit = speedLimitCalculator.GetSpeedLimitForCurrentPosition();
        return speedLimit.Match(m => Ok(m), _ => Ok("No Speed Limit"));
    }
}
