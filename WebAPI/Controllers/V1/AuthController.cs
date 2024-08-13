using Asp.Versioning;
using Business.Abstract;
using Entities.DTOs.AuthDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAPI.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [MapToApiVersion("1.0")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Register(RegisterDTO model)
        {
            var result = await _authService.RegisterAsync(model);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [MapToApiVersion("1.0")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            var result = await _authService.LoginAsync(model);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [MapToApiVersion("1.0")]
        [HttpPost("[action]")]
        public async Task<IActionResult> RefreshTokenLogin(RefreshTokenDTO model)
        {
            var result = await _authService.RefreshTokenLoginAsync(model);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [MapToApiVersion("1.0")]
        [HttpPut("[action]")]
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _authService.LogOutAsync(userId);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [MapToApiVersion("1.0")]
        [HttpGet("[action]")]
        [Authorize]
        public IActionResult GetAllUser()
        {
            var result = _authService.GetAll();
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [MapToApiVersion("1.0")]
        [HttpDelete("[action]/{userId}")]
        [Authorize]
        public async Task<IActionResult> RemoveUser(string userId)
        {
            var result = await _authService.RemoveUserAsync(userId);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
