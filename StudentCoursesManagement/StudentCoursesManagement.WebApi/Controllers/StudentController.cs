using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentCoursesManagement.Domain.Entities;
using StudentCoursesManagement.Infrastructure.Config;
using StudentCoursesManagement.Infrastructure.Context;
using StudentCoursesManagement.WebApi.Dtos;
using StudentCoursesManagement.WebApi.Dtos.Student;
using StudentCoursesManagement.WebApi.Dtos.StudentDto;
using StudentCoursesManagement.WebApi.Services;
using StudentCoursesManagement.WebApi.Services.Interfaces;

namespace StudentCoursesManagement.WebApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var userList = await _studentService.GetAllUsersAsync();

            if(userList == null)
            {
                return BadRequest("User not found.");
            }

            return Ok(userList);
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("GetAllStudents")]
        public async Task<IActionResult> GetAllStudents()
        {
            var studentUserList = await _studentService.GetAllStudentsAsync();

            return Ok(studentUserList);
        }

        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Student}")]
        [HttpGet("GetStudentById")]
        public async Task<IActionResult> GetStudentById(string userId)
        {
            var studentUserById = await _studentService.GetStudentByIdAsync(userId);

            var studentDto = new StudentDetailDto
            {
                Id = studentUserById.Id,
                FirstName = studentUserById.FirstName,
                LastName = studentUserById.LastName,
                EnrollmentDate = studentUserById.EnrollmentDate,
                IsActive = studentUserById.IsActive,
                UserName = studentUserById.UserName,
                Email = studentUserById.Email,
                UserCourses = studentUserById.UserCourses?.Select(uc => new UserCourseDto
                {
                    CourseId = uc.CoursesCourseId,
                    CourseTitle = uc.Course.Title,
                    CourseStartDate = uc.Course.StartDate,
                    CourseEndDate = uc.Course.EndDate,
                    IsActive = uc.IsActive,
                    
                }).ToList()
            };

            return Ok(studentDto);
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("GetStudentsCoursesById")]
        public async Task<IActionResult> GetStudentsCoursesById(string userId)
        {
            var studentCoursesById = await _studentService.GetStudentCourses(userId);

            return Ok(studentCoursesById);
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost("CreateStudent")]
        public async Task<IActionResult> CreateStudent(CreateStudentDto createStudentDto)
        {
            await _studentService.AddStudentAsync(createStudentDto);

            return Created(string.Empty, createStudentDto);
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut("UpdateStudent")]
        public async Task<IActionResult> UpdateStudent(UpdateStudentDto updateStudentDto)
        {
            await _studentService.UpdateStudentAsync(updateStudentDto);

            return Ok(updateStudentDto);
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpDelete("DeleteStudent")]
        public async Task<IActionResult> DeleteStudent(string id)
        {
            var studentUserById = await _studentService.GetStudentByIdAsync(id);
            await _studentService.DeleteStudentAsync(id);

            return StatusCode(200);
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost("AssignCourseToStudents")]
        public async Task<IActionResult> AssignCourseToStudents(AssignCourseToStudentsDto dto)
        {
            await _studentService.AssignCoursesToStudentAsync(dto);

            return Ok(dto);
        }

        [Authorize(Roles = UserRoles.Student)]
        [HttpGet("MyCourses")]
        public async Task<IActionResult> MyCourses(string userId)
        {
            var myCourses = await _studentService.GetStudentCourses(userId);

            return Ok(myCourses);
        }

        [Authorize(Roles = UserRoles.Student)]
        [HttpPut("UpdateMyInfos")]
        public async Task<IActionResult> UpdateMyInfos(UpdateMyInfosDto updateMyInfosDto)
        {
            await _studentService.UpdateMyInfos(updateMyInfosDto);
            return Ok();
        }
    }
}
