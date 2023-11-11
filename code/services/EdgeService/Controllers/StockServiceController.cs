using CommonMicroserviceSupport.Models;
using EdgeService.Api.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EdgeService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StockServiceController : ControllerBase
{
    private readonly ILogger<StockServiceController> _logger;
    private readonly IStockServiceApi _stockServiceApi;

    public StockServiceController(ILogger<StockServiceController> logger, IStockServiceApi stockServiceApi)
    {
        _logger = logger;
        _stockServiceApi = stockServiceApi;
    }
    
    [HttpGet("/test")]
    public async Task<List<WeatherForecast>> GetWeatherForecast()
    {
        _logger.Log(LogLevel.Information, "Test endpoint hit successfully. Calling stockServiceApi now.");
        return await _stockServiceApi.GetWeatherForecastAsync();
    }
}