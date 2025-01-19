// File: WebApi/Services/DepartmentService.cs
using Microsoft.EntityFrameworkCore;
using StudentCoursesManagement.Domain.Entities;
using StudentCoursesManagement.Infrastructure.Context;
using StudentCoursesManagement.WebApi.Services.Interfaces;

namespace StudentCoursesManagement.WebApi.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly AppDbContext _context;

        public DepartmentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Department>> GetAllDepartmentsAsync()
        {
            return await _context.Departments
                .ToListAsync();
        }

        public async Task<Department?> GetDepartmentByIdAsync(int id)
        {
            return await _context.Departments
                .Where(d => d.DepartmentId == id)
                .FirstOrDefaultAsync();
        }

        public async Task CreateDepartmentAsync(Department dept)
        {
            dept.IsActive = true;
            dept.CreatedAt = DateTime.UtcNow;
            await _context.Departments.AddAsync(dept);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDepartmentAsync(Department updated)
        {
            var dept = await GetDepartmentByIdAsync(updated.DepartmentId);
            if (dept == null) return;

            dept.DepartmentName = updated.DepartmentName;
            dept.ModifiedAt = DateTime.UtcNow;
            dept.IsActive = updated.IsActive;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDepartmentAsync(int id)
        {
            var dept = await GetDepartmentByIdAsync(id);
            if (dept != null)
            {
                dept.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }
    }
}