using TransportTicketApp.Data.Mongo.Configurations;

namespace TransportTicketApp.Entities
{
    [MongoEntity("SeatReservation")]
    public class SeatReservation
    {
        public int SeatNumber { get; set; }
        public string UserId { get; set; }
    }

}
