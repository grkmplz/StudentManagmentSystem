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
    public static class UserRoles
    {
        public const string Admin = "Admin";
        public const string Student = "Student";
        public const string None = "None";
    }
}
