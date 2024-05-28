using MongoDB.Driver;
using System.Linq.Expressions;
using TransportTicketApp.Data.IMS.Framework.Data;
using TransportTicketApp.Data.Mongo.Helpers;

namespace TransportTicketApp.Data.Mongo.Implementations
{
    public class MongoRepository<EntityType> : IRepository<EntityType> where EntityType : IEntityBase
    {

        private readonly IMongoCollection<EntityType> mongoCollection;
        public                  /*Ctor*/                        MongoRepository(IMongoCollection<EntityType>? mongoCollection)
        {
            if (mongoCollection == null)
                throw new ArgumentNullException(nameof(mongoCollection));

            this.mongoCollection = mongoCollection;
        }

        public async Task<IEnumerable<EntityType>> GetAll() => await mongoCollection.Find(_ => true).ToListAsync();
        public async Task<IEnumerable<EntityType>> Get(Expression<Func<EntityType, bool>> filter) => await mongoCollection.Find(filter).ToListAsync();
        public async Task<EntityType?> Get(string id) => await mongoCollection.Find(x => Equals(x.Id, id)).FirstOrDefaultAsync();
        public async Task Delete(EntityType? item) { if (item is not null) await Delete(item.Id.ToString()); }
        public async Task Delete(string id) => await mongoCollection.DeleteOneAsync(x => Equals(x.Id, id));
        public async Task DeleteGuid(string guid) => await mongoCollection.DeleteOneAsync(x => Equals(x.Id, guid));
        public async Task<bool> Update(EntityType? item) { if (item is not null) return await Update(item.Id.ToString(), item); else return false; }
        public IQueryable<EntityType> GetAsQueryable(Expression<Func<EntityType, bool>>? filter = null) { filter ??= _ => true; return mongoCollection.AsQueryable().Where(filter); }
        public async Task<IEnumerable<EntityType>?> GetAllPagination(int pageNumber, int pageOffset)
        {
            //if (pageNumber < 1 && pageOffset<0)
            //    return null;

            //int skip = (pageNumber - 1) * pageOffset;

            //SortDefinition<EntityType> sort = Builders<EntityType>.Sort.Ascending(x => x.Id);
            //return await mongoCollection.Find(_ => true).Sort(sort).Skip(skip).Limit(pageOffset).ToListAsync();
            return await GetAllPagination(_ => true, pageNumber, pageOffset);
        }
        public async Task<IEnumerable<EntityType>?> GetAllPagination(Expression<Func<EntityType, bool>> filter, int pageNumber, int pageOffset)
        {
            if (pageNumber < 1 && pageOffset < 0)
                return null;

            int skip = (pageNumber - 1) * pageOffset;

            SortDefinition<EntityType> sort = Builders<EntityType>.Sort.Ascending(x => x.Id);
            return await mongoCollection.Find(filter).Sort(sort).Skip(skip).Limit(pageOffset).ToListAsync();
        }

        public async Task<EntityType?> Create(EntityType? item)
        {
            if (item is not null)
                await mongoCollection.InsertOneAsync(item);

            return item;
        }

        public async Task<bool> Update(string id, EntityType? item)
        {
            if (item is not null)
            {
                //item.UpdateTime = DateTime.UtcNow;
                var result = await mongoCollection.ReplaceOneAsync(x => Equals(x.Id, id), item);
                if (result.IsAcknowledged && result.ModifiedCount > 0)
                    return true;
            }
            return false;
        }

        public async Task<IEnumerable<EntityType>?> CreateMany(IEnumerable<EntityType> items)
        {

            if (items is not null && items.Any())
                await mongoCollection.InsertManyAsync(items);

            return items;
        }

        public async Task<bool> UpdateMany(IEnumerable<EntityType> items)
        {

            if (items is not null && items.Any())
            {
                var bulkOperations = new List<WriteModel<EntityType>>();

                foreach (var entity in items)
                {
                    //entity.UpdateTime = DateTime.UtcNow;
                    var filter = Builders<EntityType>.Filter.Eq(x => x.Id, entity.Id);
                    var model = new ReplaceOneModel<EntityType>(filter, entity);
                    bulkOperations.Add(model);
                }

                var result = await mongoCollection.BulkWriteAsync(bulkOperations);
                if (result.IsAcknowledged && result.ModifiedCount > 0)
                    return true;
            }
            return false;
        }

        public async Task DeleteMany(IEnumerable<EntityType> items)
        {
            if (items is not null && items.Any())
            {
                FilterDefinition<EntityType>? filter = Builders<EntityType>.Filter.In("_id", items.Select(x => x.ToString()));
                await mongoCollection.DeleteManyAsync(filter);
            }
        }

        public async Task DeleteMany(IEnumerable<string> items)
        {
            if (items is not null && items.Any())
            {
                FilterDefinition<EntityType>? filter = Builders<EntityType>.Filter.In("_id", items.Select(x => x.ToString()));
                await mongoCollection.DeleteManyAsync(filter);
            }
        }

        public async Task DeleteEmbeddedDocument<EmbeddedDocumentType>(Expression<Func<EntityType, IEnumerable<EmbeddedDocumentType>>> embeddedSelector
                                                                     , Expression<Func<EmbeddedDocumentType, bool>> filter)
        {
            var mongoFilter = Builders<EntityType>.Filter.ElemMatch(embeddedSelector, filter);
            var mongoUpdate = Builders<EntityType>.Update.PullFilter(embeddedSelector, filter);
            await mongoCollection.UpdateOneAsync(mongoFilter, mongoUpdate);
        }

        public async Task<EntityType> UpdateEmbeddedDocumentField<EmbeddedDocumentType>(Expression<Func<EntityType, IEnumerable<EmbeddedDocumentType>>> embeddedSelector
                                                                   , Expression<Func<EmbeddedDocumentType, bool>> filter
                                                                   , Expression<Func<EmbeddedDocumentType, object>> x
                                                                   , object value)
                                                                     where EmbeddedDocumentType : IEntityBase
        {
            var mongoFilter = Builders<EntityType>.Filter.ElemMatch(embeddedSelector, filter);

            string left = embeddedSelector.GetPropertyName();
            string right = x.GetPropertyName();

            var mongoUpdateTime = Builders<EntityType>.Update.Set($"{left}.$.UpdateTime", DateTime.UtcNow);
            var mongoValueUpdate = Builders<EntityType>.Update.Set($"{left}.$.{right}", value);

            var mongoUpdates = Builders<EntityType>.Update.Combine(mongoValueUpdate, mongoUpdateTime);

            var options = new FindOneAndUpdateOptions<EntityType> { ReturnDocument = ReturnDocument.After };
            var updatedDocument = await mongoCollection.FindOneAndUpdateAsync(mongoFilter, mongoUpdates, options);

            return updatedDocument;
        }

        public async Task<EntityType> UpdateEmbeddedDocument<EmbeddedDocumentType>(Expression<Func<EntityType, IEnumerable<EmbeddedDocumentType>>> embeddedSelector
                                                              , Expression<Func<EmbeddedDocumentType, bool>> filter
                                                              , EmbeddedDocumentType value)
                                                                where EmbeddedDocumentType : IEntityBase
        {

            //value.UpdateTime = DateTime.UtcNow;
            var mongoFilter = Builders<EntityType>.Filter.ElemMatch(embeddedSelector, filter);

            string left = embeddedSelector.GetPropertyName();

            var mongoUpdate = Builders<EntityType>.Update.Set($"{left}.$", value);


            var options = new FindOneAndUpdateOptions<EntityType> { ReturnDocument = ReturnDocument.After };
            EntityType updatedDocument = await mongoCollection.FindOneAndUpdateAsync(mongoFilter, mongoUpdate, options);

            return updatedDocument;
        }
        public async Task<EmbeddedDocumentType> GetEmbeddedDocument<EmbeddedDocumentType>(Expression<Func<EntityType
                                                                                        , IEnumerable<EmbeddedDocumentType>>> embeddedSelector
                                                                                        , Expression<Func<EmbeddedDocumentType, bool>> filter)
        {
            var mongoFilter = Builders<EntityType>.Filter.ElemMatch(embeddedSelector, filter);
            var cursor = await mongoCollection.Find(mongoFilter).ToListAsync();

            var selectorFunc = embeddedSelector.Compile();

            foreach (var document in cursor)
            {
                var items = selectorFunc(document);
                var subDocument = items.FirstOrDefault(filter.Compile());

                if (subDocument != null)
                {
                    return subDocument;
                }
            }

            throw new Exception("Belge bulunamadı");
        }
    }
}
