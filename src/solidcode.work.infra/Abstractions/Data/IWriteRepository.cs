using System.Linq.Expressions;
using solidcode.work.infra.Entities;

namespace solidcode.work.infra.Abstraction;

public interface IWriteRepository<T> where T : class, IEntity
{
    Task<TResponse> CreateAsync(T entity);
    Task<TResponse> UpdateAsync(T entity);
    // Task<TResponse<T>> CreateAndReturnAsync(T entity);
    // Task<TResponse<T>> UpdateAndReturnAsync(T entity);
    Task<TResponse> DeleteAsync(Guid id);
}


