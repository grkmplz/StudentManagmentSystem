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
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet("GetAllCourses")]
        public async Task<IActionResult> GetAllCourses()
        {
            var courses = await _courseService.GetAllCoursesAsync();

            var result = courses.Select(c => new
            {
                c.CourseId,
                c.Title,
                c.Description,
                c.StartDate,
                c.EndDate,
                c.IsActive,
                c.CreatedAt,
                c.ModifiedAt,
                c.TeacherId,
                TeacherName = c.Teacher != null ? c.Teacher.FirstName + " " + c.Teacher.LastName : null,
                c.DepartmentId,
                DepartmentName = c.Department?.DepartmentName
            });
            return Ok(result);
        }

        [HttpGet("GetCourseById")]
        public async Task<IActionResult> GetCourseById(int courseId)
        {
            var course = await _courseService.GetCourseByIdAsync(courseId);
            if (course == null) return NotFound($"Course {courseId} not found.");

            var result = new
            {
                course.CourseId,
                course.Title,
                course.Description,
                course.StartDate,
                course.EndDate,
                course.IsActive,
                course.CreatedAt,
                course.ModifiedAt,
                course.TeacherId,
                TeacherName = course.Teacher != null ? course.Teacher.FirstName + " " + course.Teacher.LastName : null,
                course.DepartmentId,
                DepartmentName = course.Department?.DepartmentName
            };
            return Ok(result);
        }

        [HttpPost("CreateCourse")]
        public async Task<IActionResult> CreateCourse([FromBody] Course course) 
        {
            await _courseService.AddCourseAsync(course);
            return StatusCode(201, course.CourseId);
        }

        [HttpPut("UpdateCourse")]
        public async Task<IActionResult> UpdateCourse([FromBody] Course course)
        {
            await _courseService.UpdateCourseAsync(course);
            return Ok();
        }

        [HttpDelete("DeleteCourse")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            await _courseService.DeleteCourseAsync(id);
            return Ok();
        }
    }
}
