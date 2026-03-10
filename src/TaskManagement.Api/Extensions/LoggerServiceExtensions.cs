using Serilog;

namespace TaskManagement.Api.Extensions
{
    public static class LoggerServiceExtensions
    {
        public static WebApplicationBuilder AddLoggerConfiguration(this WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog((context, config) =>
            {
                config.ReadFrom.Configuration(context.Configuration)
                      .MinimumLevel.Debug()
                      .WriteTo.Console();
            });

            return builder;
        }
    }
}
