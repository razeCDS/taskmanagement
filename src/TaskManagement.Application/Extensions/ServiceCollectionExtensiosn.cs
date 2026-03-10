using Microsoft.Extensions.DependencyInjection;
using TaskManagement.Application.Services.TaskService;

namespace TaskManagement.Application.Extensions
{
    public static class ServiceCollectionExtensiosn
    {
        public static IServiceCollection AddApplicationConfiguration(this IServiceCollection services)
        {
            services.AddServiceDependencyInjection();
            return services;
        }

        public static IServiceCollection AddServiceDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<ITaskService, TaskService>();
            return services;
        }
    }
}
