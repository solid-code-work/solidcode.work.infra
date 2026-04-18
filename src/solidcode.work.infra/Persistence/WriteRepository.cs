using Microsoft.EntityFrameworkCore;
using solidcode.work.infra.Abstraction;
using solidcode.work.infra.Entities;

namespace solidcode.work.infra.Repositories;

public class WriteRepository<T> : IWriteRepository<T>
where T : class, IEntity
{
    private readonly DbSet<T> _dbSet;

    public WriteRepository(DbContext context)
    {
        _dbSet = context.Set<T>();
    }

    public async Task<TResponse> CreateAsync(T entity, CancellationToken ct = default)
    {
        if (entity is null)
            return TResponseFactory.BadRequest("Entity cannot be null.");

        try
        {
            await _dbSet.AddAsync(entity, ct);

            return TResponseFactory.Created("Entity added to context.");
        }
        catch (Exception ex)
        {
            return TResponseFactory.Error(ex.Message);
        }
    }

    public Task<TResponse> UpdateAsync(T entity, CancellationToken ct = default)
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

    public async Task<TResponse> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        if (id == Guid.Empty)
            return TResponseFactory.BadRequest("Id cannot be empty.");

        try
        {
            var entity = await _dbSet.FindAsync(id, ct);

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
}