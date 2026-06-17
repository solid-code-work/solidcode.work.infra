using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Solidcode.Work.Infra.Abstractions;
using Solidcode.Work.Infra.Configurations;
using Solidcode.Work.Infra.Persistence;
using Solidcode.Work.Infra.Repositories;
using Solidcode.Work.Infra.Services;

namespace Solidcode.Work.Infra.DependencyInjection;

public static class EFCoreExtensions
{
    public static IServiceCollection AddPostgreDbContext<TContext>(
        this IServiceCollection services)
        where TContext : DbContext
    {
        services.AddAuditInfrastructure();

        services.AddDbContext<TContext>((serviceProvider, options) =>
        {
            var configuration =
                serviceProvider.GetRequiredService<IConfiguration>();

            var postgresSettings = configuration
                .GetSection(nameof(PostgreSQLSetting))
                .Get<PostgreSQLSetting>();

            if (string.IsNullOrWhiteSpace(postgresSettings?.ConnectionString))
            {
                throw new ArgumentException(
                    "PostgreSQL connection string is missing or invalid.");
            }

            options.UseNpgsql(
                postgresSettings.ConnectionString,
                npgsqlOptions =>
                {
                    npgsqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorCodesToAdd: null);
                });

            options.AddInterceptors(
                serviceProvider.GetRequiredService<AuditSaveChangesInterceptor>());
        });

        services.AddScoped<DbContext>(
            sp => sp.GetRequiredService<TContext>());

        services.AddScoped<IApplicationDbContext>(
            sp => new ApplicationDbContext(
                sp.GetRequiredService<TContext>()));
        services.AddScoped<IBusinessNumberGenerator, BusinessNumberGenerator>();
        return services;
    }

    public static IServiceCollection AddSqlServerDbContext<TContext>(
        this IServiceCollection services)
        where TContext : DbContext
    {
        services.AddAuditInfrastructure();

        services.AddDbContext<TContext>((serviceProvider, options) =>
        {
            var configuration =
                serviceProvider.GetRequiredService<IConfiguration>();

            var sqlSettings = configuration
                .GetSection(nameof(MSSQLsetting))
                .Get<MSSQLsetting>();

            if (string.IsNullOrWhiteSpace(sqlSettings?.ConnectionString))
            {
                throw new ArgumentException(
                    "SQL Server connection string is missing or invalid.");
            }

            options.UseSqlServer(
                sqlSettings.ConnectionString,
                sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                });

            options.AddInterceptors(
                serviceProvider.GetRequiredService<AuditSaveChangesInterceptor>());

        });
        services.AddScoped<IBusinessNumberGenerator, BusinessNumberGenerator>();
        services.AddScoped<DbContext>(
            sp => sp.GetRequiredService<TContext>());

        services.AddScoped<IApplicationDbContext>(
            sp => new ApplicationDbContext(
                sp.GetRequiredService<TContext>()));

        return services;
    }

    public static IServiceCollection AddRepositories(
        this IServiceCollection services)
    {
        services.AddScoped(typeof(IReadRepository<>), typeof(ReadRepository<>));
        services.AddScoped(typeof(IWriteRepository<>), typeof(WriteRepository<>));

        return services;
    }

    private static IServiceCollection AddAuditInfrastructure(
        this IServiceCollection services)
    {
        services.AddScoped<AuditSaveChangesInterceptor>();

        return services;
    }
}