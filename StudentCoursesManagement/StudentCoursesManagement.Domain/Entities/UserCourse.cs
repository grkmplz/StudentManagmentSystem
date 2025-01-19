using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StudentCoursesManagement.Domain.Entities
{
    public class UserCourse : IAuditEntity
    {
        public int CoursesCourseId { get; set; }
        public string UsersUserId { get; set; }

        [ForeignKey(nameof(CoursesCourseId))]
        public Course Course { get; set; }

        [ForeignKey(nameof(UsersUserId))]
        public User User{ get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? ModifiedAt { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
