using FormSubmission.Core.Interfaces;
using FormSubmission.DAL.Configuration;
using FormSubmission.DAL.Data;
using FormSubmission.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FormSubmission.DAL.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IFormRepository, FormRepository>();
            
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
                    options.UseInMemoryDatabase(connectionString ?? "FormsDb");
                }
            });
            
            return services;
        }
    }
} 