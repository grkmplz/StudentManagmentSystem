using StudentCoursesManagement.Domain.Entities;
using StudentCoursesManagement.WebApi.Dtos.Student;
using StudentCoursesManagement.WebApi.Dtos.StudentDto;

namespace StudentCoursesManagement.WebApi.Services.Interfaces
{
    public interface IStudentService
    {
        Task<List<User>> GetAllStudentsAsync();
        Task<List<User>> GetAllUsersAsync();
        Task<User> GetStudentByIdAsync(string id);
        Task AddStudentAsync(CreateStudentDto createStudentDto);
        Task UpdateStudentAsync(UpdateStudentDto updateStudentDto);
        Task DeleteStudentAsync(string id);
        Task AssignCoursesToStudentAsync(AssignCourseToStudentsDto dto);
        Task<List<GetStudentsCourseDto>> GetStudentCourses(string userId);
        Task UpdateMyInfos(UpdateMyInfosDto updateMyInfosDto);
    }
}
