using System.Linq.Expressions;
using solidcode.work.infra.Entities;

namespace solidcode.work.infra.Repositories;

public interface IRepository<T> where T : class, IEntity
{
    // ------------------------
    // QUERIES
    // ------------------------

    Task<TResult<List<T>>> GetAllAsync();
    Task<TResult<List<T>>> GetAllAsync(Expression<Func<T, bool>> filter);
    Task<TResult<T>> GetAsync(Guid id);
    Task<TResult<T>> GetAsync(Expression<Func<T, bool>> filter);

    // ------------------------
    // COMMANDS
    // ------------------------

    Task<TResult> CreateAsync(T entity);
    Task<TResult> UpdateAsync(T entity);
    Task<TResult> DeleteAsync(Guid id);
}
