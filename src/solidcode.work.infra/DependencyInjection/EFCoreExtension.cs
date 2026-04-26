using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using solidcode.work.infra.Abstraction;
using solidcode.work.infra.Configurations;
using solidcode.work.infra.Persistence;
using solidcode.work.infra.Repositories;

namespace solidcode.work.infra.DependencyInjection;

public static class EFCoreExtensions
{
    public static IServiceCollection AddPostgreDbContext<TContext>(this IServiceCollection services)
        where TContext : DbContext
    {
        services.AddDbContext<TContext>((serviceProvider, options) =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var postgresSettings = configuration
                .GetSection(nameof(PostgreSQLSetting))
                .Get<PostgreSQLSetting>();

            if (string.IsNullOrWhiteSpace(postgresSettings?.ConnectionString))
                throw new ArgumentException("PostgreSQL connection string is missing or invalid.");

            options.UseNpgsql(postgresSettings.ConnectionString,
                npgsqlOptions =>
                {
                    npgsqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorCodesToAdd: null);
                });
        });

        return services;
    }
    public static IServiceCollection AddSqlServerDbContext<TContext>(this IServiceCollection services)
        where TContext : DbContext
    {
        services.AddDbContext<TContext>((serviceProvider, options) =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var sqlSettings = configuration
                .GetSection(nameof(MSSQLsetting))
                .Get<MSSQLsetting>();

            if (string.IsNullOrWhiteSpace(sqlSettings?.ConnectionString))
                throw new ArgumentException("SQL Server connection string is missing or invalid.");

            options.UseSqlServer(sqlSettings.ConnectionString);
        });

        services.AddScoped<DbContext>(sp => sp.GetRequiredService<TContext>());
        services.AddScoped<IApplicationDbContext>(sp => new ApplicationDbContext(sp.GetRequiredService<TContext>()));

        return services;
    }
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IReadRepository<>), typeof(ReadRepository<>));
        services.AddScoped(typeof(IWriteRepository<>), typeof(WriteRepository<>));

        return services;
    }
}