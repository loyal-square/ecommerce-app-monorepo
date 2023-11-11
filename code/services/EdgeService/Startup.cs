using EdgeService.Api.implementations;
using EdgeService.Api.interfaces;
using Microsoft.OpenApi.Models;

namespace EdgeService
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        private const string MyAllowedSpecificOrigins = "_myAllowSpecificOrigins";
        public void ConfigureServices(IServiceCollection services)
        {
            // Add services and configure dependencies here
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowedSpecificOrigins,
                    policy  =>
                    {
                        policy.WithOrigins("https://www.test-cors.org");
                    });
            });
            services.AddControllers();
            // Configure Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your Web API", Version = "v1" });
            });
            services.AddLogging(builder => builder.AddConsole());
            
            //Inject Managers
            
            //Inject internal service APIs
            services.AddSingleton<IStockServiceApi>(new StockServiceApi(new Logger<StockServiceApi>(new LoggerFactory())));


        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                // Configure error handling for the production environment here
            }
            
            app.UseRouting();
            // global cors policy
            app.UseCors(MyAllowedSpecificOrigins);
            
            // Add middleware and routing configuration here
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            
        }
    }
}