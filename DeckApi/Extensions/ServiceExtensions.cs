using DeckApi.Converters;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using DeckApi.Logging;
using DeckApi.Services;
using Microsoft.Extensions.Configuration;

namespace DeckApi.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod() // TODO: change to GET POST
                        .AllowAnyHeader());
            });
        }

        public static void ConfigureIISIntegration(this IServiceCollection services)
        {
            services.Configure<IISOptions>(options =>
            {
                
            });
        }

        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddScoped<ILoggerManager, LoggerManager>();
        }
        
        public static void ConfigureRepositoryService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IDeckRepository>(x =>
                new InMemoryDeckRepository(configuration.GetSection("DeckRepository")["ShuffleType"]));
        }

        public static void ConfigureControllers(this IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new CardJsonConverter());
                options.JsonSerializerOptions.WriteIndented = true;
            });
        }
    }
}