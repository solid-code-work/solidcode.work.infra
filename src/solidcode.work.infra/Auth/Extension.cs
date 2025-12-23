using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using solidcode.work.infra.Configurations;
using System.Text;

namespace solidcode.work.infra.Authentication;

public static class Extension
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer((serviceProvider, options) =>
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                var jwtSettings = configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();

                if (string.IsNullOrWhiteSpace(jwtSettings?.SecretKey))
                    throw new ArgumentException("JwtSettings:SecretKey is missing or invalid.");

                if (string.IsNullOrWhiteSpace(jwtSettings?.Issuer))
                    throw new ArgumentException("JwtSettings:Issuer is missing or invalid.");

                if (string.IsNullOrWhiteSpace(jwtSettings?.Audience))
                    throw new ArgumentException("JwtSettings:Audience is missing or invalid.");

                var key = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

        return services;
    }
}

