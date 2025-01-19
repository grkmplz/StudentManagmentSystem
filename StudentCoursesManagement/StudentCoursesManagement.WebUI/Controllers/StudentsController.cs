using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentCoursesManagement.Application.Services;
using StudentCoursesManagement.Application.ViewModels;
using StudentCoursesManagement.Application.ViewModels.CourseViewModels;
using StudentCoursesManagement.Application.ViewModels.StudentCoursesViewModels;
using StudentCoursesManagement.Application.ViewModels.StudentViewModels;
using System.Security.Claims;
using StudentCoursesManagement.Infrastructure.Config;

namespace StudentCoursesManagement.WebUI.Controllers
{
    public class StudentsController : Controller
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly StudentService _studentService;

        public StudentsController(IHttpClientFactory httpClient, StudentService studentService)
        {
            _httpClient = httpClient;
            _studentService = studentService;
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response = await _studentService.GetAllStudentsAsync();
            return View(response);
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost]
        public async Task<IActionResult> Create(CreateStudentViewModel createStudentViewModel)
        {
            try
            {
                var emailExists = await IsEmailAlreadyInUse(createStudentViewModel.Email);
                if (emailExists)
                {
                    ViewBag.EmailErrorMessage = "Email address is already in use.";
                    return View(createStudentViewModel);
                }

                var response = await _studentService.CreateStudentAsync(createStudentViewModel);

                if (!response)
                {
                    ViewBag.ErrorMessage = "Something went wrong.";
                    return View(createStudentViewModel);
                }

                return RedirectToAction("Index", "Students");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An unexpected error occurred: " + ex.Message;
                return View(createStudentViewModel);
            }
        }

        private async Task<bool> IsEmailAlreadyInUse(string email)
        {
            var users = await _studentService.GetAllUsers();
            return users.Any(u => string.Equals(u.Email, email, StringComparison.OrdinalIgnoreCase));
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet]
        public async Task<IActionResult> Edit(string userId)
        {
            var response = await _studentService.GetStudentByIdAsync(userId);
            return View(response);
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateStudentViewModel updateStudentViewModel)
        {
            var response = await _studentService.UpdateStudentAsync(updateStudentViewModel);

            if (!response)
            {
                ModelState.AddModelError(string.Empty, "Something went wrong.");
                return View();
            }

            return RedirectToAction("Index", "Students");
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet]
        public async Task<IActionResult> Details(string userId)
        {
            var client = _httpClient.CreateClient("AuthenticatedClient");
            var response = await client.GetAsync($"http://localhost:5133/api/Student/GetStudentById?userId={userId}");
            
            if (response.IsSuccessStatusCode is false)
            {
                ModelState.TryAddModelError(string.Empty, "Something went wrong.");
                return View();
            }

            var serializedData = await response.Content.ReadFromJsonAsync<StudentDetailsViewModel>();

            serializedData.StudentId = userId;

            return View(serializedData);
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _studentService.DeleteStudent(id);

            if (!response)
            {
                ModelState.TryAddModelError(string.Empty, "Something went wrong.");
            }

            return RedirectToAction("Index", "Students");
        }

        [Authorize(Roles = UserRoles.Student)]
        [HttpGet]
        public async Task<IActionResult> MyCourses()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _studentService.GetMyCourses(userId);

            return View(response);
        }

        [Authorize(Roles = UserRoles.Student)]
        [HttpGet]
        public async Task<IActionResult> Settings(string empty)
        {
            var client = _httpClient.CreateClient("AuthenticatedClient");
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var responseMyCourses =
                await client.GetAsync($"http://localhost:5133/api/Student/GetStudentById?userId={userId}");
            var serializedResponseMyCourses =
                await responseMyCourses.Content.ReadFromJsonAsync<StudentDetailsViewModel>();
            serializedResponseMyCourses.StudentId = userId;
            return View(serializedResponseMyCourses);
        }

        [Authorize(Roles = UserRoles.Student)]
        [HttpPost]
        public async Task<IActionResult> Settings(StudentDetailsViewModel model)
        {
            var client = _httpClient.CreateClient("AuthenticatedClient");
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            model.StudentId = userId;

            var responseMyCourses =
                await client.PutAsJsonAsync($"http://localhost:5133/api/Student/UpdateMyInfos", model);

            if (!responseMyCourses.IsSuccessStatusCode)
            {
                return View(model);
            }

            return View();
        }
    }
}