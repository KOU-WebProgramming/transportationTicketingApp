using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using TransportTicketApp.Data.Mongo.Configurations;
namespace TransportTicketApp.Entities
{
    [MongoEntity("Ticket")]
    public class Ticket
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid JourneyId { get; set; }
        public SeatReservation Seat { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
    //public class Ticket
    //{
    //    [BsonId]
    //    [BsonRepresentation(BsonType.ObjectId)]
    //    public string Id { get; set; }

    //    [BsonRepresentation(BsonType.ObjectId)]
    //    public string UserId { get; set; }
    //    public User User { get; set; }

    //    [BsonRepresentation(BsonType.ObjectId)]
    //    public string JourneyId { get; set; }
    //    public Journey Journey { get; set; }

    //    public int SeatNumber { get; set; }
    //    public DateTime PurchaseDate { get; set; }
    //}

}