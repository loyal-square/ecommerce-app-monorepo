using System.Net;

namespace EdgeService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(serverOptions =>
                    {
                        serverOptions.Listen(IPAddress.Any,
                            Convert.ToInt32(Environment.GetEnvironmentVariable("PORT")));
                    }).UseStartup<Startup>();
                }).ConfigureAppConfiguration((_, configuration) =>
                {
                    if (Environment.GetEnvironmentVariable("environment")?.Equals("heroku-prod") ?? false)
                    {
                        AppConfiguration.LoadProd();
                    }
                    else
                    {
                        configuration
                            .AddIniFile("local.properties", optional: false, reloadOnChange: true);
                        AppConfiguration.LoadLocal(configuration.Build());
                    }
                });
    }
}