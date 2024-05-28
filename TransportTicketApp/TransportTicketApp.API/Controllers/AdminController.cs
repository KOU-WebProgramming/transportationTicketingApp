using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TransportTicketApp.API.Controllers
{
    [Route("admin")]
    [Authorize(Policy = "AdminOnly")]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // Diğer admin işlemleri burada
    }
}
