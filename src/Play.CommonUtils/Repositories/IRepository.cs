using System.Linq.Expressions;
using Play.CommonUtils.Entities;

namespace Play.CommonUtils.Repositories;

public interface IRepository<T> where T : IEntity
{
    Task<TResult<T>> ApplyChangesAsync(T entity);
    Task<TResult<T>> DeleteAsync(Guid id);
    Task<TResult<List<T>>> GetAllAsync();
    Task<TResult<List<T>>> GetAllAsync(Expression<Func<T, bool>> filter);
    Task<TResult<T>> GetAsync(Guid id);
    Task<TResult<T>> GetAsync(Expression<Func<T, bool>> filter);
}
