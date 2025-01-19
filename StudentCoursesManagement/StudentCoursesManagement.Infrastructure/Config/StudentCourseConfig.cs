using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentCoursesManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentCoursesManagement.Infrastructure.Config
{
    public class StudentCourseConfig : IEntityTypeConfiguration<UserCourse>
    {
        public void Configure(EntityTypeBuilder<UserCourse> builder)
        {
            builder.HasKey(sc => new 
            {
                sc.CoursesCourseId,
                sc.UsersUserId 
            });

            builder.HasOne(sc => sc.User)
               .WithMany(s => s.UserCourses)
               .HasForeignKey(sc => sc.UsersUserId);

            builder.HasOne(sc => sc.Course)
               .WithMany(c => c.UserCourses)
               .HasForeignKey(sc => sc.CoursesCourseId);
        }
    }
}
