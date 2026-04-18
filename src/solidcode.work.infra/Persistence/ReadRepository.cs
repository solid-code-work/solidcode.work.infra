using Microsoft.EntityFrameworkCore;
using solidcode.work.infra.Entities;
using System.Linq.Expressions;



public class ReadRepository<T> : IReadRepository<T>
where T : class, IEntity
{
    private readonly DbSet<T> _dbSet;
    public ReadRepository(DbContext context)
    {
        _dbSet = context.Set<T>();
    }

    public async Task<TResponse<List<T>>> GetAllAsync(CancellationToken ct = default)
    {
        try
        {
            var data = await _dbSet
                .AsNoTracking()
                .ToListAsync(ct);

            return data.Count == 0
                ? TResponseFactory.NoContent<List<T>>("No entities found.")
                : TResponseFactory.Ok(data, "Entities retrieved successfully.");
        }
        catch (Exception ex)
        {
            return TResponseFactory.Error<List<T>>(ex.Message);
        }
    }

    public async Task<TResponse<List<T>>> GetAllAsync(Func<IQueryable<T>, IQueryable<T>> queryBuilder, CancellationToken ct = default)
    {
        try
        {
            IQueryable<T> query = _dbSet.AsNoTracking();

            query = queryBuilder(query);

            var list = await query.ToListAsync(ct);

            return list.Count == 0
                ? TResponseFactory.NoContent<List<T>>("No entities found.")
                : TResponseFactory.Ok(list);
        }
        catch (Exception ex)
        {
            return TResponseFactory.Error<List<T>>(ex.Message);
        }
    }

    public async Task<TResponse<T>> GetAsync(Guid id, Func<IQueryable<T>, IQueryable<T>>? include = null, CancellationToken ct = default)
    {
        if (id == Guid.Empty)
            return TResponseFactory.BadRequest<T>("Id cannot be empty.");

        try
        {
            IQueryable<T> query = _dbSet.AsQueryable();

            if (include != null)
                query = include(query);

            var entity = await query.FirstOrDefaultAsync(x => x.Id == id, ct);

            return entity is null
                ? TResponseFactory.NotFound<T>($"Entity with Id {id} not found.")
                : TResponseFactory.Ok(entity);
        }
        catch (Exception ex)
        {
            return TResponseFactory.Error<T>(ex.Message);
        }
    }

    public async Task<TResponse<T>> GetAsync(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IQueryable<T>>? include = null, CancellationToken ct = default)
    {
        try
        {
            IQueryable<T> query = _dbSet.AsQueryable();

            if (include != null)
                query = include(query);

            var entity = await query.FirstOrDefaultAsync(filter, ct);

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

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
    {
        return await _dbSet.AnyAsync(predicate, ct);
    }
}