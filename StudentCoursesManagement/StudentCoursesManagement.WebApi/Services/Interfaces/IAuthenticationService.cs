using Microsoft.AspNetCore.Identity;
using StudentCoursesManagement.WebApi.Dtos.User;

namespace StudentCoursesManagement.WebApi.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistrationDto);
        Task<bool> ValidateUser(UserForAuthenticationDto userForAuthenticationDto);
        Task<string> CreateToken();
    }
}
