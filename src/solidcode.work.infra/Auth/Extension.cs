using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using solidcode.work.infra.Configurations;
using solidcode.work.infra.security;

namespace solidcode.work.infra.Extensions;

public static class JwtAuthenticationExtension
{
    public static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        Console.WriteLine("🔧 Registering JWT Authentication");

        services.Configure<JwtSettings>(
            configuration.GetSection(nameof(JwtSettings)));

        var jwtSettings = configuration
            .GetSection(nameof(JwtSettings))
            .Get<JwtSettings>()
            ?? throw new InvalidOperationException("JwtSettings section is missing.");

        ValidateJwtSettings(jwtSettings);


        var signingKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings.SecretKey));

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = signingKey,

            RoleClaimType = ClaimTypes.Role,
            NameClaimType = ClaimTypes.Name,

            ClockSkew = TimeSpan.Zero
        };

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = tokenValidationParameters;
                opt.SaveToken = true;

                opt.Events = new JwtBearerEvents
                {
                    // ------------------------------------------------------------
                    // 🔍 RAW TOKEN EXTRACTION
                    // ------------------------------------------------------------
                    OnMessageReceived = ctx =>
                    {

                        var authHeader = ctx.Request.Headers["Authorization"].ToString();


                        if (string.IsNullOrWhiteSpace(authHeader))
                        {
                            Console.WriteLine("❌ No Authorization header found");
                            return Task.CompletedTask;
                        }

                        if (!authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine("❌ Authorization header does NOT start with 'Bearer '");
                            return Task.CompletedTask;
                        }

                        var token = authHeader.Substring("Bearer ".Length).Trim();

                        var dotCount = token.Count(c => c == '.');

                        if (dotCount != 2)
                        {
                            Console.WriteLine("❌ TOKEN IS MALFORMED (dot count != 2)");
                            return Task.CompletedTask;
                        }

                        ctx.Token = token;

                        return Task.CompletedTask;
                    },

                    // ------------------------------------------------------------
                    // ❌ AUTHENTICATION FAILURE
                    // ------------------------------------------------------------
                    OnAuthenticationFailed = ctx =>
                    {
                        Console.WriteLine("❌ JWT AUTHENTICATION FAILED");
                        Console.WriteLine($"Exception Type : {ctx.Exception.GetType().Name}");
                        Console.WriteLine($"Exception Msg  : {ctx.Exception.Message}");

                        if (ctx.Exception.InnerException != null)
                        {
                            Console.WriteLine($"Inner Exception: {ctx.Exception.InnerException.Message}");
                        }

                        return Task.CompletedTask;
                    },

                    // ------------------------------------------------------------
                    // ✅ TOKEN VALIDATED
                    // ------------------------------------------------------------
                    OnTokenValidated = ctx =>
                    {
                        Console.WriteLine("✅ JWT TOKEN VALIDATED");

                        var identity = ctx.Principal?.Identity as ClaimsIdentity;

                        Console.WriteLine($"Authenticated: {identity?.IsAuthenticated}");
                        Console.WriteLine($"Name: {identity?.Name}");

                        if (identity != null)
                        {
                            foreach (var claim in identity.Claims)
                            {
                                Console.WriteLine($"Claim: {claim.Type} = {claim.Value}");
                            }
                        }

                        return Task.CompletedTask;
                    },

                    // ------------------------------------------------------------
                    // 🚫 AUTHORIZATION CHALLENGE
                    // ------------------------------------------------------------
                    OnChallenge = ctx =>
                    {
                        Console.WriteLine("🚫 JWT CHALLENGE TRIGGERED");
                        Console.WriteLine($"Error: {ctx.Error}");
                        Console.WriteLine($"Description: {ctx.ErrorDescription}");

                        return Task.CompletedTask;
                    }
                };
            });

        services.AddAuthorization();
        services.AddScoped<JwtTokenHelper>();

        Console.WriteLine("✅ JWT Authentication registered");

        return services;
    }

    private static void ValidateJwtSettings(JwtSettings settings)
    {
        if (string.IsNullOrWhiteSpace(settings.SecretKey))
            throw new InvalidOperationException("JwtSettings:SecretKey is missing.");

        if (settings.SecretKey.Length < 32)
            throw new InvalidOperationException(
                "JwtSettings:SecretKey must be at least 32 characters long.");

        if (string.IsNullOrWhiteSpace(settings.Issuer))
            throw new InvalidOperationException("JwtSettings:Issuer is missing.");

        if (string.IsNullOrWhiteSpace(settings.Audience))
            throw new InvalidOperationException("JwtSettings:Audience is missing.");

        if (settings.ExpiryMinutes <= 0)
            throw new InvalidOperationException(
                "JwtSettings:ExpiryMinutes must be greater than zero.");
    }
}
