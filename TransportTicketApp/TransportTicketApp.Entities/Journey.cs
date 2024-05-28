using TransportTicketApp.Data;
using TransportTicketApp.Data.Mongo.Configurations;

namespace TransportTicketApp.Entities
{
    [MongoEntity("Journey")]
    public class Journey: IEntityBase
    {
        public Guid Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public decimal Price { get; set; }
        public string BusCompany { get; set; }
        public List<SeatReservation> SeatReservations { get; set; } = new List<SeatReservation>();
    }



    public class Seat
    {
        public int Row { get; set; }
        public int Number { get; set; }
    }

}