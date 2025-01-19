namespace StudentCoursesManagement.WebApi.Dtos.Course
{
    public record UpdateCourseDto
    {
        public int CourseId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public int? TeacherId { get; set; }
        public int? DepartmentId { get; set; }
    }
}
