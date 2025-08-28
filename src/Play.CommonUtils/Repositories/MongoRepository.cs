using System.Linq.Expressions;
using MongoDB.Driver;
using Play.CommonUtils.Entities;

namespace Play.CommonUtils.Repositories;

public class MongoRepository<T> : IRepository<T> where T : class, IEntity, new()
{
    private readonly IMongoCollection<T> _mongoCollection;
    private readonly IMongoDatabase _database;
    private readonly FilterDefinitionBuilder<T> _filterBuilder = Builders<T>.Filter;

    public MongoRepository(IMongoDatabase database, string collectionName)
    {
        _database = database;
        _mongoCollection = database.GetCollection<T>(collectionName);
    }

    // ---- QUERIES: Set Data ----
    public async Task<TResult<List<T>>> GetAllAsync()
    {
        var result = new TResult<List<T>>();
        try
        {
            result.Data = await _mongoCollection.Find(_filterBuilder.Empty).ToListAsync();
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
            result.Data = await _mongoCollection.Find(filter).ToListAsync();
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

            var filter = _filterBuilder.Eq(x => x.Id, id);
            var entity = await _mongoCollection.Find(filter).FirstOrDefaultAsync();
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
            var entity = await _mongoCollection.Find(filter).FirstOrDefaultAsync();
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

    // ---- COMMAND: Transaction-safe ApplyChangesAsync ----
    public async Task<TResult<T>> ApplyChangesAsync(T entity)
    {
        var result = new TResult<T>();
        using var session = await _database.Client.StartSessionAsync();

        try
        {
            if (entity is null) throw new ArgumentNullException(nameof(entity));
            if (entity is not IListEditEntities editable)
                throw new InvalidOperationException(
                    $"Entity of type {typeof(T).Name} must implement IListEditEntity."
                );

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
                    throw new InvalidOperationException($"Entity with Id {entity.Id} not found for update.");
            }

            await session.CommitTransactionAsync();
        }
        catch (Exception ex)
        {
            await session.AbortTransactionAsync();
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

            var existing = await _mongoCollection
                .Find(_filterBuilder.Eq(x => x.Id, id))
                .FirstOrDefaultAsync();

            if (existing is null)
                throw new InvalidOperationException($"Entity with Id {id} not found.");

            var filter = _filterBuilder.Eq(x => x.Id, id);
            await _mongoCollection.DeleteOneAsync(filter);
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.Message = ex.Message;
        }
        return result;
    }
}
