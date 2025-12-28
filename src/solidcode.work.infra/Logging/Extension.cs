using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace solidcode.work.infra.Logging;

public static class Extensions
{
    /// <summary>
    /// Configures Serilog with solid defaults and full configuration support.
    /// Works for WebAPI, Worker, and Console apps.
    /// </summary>
    public static IHostBuilder AddSolidcodeLogging(this IHostBuilder hostBuilder)
    {
        hostBuilder.UseSerilog((context, services, logger) =>
        {
            logger
                // -----------------------------
                // Configuration first (highest priority)
                // -----------------------------
                .ReadFrom.Configuration(context.Configuration)

                // -----------------------------
                // Sensible defaults (fallbacks)
                // -----------------------------
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)

                // -----------------------------
                // Enrichment (structured metadata)
                // -----------------------------
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentName()
                .Enrich.WithProcessId()
                .Enrich.WithThreadId()

                // -----------------------------
                // Console sink (developer friendly)
                // -----------------------------
                .WriteTo.Console(
                    theme: AnsiConsoleTheme.Code,
                    outputTemplate:
                        "[{Timestamp:HH:mm:ss} {Level:u3}] " +
                        "[{Environment}] " +
                        "[{CorrelationId}] " +
                        "[{UserId}] " +
                        "{Message:lj}{NewLine}{Exception}"
                )

                // -----------------------------
                // File sink (production safe)
                // -----------------------------
                .WriteTo.File(
                    path: "logs/solidcode-.log",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 14,
                    shared: true,
                    outputTemplate:
                        "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] " +
                        "[{Environment}] " +
                        "[{CorrelationId}] " +
                        "[{UserId}] " +
                        "{Message:lj}{NewLine}{Exception}"
                );
        });

        return hostBuilder;
    }
}
