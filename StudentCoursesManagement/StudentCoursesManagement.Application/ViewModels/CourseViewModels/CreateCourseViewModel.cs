using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentCoursesManagement.Application.ViewModels.CourseViewModels
{
    public record CreateCourseViewModel
    {
        public int CourseId { get; set; }
        [Required] public string Title { get; set; }
        [Required] public string Description { get; set; }
        [Required] public DateTime StartDate { get; set; }
        [Required] public DateTime EndDate { get; set; }

        public bool IsActive { get; set; }
        public int? TeacherId { get; set; }
        public int? DepartmentId { get; set; }
    }
}