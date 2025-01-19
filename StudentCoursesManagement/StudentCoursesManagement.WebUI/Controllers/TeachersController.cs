using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentCoursesManagement.Application.Services;
using StudentCoursesManagement.Domain.Entities;
using StudentCoursesManagement.Infrastructure.Config;

namespace StudentCoursesManagement.WebUI.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class TeachersController : Controller
    {
        private readonly TeacherService _teacherService;

        public TeachersController(TeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var teachers = await _teacherService.GetAllTeachersAsync();
            return View(teachers);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Teacher());
        }

        [HttpPost]
        public async Task<IActionResult> Create(Teacher teacher)
        {
            if (!ModelState.IsValid) return View(teacher);

            var result = await _teacherService.CreateTeacherAsync(teacher);
            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Failed to create teacher.");
                return View(teacher);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var teacher = await _teacherService.GetTeacherByIdAsync(id);
            if (teacher == null) return NotFound();
            return View(teacher);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Teacher teacher)
        {
            if (!ModelState.IsValid) return View(teacher);

            var result = await _teacherService.UpdateTeacherAsync(teacher);
            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Failed to update teacher.");
                return View(teacher);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _teacherService.DeleteTeacherAsync(id);
            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Failed to delete teacher.");
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Search(string query)
        {
            var results = await _teacherService.SearchTeachers(query);
            return View("Index", results);
        }
    }
}
