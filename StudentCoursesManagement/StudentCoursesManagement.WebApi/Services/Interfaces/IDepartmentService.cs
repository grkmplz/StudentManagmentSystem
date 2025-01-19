using StudentCoursesManagement.Domain.Entities;

namespace StudentCoursesManagement.WebApi.Services.Interfaces
{
    public interface IDepartmentService
    {
        Task<List<Department>> GetAllDepartmentsAsync();
        Task<Department?> GetDepartmentByIdAsync(int id);
        Task CreateDepartmentAsync(Department dept);
        Task UpdateDepartmentAsync(Department dept);
        Task DeleteDepartmentAsync(int id);
    }
}