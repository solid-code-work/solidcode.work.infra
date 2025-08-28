using System.Linq.Expressions;
using Play.CommonUtils.Entities;

namespace Play.CommonUtils.Repositories
{
    public interface IRepository<T> where T : class, IEntity
    {
        // Command methods: Data is typically null, only Success/Message populated
        Task<TResult<T>> ApplyChangesAsync(T entity);
        Task<TResult<T>> DeleteAsync(Guid id);

        // Query methods: Data is populated with entities
        Task<TResult<List<T>>> GetAllAsync();
        Task<TResult<List<T>>> GetAllAsync(Expression<Func<T, bool>> filter);
        Task<TResult<T>> GetAsync(Guid id);
        Task<TResult<T>> GetAsync(Expression<Func<T, bool>> filter);
    }
}
