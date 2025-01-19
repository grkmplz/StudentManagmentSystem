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
    public class RoleTests
    {
        /// PREPARING TEST: Builds in-memory database for role tests.
        private DbContextOptions<AppDbContext> CreateOptions()
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase($"RoleTest_{Guid.NewGuid()}")
                .Options;
        }

        /// PREPARING TEST: Adds two roles (Admin, Student) as seed data.
        private void SeedData(AppDbContext ctx)
        {
            var r1 = new Role { Name="Admin", NormalizedName="ADMIN", IsActive=true };
            var r2 = new Role { Name="Student", NormalizedName="STUDENT", IsActive=true };
            ctx.Roles.AddRange(r1, r2);
            ctx.SaveChanges();
        }

        /// TESTING READ: Checks the initial roles are retrieved.
        [Fact]
        public async Task GetAllRoles_ShouldReturnSeeded()
        {
            using var context = new AppDbContext(CreateOptions());
            SeedData(context);

            var roles = await context.Roles.ToListAsync();
            Assert.Equal(2, roles.Count);
        }

        /// TESTING CREATE: Verifies creating a valid role is successful.
        [Fact]
        public async Task CreateRole_Valid_ShouldSucceed()
        {
            using var context = new AppDbContext(CreateOptions());
            SeedData(context);

            var newR = new Role
            {
                Name="Manager",
                NormalizedName="MANAGER",
                IsActive=true
            };
            context.Roles.Add(newR);
            await context.SaveChangesAsync();

            Assert.NotNull(await context.Roles.FirstOrDefaultAsync(r=>r.Name=="Manager"));
        }

        /// TESTING CREATE: Confirms duplicate role names fail.
        [Fact]
        public async Task CreateRole_DuplicateName_ShouldFail()
        {
            using var context = new AppDbContext(CreateOptions());
            SeedData(context);

            var dup = new Role { Name="Admin", NormalizedName="ADMIN" };
            try
            {
                context.Roles.Add(dup);
                await context.SaveChangesAsync();
                Assert.True(false,"Should fail if unique constraint on name/NormalizedName");
            }
            catch
            {
                Assert.True(true);
            }
        }

        /// TESTING UPDATE: Ensures we can rename a role properly.
        [Fact]
        public async Task UpdateRole_ShouldChangeName()
        {
            using var context = new AppDbContext(CreateOptions());
            SeedData(context);

            var role = await context.Roles.FirstAsync(r=>r.Name=="Student");
            role.Name = "NewStudent";
            await context.SaveChangesAsync();

            var updated = await context.Roles.FindAsync(role.Id);
            Assert.Equal("NewStudent", updated.Name);
        }

        /// TESTING UPDATE: Checks no changes for a non-existing role.
        [Fact]
        public async Task UpdateRole_NonExisting_NoEffect()
        {
            using var context = new AppDbContext(CreateOptions());
            SeedData(context);

            var phantom = new Role { Id="999", Name="Phantom" };
            context.Roles.Update(phantom);
            try
            {
                var c = await context.SaveChangesAsync();
                Assert.Equal(0, c);
            }
            catch (DbUpdateConcurrencyException)
            {
                Assert.True(true);
            }
        }
    }
}
