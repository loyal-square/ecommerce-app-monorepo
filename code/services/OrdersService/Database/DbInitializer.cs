using OrdersService.Models;

namespace OrdersService.Database
{
    public static class DbInitializer
    {
        public static void Initialize(ApiDbContext ctx)
        {
            ctx.Database.EnsureCreated();
            if (!ctx.WeatherForecasts.Any())
            {
                ctx.WeatherForecasts.Add(new WeatherForecast { Summary = "plumbus" });
                ctx.WeatherForecasts.Add(new WeatherForecast { Summary = "flux capacitor" });
                ctx.WeatherForecasts.Add(new WeatherForecast { Summary = "spline reticulator" });
                ctx.SaveChanges();
            }
        }
    }
}
