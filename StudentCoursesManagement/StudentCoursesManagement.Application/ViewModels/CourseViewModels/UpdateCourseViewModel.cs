using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentCoursesManagement.Application.ViewModels.CourseViewModels
{
    public record UpdateCourseViewModel
    {
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "StartDate is required.")]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "EndDate is required.")]
        public DateTime? EndDate { get; set; }

        public bool IsActive { get; set; }
        public int? TeacherId { get; set; }
        public int? DepartmentId { get; set; }
    }

}
