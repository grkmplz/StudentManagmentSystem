using StudentCoursesManagement.Domain.Entities;
using System.Net.Http.Json;

namespace StudentCoursesManagement.Application.Services
{
    public class TeacherService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public TeacherService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<Teacher>> GetAllTeachersAsync()
        {
            var client = _httpClientFactory.CreateClient("AuthenticatedClient");
            var response = await client.GetAsync("http://localhost:5133/api/Teacher");
            if (!response.IsSuccessStatusCode)
                return new List<Teacher>();

            var data = await response.Content.ReadFromJsonAsync<List<Teacher>>();
            return data;
        }

        public async Task<bool> CreateTeacherAsync(Teacher teacher)
        {
            var client = _httpClientFactory.CreateClient("AuthenticatedClient");
            var response = await client.PostAsJsonAsync("http://localhost:5133/api/Teacher", teacher);
            return response.IsSuccessStatusCode;
        }

        public async Task<Teacher> GetTeacherByIdAsync(int teacherId)
        {
            var client = _httpClientFactory.CreateClient("AuthenticatedClient");
            var response = await client.GetAsync($"http://localhost:5133/api/Teacher/{teacherId}");
            if (!response.IsSuccessStatusCode) return null;

            var teacher = await response.Content.ReadFromJsonAsync<Teacher>();
            return teacher;
        }

        public async Task<bool> UpdateTeacherAsync(Teacher teacher)
        {
            var client = _httpClientFactory.CreateClient("AuthenticatedClient");
            var response = await client.PutAsJsonAsync("http://localhost:5133/api/Teacher", teacher);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteTeacherAsync(int teacherId)
        {
            var client = _httpClientFactory.CreateClient("AuthenticatedClient");
            var response = await client.DeleteAsync($"http://localhost:5133/api/Teacher/{teacherId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<List<Teacher>> SearchTeachers(string searchText)
        {
            var client = _httpClientFactory.CreateClient("AuthenticatedClient");
            var response = await client.GetAsync($"http://localhost:5133/api/Teacher/Search?query={searchText}");
            if (!response.IsSuccessStatusCode) return new List<Teacher>();

            var data = await response.Content.ReadFromJsonAsync<List<Teacher>>();
            return data;
        }
    }
}
