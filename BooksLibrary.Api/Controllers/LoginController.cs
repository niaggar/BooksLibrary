using BooksLibrary.Core.Interfaces;
using BooksLibrary.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BooksLibrary.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;

        public LoginController(ITokenService tokenService, IUserService userService)
        {
            _tokenService = tokenService;
            _userService = userService;
        }

        [HttpPost]
        [Route("Authenticate")]
        public async Task<IActionResult> Authenticate(LoginRequest login)
        {
            var user = await _userService.AuthenticateUser(login.Email, login.Password);

            if (user == null)
            {
                return Unauthorized();
            }

            return Ok(new { Token = user.Token });
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(User user)
        {
            try
            {
                var userDb = await _userService.CreateUser(user);

                var token = _tokenService.GenerateAccessToken(userDb.Username, userDb.Id);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Authorize]
        [Route("GetUser")]
        public async Task<IActionResult> GetUser()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var (userId, username) = await _tokenService.GetClaims(token);

            var user = await _userService.GetUser(int.Parse(userId));

            return Ok(user);
        }

        [HttpPost]
        [Authorize]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            await _tokenService.RevokeToken(token);

            return Ok();
        }

        [HttpGet]
        [Authorize]
        [Route("Test")]
        public IActionResult Test()
        {
            return Ok("Just a test");
        }
    }
}
