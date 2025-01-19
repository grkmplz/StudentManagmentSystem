using StudentCoursesManagement.Domain.Entities;

namespace StudentCoursesManagement.WebApi.Services.Interfaces
{
    public interface ITeacherService
    {
        Task<List<Teacher>> GetAllTeachersAsync();
        Task<Teacher?> GetTeacherByIdAsync(int id);
        Task CreateTeacherAsync(Teacher teacher);
        Task UpdateTeacherAsync(Teacher teacher);
        Task DeleteTeacherAsync(int id);
        Task<List<Teacher>> SearchTeachersAsync(string query);
    }
}