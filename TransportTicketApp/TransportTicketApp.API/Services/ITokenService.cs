using TransportTicketApp.Entities;

namespace TransportTicketApp.API.Services
{
    public interface ITokenService
    {
        string GenerateToken(ApplicationUser user);

    }
}
