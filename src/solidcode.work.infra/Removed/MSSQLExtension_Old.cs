using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using solidcode.work.infra.Persistence;
using solidcode.work.infra.Configurations;
using solidcode.work.infra.Abstraction;

namespace solidcode.work.infra;

public static class MSSQLExtension_Old
{
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
}
