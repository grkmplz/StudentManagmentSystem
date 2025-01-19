using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentCoursesManagement.Application.ViewModels.SettingsViewModel
{
    public class GetUserByRoleViewModel
    {
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public bool IsSelected { get; set; }
    }
}
