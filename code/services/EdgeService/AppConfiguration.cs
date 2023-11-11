namespace EdgeService;

public class AppConfiguration
{
    public static string? StockServiceUrl { get; private set; }
    public static void LoadLocal(IConfigurationRoot root)
    {
        StockServiceUrl = root["stock-service.url"];
    }
    public static void LoadProd()
    {
        StockServiceUrl =  Environment.GetEnvironmentVariable("stock-service-url") ?? string.Empty;
    }
}