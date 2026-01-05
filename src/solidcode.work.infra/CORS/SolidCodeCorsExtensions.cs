using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using solidcode.work.infra.Configurations;

namespace solidcode.work.infra.CORS;

public static class SolidCodeCorsExtensions
{
    //appsetting.js
    //  {
    //   "Cors": {
    //     "AllowedOrigins": [
    //       "https://frontend.example.com",
    //       "https://admin.example.com"
    //     ]
    //     }
    //     }
    //
    //


    public static IServiceCollection AddSolidcodeCors(this IServiceCollection services)
    {
        services.AddCors(); // You can defer adding the policy until runtime
        return services;
    }

    public static IApplicationBuilder UseSolideCodeCors(this IApplicationBuilder app)
    {
        var configuration = app.ApplicationServices.GetRequiredService<IConfiguration>();
        var corsOptions = configuration.GetSection("Cors").Get<CorsConfiguration>();

        if (corsOptions?.AllowedOrigins == null || corsOptions.AllowedOrigins.Length == 0)
        {
            throw new InvalidOperationException("Cors:AllowedOrigins is not configured properly in appsettings.json.");
        }

        app.UseCors(policy =>
        {
            policy
                .WithOrigins(corsOptions.AllowedOrigins)
                .AllowAnyMethod()
                .AllowAnyHeader();
        });

        return app;
    }
}