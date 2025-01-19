using StudentCoursesManagement.Application.ViewModels.DashboardViewModels;
using StudentCoursesManagement.WebApi.Dtos.DashboardDto;

namespace StudentCoursesManagement.WebApi.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardViewModel> GetDashboardDataAsync();
    }
}
