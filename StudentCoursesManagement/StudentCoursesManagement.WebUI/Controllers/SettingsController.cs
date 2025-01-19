using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentCoursesManagement.Application.Services;
using StudentCoursesManagement.Application.ViewModels.SettingsViewModel;
using StudentCoursesManagement.Application.ViewModels.StudentCoursesViewModels;
using StudentCoursesManagement.Infrastructure.Config;

namespace StudentCoursesManagement.WebUI.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class SettingsController : Controller
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly RoleService _roleService;

        public SettingsController(IHttpClientFactory httpClient, RoleService roleService)
        {
            _httpClient = httpClient;
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response = await _roleService.GetAllRolesAsync();
            return View(response);
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View(new CreateRoleViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _roleService.CreateRoleAsync(model);

                if (!response)
                {
                    return View(model);
                }

                return RedirectToAction("Index","Settings");
              
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string roleId)
        {
            var model = await _roleService.UpdateRoleGetAsync(roleId);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var response = await _roleService.UpdateRolePostAsync(model);

            if (! response )
            {
                return View(model);
            }

            return RedirectToAction("Index","Settings");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var response = await _roleService.DeleteRole(id);

            if (!response)
            {
                ModelState.AddModelError(string.Empty, "Something went wrong.");
            }

            return RedirectToAction("Index","Settings");
        }
    }
}
