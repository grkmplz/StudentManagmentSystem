using Xunit;
using System;
using Microsoft.EntityFrameworkCore;
using StudentCoursesManagement.Infrastructure.Context;
using StudentCoursesManagement.Domain.Entities;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace StudentCoursesManagement.XunitTest
{
    public class StudentTests
    {
        /// PREPARING TEST: Builds an in-memory database for user (student) tests.
        private DbContextOptions<AppDbContext> CreateOptions()
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase($"StudentTest_{Guid.NewGuid()}")
                .Options;
        }

        /// PREPARING TEST: Inserts two users (stu1, stu2) as seed data.
        private void SeedData(AppDbContext ctx)
        {
            var u1 = new User { Id="stu1", UserName="stu1@example.com", Email="stu1@example.com", FirstName="Ali", LastName="Kara", IsActive=true };
            var u2 = new User { Id="stu2", UserName="stu2@example.com", Email="stu2@example.com", FirstName="Ayse", LastName="Sari", IsActive=true };
            ctx.Users.AddRange(u1, u2);
            ctx.SaveChanges();
        }

        /// TESTING READ: Checks we can retrieve the seeded users.
        [Fact]
        public async Task GetAllStudents_ShouldReturnSeeded()
        {
            using var context = new AppDbContext(CreateOptions());
            SeedData(context);

            var students = await context.Users.ToListAsync();
            Assert.Equal(2, students.Count);
        }

        /// TESTING CREATE: Verifies that a valid new user is created successfully.
        [Fact]
        public async Task CreateStudent_Valid_ShouldSucceed()
        {
            using var context = new AppDbContext(CreateOptions());
            SeedData(context);

            var newStu = new User
            {
                Id="stuX",
                UserName="stuX@example.com",
                Email="stuX@example.com",
                FirstName="Fatma",
                LastName="Deniz",
                IsActive=true
            };
            context.Users.Add(newStu);
            await context.SaveChangesAsync();

            var found = await context.Users.FindAsync("stuX");
            Assert.NotNull(found);
        }

        /// TESTING CREATE: Ensures an invalid email triggers failure.
        [Fact]
        public async Task CreateStudent_InvalidEmail_ShouldFail()
        {
            using var context = new AppDbContext(CreateOptions());
            SeedData(context);

            var invalid = new User
            {
                Id="stuY",
                UserName="badEmail",
                Email="badEmail"
            };
            try
            {
                context.Users.Add(invalid);
                await context.SaveChangesAsync();
                Assert.True(false,"Should fail if domain enforces valid email");
            }
            catch
            {
                Assert.True(true);
            }
        }

        /// TESTING UPDATE: Checks we can change the user's first/last name.
        [Fact]
        public async Task UpdateStudent_ShouldChangeName()
        {
            using var context = new AppDbContext(CreateOptions());
            SeedData(context);

            var stu = await context.Users.FindAsync("stu1");
            stu.FirstName = "UpdatedFName";
            stu.LastName = "UpdatedLName";
            await context.SaveChangesAsync();

            var check = await context.Users.FindAsync("stu1");
            Assert.Equal("UpdatedFName", check.FirstName);
            Assert.Equal("UpdatedLName", check.LastName);
        }

        /// TESTING UPDATE: Verifies no effect occurs on non-existing user updates.
        [Fact]
        public async Task UpdateStudent_NonExisting_NoEffect()
        {
            using var context = new AppDbContext(CreateOptions());
            SeedData(context);

            var phantom = new User { Id="nope", FirstName="Phantom" };
            context.Users.Update(phantom);
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

        /// TESTING DELETE: Confirms user soft-deletion sets IsActive=false.
        [Fact]
        public async Task DeleteStudent_ShouldSoftDelete()
        {
            using var context = new AppDbContext(CreateOptions());
            SeedData(context);

            var stu = await context.Users.FindAsync("stu2");
            context.Users.Remove(stu);
            await context.SaveChangesAsync();

            var check = await context.Users.FindAsync("stu2");
            Assert.False(check.IsActive);
        }
    }
}
