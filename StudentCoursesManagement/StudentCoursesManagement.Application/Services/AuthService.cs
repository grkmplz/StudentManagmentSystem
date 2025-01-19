using Microsoft.AspNetCore.Authentication.Cookies;
using StudentCoursesManagement.Application.Handlers;
using StudentCoursesManagement.Application.ViewModels.AuthViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;

namespace StudentCoursesManagement.Application.Services
{
    public class AuthService
    {
        private readonly IHttpClientFactory _httpClient;
        public AuthService(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }
        public class TokenResponse
        {
            public string? Token { get; set; }
        }

        public class LoginReturnModel
        {
            public string? Role { get; set; }
            public ClaimsPrincipal? Principal { get; set; }
        }


        public async Task<List<string>> RegisterUser(RegisterViewModel registerViewModel)
        {
            var client = _httpClient.CreateClient();
            var response = await client.PostAsJsonAsync("http://localhost:5133/api/Account/Register", registerViewModel);

            if (!response.IsSuccessStatusCode)
            {
                var errors = await response.Content.ReadFromJsonAsync<List<string>>();

                if (errors != null)
                {
                    return errors;
                }
                else
                {
                    return null;
                }
            }

            return null;
        }

        public async Task<LoginReturnModel> Login(LoginViewModel loginViewModel)
        {
            var client = _httpClient.CreateClient();
            var response = await client.PostAsJsonAsync("http://localhost:5133/api/Account/Login", loginViewModel);

            if (response != null && response.IsSuccessStatusCode)
            {
                var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();
                ClaimToken._token = tokenResponse?.Token;

                if (tokenResponse != null)
                {
                    var role = DecodeTokenAndGetRole(tokenResponse.Token);
                    var email = DecodeTokenAndGetEmail(tokenResponse.Token);
                    var userId = DecodeTokenAndGetId(tokenResponse.Token);

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, email),
                        new Claim(ClaimTypes.Role, role),
                        new Claim(ClaimTypes.NameIdentifier, userId),
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    var loginReturnModel = new LoginReturnModel
                    {
                        Principal = principal,
                        Role = role,
                    };

                    return loginReturnModel;
                }

                return new LoginReturnModel();
            }

            return new LoginReturnModel();

        }

        private string DecodeTokenAndGetRole(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            return jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role)?.Value;
        }

        private string DecodeTokenAndGetEmail(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            return jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name)?.Value;
        }

        private string DecodeTokenAndGetId(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            return jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
        }

        public Task<string?> GetTokenAsync() => Task.FromResult(ClaimToken._token);
    }

}

       
