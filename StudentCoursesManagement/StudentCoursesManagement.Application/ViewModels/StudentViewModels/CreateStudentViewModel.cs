using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentCoursesManagement.Application.ViewModels.StudentViewModels
{
    public record CreateStudentViewModel
    {
        [Required(ErrorMessage = "Name Required.")]
        public string FirstName { get; init; }

        [Required(ErrorMessage = "Surname Required.")]
        public string LastName { get; init; }

        public string? UserName { get; set; }

        [Required(ErrorMessage = "Email Required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Format.")]
        public string Email { get; init; }

        [Required(ErrorMessage = "Enrollment Date Required.")]
        public DateTime EnrollmentDate { get; set; }

        [Required(ErrorMessage = "Password Required.")]
        [DataType(DataType.Password)]
        public string Password { get; init; }

        [Required(ErrorMessage = "Please verify password.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords doesn't match.")]
        public string ConfirmPassword { get; init; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? ModifiedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
