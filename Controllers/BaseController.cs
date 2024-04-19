using FunFoxTask.Models;
using Microsoft.AspNetCore.Mvc;

namespace FunFoxTask.Controllers
{
    public class BaseController : Controller
    {
        public IActionResult Error(ErrorViewModel errorViewModel)
        {
            return View(errorViewModel);
        }

    }
}
