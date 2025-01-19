using System.ComponentModel.DataAnnotations;

namespace StudentCoursesManagement.WebApi.Dtos.Role
{
    public record CreateRoleDto
    {
        public string RoleName { get; init; }
    }
}
