using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Infrastructure.Context;
using TaskManagement.Infrastructure.Repository.TaskRepository;

namespace TaskManagement.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureConfiguration(this IServiceCollection services)
        {
            services.AddDatabaseConfiguration();
            return services;
        }


        public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services)
        {
            services.AddScoped<ITaskRepository, TaskRepository>();

            services.AddDbContext<TaskDbContext>(opt => opt.UseInMemoryDatabase("TaskDB"));
          
            return services;
        }
    }
}
