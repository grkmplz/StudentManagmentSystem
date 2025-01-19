// File: WebApi/Controllers/TeacherController.cs
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
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherService _teacherService;

        public TeacherController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTeachers()
        {
            var teachers = await _teacherService.GetAllTeachersAsync();
            return Ok(teachers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeacherById(int id)
        {
            var teacher = await _teacherService.GetTeacherByIdAsync(id);
            if (teacher == null)
                return NotFound($"Teacher {id} not found or inactive.");

            return Ok(teacher);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTeacher([FromBody] Teacher teacher)
        {
            if (teacher == null) return BadRequest("Teacher cannot be null.");

            await _teacherService.CreateTeacherAsync(teacher);
            return StatusCode(201, teacher); 
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTeacher([FromBody] Teacher teacher)
        {
            var found = await _teacherService.GetTeacherByIdAsync(teacher.TeacherId);
            if (found == null) return NotFound("Teacher not found or inactive.");

            await _teacherService.UpdateTeacherAsync(teacher);
            return Ok(teacher);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeacher(int id)
        {
            await _teacherService.DeleteTeacherAsync(id);
            return Ok($"Teacher {id} marked as inactive.");
        }

        [HttpGet("Search")]
        public async Task<IActionResult> SearchTeachers([FromQuery] string query)
        {
            var result = await _teacherService.SearchTeachersAsync(query);
            return Ok(result);
        }
    }
}
