using Microsoft.Extensions.DependencyInjection;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Services;
using TaskManagement.Application.Validator;

namespace TaskManagement.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationConfiguration(this IServiceCollection services)
        {
            services.AddServiceDependencyInjection();
            return services;
        }

        public static IServiceCollection AddServiceDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<ITaskValidator, TaskValidator>();
            return services;

        }
    }
}
