using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentCoursesManagement.Application.ViewModels.StudentCoursesViewModels;

namespace StudentCoursesManagement.Application.ViewModels.StudentViewModels
{
    public record StudentDetailsViewModel
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
