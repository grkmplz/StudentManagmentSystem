using System;
using System.Collections.Generic;

namespace StudentCoursesManagement.Domain.Entities
{
    public class Teacher : IAuditEntity
    {
        public int TeacherId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Specialization { get; set; }

        public ICollection<Course>? Courses { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedAt { get; set; }
        public bool IsActive { get; set; } = true;
    }
}