using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentCoursesManagement.Application.ViewModels;
using StudentCoursesManagement.Application.ViewModels.CourseViewModels;
using StudentCoursesManagement.Application.ViewModels.DashboardViewModels;
using StudentCoursesManagement.Application.ViewModels.StudentCoursesViewModels;
using StudentCoursesManagement.Domain.Entities;
using StudentCoursesManagement.Infrastructure.Config;
using StudentCoursesManagement.Infrastructure.Context;
using StudentCoursesManagement.WebApi.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace StudentCoursesManagement.WebApi.Services
{
    // Replace IDashboardService to return DashboardViewModel directly
    public class DashboardService : IDashboardService
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _context;
        private readonly RoleManager<Role> _roleManager;

        public DashboardService(UserManager<User> userManager, AppDbContext context, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
        }

        public async Task<DashboardViewModel> GetDashboardDataAsync()
        {
            var studentsUser = await _userManager.GetUsersInRoleAsync(UserRoles.Student);
            int totalStudents = studentsUser.Count;
            int totalTeachers = await _context.Teachers.CountAsync(t => t.IsActive);
            int totalDepartments = await _context.Departments.CountAsync(d => d.IsActive);
            int totalCourses = await _context.Courses.CountAsync();
            int totalRoles = await _roleManager.Roles.CountAsync();
            int totalAssignedCourses = await _context.UserCourses.CountAsync(x => x.IsActive);

            var recentStudentActivities = studentsUser
                .OrderByDescending(s => s.EnrollmentDate)
                .Take(5)
                .Select(u => new UserViewModel
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    EnrollmentDate = u.EnrollmentDate,
                    IsActive = u.IsActive
                })
                .ToList();

            var recentTeacherActivities = await _context.Teachers
                .Where(t => t.IsActive)
                .OrderByDescending(t => t.CreatedAt)
                .Take(5)
                .Select(t => new TeacherViewModel
                {
                    TeacherId = t.TeacherId,
                    FirstName = t.FirstName,
                    LastName = t.LastName,
                    Specialization = t.Specialization,
                    IsActive = t.IsActive,
                    CreatedAt = t.CreatedAt
                })
                .ToListAsync();

            var recentDepartmentActivities = await _context.Departments
                .Where(d => d.IsActive)
                .OrderByDescending(d => d.CreatedAt)
                .Take(5)
                .Select(d => new DepartmentViewModel
                {
                    DepartmentId = d.DepartmentId,
                    DepartmentName = d.DepartmentName,
                    IsActive = d.IsActive,
                    CreatedAt = d.CreatedAt
                })
                .ToListAsync();

            var recentCourseActivities = await _context.Courses
                .OrderByDescending(c => c.StartDate)
                .Take(5)
                .Select(c => new CourseViewModel
                {
                    CourseId = c.CourseId,
                    Title = c.Title,
                    Description = c.Description,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    TeacherId = c.TeacherId,
                    DepartmentId = c.DepartmentId,
                    CreatedAt = c.CreatedAt,
                    ModifiedAt = c.ModifiedAt,
                    IsActive = c.IsActive
                })
                .ToListAsync();

            var recentAssingToCourseStudents = await _context.UserCourses
                .Include(uc => uc.User)
                .Include(uc => uc.Course)
                .OrderByDescending(uc => uc.CreatedAt)
                .Take(5)
                .Where(uc => uc.IsActive)
                .Select(uc => new UserCourseViewModel
                {
                    CourseId = uc.Course.CourseId,
                    CourseTitle = uc.Course.Title,
                    UsersUserId = uc.User.Id,
                    CreatedAt = uc.CreatedAt,
                    IsActive = uc.IsActive,
                    User = new UserViewModel
                    {
                        Id = uc.User.Id,
                        FirstName = uc.User.FirstName,
                        LastName = uc.User.LastName,
                        Email = uc.User.Email
                    },
                    Course = new CourseViewModel
                    {
                        CourseId = uc.Course.CourseId,
                        Title = uc.Course.Title
                    }
                })
                .ToListAsync();

            var recentRoleAssignments = new List<RoleAssignmentViewModel>();
            foreach (var user in studentsUser)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Any())
                {
                    recentRoleAssignments.Add(new RoleAssignmentViewModel
                    {
                        UserName = user.UserName,
                        RoleName = roles.First()
                    });
                }
            }

            return new DashboardViewModel
            {
                TotalStudents = totalStudents,
                TotalTeachers = totalTeachers,
                TotalDepartments = totalDepartments,
                TotalCourses = totalCourses,
                TotalRoles = totalRoles,
                TotalAssignedCourses = totalAssignedCourses,

                RecentStudentActivities = recentStudentActivities,
                RecentTeacherActivities = recentTeacherActivities,
                RecentDepartmentActivities = recentDepartmentActivities,
                RecentCourseActivities = recentCourseActivities,
                RecentAssingToCourseStudents = recentAssingToCourseStudents,
                RecentRoleAssignments = recentRoleAssignments.Take(5).ToList()
            };
        }
    }
}
