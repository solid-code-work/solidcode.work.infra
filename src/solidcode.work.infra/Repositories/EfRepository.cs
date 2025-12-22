using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using solidcode.work.infra.Entities;

namespace solidcode.work.infra.Repositories;

public class EfRepository<T> : IRepository<T> where T : class, IEntity
{
    private readonly DbContext _context;
    private readonly DbSet<T> _dbSet;

    public EfRepository(DbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<TResult<List<T>>> GetAllAsync()
    {
        try
        {
            var data = await _dbSet.ToListAsync();

            if (data.Count == 0)
                return TResultFactory.Empty<List<T>>("No entities found.");

            return TResultFactory.Ok(data, "Retrieved all entities.");
        }
        catch (Exception ex)
        {
            return TResultFactory.Error<List<T>>(ex.Message);
        }
    }

    public async Task<TResult<List<T>>> GetAllAsync(Expression<Func<T, bool>> filter)
    {
        try
        {
            var data = await _dbSet.Where(filter).ToListAsync();

            if (data.Count == 0)
                return TResultFactory.Empty<List<T>>("No matching entities found.");

            return TResultFactory.Ok(data, "Filtered entities retrieved.");
        }
        catch (Exception ex)
        {
            return TResultFactory.Error<List<T>>(ex.Message);
        }
    }

    public async Task<TResult<T>> GetAsync(Guid id)
    {
        try
        {
            if (id == Guid.Empty)
                return TResultFactory.BadRequest<T>("Id cannot be empty.");

            var entity = await _dbSet.FirstOrDefaultAsync(x => x.Id == id);

            if (entity != null)
                return TResultFactory.Ok(entity, "Entity found.");
            else
                return TResultFactory.NotFound<T>($"Entity with Id {id} not found.");
        }
        catch (Exception ex)
        {
            return TResultFactory.Error<T>(ex.Message);
        }
    }

    public async Task<TResult<T>> GetAsync(Expression<Func<T, bool>> filter)
    {
        try
        {
            var entity = await _dbSet.FirstOrDefaultAsync(filter);

            if (entity != null)
                return TResultFactory.Ok(entity, "Entity found.");
            else
                return TResultFactory.NotFound<T>("No matching entity found.");
        }
        catch (Exception ex)
        {
            return TResultFactory.Error<T>(ex.Message);
        }
    }

    // ---- COMMANDS: Data is left unset ----
    public async Task<TResult<T>> ApplyChangesAsync(T entity)
    {
        try
        {
            if (entity is null)
                return TResultFactory.BadRequest<T>("Entity cannot be null.");

            if (entity is not IListEditEntity editable)
                return TResultFactory.BadRequest<T>($"Entity of type {typeof(T).Name} must implement IListEditEntity to use ApplyChangesAsync.");

            if (editable.IsNew)
            {
                await _dbSet.AddAsync(entity);
            }
            else
            {
                var existing = await _dbSet.FirstOrDefaultAsync(x => x.Id == entity.Id);
                if (existing is null)
                    return TResultFactory.NotFound<T>($"Entity with Id {entity.Id} not found.");

                _context.Entry(existing).CurrentValues.SetValues(entity);
            }

            await _context.SaveChangesAsync();

            return TResultFactory.Ok(entity, "Changes saved successfully.");
        }
        catch (Exception ex)
        {
            return TResultFactory.Error<T>(ex.Message);
        }
    }

    public async Task<TResult<T>> DeleteAsync(Guid id)
    {
        try
        {
            if (id == Guid.Empty)
                return TResultFactory.BadRequest<T>("Id cannot be empty.");

            var entity = await _dbSet.FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
                return TResultFactory.NotFound<T>($"Entity with Id {id} not found.");

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();

            return TResultFactory.Ok(entity, "Entity deleted successfully.");
        }
        catch (Exception ex)
        {
            return TResultFactory.Error<T>(ex.Message);
        }
    }
}
