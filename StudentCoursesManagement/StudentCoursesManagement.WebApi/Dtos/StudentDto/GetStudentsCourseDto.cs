namespace StudentCoursesManagement.WebApi.Dtos.Student
{
    public record GetStudentsCourseDto
    {
        public string UserId { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
    }
}
