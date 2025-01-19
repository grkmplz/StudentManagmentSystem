using StudentCoursesManagement.WebApi.Dtos.Role;
using StudentCoursesManagement.WebApi.Dtos.User;

namespace StudentCoursesManagement.WebApi.Services.Interfaces
{
    public interface IRoleService
    {
        Task<List<SettingsDto>> GetRolesAsync();
        Task<SettingsDto> GetRoleByIdAsync(string roleId);
        Task CreateRoleAsync(CreateRoleDto model);
        Task UpdateRoleAsync(EditRoleDto model);
        Task DeleteRoleAsync(string roleId);
        Task<List<UserRoleDto>> GetUsersInRoleAsync(string roleId);
        Task UpdateUsersInRoleAsync(string roleId, List<UserRoleDto> users);
    }
}
