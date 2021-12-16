using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PersonalBlog.Domain.Responses;
using PersonalBlog.Domain.Models;
using PersonalBlog.Domain.Requests;
using PersonalBlog.Security.Jwt;
using PersonalBlog.Services.Interfaces;
using PersonalBlog.Data.Repositories.Interfaces;
using PersonalBlog.Security;

namespace PersonalBlog.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly Authenticator _authenticator;
        private readonly RefreshTokenValidator _refreshTokenValidator;

        public AuthenticationController(IUserService userService, RefreshTokenValidator refreshTokenValidator,
            IRefreshTokenService refreshTokenService, Authenticator authenticator)
        {
            _userService = userService;
            _refreshTokenValidator = refreshTokenValidator;
            _refreshTokenService = refreshTokenService;
            _authenticator = authenticator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {

            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            if (registerRequest.Password != registerRequest.ConfirmPassword) 
            {
                return BadRequest(new ErrorResponse("Password mismatch"));
            }

            var userToRegister = new User()
            {
                Email = registerRequest.Email,
                UserName = registerRequest.UserName,
            };

            await _userService.RegisterUserAsync(userToRegister, registerRequest.Password);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }
            var user = await _userService.GetUserByNameAsync(loginRequest.UserName);
            if(user == null)
            {
                return Unauthorized(new ErrorResponse("Wrong user name"));
            }
            var result = _userService.VerifyUserPassword(user, loginRequest.Password);
            if(result == PasswordVerificationResult.Failed)
            {
                return Unauthorized(new ErrorResponse("Password verifycation failed"));
            }

            var response = await _authenticator.Authenticate(user);

            return Ok(response);
        }
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest refreshRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            var isValidRefreshToken = _refreshTokenValidator.Validate(refreshRequest.RefreshToken);
            if (!isValidRefreshToken)
            {
                return BadRequest(new ErrorResponse("Invalid refresh token"));
            }

            var refreshTokenDTO = await _refreshTokenService.GetByTokenAsync(refreshRequest.RefreshToken);
            if(refreshTokenDTO == null)
            {
                return NotFound(new ErrorResponse("Token not found"));
            }

            var user = await _userService.GetUserByIdAsync(refreshTokenDTO.User.Id);
            if (user == null)
            {
                return NotFound(new ErrorResponse("User not found"));
            }

            var response = await _authenticator.Authenticate(user);

            return Ok(response);
        }

        private IActionResult BadRequestModelState()
        {
            IEnumerable<string> errorMessages = ModelState.Values
                                .SelectMany(values => values.Errors
                                .Select(error => error.ErrorMessage));

            return BadRequest(new ErrorResponse(errorMessages));
        }
    }
}
