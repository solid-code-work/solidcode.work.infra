using Microsoft.EntityFrameworkCore;
using solidcode.work.infra.Abstraction;
using solidcode.work.infra.Entities;
using System.Linq.Expressions;

namespace solidcode.work.infra.Repositories;

public class Repository<T> : IRepository<T>
where T : class, IEntity
{
    protected readonly DbContext _db;
    protected readonly DbSet<T> _dbSet;

    public Repository(DbContext db)
    {
        _db = db;
        _dbSet = db.Set<T>();
    }

    // ----------------------------
    // GET (Tracked - for commands)
    // ----------------------------
    public async Task<TResult<T>> GetAsync(
        Guid id,
        Func<IQueryable<T>, IQueryable<T>>? include = null)
    {
        try
        {
            IQueryable<T> query = _dbSet;

            if (include != null)
                query = include(query);

            var entity = await query.FirstOrDefaultAsync(x => x.Id == id);

            return entity is null
                ? TResultFactory.NotFound<T>($"Entity with id {id} not found.")
                : TResultFactory.Ok(entity);
        }
        catch (Exception ex)
        {
            return TResultFactory.Error<T>(ex.Message);
        }
    }

    // ----------------------------
    // GET (Tracked - filter)
    // ----------------------------
    public async Task<TResult<T>> GetAsync(
        Expression<Func<T, bool>> filter,
        Func<IQueryable<T>, IQueryable<T>>? include = null)
    {
        try
        {
            IQueryable<T> query = _dbSet;

            if (include != null)
                query = include(query);

            var entity = await query.FirstOrDefaultAsync(filter);

            return entity is null
                ? TResultFactory.NotFound<T>("No matching entity found.")
                : TResultFactory.Ok(entity);
        }
        catch (Exception ex)
        {
            return TResultFactory.Error<T>(ex.Message);
        }
    }

    // ----------------------------
    // READ-ONLY QUERIES
    // ----------------------------
    public async Task<TResult<List<T>>> GetAllAsync()
    {
        try
        {
            var list = await _dbSet
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
    Func<IQueryable<T>, IQueryable<T>> queryBuilder)
    {
        try
        {
            IQueryable<T> query = _dbSet.AsNoTracking();

            query = queryBuilder(query);

            var list = await query.ToListAsync();

            return TResultFactory.Ok(list);
        }
        catch (Exception ex)
        {
            return TResultFactory.Error<List<T>>(ex.Message);
        }
    }


    // ----------------------------
    // CREATE
    // ----------------------------
    public async Task<TResult> CreateAsync(T entity)
    {
        try
        {
            _dbSet.Add(entity);
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
            _dbSet.Add(entity);
            await _db.SaveChangesAsync();
            return TResultFactory.Ok(entity);
        }
        catch (Exception ex)
        {
            return TResultFactory.Error<T>(ex.Message);
        }
    }

    // ----------------------------
    // UPDATE (Tracked)
    // ----------------------------
    public async Task<TResult> UpdateAsync(T entity)
    {
        try
        {
            _dbSet.Update(entity);
            await _db.SaveChangesAsync();
            return TResultFactory.Ok();
        }
        catch (Exception ex)
        {
            return TResultFactory.Error(ex.Message);
        }
    }

    public async Task<TResult<T>> UpdateAndReturnAsync(T entity)
    {
        try
        {
            await _db.SaveChangesAsync();
            return TResultFactory.Ok(entity);
        }
        catch (Exception ex)
        {
            return TResultFactory.Error<T>(ex.Message);
        }
    }

    // ----------------------------
    // DELETE
    // ----------------------------
    public async Task<TResult> DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
            return TResultFactory.BadRequest("Id cannot be empty.");

        try
        {
            var entity = await _dbSet.FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
                return TResultFactory.NotFound(
                    $"Entity with Id {id} not found.");

            _dbSet.Remove(entity);
            await _db.SaveChangesAsync();

            return TResultFactory.Ok("Entity deleted successfully.");
        }
        catch (Exception ex)
        {
            return TResultFactory.Error(ex.Message);
        }
    }

    public async Task<TResult> SaveChangesAsync(T entity)
    {
        try
        {
            foreach (var e in _db.ChangeTracker.Entries())
            {
                Console.WriteLine($"{e.Entity.GetType().Name} => {e.State}");
            }
            await _db.SaveChangesAsync();
            return TResultFactory.Ok();
        }
        catch (Exception ex)
        {
            return TResultFactory.Error(ex.Message);
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
