using StudentCoursesManagement.Application.ViewModels.CourseViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StudentCoursesManagement.Application.ViewModels.StudentCoursesViewModels
{
    public record UserCourseViewModel
    {
        public int CourseId { get; set; }
        public string CourseTitle { get; set; }
        public DateTime CourseStartDate { get; set; }
        public DateTime CourseEndDate { get; set; }

        public int CoursesCourseId { get; set; }
        public string UsersUserId { get; set; }

        [ForeignKey(nameof(CoursesCourseId))]
        public CourseViewModel Course { get; set; }

        [ForeignKey(nameof(UsersUserId))]
        public UserViewModel User { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
