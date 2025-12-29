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

    // ------------------------
    // QUERIES
    // ------------------------

    public async Task<TResult<List<T>>> GetAllAsync()
    {
        try
        {
            var data = await _dbSet
                .AsNoTracking()
                .ToListAsync();

            return data.Count == 0
                ? TResultFactory.NoContent<List<T>>("No entities found.")
                : TResultFactory.Ok(data, "Entities retrieved successfully.");
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
            var data = await _dbSet
                .AsNoTracking()
                .Where(filter)
                .ToListAsync();

            return data.Count == 0
                ? TResultFactory.NoContent<List<T>>("No matching entities found.")
                : TResultFactory.Ok(data, "Filtered entities retrieved successfully.");
        }
        catch (Exception ex)
        {
            return TResultFactory.Error<List<T>>(ex.Message);
        }
    }

    public async Task<TResult<T>> GetAsync(Guid id)
    {
        if (id == Guid.Empty)
            return TResultFactory.BadRequest<T>("Id cannot be empty.");

        try
        {
            var entity = await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return entity is null
                ? TResultFactory.NotFound<T>($"Entity with Id {id} not found.")
                : TResultFactory.Ok(entity, "Entity found.");
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
            var entity = await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(filter);

            return entity is null
                ? TResultFactory.NotFound<T>("No matching entity found.")
                : TResultFactory.Ok(entity, "Entity found.");
        }
        catch (Exception ex)
        {
            return TResultFactory.Error<T>(ex.Message);
        }
    }

    // ------------------------
    // COMMANDS
    // ------------------------

    public async Task<TResult> CreateAsync(T entity)
    {
        if (entity is null)
            return TResultFactory.BadRequest("Entity cannot be null.");

        try
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();

            return TResultFactory.Created("Entity created successfully.");
        }
        catch (Exception ex)
        {
            return TResultFactory.Error(ex.Message);
        }
    }

    public async Task<TResult> UpdateAsync(T entity)
    {
        if (entity is null)
            return TResultFactory.BadRequest("Entity cannot be null.");

        if (entity.Id == Guid.Empty)
            return TResultFactory.BadRequest("Entity Id cannot be empty.");

        try
        {
            var existing = await _dbSet.FirstOrDefaultAsync(x => x.Id == entity.Id);

            if (existing is null)
                return TResultFactory.NotFound($"Entity with Id {entity.Id} not found.");

            _context.Entry(existing).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();

            return TResultFactory.Ok("Entity updated successfully.");
        }
        catch (Exception ex)
        {
            return TResultFactory.Error(ex.Message);
        }
    }

    public async Task<TResult> DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
            return TResultFactory.BadRequest("Id cannot be empty.");

        try
        {
            var entity = await _dbSet.FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
                return TResultFactory.NotFound($"Entity with Id {id} not found.");

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();

            return TResultFactory.Ok("Entity deleted successfully.");
        }
        catch (Exception ex)
        {
            return TResultFactory.Error(ex.Message);
        }
    }
}
