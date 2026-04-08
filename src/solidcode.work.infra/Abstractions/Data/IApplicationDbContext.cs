using solidcode.work.infra.Entities;

namespace solidcode.work.infra.Abstraction;

public interface IApplicationDbContext
{
    Task<TResponse<int>> SaveChangesAsync(CancellationToken cancellationToken = default);
}