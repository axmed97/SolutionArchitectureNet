using Asp.Versioning;
using Business.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost("[action]")]
        [MapToApiVersion("1.0")]
        [Authorize]
        public async Task<IActionResult> CreateRole([FromBody] string roleName)
        {
            var result = await _roleService.CreateRoleAsync(roleName);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPut("[action]")]
        [MapToApiVersion("1.0")]
        [Authorize]
        public async Task<IActionResult> UpdateRole([FromBody] string roleName, [FromRoute] string roleId)
        {
            var result = await _roleService.UpdateRoleAsync(roleId, roleName);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        [MapToApiVersion("1.0")]
        [Authorize]
        [HttpDelete("[action]")]
        public async Task<IActionResult> DeleteRole([FromRoute] string id)
        {
            var response = await _roleService.DeleteRoleAsync(id);
            if (response.Success)
                return Ok(response);
            return BadRequest(response);
        }
        [MapToApiVersion("1.0")]
        [Authorize]
        [HttpGet("[action]")]
        public IActionResult GetAllRoles()
        {
            var response = _roleService.GetAllRoles();
            if (response.Success)
                return Ok(response);
            return BadRequest(response);
        }
    }
}
