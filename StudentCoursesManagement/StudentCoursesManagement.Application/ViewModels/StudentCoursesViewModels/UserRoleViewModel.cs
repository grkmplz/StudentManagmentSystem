using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentCoursesManagement.Application.ViewModels.StudentCoursesViewModels
{
    public record UserRoleViewModel
    {
        public string? UserId { get; init; }
        public string? UserName { get; init; }
        public bool IsSelected { get; init; }
    }
}
