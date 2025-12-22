using System.Linq.Expressions;
using MongoDB.Driver;
using solidcode.work.infra.Entities;

namespace solidcode.work.infra.Repositories;

public class MongoRepository<T> : IRepository<T> where T : class, IEntity
{
    private readonly IMongoCollection<T> _mongoCollection;
    private readonly IMongoDatabase _database;
    private readonly FilterDefinitionBuilder<T> _filterBuilder = Builders<T>.Filter;

    public MongoRepository(IMongoDatabase database, string collectionName)
    {
        _database = database;
        _mongoCollection = database.GetCollection<T>(collectionName);
    }

    // ---- QUERIES ----
    public async Task<TResult<List<T>>> GetAllAsync()
    {
        try
        {
            var data = await _mongoCollection.Find(_filterBuilder.Empty).ToListAsync();

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
            var data = await _mongoCollection.Find(filter).ToListAsync();

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

            var entity = await _mongoCollection.Find(_filterBuilder.Eq(x => x.Id, id)).FirstOrDefaultAsync();

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
            var entity = await _mongoCollection.Find(filter).FirstOrDefaultAsync();

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

    // ---- COMMANDS ----
    public async Task<TResult<T>> ApplyChangesAsync(T entity)
    {
        using var session = await _database.Client.StartSessionAsync();

        try
        {
            if (entity is null)
                return TResultFactory.BadRequest<T>("Entity cannot be null.");

            if (entity is not IListEditEntity editable)
                return TResultFactory.BadRequest<T>($"Entity of type {typeof(T).Name} must implement IListEditEntity.");

            session.StartTransaction();

            if (editable.IsNew)
            {
                await _mongoCollection.InsertOneAsync(session, entity);
            }
            else
            {
                var filter = _filterBuilder.Eq(x => x.Id, entity.Id);
                var replaceResult = await _mongoCollection.ReplaceOneAsync(session, filter, entity);

                if (replaceResult.MatchedCount == 0)
                {
                    await session.AbortTransactionAsync();
                    return TResultFactory.NotFound<T>($"Entity with Id {entity.Id} not found for update.");
                }
            }

            await session.CommitTransactionAsync();
            return TResultFactory.Ok(entity, "Changes saved successfully.");
        }
        catch (Exception ex)
        {
            await session.AbortTransactionAsync();
            return TResultFactory.Error<T>(ex.Message);
        }
    }

    public async Task<TResult<T>> DeleteAsync(Guid id)
    {
        try
        {
            if (id == Guid.Empty)
                return TResultFactory.BadRequest<T>("Id cannot be empty.");

            var existing = await _mongoCollection.Find(_filterBuilder.Eq(x => x.Id, id)).FirstOrDefaultAsync();

            if (existing is null)
                return TResultFactory.NotFound<T>($"Entity with Id {id} not found.");

            await _mongoCollection.DeleteOneAsync(_filterBuilder.Eq(x => x.Id, id));
            return TResultFactory.Ok(existing, "Entity deleted successfully.");
        }
        catch (Exception ex)
        {
            return TResultFactory.Error<T>(ex.Message);
        }
    }
}
