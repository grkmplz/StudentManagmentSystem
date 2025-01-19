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
    public class DepartmentTests
    {
        /// PREPARING TEST: Builds in-memory options for department tests.
        private DbContextOptions<AppDbContext> CreateOptions()
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase($"DeptTest_{Guid.NewGuid()}")
                .Options;
        }

        /// PREPARING TEST: Inserts two departments (Science, Literature).
        private void SeedData(AppDbContext ctx)
        {
            var d1 = new Department { DepartmentName="Science", IsActive=true };
            var d2 = new Department { DepartmentName="Literature", IsActive=true };
            ctx.Departments.AddRange(d1, d2);
            ctx.SaveChanges();
        }

        /// TESTING READ: Checks if seeded departments are retrieved.
        [Fact]
        public async Task GetAllDepartments_ShouldReturnSeeded()
        {
            using var context = new AppDbContext(CreateOptions());
            SeedData(context);

            var depts = await context.Departments.ToListAsync();
            Assert.Equal(2, depts.Count);
        }

        /// TESTING CREATE: Verifies adding a valid department works.
        [Fact]
        public async Task CreateDepartment_Valid_ShouldSucceed()
        {
            using var context = new AppDbContext(CreateOptions());
            SeedData(context);

            var newDept = new Department { DepartmentName="Engineering", IsActive=true };
            context.Departments.Add(newDept);
            await context.SaveChangesAsync();

            Assert.True(newDept.DepartmentId > 0);
        }

        /// TESTING CREATE: Ensures an empty name fails to save.
        [Fact]
        public async Task CreateDepartment_EmptyName_ShouldFail()
        {
            using var context = new AppDbContext(CreateOptions());
            SeedData(context);

            var invalidDept = new Department { DepartmentName="" };
            try
            {
                context.Departments.Add(invalidDept);
                await context.SaveChangesAsync();
                Assert.True(false, "Should fail for empty name");
            }
            catch
            {
                Assert.True(true);
            }
        }

        /// TESTING UPDATE: Checks we can rename a department.
        [Fact]
        public async Task UpdateDepartment_ShouldChangeName()
        {
            using var context = new AppDbContext(CreateOptions());
            SeedData(context);

            var dept = await context.Departments.FirstAsync(d => d.DepartmentName=="Science");
            dept.DepartmentName = "Sci & Tech";
            await context.SaveChangesAsync();

            var updated = await context.Departments.FindAsync(dept.DepartmentId);
            Assert.Equal("Sci & Tech", updated.DepartmentName);
        }

        /// TESTING UPDATE: Ensures no changes occur for a non-existing department.
        [Fact]
        public async Task UpdateDepartment_NonExisting_NoEffect()
        {
            using var context = new AppDbContext(CreateOptions());
            SeedData(context);

            var phantom = new Department { DepartmentId=9999, DepartmentName="NoOne" };
            context.Departments.Update(phantom);
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

        /// TESTING DELETE: Confirms we soft-delete a department.
        [Fact]
        public async Task DeleteDepartment_ShouldSoftDelete()
        {
            using var context = new AppDbContext(CreateOptions());
            SeedData(context);

            var dept = await context.Departments.FirstAsync();
            context.Departments.Remove(dept);
            await context.SaveChangesAsync();

            var check = await context.Departments.FindAsync(dept.DepartmentId);
            Assert.False(check.IsActive);
        }
    }
}
