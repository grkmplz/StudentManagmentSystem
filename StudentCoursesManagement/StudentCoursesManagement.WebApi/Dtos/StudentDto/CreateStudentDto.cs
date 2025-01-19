using StudentCoursesManagement.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace StudentCoursesManagement.WebApi.Dtos.Student
{
    public record CreateStudentDto : IAuditEntity
    {
        [Required(ErrorMessage = "First name is required.")]
        public string? FirstName { get; init; }

        [Required(ErrorMessage = "Last name is required.")]
        public string? LastName { get; init; }

        [Required(ErrorMessage = "Username is required.")]
        public string? UserName { get; init; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; init; }

        public DateTime EnrollmentDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; init; }

        [Required(ErrorMessage = "Please confirm your password.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; init; }

        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool IsActive { get; set; }
    }
}