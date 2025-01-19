using Xunit;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using StudentCoursesManagement.Infrastructure.Context;
using StudentCoursesManagement.Domain.Entities;
using System.Linq;
using System.Threading.Tasks;
using StudentCoursesManagement.WebApi.Services;
using StudentCoursesManagement.WebApi.Dtos.DashboardDto;

namespace StudentCoursesManagement.XunitTest
{
    public class DashboardTests
    {
        /// PREPARING TEST: Builds an in-memory DB and Identity for dashboard.
        private (AppDbContext, UserManager<User>, RoleManager<Role>) CreateContextWithIdentity()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase($"DashTestDB_{Guid.NewGuid()}")
                .Options;

            var ctx = new AppDbContext(options);

            // Clear and recreate
            ctx.Database.EnsureDeleted();
            ctx.Database.EnsureCreated();

            var userStore = new UserStore<User, Role, AppDbContext, string>(ctx);
            var roleStore = new RoleStore<Role, AppDbContext, string>(ctx);

            var um = new UserManager<User>(
                userStore,
                null,
                new PasswordHasher<User>(),
                null, null, null, null, null, null
            );

            var rm = new RoleManager<Role>(
                roleStore,
                null, null, null, null);

            return (ctx, um, rm);
        }

        /// PREPARING TEST: Adds roles, students, and courses for dashboard checks.
        private async Task SeedData(AppDbContext context, UserManager<User> um, RoleManager<Role> rm)
        {
            var r1 = new Role { Name = "Admin", NormalizedName = "ADMIN", IsActive = true };
            var r2 = new Role { Name = "Student", NormalizedName = "STUDENT", IsActive = true };
            await rm.CreateAsync(r1);
            await rm.CreateAsync(r2);

            var stu1 = new User { UserName = "stu1@example.com", Email = "stu1@example.com", FirstName = "Ali", IsActive = true };
            var stu2 = new User { UserName = "stu2@example.com", Email = "stu2@example.com", FirstName = "Ayse", IsActive = true };
            await um.CreateAsync(stu1);
            await um.CreateAsync(stu2);
            await um.AddToRoleAsync(stu1, "Student");
            await um.AddToRoleAsync(stu2, "Student");

            var c1 = new Course
            {
                Title = "Algebra 101",
                Description = "Basic Algebra",
                StartDate = DateTime.Now.AddDays(-5),
                EndDate = DateTime.Now.AddDays(5),
                IsActive = true
            };
            var c2 = new Course
            {
                Title = "Quantum Mechanics",
                Description = "Advanced Physics",
                StartDate = DateTime.Now.AddDays(-2),
                EndDate = DateTime.Now.AddDays(8),
                IsActive = true
            };
            context.Courses.AddRange(c1, c2);

            await context.SaveChangesAsync();
        }

        /// TESTING CREATE & READ: Ensures assigned course is reflected in the dashboard’s total.
        [Fact]
        public async Task Dashboard_ReflectsAssignedCourse()
        {
            var (context, um, rm) = CreateContextWithIdentity();
            await SeedData(context, um, rm);

            var userCourse = new UserCourse
            {
                UsersUserId = (await context.Users.FirstAsync()).Id,
                CoursesCourseId = (await context.Courses.FirstAsync()).CourseId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
            context.UserCourses.Add(userCourse);
            await context.SaveChangesAsync();

            var service = new DashboardService(um, context, rm);
            var dash = await service.GetDashboardDataAsync();

            Assert.Equal(1, dash.TotalAssignedCourses);
        }
    }
}
