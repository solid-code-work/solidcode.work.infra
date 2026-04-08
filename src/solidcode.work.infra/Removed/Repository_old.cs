using Microsoft.EntityFrameworkCore;
using solidcode.work.infra.Abstraction;
using solidcode.work.infra.Entities;
using System.Linq.Expressions;

namespace solidcode.work.infra.Repositories;

public class Repository_Old<T>
where T : class, IEntity
{
    private readonly DbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository_Old(DbContext context)
    {
        _context = context;
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

    public async Task<TResponse<List<T>>> GetAllAsync(
        Func<IQueryable<T>, IQueryable<T>> queryBuilder)
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

    public async Task<TResponse<T>> GetAsync(
        Guid id,
        Func<IQueryable<T>, IQueryable<T>>? include = null)
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

    public async Task<TResponse<T>> GetAsync(
        Expression<Func<T, bool>> filter,
        Func<IQueryable<T>, IQueryable<T>>? include = null)
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


    public async Task<TResponse> CreateAsync(T entity)
    {
        if (entity is null)
            return TResponseFactory.BadRequest("Entity cannot be null.");

        try
        {
            await _dbSet.AddAsync(entity);

            return TResponseFactory.Created("Entity added to context.");
        }
        catch (Exception ex)
        {
            return TResponseFactory.Error(ex.Message);
        }
    }

    public Task<TResponse> UpdateAsync(T entity)
    {
        if (entity is null)
            return Task.FromResult(TResponseFactory.BadRequest("Entity cannot be null."));

        if (entity.Id == Guid.Empty)
            return Task.FromResult(TResponseFactory.BadRequest("Entity Id cannot be empty."));

        try
        {
            _dbSet.Update(entity);

            return Task.FromResult(TResponseFactory.Ok("Entity marked as updated."));
        }
        catch (Exception ex)
        {
            return Task.FromResult(TResponseFactory.Error(ex.Message));
        }
    }

    public async Task<TResponse<T>> CreateAndReturnAsync(T entity)
    {
        if (entity is null)
            return TResponseFactory.BadRequest<T>("Entity cannot be null.");

        try
        {
            await _dbSet.AddAsync(entity);

            return TResponseFactory.Created(entity);
        }
        catch (Exception ex)
        {
            return TResponseFactory.Error<T>(ex.Message);
        }
    }

    public Task<TResponse<T>> UpdateAndReturnAsync(T entity)
    {
        if (entity is null)
            return Task.FromResult(TResponseFactory.BadRequest<T>("Entity cannot be null."));

        if (entity.Id == Guid.Empty)
            return Task.FromResult(TResponseFactory.BadRequest<T>("Entity Id cannot be empty."));

        try
        {
            _dbSet.Update(entity);

            return Task.FromResult(TResponseFactory.Ok(entity));
        }
        catch (Exception ex)
        {
            return Task.FromResult(TResponseFactory.Error<T>(ex.Message));
        }
    }


    public async Task<TResponse> DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
            return TResponseFactory.BadRequest("Id cannot be empty.");

        try
        {
            var entity = await _dbSet.FindAsync(id);

            if (entity is null)
                return TResponseFactory.NotFound($"Entity with Id {id} not found.");

            _dbSet.Remove(entity);

            return TResponseFactory.Ok("Entity marked for deletion.");
        }
        catch (Exception ex)
        {
            return TResponseFactory.Error(ex.Message);
        }
    }

    // ------------------------
    // QUERYABLE ACCESS
    // ------------------------

    public IQueryable<T> Query()
    {
        return _dbSet.AsNoTracking();
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.AnyAsync(predicate);
    }
}