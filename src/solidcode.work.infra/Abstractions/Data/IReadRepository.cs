using System.Linq.Expressions;
using solidcode.work.infra.Entities;

namespace solidcode.work.infra.Abstraction;

public interface IReadRepository<T> where T : class, IEntity
{
    Task<TResponse<List<T>>> GetAllAsync();
    Task<TResponse<List<T>>> GetAllAsync(Func<IQueryable<T>, IQueryable<T>> queryBuilder);
    Task<TResponse<T>> GetAsync(Guid id, Func<IQueryable<T>, IQueryable<T>>? include = null);
    Task<TResponse<T>> GetAsync(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IQueryable<T>>? include = null);
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    IQueryable<T> Query();
}


