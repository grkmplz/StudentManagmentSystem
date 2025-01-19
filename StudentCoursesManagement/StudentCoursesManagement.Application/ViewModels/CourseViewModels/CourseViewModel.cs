using StudentCoursesManagement.Application.ViewModels.StudentCoursesViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentCoursesManagement.Application.ViewModels.CourseViewModels
{
    public record CourseViewModel
    {
        public int CourseId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ICollection<UserCourseViewModel>? UserCourses { get; set; }
        public int? TeacherId { get; set; }
        public string TeacherName { get; set; }
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; } 
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
