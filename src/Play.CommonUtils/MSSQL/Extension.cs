using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Play.CommonUtils.Entities;
using Play.CommonUtils.Repositories;
using Play.CommonUtils.Configurations;

namespace Play.CommonUtils.MSSQL;

public static class Extension
{
    public static IServiceCollection AddSqlServerDbContext<TContext>(
    this IServiceCollection services)
    where TContext : DbContext
    {
        services.AddDbContext<TContext>((serviceProvider, options) =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var sqlSettings = configuration.GetSection(nameof(MSSQLsettings)).Get<MSSQLsettings>();

            if (string.IsNullOrWhiteSpace(sqlSettings?.ConnectionString))
                throw new ArgumentException("SQL Server connection string is missing or invalid.");

            options.UseSqlServer(sqlSettings.ConnectionString);
        });

        return services;
    }


    public static IServiceCollection AddEfRepository<T>(this IServiceCollection services)
    where T : class, IEntity
    {
        services.AddScoped<IRepository<T>>(sp =>
        {
            var dbContext = sp.GetRequiredService<DbContext>();
            return new EfRepository<T>(dbContext);
        });

        return services;
    }

}