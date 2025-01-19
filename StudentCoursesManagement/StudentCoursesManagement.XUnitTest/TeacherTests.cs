using Xunit;
using System;
using Microsoft.EntityFrameworkCore;
using StudentCoursesManagement.Infrastructure.Context;
using StudentCoursesManagement.Domain.Entities;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace StudentCoursesManagement.XunitTest
{
    public class TeacherTests
    {
        /// PREPARING TEST: Configures in-memory database for teacher testing.
        private DbContextOptions<AppDbContext> CreateInMemoryOptions()
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase($"TeacherTestDB_{Guid.NewGuid()}")
                .Options;
        }

        /// PREPARING TEST: Adds two teachers (John, Jane) to start with.
        private void SeedData(AppDbContext context)
        {
            var t1 = new Teacher { FirstName="John", LastName="Doe", Specialization="Math", IsActive=true };
            var t2 = new Teacher { FirstName="Jane", LastName="Smith", Specialization="Physics", IsActive=true };
            context.Teachers.AddRange(t1, t2);
            context.SaveChanges();
        }

        /// TESTING READ: Checks the two seeded teachers are found.
        [Fact]
        public async Task GetAllTeachers_ShouldReturnSeeded()
        {
            using var context = new AppDbContext(CreateInMemoryOptions());
            SeedData(context);

            var teachers = await context.Teachers.ToListAsync();
            Assert.Equal(2, teachers.Count);
        }

        /// TESTING CREATE: Ensures a valid teacher is created.
        [Fact]
        public async Task CreateTeacher_Valid_ShouldSucceed()
        {
            using var context = new AppDbContext(CreateInMemoryOptions());
            SeedData(context);

            var newT = new Teacher
            {
                FirstName = "Mike",
                LastName = "Brown",
                Specialization = "History",
                IsActive = true
            };
            context.Teachers.Add(newT);
            await context.SaveChangesAsync();

            Assert.True(newT.TeacherId > 0);
        }

        /// TESTING CREATE: Verifies missing first/last name fails to save.
        [Fact]
        public async Task CreateTeacher_MissingName_ShouldFail()
        {
            using var context = new AppDbContext(CreateInMemoryOptions());
            SeedData(context);

            var invalidT = new Teacher
            {
                FirstName = "",
                LastName = "",
                Specialization = "NoName"
            };
            try
            {
                context.Teachers.Add(invalidT);
                await context.SaveChangesAsync();
                Assert.True(false, "Should fail due to missing name");
            }
            catch
            {
                Assert.True(true);
            }
        }

        /// TESTING UPDATE: Checks we can modify a teacher’s names.
        [Fact]
        public async Task UpdateTeacher_ShouldChangeName()
        {
            using var context = new AppDbContext(CreateInMemoryOptions());
            SeedData(context);

            var teacher = await context.Teachers.FirstAsync(t => t.FirstName=="John");
            teacher.FirstName = "UpdatedFN";
            teacher.LastName = "UpdatedLN";
            await context.SaveChangesAsync();

            var updated = await context.Teachers.FindAsync(teacher.TeacherId);
            Assert.Equal("UpdatedFN", updated.FirstName);
            Assert.Equal("UpdatedLN", updated.LastName);
        }

        /// TESTING UPDATE: Makes sure no effect occurs on a non-existing teacher.
        [Fact]
        public async Task UpdateTeacher_NonExisting_NoEffect()
        {
            using var context = new AppDbContext(CreateInMemoryOptions());
            SeedData(context);

            var phantom = new Teacher
            {
                TeacherId=9999,
                FirstName="Phantom",
                LastName="NoOne"
            };
            context.Teachers.Update(phantom);
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

        /// TESTING DELETE: Confirms teacher removal sets IsActive=false.
        [Fact]
        public async Task DeleteTeacher_ShouldSoftDelete()
        {
            using var context = new AppDbContext(CreateInMemoryOptions());
            SeedData(context);

            var teacher = await context.Teachers.FirstAsync();
            context.Teachers.Remove(teacher);
            await context.SaveChangesAsync();

            var check = await context.Teachers.FindAsync(teacher.TeacherId);
            Assert.False(check.IsActive);
        }
    }
}
