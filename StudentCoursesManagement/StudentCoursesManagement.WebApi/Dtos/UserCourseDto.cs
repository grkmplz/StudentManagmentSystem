using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace StudentCoursesManagement.WebApi.Dtos
{
    public record UserCourseDto
    {
        public int CourseId { get; set; }
        public string CourseTitle { get; set; }
        public DateTime CourseStartDate { get; set; }
        public DateTime CourseEndDate { get; set; }
        public bool IsActive { get; set; }
    }
}
