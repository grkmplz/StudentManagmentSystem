using StudentCoursesManagement.Application.ViewModels.CourseViewModels;
using StudentCoursesManagement.Application.ViewModels.StudentCoursesViewModels;
using System;
using System.Collections.Generic;

namespace StudentCoursesManagement.Application.ViewModels.DashboardViewModels
{
    public class DashboardViewModel
    {
        public int TotalStudents { get; set; }
        public int TotalTeachers { get; set; }
        public int TotalDepartments { get; set; }
        public int TotalCourses { get; set; }
        public int TotalRoles { get; set; }
        public int TotalAssignedCourses { get; set; }

        public List<UserViewModel> RecentStudentActivities { get; set; }
        public List<TeacherViewModel> RecentTeacherActivities { get; set; }
        public List<DepartmentViewModel> RecentDepartmentActivities { get; set; }
        public List<CourseViewModel> RecentCourseActivities { get; set; }
        public List<RoleAssignmentViewModel> RecentRoleAssignments { get; set; }
        public List<UserCourseViewModel> RecentAssingToCourseStudents { get; set; }
    }


    public class RoleAssignmentViewModel
    {
        public string UserName { get; set; }
        public string RoleName { get; set; }
    }

    public class TeacherViewModel
    {
        public int TeacherId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Specialization { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class DepartmentViewModel
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
