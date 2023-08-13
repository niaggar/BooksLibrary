using BooksLibrary.Core.Interfaces;
using BooksLibrary.Model.Models;
using BooksLibrary.Model.TO;
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
        public async Task<IActionResult> Authenticate(LoginTO login)
        {
            try
            {
                var result = new ResultTO<UserTO>();
                var user = await _userService.AuthenticateUser(login.Email, login.Password);

                if (user == null)
                {
                    result = new ResultTO<UserTO>
                    {
                        Success = false,
                        Message = "Invalid credentials"
                    };

                    return StatusCode((int)HttpStatusCode.Unauthorized, result);
                }

                result.Success = true;
                result.Data = user;

                return Ok(result);
            }
            catch (Exception ex)
            {
                var res = new ResultTO<object> { Success = false, Message = ex.Message };
                return StatusCode((int)HttpStatusCode.InternalServerError, res);
            }
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(UserTO user)
        {
            try
            {
                var userDb = await _userService.CreateUser(user);
                var token = await _tokenService.GenerateAccessToken(userDb.Username, userDb.Id);
                userDb.Token = token;

                var result = new ResultTO<UserTO>
                {
                    Data = userDb,
                    Success = true
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                var res = new ResultTO<object> { Success = false, Message = ex.Message };
                return StatusCode((int)HttpStatusCode.InternalServerError, res);
            }
        }

        [HttpGet]
        [Authorize]
        [Route("GetUser")]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var (userId, username) = await _tokenService.GetClaims(token);

                var userDB = await _userService.GetUser(int.Parse(userId));

                var res = new ResultTO<UserTO>
                {
                    Data = userDB,
                    Success = true
                };
                return Ok(res);
            }
            catch (Exception ex)
            {
                var res = new ResultTO<object> { Success = false, Message = ex.Message };
                return StatusCode((int)HttpStatusCode.InternalServerError, res);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var (userId, username) = await _tokenService.GetClaims(token);

                await _tokenService.RevokeToken(userId);

                var res = new ResultTO<string> { Success = true, Data = "Logout success" };
                return Ok(res);

            }
            catch (Exception ex)
            { 
                var res = new ResultTO<object> { Success = false, Message = ex.Message };
                return StatusCode((int)HttpStatusCode.InternalServerError, res);
            }
        }

        [HttpGet]
        [Authorize]
        [Route("Test")]
        public IActionResult Test()
        {
            var res = new ResultTO<string> { Success = true, Data = "Test success" };
            return Ok(res);
        }
    }
}
