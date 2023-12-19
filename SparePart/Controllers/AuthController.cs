using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SparePart.ModelAndPersistance;
using SparePart.Services;

namespace SparePart.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public AuthController(IAuthService authService, IUserService userService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _userService = userService ?? throw new ArgumentNullException(nameof(authService));
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            var userExists = await _authService.UserExists(request.Email);
            if (userExists)
            {
                return BadRequest("Username already exists.");
            }

            _authService.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var newUser = new User()
            {
                Email = request.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            _authService.RegisterNewUser(newUser);

            return Ok(newUser);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDto request)
        {
            var user = await _authService.GetUser(request.Email);
            if (user == null)
            {
                return Unauthorized("Incorrect username or password.");
            }

            var matchPass = _authService.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt);

            if (!matchPass)
            {
                return Unauthorized("Incorrect username or password.");
            }

            string token = _authService.CreateToken(user);
            return Ok(token);
        }

        //[HttpGet, Authorize]
        //public ActionResult<string> GetMe()
        //{
        //    var userEmail = _userService.GetUserEmail();
        //    return Ok(userEmail);

        //    //var userName = User?.Identity?.Name;
        //    //var role = User.FindFirstValue(ClaimTypes.Role);
        //    //return Ok(new { userName, role});
        //}
    }
}
