using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentCoursesManagement.Domain.Entities;

namespace StudentCoursesManagement.Infrastructure.ConfigAndSeed
{
    public class IdentityUserRoleConfig : IEntityTypeConfiguration<UserRole>
    {

        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            // Maps to the AspNetUserRoles table
            builder.ToTable("AspNetUserRoles");

            var adminUserRole = new UserRole
            {
                UserId = "1",
                RoleId = "1"
            };

            builder.HasData(adminUserRole);
        }
    }
}
