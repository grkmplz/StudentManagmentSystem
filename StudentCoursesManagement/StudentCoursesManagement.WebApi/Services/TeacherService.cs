// File: WebApi/Services/TeacherService.cs
using Microsoft.EntityFrameworkCore;
using StudentCoursesManagement.Domain.Entities;
using StudentCoursesManagement.Infrastructure.Context;
using StudentCoursesManagement.WebApi.Services.Interfaces;

namespace StudentCoursesManagement.WebApi.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly AppDbContext _context;

        public TeacherService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Teacher>> GetAllTeachersAsync()
        {
            return await _context.Teachers
                .ToListAsync();
        }

        public async Task<Teacher?> GetTeacherByIdAsync(int id)
        {
            return await _context.Teachers
                .Where(t => t.TeacherId == id)
                .FirstOrDefaultAsync();
        }

        public async Task CreateTeacherAsync(Teacher teacher)
        {
            teacher.CreatedAt = DateTime.UtcNow;
            teacher.IsActive = true;
            await _context.Teachers.AddAsync(teacher);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTeacherAsync(Teacher updated)
        {
            var teacher = await GetTeacherByIdAsync(updated.TeacherId);
            if (teacher == null) return;

            teacher.FirstName = updated.FirstName;
            teacher.LastName = updated.LastName;
            teacher.Specialization = updated.Specialization;
            teacher.ModifiedAt = DateTime.UtcNow; 
            teacher.IsActive = updated.IsActive;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteTeacherAsync(int id)
        {
            var teacher = await GetTeacherByIdAsync(id);
            if (teacher != null)
            {
                teacher.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Teacher>> SearchTeachersAsync(string query)
        {
            return await _context.Teachers
                .Where(t => t.IsActive
                    && (t.FirstName.Contains(query)
                     || t.LastName.Contains(query)
                     || t.Specialization.Contains(query)))
                .ToListAsync();
        }
    }
}
