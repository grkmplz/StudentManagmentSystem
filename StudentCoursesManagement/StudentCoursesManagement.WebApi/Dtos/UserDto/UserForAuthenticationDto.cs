using System.ComponentModel.DataAnnotations;

namespace StudentCoursesManagement.WebApi.Dtos.User
{
    public record UserForAuthenticationDto
    {
        [Required(ErrorMessage = "Required email.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; init; }

        [Required(ErrorMessage = "Required password.")]
        [DataType(DataType.Password)]
        public string Password { get; init; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; init; }
    }
}