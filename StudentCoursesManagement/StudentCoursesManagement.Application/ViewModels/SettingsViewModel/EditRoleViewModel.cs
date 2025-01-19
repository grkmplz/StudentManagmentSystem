using StudentCoursesManagement.Application.ViewModels.StudentCoursesViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentCoursesManagement.Application.ViewModels.SettingsViewModel
{
    public class EditRoleViewModel
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public List<UserRoleViewModel> Users { get; set; }
    }
}
