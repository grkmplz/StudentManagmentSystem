using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentCoursesManagement.Application.Services;
using StudentCoursesManagement.Application.ViewModels.CourseViewModels;
using StudentCoursesManagement.Infrastructure.Config;

namespace StudentCoursesManagement.WebUI.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class CoursesController : Controller
    {
        private readonly CourseService _courseService;
        private readonly TeacherService _teacherService;
        private readonly DepartmentService _departmentService;

        public CoursesController(CourseService courseService,
            TeacherService teacherService,
            DepartmentService departmentService)
        {
            _courseService = courseService;
            _teacherService = teacherService;
            _departmentService = departmentService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response = await _courseService.GetAllCoursesAsync();
            return View(response);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Teachers = await _teacherService.GetAllTeachersAsync();
            ViewBag.Departments = await _departmentService.GetAllDepartmentsAsync();
            return View(new CreateCourseViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCourseViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Teachers = await _teacherService.GetAllTeachersAsync();
                ViewBag.Departments = await _departmentService.GetAllDepartmentsAsync();
                return View(model);
            }

            bool success = await _courseService.CreateCourseAsync(model);
            if (!success)
            {
                ModelState.AddModelError("", "Something went wrong in CreateCourseAsync");
                ViewBag.Teachers = await _teacherService.GetAllTeachersAsync();
                ViewBag.Departments = await _departmentService.GetAllDepartmentsAsync();
                return View(model);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var courseVM = await _courseService.GetCourseByIdAsync(id);
            if (courseVM == null) return NotFound();

            var updateModel = new UpdateCourseViewModel
            {
                CourseId = courseVM.CourseId,
                Title = courseVM.Title,
                Description = courseVM.Description,
                StartDate = courseVM.StartDate,
                EndDate = courseVM.EndDate,
                IsActive = courseVM.IsActive,
                TeacherId = courseVM.TeacherId,
                DepartmentId = courseVM.DepartmentId
            };

            ViewBag.Teachers = await _teacherService.GetAllTeachersAsync();
            ViewBag.Departments = await _departmentService.GetAllDepartmentsAsync();
            return View(updateModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateCourseViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Teachers = await _teacherService.GetAllTeachersAsync();
                ViewBag.Departments = await _departmentService.GetAllDepartmentsAsync();
                return View(model);
            }

            bool success = await _courseService.UpdateCourseAsync(model);
            if (!success)
            {
                ModelState.AddModelError("", "Update failed");
                ViewBag.Teachers = await _teacherService.GetAllTeachersAsync();
                ViewBag.Departments = await _departmentService.GetAllDepartmentsAsync();
                return View(model);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _courseService.DeleteCourseAsync(id);
            if (!success)
            {
                ModelState.AddModelError("", "Something went wrong.");
            }

            return RedirectToAction("Index");
        }
    }
}