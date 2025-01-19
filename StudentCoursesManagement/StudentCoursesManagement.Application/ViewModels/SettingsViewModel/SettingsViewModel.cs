using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentCoursesManagement.Application.ViewModels.SettingsViewModel
{
    public record SettingsViewModel
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public int UserCount { get; set; }
        public bool IsActive { get; set; }
    }
}
