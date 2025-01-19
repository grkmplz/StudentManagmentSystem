using Microsoft.AspNetCore.Http;
using StudentCoursesManagement.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace StudentCoursesManagement.Application.Handlers
{
    public interface ITokenService
    {
        Task<string> GetTokenAsync();
    }

    public class AuthenticatedHttpClientHandler : DelegatingHandler
    {
        private readonly AuthService _authService;

        public AuthenticatedHttpClientHandler(AuthService authService)
        {
            _authService = authService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _authService.GetTokenAsync();
            
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
