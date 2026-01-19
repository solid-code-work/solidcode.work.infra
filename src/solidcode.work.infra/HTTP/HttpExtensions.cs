using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using solidcode.work.infra.Configurations;
using solidcode.work.infra.Configurations;
using Polly;

namespace solidcode.work.infra;

public static class HttpExtensions
{
    public static IServiceCollection AddSolidCodeHttpClient(this IServiceCollection services, IConfiguration configuration)
    {
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