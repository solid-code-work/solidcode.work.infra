using System.Linq.Expressions;
using solidcode.work.infra.Entities;


public interface IReadRepository<T> where T : class, IEntity
{
    Task<TResponse<List<T>>> GetAllAsync(CancellationToken ct = default);
    Task<TResponse<List<T>>> GetAllAsync(Func<IQueryable<T>, IQueryable<T>> queryBuilder, CancellationToken ct = default);
    Task<TResponse<T>> GetAsync(Guid id, Func<IQueryable<T>, IQueryable<T>>? include = null, CancellationToken ct = default);
    Task<TResponse<T>> GetAsync(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IQueryable<T>>? include = null, CancellationToken ct = default);
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
    IQueryable<T> Query();
}


