using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace solidcode.work.infra;

public static class SolidCodeSwagger
{
    public static IServiceCollection AddSolidCodeSwagger(
        this IServiceCollection services,
        string title = "API",
        string version = "v1")
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc(version, new OpenApiInfo
            {
                Title = title,
                Version = version
            });

            // ✅ USE CONCRETE TYPE (NOT INTERFACE)
            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter: Bearer {your JWT token}"
            };

            c.AddSecurityDefinition("Bearer", securityScheme);

            // ✅ USE CONCRETE TYPE
            var securityRequirement = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            };

            c.AddSecurityRequirement(securityRequirement);
        });

        return services;
    }

    public static WebApplication UseSwaggerWithUi(
    this WebApplication app,
    string title = "API",
    string version = "v1")
    {
        app.UseSwagger();

        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint(
                $"/swagger/{version}/swagger.json",
                $"{title} {version}"
            );
        });

        return app;
    }
}
