using System.Text.Json.Serialization;
using TaskManagement.Application.Extensions;
using TaskManagement.Infrastructure.Extensions;

namespace TaskManagement.Api.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IHostApplicationBuilder AddApplicationServices(this IHostApplicationBuilder builder)
        {
            var services = builder.Services;
            var configuration = builder.Configuration;

            services.AddControllers();
            services.AddSwaggerConfiguration();
            services.AddApplicationConfiguration();

            return builder;
        }

        public static IHostApplicationBuilder AddInfraStructureServices(this IHostApplicationBuilder builder)
        {
            var services = builder.Services;
            var configuration = builder.Configuration;

            services.AddInfrastructureConfiguration();
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                {
                    policy
                        .WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });


            return builder;
        }
    }
}
