using Microsoft.AspNetCore.Identity;
using TransportTicketApp.Entities;

namespace TransportTicketApp.API.Services
{
    public interface IUserService
    {
        Task<List<ApplicationUser>> GetAllUsersAsync();
        Task<ApplicationUser> GetUserByIdAsync(string id);
        Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password);
        Task<IdentityResult> UpdateUserAsync(string id, ApplicationUser user);
        Task<IdentityResult> DeleteUserAsync(string id);
    }
}
