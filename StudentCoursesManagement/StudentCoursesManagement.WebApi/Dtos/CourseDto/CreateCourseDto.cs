using StudentCoursesManagement.Domain.Entities;

namespace StudentCoursesManagement.WebApi.Dtos.Course
{
    public record CreateCourseDto
    {
        public string Title { get; init; }
        public string Description { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
        public int? TeacherId { get; init; }
        public int? DepartmentId { get; init; }
    }
}
