using Microsoft.OpenApi.Models;

namespace TaskManagement.Api.Extensions
{
    public static class SwaggerServiceExtensions
    {
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(setup =>
            {
                setup.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "API Gestão de Tarefas",
                    Description = "O objetivo é permitir que os usuários criem, editem e removam tarefas, e visualizem as tarefas de maneira organizada.",
                    Contact = new OpenApiContact
                    {
                        Name = "Cézar Camargo da Silva",
                        Email = "cezar.cds71@gmail.com"
                    }
                });
            });

            return services;
        }
    }
}