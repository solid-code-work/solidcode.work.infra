using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace SolidCode.Work.Infra.Security;

public static class RateLimiterExtensions
{
    /// <summary>
    /// Registers a rate limiter with custom parameters.
    /// Call in Program.cs:
    /// builder.Services.AddSolidCodeRateLimiter(permitLimit: 10, window: TimeSpan.FromSeconds(30), queueLimit: 5);
    /// </summary>
    public static IServiceCollection AddSolidCodeRateLimiter(
        this IServiceCollection services,
        int permitLimit,
        TimeSpan window,
        int queueLimit)
    {
        services.AddRateLimiter(options =>
        {
            options.AddFixedWindowLimiter("default", limiterOptions =>
            {
                limiterOptions.PermitLimit = permitLimit;
                limiterOptions.Window = window;
                limiterOptions.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
                limiterOptions.QueueLimit = queueLimit;
            });
        });

        return services;
    }

    /// <summary>
    /// Applies the rate limiter middleware.
    /// Call in Program.cs:
    /// app.UseSolidCodeRateLimiter();
    /// </summary>
    public static WebApplication UseSolidCodeRateLimiter(this WebApplication app)
    {
        app.UseRateLimiter();
        return app;
    }
}

