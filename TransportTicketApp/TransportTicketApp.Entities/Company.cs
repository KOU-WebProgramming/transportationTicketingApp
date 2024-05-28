using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using TransportTicketApp.Data.Mongo.Configurations;

[MongoEntity("Company")]
public class Company
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }
    public List<string> JourneyIds { get; set; } // Gömme yerine referans
}
