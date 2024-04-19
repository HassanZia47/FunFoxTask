using FunFoxTask.BusinessLogic;
using FunFoxTask.Data;
using FunFoxTask.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FunFoxTask.Controllers
{
    [Authorize]
    public class EnrollmentController : BaseController
    {
        private readonly BusinessLayer _busLayer;
        public EnrollmentController(BusinessLayer busLayer)
        {
            _busLayer = busLayer;
        }
        // GET: EnrollmentController
        public IActionResult Index()
        {
            try
            {
                List<CourseVM> coursesList = _busLayer.GetAvailableCoursesToEnroll(HttpContext.User);
                return View(coursesList);
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Error), new ErrorViewModel { ErrorMessage = "Error while fetching all available courses" });
            }
        }

        public IActionResult Enroll(int? id)
        {
            try
            {
                _busLayer.EnrollInCourse(HttpContext.User, id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Error), new ErrorViewModel { ErrorMessage = "Error while enrolling course" });
            }
        }

        public IActionResult Remove(int? id)
        {
            try
            {
                _busLayer.RemoveEnrollment(HttpContext.User, id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Error), new ErrorViewModel { ErrorMessage = "Error while removing enrollment" });
            }
        }
    }
}
