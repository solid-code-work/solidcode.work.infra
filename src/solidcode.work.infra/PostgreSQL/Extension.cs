using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using solidcode.work.infra.Configurations;

namespace solidcode.work.infra;

public static class PostgreSQLExtension
{
    public static IServiceCollection AddPostgresDbContext<TContext>(
    this IServiceCollection services, IConfiguration configuration)
    where TContext : DbContext
    {
        services.AddDbContext<TContext>((serviceProvider, options) =>
        {
            var postgresSettings = configuration.GetSection(nameof(PostgreSQLSettings)).Get<PostgreSQLSettings>();

            if (string.IsNullOrWhiteSpace(postgresSettings?.ConnectionString))
                throw new ArgumentException("PostgreSQL connection string is missing or invalid.");

            options.UseNpgsql(postgresSettings.ConnectionString);
        });

        return services;
    }


}