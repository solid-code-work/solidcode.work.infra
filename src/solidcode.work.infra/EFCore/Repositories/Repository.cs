using Microsoft.EntityFrameworkCore;
using solidcode.work.infra.Abstraction;
using solidcode.work.infra.Entities;
using System.Linq.Expressions;

namespace solidcode.work.infra.Repositories;

public class Repository<T> : IRepository<T>
    where T : class, IEntity
{
    protected readonly DbContext _db;
    protected readonly DbSet<T> _set;

    public Repository(DbContext db)
    {
        _db = db;
        _set = db.Set<T>();
    }

    public async Task<TResult<T>> GetAsync(Expression<Func<T, bool>> filter,
    Func<IQueryable<T>, IQueryable<T>>? include = null)
    {
        try
        {
            var entity = await _set
                .AsNoTracking()
                .FirstOrDefaultAsync(filter);

            return entity is null
                ? TResultFactory.NotFound<T>("No matching entity found.")
                : TResultFactory.Ok(entity);
        }
        catch (Exception ex)
        {
            return TResultFactory.Error<T>(ex.Message);
        }
    }

    public async Task<TResult<T>> GetAsync(Guid id, Func<IQueryable<T>, IQueryable<T>>? include = null)
    {
        try
        {
            var entity = await _set.FindAsync(id);

            return entity is null
                ? TResultFactory.NotFound<T>($"Entity with id {id} not found.")
                : TResultFactory.Ok(entity);
        }
        catch (Exception ex)
        {
            return TResultFactory.Error<T>(ex.Message);
        }
    }

    public async Task<TResult<List<T>>> GetAllAsync()
    {
        try
        {
            var list = await _set
                .AsNoTracking()
                .ToListAsync();

            return TResultFactory.Ok(list);
        }
        catch (Exception ex)
        {
            return TResultFactory.Error<List<T>>(ex.Message);
        }
    }

    public async Task<TResult<List<T>>> GetAllAsync(
        Expression<Func<T, bool>> predicate)
    {
        try
        {
            var list = await _set
                .AsNoTracking()
                .Where(predicate)
                .ToListAsync();

            return TResultFactory.Ok(list);
        }
        catch (Exception ex)
        {
            return TResultFactory.Error<List<T>>(ex.Message);
        }
    }

    public async Task<TResult> CreateAsync(T entity)
    {
        try
        {
            _set.Add(entity);
            await _db.SaveChangesAsync();
            return TResultFactory.Ok();
        }
        catch (Exception ex)
        {
            return TResultFactory.Error(ex.Message);
        }
    }

    public async Task<TResult> UpdateAsync(T entity)
    {
        try
        {
            _set.Update(entity);
            await _db.SaveChangesAsync();
            return TResultFactory.Ok();
        }
        catch (Exception ex)
        {
            return TResultFactory.Error(ex.Message);
        }
    }
    public async Task<TResult<T>> CreateAndReturnAsync(T entity)
    {
        try
        {
            _set.Add(entity);
            await _db.SaveChangesAsync();
            return TResultFactory.Ok(entity);
        }
        catch (Exception ex)
        {
            return TResultFactory.Error<T>(ex.Message);
        }
    }

    public async Task<TResult<T>> UpdateAndReturnAsync(T entity)
    {
        try
        {
            _set.Update(entity);
            await _db.SaveChangesAsync();
            return TResultFactory.Ok(entity);
        }
        catch (Exception ex)
        {
            return TResultFactory.Error<T>(ex.Message);
        }
    }

    public async Task<TResult> DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
            return TResultFactory.BadRequest("Id cannot be empty.");

        try
        {
            var entity = await _set.FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
                return TResultFactory.NotFound(
                    $"Entity with Id {id} not found.");

            _set.Remove(entity);
            await _db.SaveChangesAsync();

            return TResultFactory.Ok("Entity deleted successfully.");
        }
        catch (Exception ex)
        {
            return TResultFactory.Error(ex.Message);
        }
    }
}
