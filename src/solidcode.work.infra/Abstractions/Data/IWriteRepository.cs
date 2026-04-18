using System.Linq.Expressions;
using solidcode.work.infra.Entities;

namespace solidcode.work.infra.Abstraction;

public interface IWriteRepository<T> where T : class, IEntity
{
    Task<TResponse> CreateAsync(T entity, CancellationToken ct = default);
    Task<TResponse> UpdateAsync(T entity, CancellationToken ct = default);
    Task<TResponse> DeleteAsync(Guid id, CancellationToken ct = default);
}


