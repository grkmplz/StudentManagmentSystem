using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentCoursesManagement.Domain.Entities
{
    public class User : IdentityUser, IAuditEntity
    {

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public DateTime EnrollmentDate { get; set; } = DateTime.Now;

        public ICollection<UserCourse>? UserCourses { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? ModifiedAt { get; set; }
        public bool IsActive { get; set; } = true;
    }
}