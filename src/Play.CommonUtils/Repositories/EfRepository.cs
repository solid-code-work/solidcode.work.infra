using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Play.CommonUtils.Entities;

namespace Play.CommonUtils.Repositories;

public class EfRepository<T> : IRepository<T> where T : class, IEntity, new()
{
    private readonly DbContext _context;
    private readonly DbSet<T> _dbSet;

    public EfRepository(DbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    // ---- QUERIES: Data is set ----
    public async Task<TResult<List<T>>> GetAllAsync()
    {
        var result = new TResult<List<T>>();
        try
        {
            result.Data = await _dbSet.ToListAsync();
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.Message = ex.Message;
        }
        return result;
    }

    public async Task<TResult<List<T>>> GetAllAsync(Expression<Func<T, bool>> filter)
    {
        var result = new TResult<List<T>>();
        try
        {
            result.Data = await _dbSet.Where(filter).ToListAsync();
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.Message = ex.Message;
        }
        return result;
    }

    public async Task<TResult<T>> GetAsync(Guid id)
    {
        var result = new TResult<T>();
        try
        {
            if (id == Guid.Empty) throw new ArgumentException("Id cannot be empty", nameof(id));

            var entity = await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
            if (entity != null)
                result.Data = entity;
            else
                result.Success = false;
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.Message = ex.Message;
        }
        return result;
    }

    public async Task<TResult<T>> GetAsync(Expression<Func<T, bool>> filter)
    {
        var result = new TResult<T>();
        try
        {
            var entity = await _dbSet.FirstOrDefaultAsync(filter);
            if (entity != null)
                result.Data = entity;
            else
                result.Success = false;
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.Message = ex.Message;
        }
        return result;
    }

    // ---- COMMANDS: Data left unset ----
    public async Task<TResult<T>> ApplyChangesAsync(T entity)
    {
        var result = new TResult<T>();
        try
        {
            if (entity is null) throw new ArgumentNullException(nameof(entity));

            if (entity is not IListEditEntities editable)
                throw new InvalidOperationException(
                    $"Entity of type {typeof(T).Name} must implement IListEditEntity to use ApplyChangesAsync."
                );

            if (editable.IsNew)
            {
                await _dbSet.AddAsync(entity);
            }
            else
            {
                var existing = await _dbSet.FirstOrDefaultAsync(x => x.Id == entity.Id);
                if (existing is null)
                    throw new InvalidOperationException($"Entity with Id {entity.Id} not found.");

                _context.Entry(existing).CurrentValues.SetValues(entity);
            }

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.Message = ex.Message;
        }
        return result;
    }

    public async Task<TResult<T>> DeleteAsync(Guid id)
    {
        var result = new TResult<T>();
        try
        {
            if (id == Guid.Empty) throw new ArgumentException("Id cannot be empty", nameof(id));

            var entity = await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
            if (entity is null)
                throw new InvalidOperationException($"Entity with Id {id} not found.");

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.Message = ex.Message;
        }
        return result;
    }
}
