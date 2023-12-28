using Ecommerce.Models.Catalog;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Ecommerce.UI.Areas.Inventory.Controllers
{
    [Area("Inventory")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
