using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using solidcode.work.infra.Entities;
using solidcode.work.infra.Repositories;
using solidcode.work.infra.Configurations;

namespace solidcode.work.infra.PostgreSQL;

public static class Extension
{
    public static IServiceCollection AddPostgresDbContext<TContext>(
    this IServiceCollection services)
    where TContext : DbContext
    {
        services.AddDbContext<TContext>((serviceProvider, options) =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var postgresSettings = configuration.GetSection(nameof(PostgreSQLSettings)).Get<PostgreSQLSettings>();

            if (string.IsNullOrWhiteSpace(postgresSettings?.ConnectionString))
                throw new ArgumentException("PostgreSQL connection string is missing or invalid.");

            options.UseNpgsql(postgresSettings.ConnectionString);
        });

        return services;
    }


}