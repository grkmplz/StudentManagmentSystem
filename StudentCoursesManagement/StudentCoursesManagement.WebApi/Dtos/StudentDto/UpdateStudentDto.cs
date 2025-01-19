using System.ComponentModel.DataAnnotations;

namespace StudentCoursesManagement.WebApi.Dtos.Student
{
    public class UpdateStudentDto
    {
        [Required(ErrorMessage = "Id is required.")]
        public string Id { get; init; }

        public string FirstName { get; init; }

        public string LastName { get; init; }

        public DateTime EnrollmentDate { get; set; } 

        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool IsActive { get; set; }
    }
}