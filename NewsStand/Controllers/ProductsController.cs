using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsStand.Core.ViewModels;

namespace NewsStand.Controllers
{
    [Authorize]
    public class ProductsController : Controller
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
            var productViewModel = new ProductViewModel
            {
                Id = id
            };

            return View(productViewModel);
        }
    }
}
