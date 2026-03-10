using Microsoft.Extensions.DependencyInjection;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Mappings;
using TaskManagement.Application.Services;
using TaskManagement.Application.Validator;

namespace TaskManagement.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationConfiguration(this IServiceCollection services)
        {
            services.AddServiceDependencyInjection();
            services.AddMapperConfiguration();
            return services;
        }

        public static IServiceCollection AddServiceDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<ITaskValidator, TaskValidator>();
            return services;
        }

        public static IServiceCollection AddMapperConfiguration(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg => { }, typeof(TaskMappingProfile));
            return services;
        }
    }
}
