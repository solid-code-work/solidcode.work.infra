using System.Linq.Expressions;
using MongoDB.Driver;
using solidcode.work.infra.Abstraction;
using solidcode.work.infra.Entities;

namespace solidcode.work.infra;

public interface IMongoRepository<T> where T : class, IEntity
{
    Task<TResult<T>> CreateAndReturnAsync(T entity);
    Task<TResult> CreateAsync(T entity);
    Task<TResult> DeleteAsync(Guid id);
    Task<TResult<List<T>>> GetAllAsync();
    Task<TResult<List<T>>> GetAllAsync(Expression<Func<T, bool>> filter);
    Task<TResult<T>> GetAsync(Guid id, Func<IQueryable<T>, IQueryable<T>>? include = null);
    Task<TResult<T>> GetAsync(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IQueryable<T>>? include = null);
    Task<TResult> SaveChangesAsync(T entity);
    Task<TResult<T>> UpdateAndReturnAsync(T entity);
    Task<TResult> UpdateAsync(T entity);
}

public class MongoRepository<T> : IMongoRepository<T>
where T : class, IEntity
{
    private readonly IMongoCollection<T> _collection;
    private readonly FilterDefinitionBuilder<T> _filter = Builders<T>.Filter;

    public MongoRepository(IMongoDatabase database, string collectionName)
    {
        _collection = database.GetCollection<T>(collectionName);
    }

    // ------------------------
    // QUERIES
    // ------------------------

    public async Task<TResult<List<T>>> GetAllAsync()
    {
        try
        {
            var data = await _collection
                .Find(_filter.Empty)
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
            var data = await _collection
                .Find(filter)
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

    public async Task<TResult<T>> GetAsync(Guid id, Func<IQueryable<T>, IQueryable<T>>? include = null)
    {
        if (id == Guid.Empty)
            return TResultFactory.BadRequest<T>("Id cannot be empty.");

        try
        {
            var entity = await _collection
                .Find(_filter.Eq(x => x.Id, id))
                .FirstOrDefaultAsync();

            return entity is null
                ? TResultFactory.NotFound<T>($"Entity with Id {id} not found.")
                : TResultFactory.Ok(entity, "Entity found.");
        }
        catch (Exception ex)
        {
            return TResultFactory.Error<T>(ex.Message);
        }
    }

    public async Task<TResult<T>> GetAsync(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IQueryable<T>>? include = null)
    {
        try
        {
            var entity = await _collection
                .Find(filter)
                .FirstOrDefaultAsync();

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
            await _collection.InsertOneAsync(entity);
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
            var result = await _collection.ReplaceOneAsync(
                _filter.Eq(x => x.Id, entity.Id),
                entity);

            return result.MatchedCount == 0
                ? TResultFactory.NotFound($"Entity with Id {entity.Id} not found.")
                : TResultFactory.Ok("Entity updated successfully.");
        }
        catch (Exception ex)
        {
            return TResultFactory.Error(ex.Message);
        }
    }
    public async Task<TResult<T>> CreateAndReturnAsync(T entity)
    {
        if (entity is null)
            return TResultFactory.BadRequest<T>("Entity cannot be null.");

        try
        {
            await _collection.InsertOneAsync(entity);
            return TResultFactory.Created(entity, "Entity created successfully.");
        }
        catch (Exception ex)
        {
            return TResultFactory.Error<T>(ex.Message);
        }
    }

    public async Task<TResult<T>> UpdateAndReturnAsync(T entity)
    {
        if (entity is null)
            return TResultFactory.BadRequest<T>("Entity cannot be null.");

        if (entity.Id == Guid.Empty)
            return TResultFactory.BadRequest<T>("Entity Id cannot be empty.");

        try
        {
            var result = await _collection.ReplaceOneAsync(
                _filter.Eq(x => x.Id, entity.Id),
                entity);

            return result.MatchedCount == 0
                ? TResultFactory.NotFound<T>($"Entity with Id {entity.Id} not found.")
                : TResultFactory.Ok(entity, "Entity updated successfully.");
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
            var result = await _collection.DeleteOneAsync(
                _filter.Eq(x => x.Id, id));

            return result.DeletedCount == 0
                ? TResultFactory.NotFound($"Entity with Id {id} not found.")
                : TResultFactory.Ok("Entity deleted successfully.");
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
            //await _db.SaveChangesAsync();
            return TResultFactory.Ok();
        }
        catch (Exception ex)
        {
            return TResultFactory.Error(ex.Message);
        }
    }
}
