using Microsoft.AspNetCore.Http;
using StudentCoursesManagement.Application.ViewModels.DashboardViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using StudentCoursesManagement.Application.Handlers;

namespace StudentCoursesManagement.Application.Services
{
    public class DashboardService
    {
        private readonly IHttpClientFactory _httpClient;

        public DashboardService(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<DashboardViewModel> GetDashboardDataAsync()
        {
            var client = _httpClient.CreateClient("AuthenticatedClient");
            var response = await client.GetAsync("http://localhost:5133/api/Dashboard/GetDashboardData");

            if (!response.IsSuccessStatusCode || response == null)
            {
                return new DashboardViewModel();
            }

            var dashboardData = await response.Content.ReadFromJsonAsync<DashboardViewModel>();
            return dashboardData;
        }

    }
}
