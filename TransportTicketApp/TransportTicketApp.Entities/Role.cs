using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using TransportTicketApp.Data.Mongo.Configurations;

namespace TransportTicketApp.Entities
{

    [MongoEntity("Role")]
    public class Role
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }
        public ICollection<Permission> Permissions { get; set; }
    }

}