using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FunFoxTask.Data;
using FunFoxTask.Models;
using Microsoft.AspNetCore.Authorization;
using FunFoxTask.BusinessLogic;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FunFoxTask.Controllers
{
    [Authorize(Roles = "admin")]
    public class CourseController : BaseController
    {
        private readonly BusinessLayer _busLayer;

        public CourseController(BusinessLayer busLayer)
        {
            _busLayer = busLayer;
        }

        // GET: Course
        public IActionResult Index()
        {
            try
            {
                return View(_busLayer.GetAllCoursesList());
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Error), new ErrorViewModel { ErrorMessage = "Error while fetching all courses" });
            }
        }

        // GET: Course/Details/5
        public IActionResult Details(int? id)
        {
            try
            {
                Course? course = _busLayer.GetCourseDetail(id);
                if (course != null)
                    return View(course);
                else
                    return RedirectToAction(nameof(Error), new ErrorViewModel { ErrorMessage = "Error while fetching course detail" });
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Error), new ErrorViewModel { ErrorMessage = "Error while fetching course detail" });
            }
        }

        // GET: Course/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Course/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name,Description,Levels,Timings,MaxStrengh")] Course course)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _busLayer.AddCourse(course);
                    return RedirectToAction(nameof(Index));
                }
                return View(course);
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Error), new ErrorViewModel { ErrorMessage = "Error while Creating new course" });
            }
        }

        // GET: Course/Edit/5
        public IActionResult Edit(int? id)
        {
            try
            {
                Course? course = _busLayer.GetEditCourse(id);
                if (course != null)
                    return View(course);
                else
                    return RedirectToAction(nameof(Error), new ErrorViewModel { ErrorMessage = "Error while fetching details to edit course" });
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Error), new ErrorViewModel { ErrorMessage = "Error while fetching details to edit course" });
            }
        }

        // POST: Course/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name,Description,Levels,Timings,MaxStrengh")] Course course)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _busLayer.PostEditCourse(id, course);
                    return View(course);
                }
                catch (Exception)
                {
                    return RedirectToAction(nameof(Error), new ErrorViewModel { ErrorMessage = "Error while saving course" });
                }
            }
            return RedirectToAction(nameof(Error), new ErrorViewModel { ErrorMessage = "Error while saving course" });
        }

        // GET: Course/Delete/5
        public IActionResult Delete(int? id)
        {
            try
            {
                Course? course = _busLayer.GetDelete(id);
                if (course != null)
                    return View(course);
                else
                    return RedirectToAction(nameof(Error), new ErrorViewModel { ErrorMessage = "Error while fetching data to delete course" });
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Error), new ErrorViewModel { ErrorMessage = "Error while fetching data to delete course" });
            }
        }

        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _busLayer.DeleteConfirmed(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Error), new ErrorViewModel { ErrorMessage = "Error while deleting course" });
            }
        }

        public IActionResult EnrolledUsers(int? id)
        {
            try
            {
                EnrolledUserVM? enrolledUserVM = _busLayer.GetEnrolledUsers(id);
                if (enrolledUserVM != null)
                    return View(enrolledUserVM);
                else
                    return RedirectToAction(nameof(Error), new ErrorViewModel { ErrorMessage = "Error while fetching enrolled user list" });
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Error), new ErrorViewModel { ErrorMessage = "Error while fetching enrolled user list" });
            }
        }

        public IActionResult EnrolledUserDelete(string userId, int courseId)
        {
            try
            {
                _busLayer.EnrolledUserDelete(userId, courseId);
                return RedirectToAction(nameof(EnrolledUsers), new { id = courseId });
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Error), new ErrorViewModel { ErrorMessage = "Error while deleting enrolled user" });
            }
        }

        public IActionResult EnrolledUserDetail(string userId, int courseId)
        {
            ViewData["courseId"] = courseId;
            AppUser? user = _busLayer.GetEnrolledUserDetail(userId);

            if (user != null)
            {
                return View(user);
            }
            return RedirectToAction(nameof(Error), new ErrorViewModel { ErrorMessage = "Error while fetching enrolled user details" });
        }
    }
}
