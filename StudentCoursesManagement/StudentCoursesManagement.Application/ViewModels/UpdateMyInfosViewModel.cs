using StudentCoursesManagement.Application.ViewModels.StudentCoursesViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentCoursesManagement.Application.ViewModels
{
    public record UpdateMyInfosViewModel
    {
        public string StudentId { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? UserName { get; set; }

        public DateTime? EnrollmentDate { get; set; }

        public string? Email { get; set; }

        public ICollection<UserCourseViewModel>? UserCourses { get; set; }

        // Audit Properties
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
