using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SparePart.ModelAndPersistance;
using SparePart.ModelAndPersistance.Context;
using SparePart.Services;
using System.Security.Cryptography;

namespace SparePart.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static User user = new User();
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly SparePartContext sparePartContext;

        public AuthController(IAuthService authService, IUserService userService, SparePartContext sparePartContext)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _userService = userService ?? throw new ArgumentNullException(nameof(authService));
            this.sparePartContext = sparePartContext;
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
                PasswordSalt = passwordSalt,
                //RefreshToken = "",
                //TokenCreated = DateTime.Now,
                //TokenExpires = DateTime.Now.AddDays(7),
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

            var refreshToken = GenerateRefreshToken();
            SetRefreshToken(refreshToken);
            var users = user;

            UpdateRefreshToken(users, refreshToken);

            return Ok(token);
        }

        [HttpPost("refreshToken")]
        public async Task<ActionResult<string>> RefreshToken([FromBody] string email)
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var user = await _authService.GetUser(email);

            if ((!user.RefreshToken.Equals(refreshToken)))
            {
                return Unauthorized("invalid refresh token");
            }
            else if (user == null || user.TokenExpires < DateTime.Now)
            {
                return Unauthorized("invalid refresh token");
            }
            else
            {
                string token = _authService.CreateToken(user);
                var newRefreshToken = GenerateRefreshToken();
                SetRefreshToken(newRefreshToken);

                UpdateRefreshToken(user, newRefreshToken);

                return Ok(token);
            }
        }

        // Update the user's refresh token in the database
        private void UpdateRefreshToken(User users, RefreshToken newRefreshToken)
        {
            users.RefreshToken = newRefreshToken.Token;
            users.TokenCreated = newRefreshToken.Created;
            users.TokenExpires = newRefreshToken.Expires;

            // Save the changes to the database
            _authService.UpdateUser(users);
        }


        private RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(7),
                Created = DateTime.Now
            };
            return refreshToken;
        }

        private void SetRefreshToken(RefreshToken newRefreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires
            };
            Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);

            user.RefreshToken = newRefreshToken.Token;
            user.TokenCreated = newRefreshToken.Created;
            user.TokenExpires = newRefreshToken.Expires;
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
