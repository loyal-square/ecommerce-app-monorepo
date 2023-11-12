using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MonolithServer.Database;
using Npgsql;

namespace MonolithServer
{
    public class Program
    {

        public static void Main(string[] args)
        {

            var loggerFactory = LoggerFactory.Create(
                builder => builder
                    // add console as logging target
                    .AddConsole()
                    // add debug output as logging target
                    .AddDebug()
                    // set minimum level to log
                    .SetMinimumLevel(LogLevel.Debug));
            var logger = loggerFactory.CreateLogger<Program>();
            
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."

                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        
                        new string[] {}
                    }
                });
            });

            var initString = (Environment.GetEnvironmentVariable("environment")?.Equals("heroku-prod") ?? false
                                 ? Environment.GetEnvironmentVariable("DATABASE_URL") ?? "invalidString"
                                 : builder.Configuration.GetConnectionString("DbContext"));

            if (Environment.GetEnvironmentVariable("environment")?.Equals("heroku-prod") ?? false)
            {
                var databaseUrl = initString;
                var databaseUri = new Uri(databaseUrl);
                var userInfo = databaseUri.UserInfo.Split(':');

                var sqlBuilder = new NpgsqlConnectionStringBuilder
                {
                    Host = databaseUri.Host,
                    Port = databaseUri.Port,
                    Username = userInfo[0],
                    Password = userInfo[1],
                    Database = databaseUri.LocalPath.TrimStart('/'),
                    SslMode = SslMode.Require, // Adjust based on your security requirements,
                    TrustServerCertificate = true
                };

                var connectionString = sqlBuilder.ToString();
            
                logger.LogInformation(connectionString);

                builder.Services.AddEntityFrameworkNpgsql().AddDbContext<ApiDbContext>(options =>
                    options.UseNpgsql(connectionString));
            }
            else
            {
                builder.Services.AddEntityFrameworkNpgsql().AddDbContext<ApiDbContext>(options =>
                    options.UseNpgsql(initString));
            }
            
            
            builder.Services.AddCognitoIdentity();
            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.Authority = builder.Configuration["Cognito:Authority"];
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = false
                    };
                });

            var app = builder.Build();
            using var scope = app.Services.CreateScope();
            var ctx = scope.ServiceProvider.GetRequiredService<ApiDbContext>();

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
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.MapControllers();

            app.Run();
        }
    }
}