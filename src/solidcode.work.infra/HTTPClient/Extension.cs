using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using solidcode.work.infra.Cofigurations;
using solidcode.work.infra.Configurations;
using solidcode.work.infra.Helpers;
using Polly;

namespace solidcode.work.infra.HTTPClient;

public static class Extensions
{
    public static IServiceCollection AddHttpClient(this IServiceCollection services)
    {
        // Build a temporary provider to resolve IConfiguration
        using var scope = services.BuildServiceProvider();
        var configuration = scope.GetRequiredService<IConfiguration>();

        var httpClientSettings = configuration
            .GetSection("HttpClientSetting")
            .Get<HTTPClientSetting>() ??
            throw new InvalidOperationException("HttpClientSetting section is missing from configuration");
        var serviceSettings = configuration
            .GetSection(nameof(ServiceSettings))
            .Get<ServiceSettings>() ??
            throw new InvalidOperationException("Service section is missing from configuration");
        if (string.IsNullOrWhiteSpace(httpClientSettings.BaseUrl))
            throw new InvalidOperationException("BaseUrl in HttpClientSetting cannot be null or empty.");

        services.AddHttpClient("CommonHttpClient", client =>
        {
            client.BaseAddress = new Uri(httpClientSettings.BaseUrl);
            client.Timeout = TimeSpan.FromSeconds(30);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        })
            .AddTransientHttpErrorPolicy(policyBuilder =>
                policyBuilder.WaitAndRetryAsync(3, retryAttempt =>
                    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));

        services.AddScoped<HttpServiceHelper>();
        return services;
    }
}