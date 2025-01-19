using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentCoursesManagement.Application.Services;
using StudentCoursesManagement.Application.ViewModels.DashboardViewModels;
using StudentCoursesManagement.Infrastructure.Config;

namespace StudentCoursesManagement.WebUI.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class DashboardsController : Controller
    {
        private readonly DashboardService _dashboardService;

        public DashboardsController( DashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response = await _dashboardService.GetDashboardDataAsync();
            return View(response);
        }
    }
}
