using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using StudentCoursesManagement.Domain.Entities;
using StudentCoursesManagement.WebApi.Dtos.User;
using StudentCoursesManagement.WebApi.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using StudentCoursesManagement.Infrastructure.Config;

namespace StudentCoursesManagement.WebApi.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        public AuthenticationService(UserManager<User> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }
        private User _user;


        public async Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistrationDto)
        {
            var user = new User
            {
                Email = userForRegistrationDto.Email,
                UserName = userForRegistrationDto.Email,
            };         

            var result = await _userManager.CreateAsync(user, userForRegistrationDto.Password);
            // In scenario, admin assign to "none" role to new user in the beginning.
            await _userManager.AddToRoleAsync(user, UserRoles.None);

            if (result.Succeeded)
            {
                return result;
            }

            return result;
        }

        public async Task<bool> ValidateUser(UserForAuthenticationDto userForAuthenticationDto)
        {
            string email = userForAuthenticationDto.Email;
            string password = userForAuthenticationDto.Password;    

            _user = await _userManager.FindByEmailAsync(email);
            var checkStatus = await _userManager.CheckPasswordAsync(_user,password);

            return checkStatus;
        }

        public async Task<string> CreateToken()
        {
            var signInCredentials = GetSignInCredentials();
            var claims = await GetClaims();
            var tokenOptions = GenerateTokenOptions(signInCredentials, claims);

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        private SigningCredentials GetSignInCredentials()
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = Encoding.UTF8.GetBytes(jwtSettings["secretKey"]);
            var secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, _user.Email ?? string.Empty),
                new Claim("userId", _user.Id),
                new Claim("username", _user.UserName ?? string.Empty),
            };

            var roles = await _userManager.GetRolesAsync(_user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, _user.Id));
            }

            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signinCredentials, List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");

            var tokenOption = new JwtSecurityToken(
                issuer: jwtSettings["validIssuer"],
                audience: jwtSettings["validAudience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["expires"])),
                signingCredentials: signinCredentials);

            return tokenOption;
        }
    }
}
