using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentCoursesManagement.Application.ViewModels.StudentViewModels
{
    public record UpdateStudentViewModel
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "Name required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Surname required.")]
        public string LastName { get; set; }

        public DateTime EnrollmentDate { get; set; }

        //public ICollection<StudentCourse>? StudentCourses { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
