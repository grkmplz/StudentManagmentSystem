using StudentCoursesManagement.Domain.Entities;

namespace StudentCoursesManagement.WebApi.Dtos.Course
{
    public record CourseViewDto
    {
        public int CourseId { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
        public bool IsActive { get; init; }

        public DateTime CreatedAt { get; init; }
        public DateTime? ModifiedAt { get; init; }

        public int? TeacherId { get; init; }
        public string TeacherName { get; init; } 
        public int? DepartmentId { get; init; }
        public string DepartmentName { get; init; }
    }

}
