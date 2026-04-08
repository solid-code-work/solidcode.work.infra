using Microsoft.Extensions.DependencyInjection;
using solidcode.work.infra.Abstraction;
using solidcode.work.infra.Repositories;

public static class RepositoryServiceCollectionExtensions_Old
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IReadRepository<>), typeof(ReadRepository<>));
        services.AddScoped(typeof(IWriteRepository<>), typeof(WriteRepository<>));

        return services;
    }
}
