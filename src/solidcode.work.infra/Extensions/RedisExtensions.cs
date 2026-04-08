using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using solidcode.work.infra.Abstraction;
using solidcode.work.infra.Configurations;
using StackExchange.Redis;

namespace solidcode.work.infra;

public static class RedisExtensions
{
    public static IServiceCollection AddRedisCacheService(this IServiceCollection services, IConfiguration configuration)
    {
        var redisSettings = configuration.GetSection(nameof(RedisSetting)).Get<RedisSetting>();
        var serviceSettings = configuration.GetSection(nameof(ServiceSetting)).Get<ServiceSetting>();

        if (redisSettings == null || string.IsNullOrWhiteSpace(redisSettings.ConnectionString))
            throw new InvalidOperationException("Redis settings are missing or invalid.");

        // Register the multiplexer as a singleton (shared across the app)
        services.AddSingleton<IConnectionMultiplexer>(sp =>
            ConnectionMultiplexer.Connect(redisSettings.ConnectionString));

        // Register the cache service using IDatabase from the multiplexer
        services.AddScoped<ICacheService, RedisCacheService>();

        return services;
    }
}
