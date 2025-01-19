using StudentCoursesManagement.Application.ViewModels;
using StudentCoursesManagement.Application.ViewModels.StudentViewModels;
using System.Net.Http.Json;

namespace StudentCoursesManagement.Application.Services
{
    public class StudentService
    {
        private readonly IHttpClientFactory _httpClient;

        public StudentService(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<UserViewModel>> GetAllStudentsAsync()
        {
            var client = _httpClient.CreateClient("AuthenticatedClient");

            var response = await client.GetAsync("http://localhost:5133/api/Student/GetAllStudents");

            if (!response.IsSuccessStatusCode || response == null)
            {
                return new List<UserViewModel>();
            }

            var serializedData = await response.Content.ReadFromJsonAsync<List<UserViewModel>>();

            return serializedData;
        }
        public async Task<List<UserViewModel>> GetAllUsers()
        {
            var client = _httpClient.CreateClient("AuthenticatedClient");

            var response = await client.GetAsync("http://localhost:5133/api/Student/GetAllUsers");

            if (!response.IsSuccessStatusCode || response == null)
            {
                return new List<UserViewModel>();
            }

            var serializedData = await response.Content.ReadFromJsonAsync<List<UserViewModel>>();

            return serializedData;
        }

        public async Task<UserViewModel> GetStudentByIdAsync(string userId)
        {
            var client = _httpClient.CreateClient("AuthenticatedClient");
            var response = await client.GetAsync($"http://localhost:5133/api/Student/GetStudentById?userId={userId}");

            if (response.IsSuccessStatusCode == false)
            {
                return new UserViewModel();
            }

            var serializedData = await response.Content.ReadFromJsonAsync<UserViewModel>();

            return serializedData;
        }

        public async Task<bool> UpdateStudentAsync(UpdateStudentViewModel model)
        {
            var client = _httpClient.CreateClient("AuthenticatedClient");
            var response = await client.PutAsJsonAsync($"http://localhost:5133/api/Student/UpdateStudent", model);

            if (response.IsSuccessStatusCode is false)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> CreateStudentAsync(CreateStudentViewModel model)
        {
            model.UserName = (model.FirstName + model.LastName).ToLower();
    
            var client = _httpClient.CreateClient("AuthenticatedClient");
            var response = await client.PostAsJsonAsync("http://localhost:5133/api/Student/CreateStudent", model);

            if (response is null || response.IsSuccessStatusCode is false)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> DeleteStudent(string id)
        {
            var client = _httpClient.CreateClient("AuthenticatedClient");
            var response = await client.DeleteAsync($"http://localhost:5133/api/Student/DeleteStudent?id={id}");

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            return true;
        }

        public async Task<List<MyCoursesViewModel>> GetMyCourses(string userId)
        {
            var client = _httpClient.CreateClient("AuthenticatedClient");
            var responseMyCourses = await client.GetAsync($"http://localhost:5133/api/Student/MyCourses?userId={userId}");
            var serializedResponseMyCourses = await responseMyCourses.Content.ReadFromJsonAsync<List<MyCoursesViewModel>>();

            return serializedResponseMyCourses;
        }
    }
}
