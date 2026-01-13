using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using solidcode.work.infra.Entities;
using solidcode.work.infra.Repositories;
using solidcode.work.infra.Configurations;
using solidcode.work.infra.Abstraction;

namespace solidcode.work.infra;

public static class MSSQLExtension
{
    public static IServiceCollection AddSqlServerDbContext<TContext>(
    this IServiceCollection services)
    where TContext : DbContext
    {
        services.AddDbContext<TContext>((serviceProvider, options) =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var sqlSettings = configuration
                .GetSection(nameof(MSSQLsettings))
                .Get<MSSQLsettings>();

            if (string.IsNullOrWhiteSpace(sqlSettings?.ConnectionString))
                throw new ArgumentException("SQL Server connection string is missing or invalid.");

            options.UseSqlServer(sqlSettings.ConnectionString);
        });

        // 🔴 THIS IS THE IMPORTANT LINE
        services.AddScoped<DbContext>(sp => sp.GetRequiredService<TContext>());

        return services;
    }


    public static IServiceCollection AddEfRepository<T, TContext>(this IServiceCollection services)
        where T : class, IEntity
        where TContext : DbContext
    {
        services.AddScoped<IRepository<T>>(sp =>
        {
            var dbContext = sp.GetRequiredService<TContext>();
            return new EfRepository<T>(dbContext);
        });

        return services;
    }
}
