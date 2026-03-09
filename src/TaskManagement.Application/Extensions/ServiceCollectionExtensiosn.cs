using Microsoft.Extensions.DependencyInjection;

namespace TaskManagement.Application.Extensions
{
    public static class ServiceCollectionExtensiosn
    {
        public static IServiceCollection AddApplicationConfiguration(this IServiceCollection services)
        {

            return services;
        }
    }
}
