namespace StudentCoursesManagement.WebApi.Dtos.Student
{
    public record AssignCourseToStudentsDto
    {
        public string StudentId { get; init; }
        public List<int>? CourseIds { get; init; }
    }
}
