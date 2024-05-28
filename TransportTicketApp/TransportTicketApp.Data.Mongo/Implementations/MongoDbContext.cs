using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TransportTicketApp.Data.IMS.Framework.Data;
using TransportTicketApp.Data.Mongo.Abstractions;
using TransportTicketApp.Data.Mongo.Configurations;

namespace TransportTicketApp.Data.Mongo.Implementations
{
    public class MongoDbContext : IDbContext
    {
        private readonly IReadOnlyDictionary<Type, string> typeCollectionTable = null!;
        private readonly IMongoDatabase? mongoDatabase;
        private readonly MongoClient? mongoClient;
        public                  /*Ctor*/                            MongoDbContext(IOptions<MongoDbOptions> options
                                                                                                , IMongoEntityTypeProvider typeProvider
                                                                                                )
        {
            mongoClient = new MongoClient(options.Value.ConnectionString);
            mongoDatabase = mongoClient.GetDatabase(options.Value.DatabaseName);
            typeCollectionTable = typeProvider.GetEntityTypeTypes();
        }
        public IRepository<EntityType>? GetRepository<EntityType>() where EntityType : IEntityBase => typeCollectionTable.TryGetValue(typeof(EntityType), out string? collectionName)
                                                                                                                              ? new MongoRepository<EntityType>(mongoDatabase?.GetCollection<EntityType>(collectionName))
                                                                                                                              : null;
    }
}
