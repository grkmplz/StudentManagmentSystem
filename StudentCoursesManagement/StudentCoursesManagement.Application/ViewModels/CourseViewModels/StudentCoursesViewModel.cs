using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentCoursesManagement.Application.ViewModels.CourseViewModels
{
    public record StudentCoursesViewModel
    {
        public string UserId { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
    }
}

