using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Play.CommonUtils.Entities;

namespace Play.CommonUtils.Repositories;

public class EfRepository<T> : IRepository<T> where T : class, IEntity
{
    private readonly DbContext _context;
    private readonly DbSet<T> _dbSet;

    public EfRepository(DbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter)
    {
        return await _dbSet.Where(filter).ToListAsync();
    }

    public async Task<T?> GetAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty", nameof(id));

        return await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<T?> GetAsync(Expression<Func<T, bool>> filter)
    {
        return await _dbSet.FirstOrDefaultAsync(filter);
    }
    public async Task ApplyChangesAsync(T entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        if (entity is not IListEditEntities editable)
            throw new InvalidOperationException($"Entity of type {typeof(T).Name} must implement IListEditEntity to use SaveAsync.");

        if (editable.IsNew)
        {
            await _dbSet.AddAsync(entity);
        }
        else
        {
            var existing = await GetAsync(entity.Id);
            if (existing is null)
                throw new InvalidOperationException($"Entity with Id {entity.Id} not found.");

            _context.Entry(existing).CurrentValues.SetValues(entity);
        }

        await _context.SaveChangesAsync();
    }
    public async Task CreateAsync(T entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        var existing = await GetAsync(entity.Id);
        if (existing is null)
            throw new InvalidOperationException($"Entity with Id {entity.Id} not found.");

        _context.Entry(existing).CurrentValues.SetValues(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty", nameof(id));

        var entity = await GetAsync(id);
        if (entity is null)
            throw new InvalidOperationException($"Entity with Id {id} not found.");

        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }
}
