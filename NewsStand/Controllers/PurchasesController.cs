using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NewsStand.Controllers
{
    [Authorize]
    public class PurchasesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            TempData["Id"] = id;

            return View();
        }

        public IActionResult Edit(int id)
        {
            TempData["Id"] = id;

            return View();
        }
    }
}