using Solidcode.Work.Infra.Entities;

namespace Solidcode.Work.Infra.Abstractions;

public interface IApplicationDbContext
{
    Task<TResponse<int>> SaveChangesAsync(CancellationToken cancellationToken = default);
}