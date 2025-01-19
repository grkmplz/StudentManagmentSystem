using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentCoursesManagement.Infrastructure.Config;
using StudentCoursesManagement.WebApi.Services.Interfaces;
using StudentCoursesManagement.Application.ViewModels.DashboardViewModels;
using System.Threading.Tasks;

namespace StudentCoursesManagement.WebApi.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("GetDashboardData")]
        public async Task<IActionResult> GetDashboardData()
        {
            DashboardViewModel dashboardDataModel = await _dashboardService.GetDashboardDataAsync();
            return Ok(dashboardDataModel);
        }
    }
}