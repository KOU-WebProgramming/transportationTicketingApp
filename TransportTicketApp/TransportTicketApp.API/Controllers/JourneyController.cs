using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TransportTicketApp.API.Models;
using TransportTicketApp.API.Services;
using TransportTicketApp.Entities;

namespace TransportTicketApp.API.Controllers
{
    [Route("journeys")]
    [Authorize]
    public class JourneyController : Controller
    {
        private readonly IJourneyService _journeyService;
        private readonly UserManager<ApplicationUser> _userManager;

        public JourneyController(IJourneyService journeyService, UserManager<ApplicationUser> userManager)
        {
            _journeyService = journeyService;
            _userManager = userManager;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var journeys = await _journeyService.GetAllJourneysAsync();
            return View(journeys);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("create")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Create(JourneyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var journey = new Journey
            {
                Id = Guid.NewGuid(),
                From = model.From,
                To = model.To,
                DepartureTime = model.DepartureTime,
                ArrivalTime = model.ArrivalTime,
                Price = model.Price,
                BusCompany = model.BusCompany
            };

            await _journeyService.CreateJourneyAsync(journey);

            return RedirectToAction("Index");
        }



        [HttpPost("reserve")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> ReserveSeat(Guid journeyId, int seatNumber)
        {
            var userId = _userManager.GetUserId(User);
            var result = await _journeyService.ReserveSeatAsync(journeyId, seatNumber, userId);

            if (result)
            {
                return Ok(new { success = true });
            }

            return BadRequest(new { success = false, message = "Seat already reserved or journey not found" });
        }
    }

}
