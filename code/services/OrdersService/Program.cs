using Microsoft.EntityFrameworkCore;
using ProfileService.Database;

namespace ProfileService
{
    public class Program
    {

        public static void Main(string[] args)
        {
            
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddEntityFrameworkNpgsql().AddDbContext<ApiDbContext>(options =>
                  options.UseNpgsql(builder.Configuration.GetConnectionString("DbContext")));

            var app = builder.Build();
            using var scope = app.Services.CreateScope();
            var ctx = scope.ServiceProvider.GetRequiredService<ApiDbContext>();
            var logger = app.Services.GetRequiredService<ILogger<Program>>();

            try
            {
                DbInitializer.Initialize(ctx);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred creating the DB.");
            }


             // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                
            }
            
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}