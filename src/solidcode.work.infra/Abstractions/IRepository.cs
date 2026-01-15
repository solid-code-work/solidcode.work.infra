using System.Linq.Expressions;
using solidcode.work.infra.Entities;

namespace solidcode.work.infra.Abstraction;

public interface IRepository<T> where T : class, IEntity
{
    Task<TResult<List<T>>> GetAllAsync();
    Task<TResult<List<T>>> GetAllAsync(Func<IQueryable<T>, IQueryable<T>> queryBuilder);
    Task<TResult<T>> GetAsync(Guid id, Func<IQueryable<T>, IQueryable<T>>? include = null);
    Task<TResult<T>> GetAsync(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IQueryable<T>>? include = null);
    Task<TResult> CreateAsync(T entity);
    Task<TResult> UpdateAsync(T entity);
    Task<TResult<T>> CreateAndReturnAsync(T entity);
    Task<TResult<T>> UpdateAndReturnAsync(T entity);
    Task<TResult> DeleteAsync(Guid id);
    Task<TResult> SaveChangesAsync(T entity);
}


