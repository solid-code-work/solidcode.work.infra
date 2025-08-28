using System.Linq.Expressions;
using MongoDB.Driver;
using Play.CommonUtils.Entities;

namespace Play.CommonUtils.Repositories;

public class MongoRepository<T> : IRepository<T> where T : IEntity
{
    private readonly IMongoCollection<T> _mongoCollection;
    private readonly FilterDefinitionBuilder<T> _filterBuilder = Builders<T>.Filter;

    public MongoRepository(IMongoDatabase database, string collectionName)
    {
        _mongoCollection = database.GetCollection<T>(collectionName);
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _mongoCollection.Find(_filterBuilder.Empty).ToListAsync();
    }

    public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter)
    {
        return await _mongoCollection.Find(filter).ToListAsync();
    }

    public async Task<T?> GetAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty", nameof(id));

        var filter = _filterBuilder.Eq(x => x.Id, id);
        return await _mongoCollection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<T?> GetAsync(Expression<Func<T, bool>> filter)
    {
        return await _mongoCollection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task ApplyChangesAsync(T entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        if (entity is not IListEditEntities editable)
            throw new InvalidOperationException($"Entity of type {typeof(T).Name} must implement IListEditEntity to use SaveAsync.");

        if (editable.IsNew)
        {
            await _mongoCollection.InsertOneAsync(entity);
        }
        else
        {
            var filter = _filterBuilder.Eq(x => x.Id, entity.Id);
            var result = await _mongoCollection.ReplaceOneAsync(filter, entity);

            if (result.MatchedCount == 0)
                throw new InvalidOperationException($"Entity with Id {entity.Id} not found for update.");
        }
    }


    public async Task CreateAsync(T entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        await _mongoCollection.InsertOneAsync(entity);
    }

    public async Task UpdateAsync(T entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        var filter = _filterBuilder.Eq(x => x.Id, entity.Id);
        var existing = await GetAsync(entity.Id);

        if (existing is null)
            throw new InvalidOperationException($"Entity with Id {entity.Id} not found.");

        await _mongoCollection.ReplaceOneAsync(filter, entity);
    }

    public async Task DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty", nameof(id));

        var existing = await GetAsync(id);
        if (existing is null)
            throw new InvalidOperationException($"Entity with Id {id} not found.");

        var filter = _filterBuilder.Eq(x => x.Id, id);
        await _mongoCollection.DeleteOneAsync(filter);
    }
}
