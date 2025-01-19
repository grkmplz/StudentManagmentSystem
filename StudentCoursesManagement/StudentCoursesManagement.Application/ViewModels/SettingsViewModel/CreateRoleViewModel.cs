using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentCoursesManagement.Application.ViewModels.SettingsViewModel
{
    public record CreateRoleViewModel
    {
        [Required(ErrorMessage = "Role name is required.")]
        [Display(Name = "Role Name")]
        public string? RoleName { get; set; }
    }
}