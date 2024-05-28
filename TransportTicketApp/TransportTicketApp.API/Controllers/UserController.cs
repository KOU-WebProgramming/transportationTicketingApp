using Microsoft.AspNetCore.Mvc;
using TransportTicketApp.API.Services;
using TransportTicketApp.Entities;

namespace TransportTicketApp.API.Controllers
{
    [Route("users")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllUsersAsync();
            return View(users);
        }

        [HttpGet("edit/{id:length(24)}")]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost("edit/{id:length(24)}")]
        public async Task<IActionResult> Edit(string id, ApplicationUser user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            var result = await _userService.UpdateUserAsync(id, user);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(user);
        } 

        [HttpGet("delete/{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost("delete/{id:length(24)}"), ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            return View();
        }
    }

}
