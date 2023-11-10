using EdgeService.Managers;
using EdgeService.Managers.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EdgeService.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IWeatherForcastManager _weatherForcastManager;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherForcastManager weatherForcastManager)
    {
        _logger = logger;
        _weatherForcastManager = weatherForcastManager;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return _weatherForcastManager.Get();
    }
}