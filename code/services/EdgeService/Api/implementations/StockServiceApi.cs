using System.Text.Json;
using CommonMicroserviceSupport.Models;
using EdgeService.Api.interfaces;

namespace EdgeService.Api.implementations;

public class StockServiceApi: IStockServiceApi
{
    private readonly HttpClient _httpClient = new HttpClient();
    private readonly ILogger _logger;
    public StockServiceApi(ILogger logger)
    {
        _logger = logger;
        
        _logger.LogInformation(AppConfiguration.StockServiceUrl);
        
        var stockServiceUrl = AppConfiguration.StockServiceUrl ?? "The url was not found. So this one was used instead xD";
        _httpClient.BaseAddress = new Uri(stockServiceUrl);
    }
    public async Task<List<WeatherForecast>> GetWeatherForecastAsync()
    {
        var httpResponse = await _httpClient.GetAsync("/api/WeatherForecast");
        return await httpResponse.Content.ReadFromJsonAsync<List<WeatherForecast>>() ?? new List<WeatherForecast>();
    }
}