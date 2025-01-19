// File: WebApi/Controllers/DepartmentController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentCoursesManagement.Domain.Entities;
using StudentCoursesManagement.Infrastructure.Config;
using StudentCoursesManagement.WebApi.Services.Interfaces;

namespace StudentCoursesManagement.WebApi.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDepartments()
        {
            var depts = await _departmentService.GetAllDepartmentsAsync();
            return Ok(depts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartmentById(int id)
        {
            var dept = await _departmentService.GetDepartmentByIdAsync(id);
            if (dept == null) return NotFound($"Department {id} not found or inactive.");
            return Ok(dept);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDepartment([FromBody] Department dept)
        {
            if (dept == null) return BadRequest("Null Department.");

            await _departmentService.CreateDepartmentAsync(dept);
            return StatusCode(201, dept);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateDepartment([FromBody] Department dept)
        {
            var found = await _departmentService.GetDepartmentByIdAsync(dept.DepartmentId);
            if (found == null) return NotFound($"Department {dept.DepartmentId} not found.");

            await _departmentService.UpdateDepartmentAsync(dept);
            return Ok(dept);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            await _departmentService.DeleteDepartmentAsync(id);
            return Ok($"Department {id} inactivated.");
        }
    }
}
