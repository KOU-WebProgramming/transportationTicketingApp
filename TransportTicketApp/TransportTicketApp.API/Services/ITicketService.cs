using TransportTicketApp.Entities;

namespace TransportTicketApp.API.Services
{
    public interface ITicketService
    {
        Task<IEnumerable<Ticket>> GetAllTicketsAsync();
        Task<Ticket> GetTicketByIdAsync(Guid id);
        Task CreateTicketAsync(Ticket ticket);
        Task DeleteTicketAsync(Guid id);
        Task<bool> IsSeatTakenAsync(Guid journeyId, Seat seat);
    }
}
