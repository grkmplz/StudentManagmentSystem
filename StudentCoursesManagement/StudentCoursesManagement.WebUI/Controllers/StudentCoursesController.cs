
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentCoursesManagement.Application.ViewModels.CourseViewModels;
using StudentCoursesManagement.Application.ViewModels.StudentCoursesViewModels;
using StudentCoursesManagement.Application.ViewModels.StudentViewModels;
using System.Net.Http;
using System.Net.Http.Json;
using StudentCoursesManagement.Infrastructure.Config;

namespace StudentCoursesManagement.WebUI.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class StudentCoursesController : Controller
    {
        private readonly IHttpClientFactory _httpClient;

        public StudentCoursesController(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> AssignCourse(string userId)
        {
            var client = _httpClient.CreateClient("AuthenticatedClient");
            var student = await client.GetAsync($"http://localhost:5133/api/Student/GetStudentById?userId={userId}");

            if (student.IsSuccessStatusCode is false)
            {
                ModelState.TryAddModelError(string.Empty, "Something went wrong.");
                return View();
            }
            var serializedStudent = await student.Content.ReadFromJsonAsync<StudentDetailsViewModel>();
            serializedStudent.StudentId = userId;

            var availableCourses = await client.GetAsync("http://localhost:5133/api/Course/GetAllCourses");
            var serializedCourse = await availableCourses.Content.ReadFromJsonAsync<List<CourseViewModel>>();
            var userCourses = serializedStudent.UserCourses.Where(x=>x.IsActive==true).Select(x => x.CourseId).ToList();

            var model = new AssignCourseViewModel
            {
                StudentDetailsViewModel = serializedStudent,
                AvailableCourses = serializedCourse,
                SelectedCourseIds = userCourses,
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AssignCourse(AssignCourseViewModel model)
        {
            var client = _httpClient.CreateClient("AuthenticatedClient");
            string userId = model.StudentDetailsViewModel.StudentId;
            var getStudent = await client.GetAsync($"http://localhost:5133/api/Student/GetStudentById?userId={userId}");

            if (getStudent.IsSuccessStatusCode is false)
            {
                ModelState.TryAddModelError(string.Empty, "Something went wrong.");
                return View();
            }

            var serializedStudent = await getStudent.Content.ReadFromJsonAsync<StudentDetailsViewModel>();
            serializedStudent.StudentId = userId;

            var availableCourses = await client.GetAsync("http://localhost:5133/api/Course/GetAllCourses");
            var serializedCourse = await availableCourses.Content.ReadFromJsonAsync<List<CourseViewModel>>();

            if (model.AvailableCourses == null)
            {
                model.AvailableCourses = serializedCourse;
            }

            model.StudentDetailsViewModel = serializedStudent;

            var assignModel = new AssignCourseToStudentViewModel
            {
                CourseIds = model.SelectedCourseIds,
                StudentId = serializedStudent.StudentId,
            };

            var assignCourseToStudent = await client.PostAsJsonAsync("http://localhost:5133/api/Student/AssignCourseToStudents", assignModel);

            return RedirectToAction("Details", "Students", new { userId = model.StudentDetailsViewModel.StudentId });
        }
    }
}
