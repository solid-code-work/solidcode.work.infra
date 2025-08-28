using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Play.CommonUtils.Settings;

namespace Play.CommonUtils.EF;

public static class Extension
{
    public static IServiceCollection AddEFDbContext<TContext>(
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