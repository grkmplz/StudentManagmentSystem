namespace StudentCoursesManagement.WebApi.Dtos.User
{
    public record UserRoleDto
    {
        public string? UserId { get; init; }
        public string? UserName { get; init; }
        public bool IsSelected { get; init; }
    }
}
