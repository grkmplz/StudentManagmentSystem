using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentCoursesManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks; 

namespace StudentCoursesManagement.Infrastructure.Config
{
    public class IdentityRoleConfig : IEntityTypeConfiguration<Role>
    {       
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("AspNetRoles");

            builder.HasData(
               new Role
               {
                   Id = "1",
                   Name = UserRoles.Admin,
                   NormalizedName = UserRoles.Admin.ToUpper(),
                   IsActive = true,
               },
               new Role
               { 
                   Id = "2",
                   Name = UserRoles.Student,
                   NormalizedName = UserRoles.Student.ToUpper(),
                   IsActive = true
               },
            new Role
            {
                Id = "3",
                Name = UserRoles.None,
                NormalizedName = UserRoles.None.ToUpper(),
                IsActive=true
            });
        }
    }
}
