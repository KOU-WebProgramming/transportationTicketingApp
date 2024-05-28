using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using TransportTicketApp.Data.Mongo.Configurations;

[MongoEntity("Permission")]
public class Permission
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string Name { get; set; }
}
