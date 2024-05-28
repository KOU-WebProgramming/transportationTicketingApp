using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TransportTicketApp.API.Models;
using TransportTicketApp.API.Services;
using TransportTicketApp.Entities;

namespace TransportTicketApp.API.Controllers
{
    [ApiController]
    [Route("account")]
    public class AccountController : Controller
    {
        private readonly IUserService userService;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ITokenService tokenService;
        public AccountController(IUserService userService, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ITokenService tokenService)
        {
            this.userService = userService;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.tokenService = tokenService;
        }


        public IActionResult UserProfile()
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (userEmail == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var user = userManager.FindByEmailAsync(userEmail).Result;
            return View(user);
        }


        [HttpGet("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var result = await userService.CreateUserAsync(user, model.Password);

            if (result.Succeeded)
            {
                var token = tokenService.GenerateToken(user);
                HttpContext.Session.SetString("JwtToken", token);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login( LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            ApplicationUser? appUser = await userManager.FindByEmailAsync(model.Email);

            if (appUser != null)
            {
                await signInManager.SignOutAsync().ConfigureAwait(false);
                Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(appUser, model.Password, model.RememberMe, lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    var token = tokenService.GenerateToken(appUser);
                    HttpContext.Session.SetString("JwtToken", token);
                    return RedirectToAction("Index", "Home");
                }
            }
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");

            return View(model);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }


    //public class UsersController : ControllerBase
    //{
    //    private readonly IUserService userService;

    //    public UsersController(IUserService userService)
    //    {
    //        userService = userService;
    //    }

    //    [HttpGet]
    //    public async Task<IActionResult> Get()
    //    {
    //        var users = await userService.GetAllUsersAsync();
    //        return Ok(users);
    //    }

    //    [HttpGet("{id:length(24)}", Name = "GetUser")]
    //    public async Task<IActionResult> Get(string id)
    //    {
    //        var user = await userService.GetUserByIdAsync(id);
    //        if (user == null)
    //        {
    //            return NotFound();
    //        }
    //        return Ok(user);
    //    }

    //    [HttpPost]
    //    public async Task<IActionResult> Create([FromBody] RegisterViewModel model)
    //    {
    //        var user = new ApplicationUser { UserName = model.Email, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName };
    //        var result = await userService.CreateUserAsync(user, model.Password);

    //        if (result.Succeeded)
    //        {
    //            return CreatedAtRoute("GetUser", new { id = user.Id.ToString() }, user);
    //        }
    //        return BadRequest(result.Errors);
    //    }

    //    [HttpPut("{id:length(24)}")]
    //    public async Task<IActionResult> Update(string id, [FromBody] ApplicationUser user)
    //    {
    //        var result = await userService.UpdateUserAsync(id, user);

    //        if (result.Succeeded)
    //        {
    //            return NoContent();
    //        }
    //        return BadRequest(result.Errors);
    //    }

    //    [HttpDelete("{id:length(24)}")]
    //    public async Task<IActionResult> Delete(string id)
    //    {
    //        var result = await userService.DeleteUserAsync(id);

    //        if (result.Succeeded)
    //        {
    //            return NoContent();
    //        }
    //        return BadRequest(result.Errors);
    //    }
    //}

}
