using TransportTicketApp.Entities;

namespace TransportTicketApp.API.Services
{
    public interface IJourneyService
    {
        Task<IEnumerable<Journey>> GetAllJourneysAsync();
        Task<Journey> GetJourneyByIdAsync(Guid id);
        Task CreateJourneyAsync(Journey journey);
        Task UpdateJourneyAsync(Journey journey);
        Task DeleteJourneyAsync(Guid id);
        Task<bool> ReserveSeatAsync(Guid journeyId, int seatNumber, string userId);
    }

}
