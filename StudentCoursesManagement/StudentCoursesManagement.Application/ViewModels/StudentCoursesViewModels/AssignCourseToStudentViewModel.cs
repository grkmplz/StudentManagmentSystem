using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentCoursesManagement.Application.ViewModels.StudentCoursesViewModels
{
    public record AssignCourseToStudentViewModel
    {
        public string StudentId { get; init; }
        public List<int> CourseIds { get; init; }
    }
}
