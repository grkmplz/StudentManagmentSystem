using StudentCoursesManagement.Application.ViewModels.StudentCoursesViewModels;
using StudentCoursesManagement.Domain.Entities;
using System.Collections.Generic;

namespace StudentCoursesManagement.WebApi.Dtos.DashboardDto
{
    public class DashboardDto
    {
        public int TotalStudents { get; set; }
        public int TotalTeachers { get; set; } 
        public int TotalDepartments { get; set; }
        public int TotalCourses { get; set; }
        public int TotalRoles { get; set; }
        public int TotalAssignedCourses { get; set; }
        public List<Domain.Entities.User> RecentStudentActivities { get; set; }
        public List<Teacher> RecentTeacherActivities { get; set; }
        public List<Department> RecentDepartmentActivities { get; set; }
        public List<Domain.Entities.Course> RecentCourseActivities { get; set; }
        public List<RoleAssignmentDto> RecentRoleAssignments { get; set; }
        public List<UserCourseViewModel> RecentAssingToCourseStudents { get; set; }
    }
}