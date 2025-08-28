using System.Linq.Expressions;
using Play.CommonUtils.Entities;

namespace Play.CommonUtils.Repositories;

public interface IRepository<T> where T : IEntity
{
    Task CreateAsync(T entity);
    Task DeleteAsync(Guid id);
    Task<List<T>> GetAllAsync();
    Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter);
    Task<T?> GetAsync(Guid Id);
    Task<T?> GetAsync(Expression<Func<T, bool>> filter);
    Task UpdateAsync(T entity);
}
