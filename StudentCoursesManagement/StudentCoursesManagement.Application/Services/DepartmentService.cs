using StudentCoursesManagement.Domain.Entities;
using System.Net.Http.Json;

namespace StudentCoursesManagement.Application.Services
{
    public class DepartmentService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DepartmentService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<Department>> GetAllDepartmentsAsync()
        {
            var client = _httpClientFactory.CreateClient("AuthenticatedClient");
            var response = await client.GetAsync("http://localhost:5133/api/Department");
            if (!response.IsSuccessStatusCode) return new List<Department>();

            return await response.Content.ReadFromJsonAsync<List<Department>>();
        }

        public async Task<Department> GetDepartmentByIdAsync(int id)
        {
            var client = _httpClientFactory.CreateClient("AuthenticatedClient");
            var response = await client.GetAsync($"http://localhost:5133/api/Department/{id}");
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<Department>();
        }

        public async Task<bool> CreateDepartmentAsync(Department dept)
        {
            var client = _httpClientFactory.CreateClient("AuthenticatedClient");
            var response = await client.PostAsJsonAsync("http://localhost:5133/api/Department", dept);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateDepartmentAsync(Department dept)
        {
            var client = _httpClientFactory.CreateClient("AuthenticatedClient");
            var response = await client.PutAsJsonAsync($"http://localhost:5133/api/Department", dept);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteDepartmentAsync(int id)
        {
            var client = _httpClientFactory.CreateClient("AuthenticatedClient");
            var response = await client.DeleteAsync($"http://localhost:5133/api/Department/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
