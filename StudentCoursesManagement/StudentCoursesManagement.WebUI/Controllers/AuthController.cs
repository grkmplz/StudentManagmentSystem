using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using StudentCoursesManagement.Application.Services;
using StudentCoursesManagement.Application.ViewModels.AuthViewModels;
using Microsoft.AspNetCore.Authorization;
using StudentCoursesManagement.Infrastructure.Config;


namespace StudentCoursesManagement.WebUI.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService, IHttpClientFactory httpClient)
        {
            _authService = authService;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var errors = _authService.RegisterUser(registerViewModel);

                if (errors.Result != null && errors.Result.Count <= 1)
                {
                    ViewBag.MailError = (await errors)[0];
                    return View();
                }
                else if (errors.Result != null && errors.Result.Count >= 1)
                {
                    ViewBag.MailError = (await errors)[1];
                    return View();
                }

                ViewBag.RegisterSuccess = true;
                TempData["SuccessMessage"] = "User created successfully.";

                return RedirectToAction("Login", "Auth");
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var response = await _authService.Login(loginViewModel);

                if (response.Role == UserRoles.None)
                {
                    ModelState.TryAddModelError(string.Empty, "Your role did not defined. Contact with Admin please.");
                    return View();
                }

                if (response.Principal != null)
                {
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, response.Principal);

                    if (response.Role == UserRoles.Admin)
                    {
                        return RedirectToAction("Index", "Dashboards");
                    }

                    if (response.Role == UserRoles.Student)
                    {
                        return RedirectToAction("MyCourses", "Students");
                    }
                }
            }

            ModelState.TryAddModelError(string.Empty, "Invalid user information.");
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction("Login", "Auth");
        }



    }
}
