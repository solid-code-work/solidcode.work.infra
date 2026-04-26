using System.Reflection;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using solidcode.work.infra.Abstraction;
using solidcode.work.infra.Configurations;

namespace solidcode.work.infra.DependencyInjection;

public static class MassTraansitExtension
{
    public static IServiceCollection AddSolidCodeMassTransitWithRabbitMq(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<IRegistrationConfigurator>? configureConsumers = null)
    {
        services.AddMassTransit(x =>
        {
            // If caller provided consumers, register them
            configureConsumers?.Invoke(x);

            // Otherwise, fall back to scanning the entry assembly
            if (configureConsumers == null)
            {
                x.AddConsumers(Assembly.GetEntryAssembly());
            }

            x.UsingRabbitMq((serviceProvider, options) =>
            {
                var rabbitMqSettings = configuration.GetSection(nameof(RabbitMqSetting)).Get<RabbitMqSetting>();
                var serviceSettings = configuration.GetSection(nameof(ServiceSetting)).Get<ServiceSetting>();

                if (string.IsNullOrWhiteSpace(rabbitMqSettings?.Host))
                    throw new ArgumentException("RabbitMQ host is missing or invalid.");

                if (string.IsNullOrWhiteSpace(serviceSettings?.ServiceName))
                    throw new ArgumentException("Service name is missing or invalid.");

                //options.Host(rabbitMqSettings.Host);
                options.Host(rabbitMqSettings.Host, rabbitMqSettings.VirtualHost, h =>
                {
                    h.Username(rabbitMqSettings.Username);
                    h.Password(rabbitMqSettings.Password);
                });


                options.ConfigureEndpoints(
                    serviceProvider,
                    new KebabCaseEndpointNameFormatter(serviceSettings.ServiceName, false));

                options.UseMessageRetry(retry =>
                {
                    retry.Interval(5, TimeSpan.FromSeconds(3));
                });
            });
        });

        services.AddScoped<IMessageProducer, MassTransitHelpercs>();
        return services;
    }
}
