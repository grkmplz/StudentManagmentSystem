using System;
using System.Collections.Generic;

namespace StudentCoursesManagement.Domain.Entities
{
    public class Department : IAuditEntity
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        public ICollection<Course>? Courses { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedAt { get; set; }
        public bool IsActive { get; set; } = true;
    }
}