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
                    webBuilder.UseStartup<Startup>();
                    
                }).ConfigureAppConfiguration((hostingContext, configuration) =>
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