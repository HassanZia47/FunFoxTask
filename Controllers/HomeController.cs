using FunFoxTask.BusinessLogic;
using FunFoxTask.Models;
using Microsoft.AspNetCore.Mvc;

namespace FunFoxTask.Controllers
{
    public class HomeController : BaseController
    {
        private readonly BusinessLayer _busLayer;

        public HomeController(BusinessLayer busLayer)
        {
            _busLayer = busLayer;
        }

        public IActionResult Index()
        {
            try
            {
                _busLayer.CreateAdminUserIfNotExist();
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Error), new ErrorViewModel { ErrorMessage = "Error while creating admin user" });
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}