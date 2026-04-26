using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wolverine;
using Wolverine.EntityFrameworkCore;
using Wolverine.RabbitMQ;
using solidcode.work.infra.Configurations;
using Microsoft.EntityFrameworkCore;


namespace solidcode.work.infra.DependencyInjection;

// dotnet ef migrations add AddWolverineMessaging
// dotnet ef database update

public static class WolverineExtension
{
    public static IServiceCollection AddSolidCodeWolverineWithRabbitMq<TContext>(
        this IServiceCollection services,
        IConfiguration configuration)
        where TContext : DbContext
    {
        services.AddWolverine(opts =>
        {
            opts.UseEntityFrameworkCoreTransactions();

            opts.Services.AddDbContext<TContext>();

            var rabbitMqSettings = configuration
                .GetSection(nameof(RabbitMqSetting))
                .Get<RabbitMqSetting>();

            var serviceSettings = configuration
                .GetSection(nameof(ServiceSetting))
                .Get<ServiceSetting>();

            opts.UseRabbitMq(factory =>
            {
                factory.HostName = rabbitMqSettings!.Host;
                factory.VirtualHost = rabbitMqSettings.VirtualHost;
                factory.UserName = rabbitMqSettings.Username;
                factory.Password = rabbitMqSettings.Password;
            })
            .AutoProvision();
        });
        return services;
    }
}

/// ✅ HOW TO USE IN A WEB API / MICROSERVICE:
///
/// 1. Register your DbContext (IMPORTANT: done in the application, not here)
///    services.AddDbContext<MyDbContext>(options =>
///        options.UseSqlServer(configuration.GetConnectionString("Default")));
///
/// 2. Call this extension:
///    services.AddSolidCodeWolverineWithRabbitMq<MyDbContext>(configuration);
///
/// 3. Define message handlers anywhere in your project:
///    public class MyHandler
///    {
///        public Task Handle(MyMessage message)
///        {
///            // handle message
///        }
///    }
///
/// 4. Send messages via IMessageBus:
///    public class MyService
///    {
///        private readonly IMessageBus _bus;
///
///        public MyService(IMessageBus bus)
///        {
///            _bus = bus;
///        }
///
///        public async Task DoWork()
///        {
///            await _bus.SendAsync(new MyMessage());
///        }
///    }
///
/// ⚠️ NOTES:
/// - DbContext MUST be registered before calling this extension
/// - Uses EF Core transactional outbox → prevents message loss
/// - Duplicate messages are still possible → handlers should be idempotent