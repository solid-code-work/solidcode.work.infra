using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using solidcode.work.infra.Abstraction;
using solidcode.work.infra.Configurations;
using solidcode.work.infra.Entities;

namespace solidcode.work.infra.Mongo;

public static class Extensions
{
    public static IServiceCollection AddSolidCodeMongoDB(this IServiceCollection services, IConfiguration configuration)
    {
        BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
        BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

        services.AddSingleton<IMongoDatabase>(sp =>
        {

            var serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
            var mongoSettings = configuration.GetSection(nameof(MongoDBsettings)).Get<MongoDBsettings>();

            if (string.IsNullOrWhiteSpace(mongoSettings?.ConnectionString))
                throw new ArgumentException("MongoDB connection string is missing or empty.");

            if (string.IsNullOrWhiteSpace(serviceSettings?.ServiceName))
                throw new ArgumentException("Service name is missing or empty.");

            var client = new MongoClient(mongoSettings.ConnectionString);
            return client.GetDatabase(serviceSettings.ServiceName);
        });

        return services;
    }

    public static IServiceCollection AddSolidCodeMongoRepository<T>(this IServiceCollection services, string collectionName) where T : class, IEntity, new()
    {
        services.AddScoped<IRepository<T>>(x =>
        {
            var database = x.GetRequiredService<IMongoDatabase>();
            return new MongoRepository<T>(database, collectionName);
        });
        return services;
    }
}