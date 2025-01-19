using StudentCoursesManagement.Application.ViewModels.CourseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace StudentCoursesManagement.Application.Services
{
    public class CourseService
    {
        private readonly IHttpClientFactory _httpClient;

        public CourseService(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CourseViewModel>> GetAllCoursesAsync() 
        {
            var client = _httpClient.CreateClient("AuthenticatedClient");

            var response = await client.GetAsync("http://localhost:5133/api/Course/GetAllCourses");

            if (response == null || response.IsSuccessStatusCode == false)
            {
                return new List<CourseViewModel>();
            }

            var serializedData = await response.Content.ReadFromJsonAsync<List<CourseViewModel>>();

            return serializedData;
        }

        public async Task<CourseViewModel> GetCourseByIdAsync(int id)
        {
            var client = _httpClient.CreateClient("AuthenticatedClient");
            var response = await client.GetAsync($"http://localhost:5133/api/Course/GetCourseById?courseId={id}");

            if (response == null || response.IsSuccessStatusCode == false)
            {
                return new CourseViewModel();
            }

            var serializedData = await response.Content.ReadFromJsonAsync<CourseViewModel>();

            return serializedData;
        }

        public async Task<bool> CreateCourseAsync(CreateCourseViewModel model)
        {
            var client = _httpClient.CreateClient("AuthenticatedClient");

            var response = await client.PostAsJsonAsync("http://localhost:5133/api/Course/CreateCourse", model);

            if (response.IsSuccessStatusCode == false || response == null)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> UpdateCourseAsync(UpdateCourseViewModel model)
        {
            var client = _httpClient.CreateClient("AuthenticatedClient");

            var response = await client.PutAsJsonAsync($"http://localhost:5133/api/Course/UpdateCourse", model);

            if (response is null || response.IsSuccessStatusCode is false)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> DeleteCourseAsync(int id)
        {
            var client = _httpClient.CreateClient("AuthenticatedClient");

            var response = await client.DeleteAsync($"http://localhost:5133/api/Course/DeleteCourse?id={id}");

            if (response is null || response.IsSuccessStatusCode is false)
            {
                return false;
            }

            return true;
        }
    }
}
