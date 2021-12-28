using CookRun.Domain.Entities;
using CookRun.Domain.Models;
using CookRun.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(IUserService userService, UserManager<ApplicationUser> userManager)
        {
            _userService = userService;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost("Authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequest model)
        {
            var response = await _userService.Authenticate(model, GpAddress());

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }

        [HttpPost("GetUsers")]
        public List<UserModel> GetUsers()
        {
            return _userService.GetUsers();
        }

        [HttpPost("CreateUser")]
        public async Task<Guid> CreateUser(CreateUserModel createUserModel)
        {
            var user = await _userManager.FindByIdAsync(User.Identity.Name);
            if (user == null)
                return Guid.Empty;
            
            return await _userService.CreateUser(createUserModel);
        }

        [HttpPost("DeleteUser")]
        public async Task DeleteUser(string userName)
        {
            await _userService.DeleteUser(userName);
        }

        //[AllowAnonymous]
        //[HttpPost("refresh-token")]
        //public IActionResult RefreshToken()
        //{
        //    var refreshToken = Request.Cookies["refreshToken"];
        //    var response = _userService.RefreshToken(refreshToken, ipAddress());

        //    if (response == null)
        //        return Unauthorized(new { message = "Invalid token" });

        //    setTokenCookie(response.RefreshToken);

        //    return Ok(response);
        //}

        //[HttpPost("revoke-token")]
        //public IActionResult RevokeToken([FromBody] RevokeTokenRequest model)
        //{
        //    // accept token from request body or cookie
        //    var token = model.Token ?? Request.Cookies["refreshToken"];

        //    if (string.IsNullOrEmpty(token))
        //        return BadRequest(new { message = "Token is required" });

        //    var response = _userService.RevokeToken(token, ipAddress());

        //    if (!response)
        //        return NotFound(new { message = "Token not found" });

        //    return Ok(new { message = "Token revoked" });
        //}

        //[HttpGet("{id}/refresh-tokens")]
        //public IActionResult GetRefreshTokens(int id)
        //{
        //    var user = _userService.GetById(id);
        //    if (user == null) return NotFound();

        //    return Ok(user.RefreshTokens);
        //}

        //// helper methods

        private string GpAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}