using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace solidcode.work.infra.Logging;

public static class SerilogServiceCollectionExtensions
{
    /// <summary>
    /// Configures Serilog with sensible defaults (console + file, structured output, colors).
    /// </summary>
    public static IHostBuilder AddLogging(this IHostBuilder hostBuilder)
    {
        hostBuilder.UseSerilog((ctx, cfg) =>
        {
            cfg.ReadFrom.Configuration(ctx.Configuration) // allow overrides via appsettings.json
               .Enrich.FromLogContext()
               .WriteTo.Console(
                   outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}",
                   theme: AnsiConsoleTheme.Code
               )
               .WriteTo.File("logs/solidcode.log",
                   rollingInterval: RollingInterval.Day,
                   outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"
               );
        });

        return hostBuilder;
    }
}

