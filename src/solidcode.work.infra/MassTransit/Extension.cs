using System.Reflection;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using solidcode.work.infra.Configurations;

namespace solidcode.work.infra.MassTransit;

public static class Extension
{
    public static IServiceCollection AddMassTransitWithRabbitMq(this IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumers(Assembly.GetEntryAssembly());

            x.UsingRabbitMq((serviceProvider, options) =>
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                var rabbitMqSettings = configuration.GetSection(nameof(RabbitMqSettings)).Get<RabbitMqSettings>();
                var serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();

                if (string.IsNullOrWhiteSpace(rabbitMqSettings?.Host))
                    throw new ArgumentException("RabbitMQ host is missing or invalid.");

                if (string.IsNullOrWhiteSpace(serviceSettings?.ServiceName))
                    throw new ArgumentException("Service name is missing or invalid.");

                options.Host(rabbitMqSettings.Host);

                options.ConfigureEndpoints(serviceProvider, new KebabCaseEndpointNameFormatter(serviceSettings.ServiceName, false));

                options.UseMessageRetry(retry =>
                {
                    retry.Interval(5, TimeSpan.FromSeconds(3));
                });
            });
        });

        return services;
    }

}