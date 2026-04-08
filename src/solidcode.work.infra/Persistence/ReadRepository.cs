using Microsoft.EntityFrameworkCore;
using solidcode.work.infra.Abstraction;
using solidcode.work.infra.Entities;
using System.Linq.Expressions;

namespace solidcode.work.infra.Repositories;

public class ReadRepository<T> : IReadRepository<T>
where T : class, IEntity
{
    private readonly DbSet<T> _dbSet;
    public ReadRepository(DbContext context)
    {
        _dbSet = context.Set<T>();
    }

    public async Task<TResponse<List<T>>> GetAllAsync()
    {
        try
        {
            var data = await _dbSet
                .AsNoTracking()
                .ToListAsync();

            return data.Count == 0
                ? TResponseFactory.NoContent<List<T>>("No entities found.")
                : TResponseFactory.Ok(data, "Entities retrieved successfully.");
        }
        catch (Exception ex)
        {
            return TResponseFactory.Error<List<T>>(ex.Message);
        }
    }

    public async Task<TResponse<List<T>>> GetAllAsync(Func<IQueryable<T>, IQueryable<T>> queryBuilder)
    {
        try
        {
            IQueryable<T> query = _dbSet.AsNoTracking();

            query = queryBuilder(query);

            var list = await query.ToListAsync();

            return list.Count == 0
                ? TResponseFactory.NoContent<List<T>>("No entities found.")
                : TResponseFactory.Ok(list);
        }
        catch (Exception ex)
        {
            return TResponseFactory.Error<List<T>>(ex.Message);
        }
    }

    public async Task<TResponse<T>> GetAsync(Guid id, Func<IQueryable<T>, IQueryable<T>>? include = null)
    {
        if (id == Guid.Empty)
            return TResponseFactory.BadRequest<T>("Id cannot be empty.");

        try
        {
            IQueryable<T> query = _dbSet.AsQueryable();

            if (include != null)
                query = include(query);

            var entity = await query.FirstOrDefaultAsync(x => x.Id == id);

            return entity is null
                ? TResponseFactory.NotFound<T>($"Entity with Id {id} not found.")
                : TResponseFactory.Ok(entity);
        }
        catch (Exception ex)
        {
            return TResponseFactory.Error<T>(ex.Message);
        }
    }

    public async Task<TResponse<T>> GetAsync(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IQueryable<T>>? include = null)
    {
        try
        {
            IQueryable<T> query = _dbSet.AsQueryable();

            if (include != null)
                query = include(query);

            var entity = await query.FirstOrDefaultAsync(filter);

            return entity is null
                ? TResponseFactory.NotFound<T>("No matching entity found.")
                : TResponseFactory.Ok(entity);
        }
        catch (Exception ex)
        {
            return TResponseFactory.Error<T>(ex.Message);
        }
    }


    public IQueryable<T> Query()
    {
        return _dbSet.AsNoTracking();
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.AnyAsync(predicate);
    }
}