using Microsoft.AspNetCore.Identity;

namespace StudentCoursesManagement.WebApi.Dtos.StudentDto
{
    public class StudentDetailDto : IdentityUser
    {
        public string Id { get; set; }          
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public List<UserCourseDto>? UserCourses { get; set; }
        public bool IsActive { get; set; }
    }
}
