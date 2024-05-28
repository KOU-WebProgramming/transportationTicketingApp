using Microsoft.AspNetCore.Authorization;
using MongoDB.Driver;
using TransportTicketApp.Data;
using TransportTicketApp.Entities;

namespace TransportTicketApp.API.Services
{
    public class JourneyService : IJourneyService
    {
        private readonly IDbContext dbContext;
        private readonly IMongoCollection<Journey> _journeyCollection;

        public JourneyService(IDbContext dbContext)
        {
            this.dbContext = dbContext;
            //_journeyCollection = dbContext.GetRepository<Journey>().GetAll();
        }

        public async Task<IEnumerable<Journey>> GetAllJourneysAsync()
        {
            return await dbContext.GetRepository<Journey>().GetAll();
        }

        public async Task<Journey> GetJourneyByIdAsync(Guid id)
        {
            return (await dbContext.GetRepository<Journey>().Get(journey => journey.Id == id)).FirstOrDefault();
        }

        public async Task CreateJourneyAsync(Journey journey)
        {
            await dbContext.GetRepository<Journey>().Create(journey);
        }

        public async Task UpdateJourneyAsync(Journey journey)
        {
            await dbContext.GetRepository<Journey>().Update(journey);
        }

        public async Task DeleteJourneyAsync(Guid id)
        {
            await _journeyCollection.DeleteOneAsync(journey => journey.Id == id);
        }



        public async Task<bool> ReserveSeatAsync(Guid journeyId, int seatNumber, string userId)
        {
            var journey = await GetJourneyByIdAsync(journeyId);
            if (journey == null)
            {
                return false;
            }

            if (journey.SeatReservations.Any(sr => sr.SeatNumber == seatNumber))
            {
                return false;
            }

            journey.SeatReservations.Add(new SeatReservation
            {
                SeatNumber = seatNumber,
                UserId = userId
            });

            await UpdateJourneyAsync(journey);
            return true;
        }
    }

}
