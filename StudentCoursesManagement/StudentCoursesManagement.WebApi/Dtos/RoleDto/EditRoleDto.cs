using StudentCoursesManagement.WebApi.Dtos.User;

namespace StudentCoursesManagement.WebApi.Dtos.Role
{
    public record EditRoleDto
    {
        public string? RoleId { get; init; }
        public string? RoleName { get; init; }
        public List<UserRoleDto>? Users { get; init; }
    }
}
