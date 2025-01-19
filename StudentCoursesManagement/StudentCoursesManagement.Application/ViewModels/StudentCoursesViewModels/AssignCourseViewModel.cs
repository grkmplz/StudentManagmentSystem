using StudentCoursesManagement.Application.ViewModels.CourseViewModels;
using StudentCoursesManagement.Application.ViewModels.StudentViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentCoursesManagement.Application.ViewModels.StudentCoursesViewModels
{
    public record AssignCourseViewModel
    {
        public StudentDetailsViewModel ?StudentDetailsViewModel { get; set; }
        public List<CourseViewModel>? AvailableCourses { get; set; }
        public List<int> ?SelectedCourseIds { get; set; }
    }
}
