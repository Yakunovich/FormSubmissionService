using FormSubmissionService.Configuration;
using FormSubmissionService.Data;
using FormSubmissionService.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;

namespace FormSubmissionService.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IFormService, FormService>();
            
            return services;
        }
        
        public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });
            
            return services;
        }
        
        public static IServiceCollection AddApplicationDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DatabaseSettings>(
                configuration.GetSection(DatabaseSettings.SectionName));

            var databaseSettings = configuration
                .GetSection(DatabaseSettings.SectionName)
                .Get<DatabaseSettings>();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<FormDbContext>(options =>
            {
                if (databaseSettings?.Provider == "InMemory")
                {
                    options.UseInMemoryDatabase(connectionString);
                }
            });
            
            return services;
        }
        
        public static IServiceCollection AddControllerConfig(this IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                });
                
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            
            return services;
        }
    }
} 