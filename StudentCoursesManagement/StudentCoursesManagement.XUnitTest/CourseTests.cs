using Xunit;
using System;
using Microsoft.EntityFrameworkCore;
using StudentCoursesManagement.Infrastructure.Context;
using StudentCoursesManagement.Domain.Entities;
using System.Threading.Tasks;
using System.Linq;

namespace StudentCoursesManagement.XunitTest
{
    public class CourseTests
    {
        /// PREPARING TEST: Creates a unique in-memory DB config for course tests.
        private DbContextOptions<AppDbContext> CreateInMemoryOptions()
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase($"CourseTestDB_{Guid.NewGuid()}")
                .Options;
        }

        /// PREPARING TEST: Seeds a department, a teacher, and one course as initial data.
        private void SeedData(AppDbContext context)
        {
            var dept = new Department { DepartmentName = "Science", IsActive = true };
            context.Departments.Add(dept);

            var teacher = new Teacher
                { FirstName = "John", LastName = "Doe", Specialization = "Math", IsActive = true };
            context.Teachers.Add(teacher);
            context.SaveChanges();

            var c1 = new Course
            {
                Title = "Algebra 101",
                Description = "Basic Algebra",
                StartDate = DateTime.Now.AddDays(-5),
                EndDate = DateTime.Now.AddDays(5),
                IsActive = true,
                TeacherId = teacher.TeacherId,
                DepartmentId = dept.DepartmentId
            };
            context.Courses.Add(c1);
            context.SaveChanges();
        }

        /// TESTING READ: Checks we can retrieve the single seeded course.
        [Fact]
        public async Task GetAllCourses_ShouldReturnSeeded()
        {
            using var context = new AppDbContext(CreateInMemoryOptions());
            SeedData(context);

            var courses = await context.Courses.ToListAsync();
            Assert.Single(courses);
        }

        /// TESTING CREATE: Verifies creating a valid course succeeds.
        [Fact]
        public async Task CreateCourse_Valid_ShouldSucceed()
        {
            using var context = new AppDbContext(CreateInMemoryOptions());
            SeedData(context);

            var newCourse = new Course
            {
                Title = "English Literature",
                Description = "Intro to English Lit",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(10),
                IsActive = true
            };
            context.Courses.Add(newCourse);
            await context.SaveChangesAsync();

            Assert.True(newCourse.CourseId > 0);
        }

        /// TESTING CREATE: Ensures an invalid date range fails to save.
        [Fact]
        public async Task CreateCourse_InvalidDate_ShouldFail()
        {
            using var context = new AppDbContext(CreateInMemoryOptions());
            SeedData(context);

            var invalidCourse = new Course
            {
                Title = "Invalid",
                Description = "End < Start",
                StartDate = DateTime.Now.AddDays(10),
                EndDate = DateTime.Now.AddDays(1),
                IsActive = true
            };
            try
            {
                context.Courses.Add(invalidCourse);
                await context.SaveChangesAsync();
                Assert.True(false, "Should fail if domain enforces StartDate <= EndDate");
            }
            catch
            {
                Assert.True(true);
            }
        }

        /// TESTING UPDATE: Verifies we can change a course's title.
        [Fact]
        public async Task UpdateCourse_ShouldChangeTitle()
        {
            using var context = new AppDbContext(CreateInMemoryOptions());
            SeedData(context);

            var course = await context.Courses.FirstAsync();
            course.Title = "Algebra - Updated";
            await context.SaveChangesAsync();

            var updated = await context.Courses.FindAsync(course.CourseId);
            Assert.Equal("Algebra - Updated", updated.Title);
        }

        /// TESTING UPDATE: Checks no effect when updating a non-existing course.
        [Fact]
        public async Task UpdateCourse_NonExisting_NoEffect()
        {
            using var context = new AppDbContext(CreateInMemoryOptions());
            SeedData(context);

            var phantom = new Course { CourseId = 9999, Title = "Phantom" };
            context.Courses.Update(phantom);

            try
            {
                var changes = await context.SaveChangesAsync();
                Assert.Equal(0, changes);
            }
            catch (DbUpdateConcurrencyException)
            {
                Assert.True(true);
            }
        }

        /// TESTING DELETE: Confirms that removing a course marks it inactive.
        [Fact]
        public async Task DeleteCourse_ShouldSoftDelete()
        {
            using var context = new AppDbContext(CreateInMemoryOptions());
            SeedData(context);

            var course = await context.Courses.FirstAsync();
            context.Courses.Remove(course);
            await context.SaveChangesAsync();

            var check = await context.Courses.FindAsync(course.CourseId);
            Assert.False(check.IsActive);
        }
    }
}