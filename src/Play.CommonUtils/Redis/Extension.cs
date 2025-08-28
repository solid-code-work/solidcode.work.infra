using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Play.CommonUtils.Configurations;

namespace Play.CommonUtils.Redis;

public static class Extension
{
    public static IServiceCollection AddRedisCacheService(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var redisSettings = configuration.GetSection(nameof(RedisSettings)).Get<RedisSettings>();
        var serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
        if (redisSettings == null || string.IsNullOrWhiteSpace(redisSettings.ConnectionString))
            throw new InvalidOperationException("Redis settings are missing or invalid.");

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisSettings.ConnectionString;
            options.InstanceName = serviceSettings.ServiceName;
        });
        services.AddScoped<ICacheService, RedisCacheService>();

        return services;
    }

}