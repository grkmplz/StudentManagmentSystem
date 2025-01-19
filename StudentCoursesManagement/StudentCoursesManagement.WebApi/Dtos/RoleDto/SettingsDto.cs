namespace StudentCoursesManagement.WebApi.Dtos.Role
{
    public record SettingsDto
    {
        public string RoleId { get; init; }
        public string RoleName { get; init; }
        public int UserCount { get; init; }
        public bool IsActive { get; init; }
    }
}
