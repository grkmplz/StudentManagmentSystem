// File: WebApi/Services/CourseService.cs
using Microsoft.EntityFrameworkCore;
using StudentCoursesManagement.Domain.Entities;
using StudentCoursesManagement.Infrastructure.Context;
using StudentCoursesManagement.WebApi.Services.Interfaces;

namespace StudentCoursesManagement.WebApi.Services
{
    public class CourseService : ICourseService
    {
        private readonly AppDbContext _context;
        public CourseService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Course>> GetAllCoursesAsync()
        {
            return await _context.Courses
                .Include(c => c.Teacher)
                .Include(c => c.Department)
                .ToListAsync();
        }

        public async Task<Course?> GetCourseByIdAsync(int id)
        {
            return await _context.Courses
                .Include(c => c.Teacher)
                .Include(c => c.Department)
                .FirstOrDefaultAsync(c => c.CourseId == id);
        }

        public async Task AddCourseAsync(Course course)
        {
            course.CreatedAt = DateTime.UtcNow;
            course.IsActive = true;
            await _context.Courses.AddAsync(course);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCourseAsync(Course updated)
        {
            var course = await GetCourseByIdAsync(updated.CourseId);
            if (course == null) return;

            course.Title = updated.Title;
            course.Description = updated.Description;
            course.StartDate = updated.StartDate;
            course.EndDate = updated.EndDate;
            course.IsActive = updated.IsActive;
            course.TeacherId = updated.TeacherId;
            course.DepartmentId = updated.DepartmentId;
            course.ModifiedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteCourseAsync(int id)
        {
            var course = await GetCourseByIdAsync(id);
            if (course != null)
            {
                course.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }
    }
}
