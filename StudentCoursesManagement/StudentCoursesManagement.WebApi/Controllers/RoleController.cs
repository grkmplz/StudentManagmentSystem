using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentCoursesManagement.Infrastructure.Config;
using StudentCoursesManagement.WebApi.Dtos.Role;
using StudentCoursesManagement.WebApi.Dtos.User;
using StudentCoursesManagement.WebApi.Services;
using StudentCoursesManagement.WebApi.Services.Interfaces;

namespace StudentCoursesManagement.WebApi.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var roleList = await _roleService.GetRolesAsync();

            return Ok(roleList);
        }

        [HttpGet("GetByIdRole")]
        public async Task<IActionResult> GetByIdRole(string id)
        {
            var roleById = await _roleService.GetRoleByIdAsync(id);

            return Ok(roleById);
        }

        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRole(CreateRoleDto createRoleDto)
        {
            await _roleService.CreateRoleAsync(createRoleDto);

            return StatusCode(201);
        }

        [HttpPut("UpdateRole")]
        public async Task<IActionResult> UpdateRole(EditRoleDto editRoleDto)
        {
            await _roleService.UpdateRoleAsync(editRoleDto);

            return StatusCode(200);
        }

        [HttpDelete("DeleteRole")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var roleById = await _roleService.GetRoleByIdAsync(id);
            
            await _roleService.DeleteRoleAsync(id);

            return StatusCode(200, $"{roleById.RoleName} deleted.");
        }

        [HttpGet("GetUsersDependOnRole")]
        public async Task<IActionResult> GetUsersDependOnRole(string id)
        {
            var resultModel = await _roleService.GetUsersInRoleAsync(id);

            return Ok(resultModel);
        }

        [HttpPut("UpdateUsersDependOnRole")]
        public async Task<IActionResult> UpdateUsersDependOnRole(string roleId , List<UserRoleDto> userRoleListDto)
        {
            await _roleService.UpdateUsersInRoleAsync(roleId, userRoleListDto);

            return StatusCode(200);
        }


    }
}
