using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentCoursesManagement.Domain.Entities;
using StudentCoursesManagement.Infrastructure.Config;
using StudentCoursesManagement.Infrastructure.Context;
using StudentCoursesManagement.WebApi.Dtos;
using StudentCoursesManagement.WebApi.Dtos.Student;
using StudentCoursesManagement.WebApi.Dtos.StudentDto;
using StudentCoursesManagement.WebApi.Services.Interfaces;

namespace StudentCoursesManagement.WebApi.Services
{
    public class StudentService : IStudentService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly AppDbContext _context;

        public StudentService(UserManager<User> userManager, RoleManager<Role> roleManager, AppDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            var users = await _context.Users.ToListAsync();

            return users;
        }

        public async Task<List<User>> GetAllStudentsAsync()
        {
            var students = await _context.Users.ToListAsync();

            var studentUserList = new List<User>();

            foreach (var student in students)
            {
                var studentUser = _userManager.IsInRoleAsync(student, UserRoles.Student);

                if (studentUser.Result is true)
                {
                    studentUserList.Add(student);
                }
            }

            return studentUserList;
        }

        public async Task<User> GetStudentByIdAsync(string id)
        {
            var studentUser = await _userManager.Users
                   .Include(u => u.UserCourses)
                       .ThenInclude(uc => uc.Course).FirstOrDefaultAsync(x => x.Id == id);

            var isRoleStudent = await _userManager.IsInRoleAsync(studentUser, UserRoles.Student);

            if (!isRoleStudent)
            {
                return null;
            }

            return studentUser;
        }

        public async Task AddStudentAsync(CreateStudentDto createStudentDto)
        {
            createStudentDto.CreatedAt = DateTime.UtcNow;
            createStudentDto.IsActive = true;

            var studentUser = new User
            {
                UserName = createStudentDto.Email,
                Email = createStudentDto.Email,
                FirstName = createStudentDto.FirstName,
                LastName = createStudentDto.LastName,
                EnrollmentDate = createStudentDto.EnrollmentDate,
            };

            var result = await _userManager.CreateAsync(studentUser, createStudentDto.Password);

            // In the scenario, system assign to "student" role to student while student creating.
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(studentUser, UserRoles.Student);
            }

            await _context.SaveChangesAsync();
        }

        public async Task UpdateStudentAsync(UpdateStudentDto updateStudentDto)
        {
            var studentById = await GetStudentByIdAsync(updateStudentDto.Id);

            updateStudentDto.ModifiedAt = DateTime.UtcNow;
            updateStudentDto.EnrollmentDate = DateTime.SpecifyKind(updateStudentDto.EnrollmentDate, DateTimeKind.Utc);

            studentById.Id = updateStudentDto.Id;

            studentById.FirstName = updateStudentDto.FirstName;
            studentById.LastName = updateStudentDto.LastName;
            studentById.EnrollmentDate = updateStudentDto.EnrollmentDate;
            studentById.IsActive = updateStudentDto.IsActive;

            await _userManager.UpdateAsync(studentById);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteStudentAsync(string id)
        {
            var studentUser = await GetStudentByIdAsync(id);

            if (studentUser != null)
            {
                studentUser.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }

        public async Task AssignCoursesToStudentAsync(AssignCourseToStudentsDto dto)
        {           
            var userCourses = await _context.UserCourses
                .Where(x => x.UsersUserId == dto.StudentId)
                .ToListAsync();

            foreach (var userCourse in userCourses)
            {
                userCourse.IsActive = false;
            }

            if(dto.CourseIds != null)
            {
                foreach (var courseId in dto.CourseIds)
                {
                    var existingCourse = userCourses.FirstOrDefault(x => x.CoursesCourseId == courseId);

                    if (existingCourse != null)
                    {
                        existingCourse.IsActive = true;
                        _context.Entry(existingCourse).State = EntityState.Modified;
                    }
                    else
                    {
                        var newCourse = new UserCourse
                        {
                            UsersUserId = dto.StudentId,
                            CoursesCourseId = courseId,
                            IsActive = true
                        };
                        _context.UserCourses.Add(newCourse);
                    }
                }
            }
            
            await _context.SaveChangesAsync();
        }


        public async Task<List<GetStudentsCourseDto>> GetStudentCourses(string userId)
        {
            var studentCourseList = await _context.UserCourses.Where(x => x.UsersUserId == userId && x.IsActive).ToListAsync();
            var returnModel = new List<GetStudentsCourseDto>();

            if (studentCourseList == null)
            {
                return returnModel;
            }

            foreach (var items in studentCourseList)
            {
                var course = await _context.Courses.Where(x => x.CourseId == items.CoursesCourseId).FirstOrDefaultAsync();

                if (course != null)
                {
                    var dtoModel = new GetStudentsCourseDto
                    {
                        CourseId = course.CourseId,
                        CourseName = course.Title,
                        UserId = userId,
                    };

                    returnModel.Add(dtoModel);
                }
            }

            return returnModel;
        }

        public async Task UpdateMyInfos(UpdateMyInfosDto updateMyInfosDto)
        {
            var studentById = await GetStudentByIdAsync(updateMyInfosDto.StudentId);

            updateMyInfosDto.ModifiedAt = DateTime.UtcNow;
            updateMyInfosDto.EnrollmentDate = DateTime.SpecifyKind(updateMyInfosDto.EnrollmentDate, DateTimeKind.Utc);

            studentById.Id = updateMyInfosDto.StudentId;

            studentById.Email = updateMyInfosDto.Email;
            studentById.UserName = updateMyInfosDto.UserName;
            studentById.FirstName = updateMyInfosDto.FirstName;
            studentById.LastName = updateMyInfosDto.LastName;
            studentById.IsActive = updateMyInfosDto.IsActive;

            await _userManager.UpdateAsync(studentById);

            await _context.SaveChangesAsync();
        }

    }
}

