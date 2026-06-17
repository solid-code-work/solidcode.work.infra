using Solidcode.Work.Infra.Entities;
using Solidcode.Work.Infra.Abstractions;

namespace Solidcode.Work.Infra.Abstractions;

public interface IWriteRepository<T> where T : class, IEntity
{
    Task<TResponse> CreateAsync(T entity, CancellationToken ct = default);
    Task<TResponse> UpdateAsync(T entity, CancellationToken ct = default);
    Task<TResponse> DeleteAsync(Guid id, CancellationToken ct = default);
}


