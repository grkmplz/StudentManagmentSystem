
namespace StudentCoursesManagement.WebApi.Dtos.StudentDto
{
    public record UpdateMyInfosDto
    {
        public string StudentId { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? UserName { get; set; }

        public DateTime EnrollmentDate { get; set; }

        public string? Email { get; set; }

        public ICollection<UserCourseDto>? UserCourses { get; set; }

        // Audit Properties
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
