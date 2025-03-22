using FormSubmission.BLL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FormSubmission.BLL.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services)
        {
            services.AddScoped<IFormService, FormService>();
            
            return services;
        }
    }
} 