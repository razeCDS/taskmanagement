using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Services;
using TaskManagement.Application.Validator;
using TaskManagement.Domain.Enum;
using TaskManagement.Domain.Result;

namespace TaskManagement.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationConfiguration(this IServiceCollection services)
        {
            services.AddServiceDependencyInjection();
            services.AddModelValidation();
            return services;
        }

        public static IServiceCollection AddServiceDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<ITaskValidator, TaskValidator>();
            return services;

        }

        public static IServiceCollection AddModelValidation(this IServiceCollection services)
        {

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState.Values
    .SelectMany(x => x.Errors)
    .Select(x => x.ErrorMessage).ToList();

                    var error = new Error($"Request inválido - {string.Join(" | ", errors)}", ErrorType.Validation);

                    return new BadRequestObjectResult(Result.Failure(error));
                };
            });
            return services;

        }
    }
}
