using CommonMicroserviceSupport.Models;

namespace EdgeService.Api.interfaces;

public interface IStockServiceApi
{
    public Task<List<WeatherForecast>> GetWeatherForecastAsync();
}